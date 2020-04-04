using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private Vector3 _target, _mousePos, _refVel;

    [FormerlySerializedAs("_cameraDist")] [SerializeField]
    private float cameraDist = 3.5f;

    [FormerlySerializedAs("_smoothTime")] [SerializeField]
    private float smoothTime = .2f;

    private float _zStart;
    private static Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        _target = player.position;
        _zStart = transform.position.z;
    }

    private void Update()
    {
        _mousePos = CaptureMousePos();
        _target = UpdateTargetPos();
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        var tempPos = Vector3.SmoothDamp(transform.position, _target, ref _refVel, smoothTime);
        transform.position = tempPos;
    }

    private Vector3 UpdateTargetPos()
    {
        var mouseOffset = _mousePos * cameraDist;
        var ret = player.position + mouseOffset;
        ret.z = _zStart;
        return ret;
    }

    private static Vector3 CaptureMousePos()
    {
        Vector2 ret = _camera.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        const float max = .9f;
        if (Math.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }

        return ret;
    }
}