using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Screenshooter.Utils;
using static Screenshooter.Utils.ReflectionEx;
using System.Collections;
#if POLYGLOT
using Polyglot;
#endif

public static class ScreenshotTakerEditor {
	public static bool isScreenshotQueueEmpty => queuedScreenshots.Count == 0;

	static object SizeHolder { get { return GetWindowType("GameViewSizes").FetchProperty("instance").FetchProperty("currentGroup"); } }
	static EditorWindow GameView { get { return EditorWindow.GetWindow(GetWindowType("GameView")); } }

	static Queue<ScreenshotData> queuedScreenshots = new Queue<ScreenshotData>();

	static int totalStages = 0;
	static int currentStage = 0;
	static bool isTakeScreenshot = false;
	static int originalIndex = 0;
	static int newIndex = 0;
	static int resolutionIndex = 0;
#if POLYGLOT
	static Language usedLanguage = Language.English;
#endif

	public static void AddAllSizes(List<ScreenshotData> datas) {
		foreach (ScreenshotData data in datas) {
			if (!data.isEnabled)
				continue;
			object customSize = GetFixedResolution((int)data.resolution.x, (int)data.resolution.y, data.name);
			SizeHolder.CallMethod("AddCustomSize", customSize);
		}
	}

	public static void ClearAllSizes() {
		int buildin = (int)SizeHolder.CallMethod("GetBuiltinCount");
		for (int i = buildin; i < (int)SizeHolder.CallMethod("GetTotalCount");)
			SizeHolder.CallMethod("RemoveCustomSize", buildin);
	}

	public static void CaptureScreenshootQueueAllLanguages(List<ScreenshotData> data, string outputFolder) {
		if (!Directory.Exists(outputFolder))
			Directory.CreateDirectory(outputFolder);

		EditorApplication.isPaused = true;

		for (int i = 0; i < data.Count; i++)
		{
			if (data[i].isEnabled)
			{		
				CaptureScreenshot(data[i]);
			}
		}

		EditorApplication.update -= () => CaptureQueuedScreenshots(outputFolder);
		isTakeScreenshot = false;
		originalIndex = -1;
		currentStage = 0;
		totalStages = queuedScreenshots.Count * 2;
		EditorApplication.update += () => CaptureQueuedScreenshots(outputFolder);
	}

	private static void CaptureScreenshot(ScreenshotData data
#if POLYGLOT
		,Language language
#endif
	) {
		int width = Mathf.RoundToInt(data.resolution.x * data.resolutionMultiplier);
		int height = Mathf.RoundToInt(data.resolution.y * data.resolutionMultiplier);

		if (width <= 0 || height <= 0) {
			Debug.LogWarning("Skipped resolution: " + data.resolution);
		}
		else {
			data = data.Clone();
#if POLYGLOT
			data.lang = language;
#endif
			queuedScreenshots.Enqueue(data);
		}
	}

	private static void CaptureQueuedScreenshots(string path) {
		if (queuedScreenshots.Count == 0)
			return;

		ScreenshotData data = queuedScreenshots.Peek();

		int width = Mathf.RoundToInt(data.resolution.x * data.resolutionMultiplier);
		int height = Mathf.RoundToInt(data.resolution.y * data.resolutionMultiplier);

		Screen.SetResolution(width, height, false);
		EditorApplication.Step();

		string savePath = Path.Combine(path, width.ToString() + "x" + height.ToString());

        if (Directory.Exists(path))
        {
			Directory.CreateDirectory(savePath);
        }

#if POLYGLOT
		EditorUtility.DisplayProgressBar("Making screenshots", $"{data.targetCamera} {width}x{height} {data.lang}", currentStage / (float)totalStages);
#else
		EditorUtility.DisplayProgressBar("Making screenshots", $"{data.targetCamera} {width}x{height}", currentStage / (float)totalStages);
#endif
		++currentStage;

		if (!isTakeScreenshot) {
			isTakeScreenshot = true;

			if (originalIndex == -1)
				originalIndex = (int)GameView.FetchProperty("selectedSizeIndex");

			object customSize = GetFixedResolution(width, height);
			SizeHolder.CallMethod("AddCustomSize", customSize);
			newIndex = (int)SizeHolder.CallMethod("IndexOf", customSize) + (int)SizeHolder.CallMethod("GetBuiltinCount");
			resolutionIndex = newIndex;

#if POLYGLOT
			Localization.Instance.SelectedLanguage = data.lang;
#endif

			GameView.CallMethod("SizeSelectionCallback", resolutionIndex, null);
			GameView.Repaint();
		}
		else {
			isTakeScreenshot = false;

			if (!data.captureOverlayUI || data.targetCamera == ScreenshooterTargetCamera.SceneView)
				CaptureScreenshotWithoutUI(data, savePath);
			else
				CaptureScreenshotWithUI(data, savePath);

			SizeHolder.CallMethod("RemoveCustomSize", newIndex);

			queuedScreenshots.Dequeue();
			if (queuedScreenshots.Count == 0) {
				resolutionIndex = originalIndex;
				GameView.CallMethod("SizeSelectionCallback", resolutionIndex, null);

#if POLYGLOT
				Localization.Instance.SelectedLanguage = usedLanguage;
#endif

				EditorApplication.update -= () => CaptureQueuedScreenshots(path);

				Debug.Log("<b>Saved screenshots:</b> " + path);
				EditorUtility.ClearProgressBar();
			}
		}
	}

	private static void CaptureScreenshotWithoutUI(ScreenshotData data, string path) {
		Camera camera = data.targetCamera == ScreenshooterTargetCamera.GameView ? Camera.main : SceneView.lastActiveSceneView.camera;

		RenderTexture temp = RenderTexture.active;
		RenderTexture temp2 = camera.targetTexture;

		int width = Mathf.RoundToInt(data.resolution.x * data.resolutionMultiplier);
		int height = Mathf.RoundToInt(data.resolution.y * data.resolutionMultiplier);
		RenderTexture renderTex = RenderTexture.GetTemporary(width, height, 24);
		Texture2D screenshot = null;

		try {
			RenderTexture.active = renderTex;

			camera.targetTexture = renderTex;
			camera.Render();

			screenshot = new Texture2D(renderTex.width, renderTex.height, TextureFormat.RGBA32, false);
			screenshot.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0, false);
			screenshot.Apply(false, false);

#if POLYGLOT
			File.WriteAllBytes(ScreenshotTaker.GetUniqueFilePath(renderTex.width, renderTex.height, data.targetCamera == ScreenshooterTargetCamera.SceneView, false, data.lang.ToString(), usedOutputFolder, "jpeg"), screenshot.EncodeToJPG(100));
#else
			File.WriteAllBytes(ScreenshotTaker.GetUniqueFilePath(renderTex.width, renderTex.height, data.targetCamera == ScreenshooterTargetCamera.SceneView, false, "", path, "jpeg"), screenshot.EncodeToJPG(100));
#endif
		}
		finally {
			camera.targetTexture = temp2;

			RenderTexture.active = temp;
			RenderTexture.ReleaseTemporary(renderTex);

			if (screenshot != null)
				GameObject.DestroyImmediate(screenshot);
		}
	}

	private static void CaptureScreenshotWithUI(ScreenshotData data,string path) {
		RenderTexture temp = RenderTexture.active;

		RenderTexture renderTex = (RenderTexture)GameView.FetchField("m_TargetTexture");
		Texture2D screenshot = null;

		int width = renderTex.width;
		int height = renderTex.height;

		try {
			RenderTexture.active = renderTex;

			screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
			screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);

			if (SystemInfo.graphicsUVStartsAtTop) {
				Color32[] pixels = screenshot.GetPixels32();
				for (int i = 0; i < height / 2; i++) {
					int startIndex0 = i * width;
					int startIndex1 = (height - i - 1) * width;
					for (int x = 0; x < width; x++) {
						Color32 color = pixels[startIndex0 + x];
						pixels[startIndex0 + x] = pixels[startIndex1 + x];
						pixels[startIndex1 + x] = color;
					}
				}

				screenshot.SetPixels32(pixels);
			}

			screenshot.Apply(false, false);

#if POLYGLOT
			File.WriteAllBytes(ScreenshotTaker.GetUniqueFilePath(width, height, data.targetCamera == ScreenshooterTargetCamera.SceneView, true, data.lang.ToString(), usedOutputFolder, "jpeg"), screenshot.EncodeToJPG(100));
#else
			File.WriteAllBytes(ScreenshotTaker.GetUniqueFilePath(width, height, data.targetCamera == ScreenshooterTargetCamera.SceneView, true, "", path, "jpeg"), screenshot.EncodeToJPG(100));
#endif
		}
		finally {
			RenderTexture.active = temp;

			if (screenshot != null)
				GameObject.DestroyImmediate(screenshot);
		}
	}

	private static object GetFixedResolution(int width, int height, string name = "MSC_temp") {
		object sizeType = Enum.Parse(GetWindowType("GameViewSizeType"), "FixedResolution");
		return GetWindowType("GameViewSize").CreateInstance(sizeType, width, height, name);
	}
}
