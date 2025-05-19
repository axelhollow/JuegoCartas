using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
{
    private Camera cam;
    private GameObject selectedObject;
    private float distanceToObject;
    private float fixedY = 0.2f;
    public float stackOffset = 0.2f; // Altura entre objetos apilados
    public bool agarrada = false;
    public int valorCarta;
    //public GameObject Moneda;
    public TextMeshProUGUI monedas;
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
            // Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);
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
            TryStack(selectedObject);
            point.y = -0.8450004f;
            selectedObject.transform.position = point;
            //Agarre y seleccion
            selectedObject.GetComponent<DragAndDropController>().agarrada = false;
            selectedObject = null;
        }
    }

    void TryStack(GameObject droppedObject)
    {


        Ray downRay = new Ray(droppedObject.transform.position + Vector3.down * 0.1f, Vector3.down); // Rayo apuntando hacia abajo
        //Debug.DrawRay(downRay.origin, downRay.direction * 1f, Color.green, 2f);
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
            if (hit.collider.CompareTag("Mercado"))
            {
                //for (int i = 1; i <= valorCarta; i++)
                //{
                //    GameObject monedaAux = Instantiate(Moneda);
                //    Vector3 posicion = new Vector3(-8.341585f, 0, 4.220949f);
                //    monedaAux.transform.position = posicion;
                //    downRay = new Ray(posicion + Vector3.down * 0.1f, Vector3.down); // Rayo apuntando hacia abajo
                //    Debug.DrawRay(downRay.origin, downRay.direction * 1f, Color.green, 2f);
                //    if (Physics.Raycast(downRay, out RaycastHit detecta, 1f)) // Alcance del Raycast de 1 metro (ajustable)
                //    {
                //        if (detecta.collider.CompareTag("Draggable"))
                //        {
                //            if (detecta.collider.gameObject.transform.parent == null)
                //            {


                //                monedaAux.transform.SetParent(detecta.collider.gameObject.transform);

                //            }
                //            else
                //            {

                //                monedaAux.transform.SetParent(detecta.collider.gameObject.transform.parent);
                //            }
                //        }
                //    }
                //    monedaAux.transform.position = new Vector3(-8.341585f, -0.8450004f, 4.220949f);

                //}

                int valor_actual = int.Parse(monedas.text.ToString()) + valorCarta;
                monedas.text=valor_actual.ToString();
                Destroy(gameObject);

            }

        }
    }
}
