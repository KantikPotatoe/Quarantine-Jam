using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    public float smoothSpeed = 5f;

    public Vector3 offset = new Vector3(0, 0, -10);
    private Vector3 target, mousePos, refVel;
    private readonly float cameraDist = 3.5f;
    private readonly float smoothTime = .2f;
    private float zStart;

    private void LateUpdate()
    {
       // Vector3 desiredPosition = player.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        //transform.position = smoothedPosition;

        //POST PROCESS EFFECT -- TODO
        //transform.LookAt(target);
    }

    private void Start()
    {
        target = player.position;
        zStart = transform.position.z;

    }

    private void Update()
    {
        mousePos = CaptureMousePos();
        target = UpdateTargetPos();
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 tempPos;
        tempPos = Vector3.SmoothDamp(transform.position, target, ref refVel, smoothTime);
        transform.position = tempPos;
    }

    private Vector3 UpdateTargetPos()
    {
        Vector3 mouseOffset = mousePos * cameraDist;
        Vector3 ret = player.position + mouseOffset;
        ret.z = zStart;
        return ret;
    }

    private Vector3 CaptureMousePos()
    {
        Vector2 ret = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        ret *= 2;
        ret -= Vector2.one;
        float max = .9f;
        if (Math.Abs(ret.x) > max || Mathf.Abs(ret.y) > max)
        {
            ret = ret.normalized;
        }
        return ret;
    }
}
