using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using Zenta.Core.Runtime;
using Zenta.Core.Runtime.Managers;
using Zenta.Core.Runtime.Interfaces;
using WSMGameStudio.Splines;

public class PlayerMovement : MonoBehaviour, IGameStateListener
{
    [MinMaxSlider(-5.0f, 5.0f)] public Vector2 moveClamp;

    [SerializeField] private int speed = 5;

    public float sensitivity = 0.03f;

    private Vector3 mousePos;
    private Vector3 delta;

    public SplineFollower splineFollower;

    #region Singleton

    public static PlayerMovement Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void Start()
    {
        splineFollower.speed = 0f;
    }

    public void OnGameStateChanged(GameState from, GameState to)
    {
        if (to == GameState.Playing)
        {
            splineFollower.speed = speed;
        }
        if (to == GameState.Failed)
        {
            splineFollower.speed = 0f;
        }
    }

    private void Update()
    {
        if (GameManager.Instance.State == GameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                delta = Input.mousePosition - mousePos;

                mousePos = Input.mousePosition;

                transform.position = new Vector3(Mathf.Clamp(transform.position.x + (delta.x * sensitivity), moveClamp.x, moveClamp.y), transform.position.y, transform.position.z);
            }
        }
    }
}
