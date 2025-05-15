using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOrtho : MonoBehaviour
{
    public float velocidadZoom = 5f;
    public float zoomMin = 2f;
    public float zoomMax = 20f;

    private Camera camara;

    void Start()
    {
        camara = GetComponent<Camera>();
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        camara.orthographicSize -= scroll * velocidadZoom;
        camara.orthographicSize = Mathf.Clamp(camara.orthographicSize, zoomMin, zoomMax);
    }
}
