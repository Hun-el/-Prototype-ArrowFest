using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public Transform PlayerTransform;
    private Vector3 _cameraOffset;
    float SmoothFactor = 0.1f;

    private void Start()
    {
        SetOffSet();
    }

    void SetOffSet()
    {
        _cameraOffset = transform.position - PlayerTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 newPos = PlayerTransform.position + _cameraOffset;
        Vector3 newPos2 = new Vector3(0,newPos.y,newPos.z);
        transform.position = Vector3.Slerp(transform.position, newPos2, SmoothFactor);
    }
}
