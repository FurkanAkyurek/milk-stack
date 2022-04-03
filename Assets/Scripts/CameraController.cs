using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    public Vector3 offset;

    #region Singleton
    public static CameraController Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    private void LateUpdate()
    {
        transform.position = target.position + offset;
        transform.rotation = target.rotation;
    }
}
