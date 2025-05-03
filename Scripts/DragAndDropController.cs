using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    private Camera cam;
    private GameObject selectedObject;
    private float distanceToObject;
    private float fixedY=0.5f;
    public float stackOffset = 0.2f; // Altura entre objetos apilados
    public bool agarrada=false;

    void Start()
    {
        cam = Camera.main;
    
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Click izquierdo
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Draggable"))
                {
                    selectedObject = hit.collider.gameObject;
                    selectedObject.GetComponent<DragAndDropController>().agarrada = true;
                    distanceToObject = Vector3.Distance(cam.transform.position, selectedObject.transform.position);
                }
            }
        }

        if (Input.GetMouseButton(0) && selectedObject != null && selectedObject.GetComponent<DragAndDropController>().agarrada)
        {
            selectedObject.transform.SetParent(null);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector3 point = ray.GetPoint(distanceToObject);
            point.y = fixedY; // mantener a altura fija mientras arrastras
            selectedObject.transform.position = point;
        }

        if (Input.GetMouseButtonUp(0) && selectedObject != null && agarrada)
        {
            Vector3 point = gameObject.transform.position;
            gameObject.transform.position = point;
            TryStack(selectedObject);
            point.y = 0;
            gameObject.transform.position= point;


            //Agarre y seleccion
            selectedObject.GetComponent<DragAndDropController>().agarrada=false;
            selectedObject = null;
        }
    }

    void TryStack(GameObject droppedObject)
    {


        Ray downRay = new Ray(droppedObject.transform.position + Vector3.down *0.1f, Vector3.down); // Rayo apuntando hacia abajo
        Debug.DrawRay(downRay.origin, downRay.direction * 1f, Color.green, 2f);
        // Usar un alcance mayor (1f o el valor que mejor te funcione)
        if (Physics.Raycast(downRay, out RaycastHit hit, 1f)) // Alcance del Raycast de 1 metro (ajustable)
        {
            // Asegurarse de que el objeto que está debajo tiene la etiqueta "Draggable"
            if (hit.collider.CompareTag("Draggable"))
            {
                if (hit.collider.gameObject.transform.parent == null) 
                {
                    selectedObject.transform.SetParent(hit.collider.gameObject.transform);

                }
                else 
                {
                    selectedObject.transform.SetParent(hit.collider.gameObject.transform.parent);
                }
            }
        }

    }
}
