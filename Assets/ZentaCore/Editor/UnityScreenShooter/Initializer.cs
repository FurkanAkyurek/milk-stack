using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using UnityEngine;
using System;

namespace Zenta.Core.Editor.Screenshot
{
    public class Initializer
    {
        [MenuItem("Zenta/Take Screenshot &s")]
        static void TakeScreenshot()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Application.productName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

                Debug.Log("Screenshots folder created to desktop");
            }

            ScreenshotTakerEditor.CaptureScreenshootQueueAllLanguages(new List<ScreenshotData>() 
            {
                new ScreenshotData("1242x2208",new Vector2(1242,2208))
                {
                    captureOverlayUI = true,
                    isEnabled = true,
                    targetCamera = ScreenshooterTargetCamera.GameView
                },
                new ScreenshotData("1242,2688",new Vector2(1242,2688))
                {
                    captureOverlayUI = true,
                    isEnabled = true,
                    targetCamera = ScreenshooterTargetCamera.GameView
                },
                new ScreenshotData("2048,2732",new Vector2(2048,2732))
                {
                    captureOverlayUI = true,
                    isEnabled = true,
                    targetCamera = ScreenshooterTargetCamera.GameView
                },
                new ScreenshotData("1080x1920",new Vector2(1080,1920))
                {
                    captureOverlayUI = true,
                    isEnabled = true,
                    targetCamera = ScreenshooterTargetCamera.GameView
                },
            },
            path);;
        }

        /*
        [InitializeOnLoadMethod]
        static void EditorInit()
        {
            System.Reflection.FieldInfo info = typeof(Event).GetField("s_MasterEvent", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);

            Event value = (Event)info.GetValue(null);

            Debug.Log(value);

            if (value != null)
            {
                Debug.Log(value);
            }
        }

        static void EditorGlobalKeyPress()
        {
            Debug.Log("KEY CHANGE " + Event.current.keyCode);
        }*/
    }
}
