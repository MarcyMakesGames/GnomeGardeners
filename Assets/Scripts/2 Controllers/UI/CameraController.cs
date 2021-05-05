using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float maxFOV = 40f;
    public float minFOV = 10f;
    public float zoomLimiter = 50f;

    [Header("Boundaries")]
    public float northBound = 50f;
    public float eastBound = 50f;
    public float southBound = -50f;
    public float westBound = -50f;

    private Vector3 topLeftScreen;
    private Vector3 topRightScreen;
    private Vector3 bottomLeftScreen;
    private Vector3 bottomRightScreen;

    private Vector3 topLeftOcclusionPoint;
    private Vector3 topRightOcclusionPoint;
    private Vector3 bottomLeftOcclusionPoint;
    private Vector3 bottomRightOcclusionPoint;

    private float northPositionBound;
    private float eastPositionBound;
    private float southPositionBound;
    private float westPositionBound;

    private float FOVBound;

    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        targets = new List<Transform>();
        topLeftScreen = new Vector3(0f, Screen.height, -offset.z);
        topRightScreen = new Vector3(Screen.width, Screen.height, -offset.z);
        bottomLeftScreen = new Vector3(0f, 0f, -offset.z);
        bottomRightScreen = new Vector3(Screen.width, 0f, -offset.z);
        FOVBound = maxFOV;
    }

    private void LateUpdate()
    {
        if(targets.Count == 0) { return; }
        Move();
        Zoom();
    }

    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        topLeftOcclusionPoint = cam.ScreenToWorldPoint(topLeftScreen);
        topRightOcclusionPoint = cam.ScreenToWorldPoint(topRightScreen);
        bottomLeftOcclusionPoint = cam.ScreenToWorldPoint(bottomLeftScreen);
        bottomRightOcclusionPoint = cam.ScreenToWorldPoint(bottomRightScreen);

        if (topLeftOcclusionPoint.y > northBound)
        {
            northPositionBound = transform.position.y;
            newPosition.y = northPositionBound;
        }
        if (bottomRightOcclusionPoint.x > eastBound)
        {
            eastPositionBound = transform.position.x;
            newPosition.x = eastPositionBound;
        }
        if (topLeftOcclusionPoint.y < southBound)
        {
            southPositionBound = transform.position.y;
            newPosition.y = southPositionBound;
        }
        if (bottomRightOcclusionPoint.x < westBound)
        {
            westPositionBound = transform.position.x;
            newPosition.x = westPositionBound;
        }


        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(minFOV, maxFOV, GetGreatestDistance() / zoomLimiter);

        if (topLeftOcclusionPoint.y > northBound)
        {
            FOVBound = cam.fieldOfView;
            newZoom = FOVBound;
        }
        if (bottomRightOcclusionPoint.x > eastBound)
        {
            FOVBound = cam.fieldOfView;
            newZoom = FOVBound;
        }
        if (topLeftOcclusionPoint.y < southBound)
        {
            FOVBound = cam.fieldOfView;
            newZoom = FOVBound;
        }
        if (bottomRightOcclusionPoint.x < westBound)
        {
            FOVBound = cam.fieldOfView;
            newZoom = FOVBound;
        }

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; ++i)
        {
            bounds.Encapsulate(targets[i].position);
        }

        var width = bounds.size.x;
        var height = bounds.size.y;
        return width > height ? width : height;
    }

    private Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; ++i)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
