using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public int scaleFactor = 64;

    Camera camera;

    Vector3 startPanPos, currPanPos, savedCamPos;
    bool panning = false;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
        savedCamPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
            handleZoomChanged(-1 * Convert.ToInt32(Input.mouseScrollDelta.y));

        if (panning && Input.GetMouseButtonUp(2)) {
            savedCamPos = transform.position;

            panning = false;
            return;
        }

        if(!panning && Input.GetMouseButtonDown(2)) {
            startPanPos = Input.mousePosition;
            panning = true;
        }

        if (!panning)
            return;

        transform.position = savedCamPos - (-1 * ((startPanPos - Input.mousePosition)/scaleFactor));
    }

    void handleZoomChanged(int dy) =>
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + dy, 10, 30);
}
