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
    public float maxDistance = -40f;
    public float minDistance = -10f;
    public float zoomLimiter = 25f;
    public float minPlayArea = 20f;
    public float maxPlayArea = 40f;
    public float boxLimiter = 25f;

    private Vector3 velocity;
    private Camera cam;
    private BoxCollider box;
    private Rigidbody rb;
    private float aspectRatio;

    private void Start()
    {
        cam = GetComponent<Camera>();
        box = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        targets = new List<Transform>();
        box.center = new Vector3(box.center.x, box.center.y, -transform.position.z);
        aspectRatio = Screen.currentResolution.width / Screen.currentResolution.height;
    }

    private void LateUpdate()
    {
        if(targets.Count == 0) { return; }
        Move();
    }

    public void AddTarget(Transform target)
    {
        targets.Add(target);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        float newZ = Mathf.Lerp(minDistance, maxDistance, GetGreatestDistance() / zoomLimiter);

        newPosition.z = newZ;

        // Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        rb.MovePosition(newPosition);

        box.center = new Vector3(box.center.x, box.center.y, -transform.position.z);

        box.size = new Vector3(Mathf.Lerp(minPlayArea, maxPlayArea, GetGreatestDistance() / boxLimiter) * aspectRatio, Mathf.Lerp(minPlayArea, maxPlayArea, GetGreatestDistance() / boxLimiter), 1f);
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
