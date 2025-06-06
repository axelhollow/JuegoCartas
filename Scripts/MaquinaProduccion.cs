using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Carta;

public class MaquinaProduccion : MonoBehaviour
{
    public int numHijos;
    public GameObject lecheOBJ;
    public CardEnum EnumObjetivo;
    public Slider slider;
    public bool fabricando=false;
    public List<GameObject> cartasAProcesar = new List<GameObject>();

    //Cancelar Fusion
    public bool fusionCancelada = false;
    public Coroutine fusionCoroutine = null;  // Referencia a la coroutine activa para poder cancelarla
    public HashSet<GameObject> cartasEnFusion = new HashSet<GameObject>();
    public float duration;
    public float elapsed;
    //Nivel De Hijos
    public int profundidadMaxima = 99;

    private void Awake()
    {
        numHijos = transform.childCount;

        switch (EnumObjetivo) 
        {
            case CardEnum.Leche: duration = 2;break;
            case CardEnum.Queso: duration = 5;break;
        
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(numHijos<1)numHijos = 1;

            if (transform.childCount > numHijos && fabricando == false)
            {


                fusionCoroutine = StartCoroutine("procesarHijos");

            }
        
        if(transform.childCount < numHijos && fabricando==true) 
        {
            CancelarFusion();
        }

        
    }



    IEnumerator procesarHijos()
    {
        fabricando = true;

        // Guardamos una copia de los hijos originales antes de modificar la jerarqu�a
        List<Transform> hijosOriginales = new List<Transform>();
        foreach (Transform hijo in transform)
        {
            hijosOriginales.Add(hijo);
        }

        foreach (Transform hijo in hijosOriginales)
        {
            // Hijo correcto
            if (hijo.GetComponent<Carta>() != null)
            {
                if (hijo.GetComponent<Carta>().cartaEnum == EnumObjetivo)
                {
                    Queue<(Transform nodo, int nivel)> cola = new Queue<(Transform, int)>();
                    cola.Enqueue((hijo, 0));
                    cartasAProcesar.Insert(0, hijo.gameObject);

                    while (cola.Count > 0)
                    {
                        var (actual, nivel) = cola.Dequeue();

                        if (nivel > profundidadMaxima)
                            continue;

                        // Copia temporal de hijos para evitar problemas al cambiar jerarqu�a
                        List<Transform> nietos = new List<Transform>();
                        foreach (Transform n in actual)
                        {
                            nietos.Add(n);
                        }

                        foreach (Transform nieto in nietos)
                        {
                            if (nieto.TryGetComponent<Carta>(out Carta carta))
                            {
                                if (carta.cartaEnum == EnumObjetivo)
                                {
                                    cartasAProcesar.Insert(0, nieto.gameObject);
                                    nieto.gameObject.transform.SetParent(transform);
                                }
                            }

                            if (nieto.childCount > 0 && nivel < profundidadMaxima)
                            {
                                cola.Enqueue((nieto, nivel + 1));
                            }
                        }
                    }
                    numHijos = transform.childCount;
                }
            }
            else 
            {
                    print("EL CANVAS SE IGNORA"); 
            }
            


            foreach (GameObject item in cartasAProcesar)
            {

                elapsed = 0;

                slider.gameObject.SetActive(true);

                while (elapsed < duration && fabricando)
                {

                    if (GameManager.EstaPausado != true)
                    {
                        elapsed += Time.deltaTime;
                        slider.value = Mathf.Clamp01(elapsed / duration);
                       
                    }
                    yield return null;
                }

                slider.gameObject.SetActive(false);

                //Instanciamos la leche
                GameObject leche = Instantiate(lecheOBJ);
                AudioManager.Instance.PlaySFX("Listo");
                Vector3 direccion = Vector3.down;
                    RaycastHit hit;
                    
                    leche.transform.position = transform.position + Vector3.up * 1f;
                    leche.transform.position = leche.transform.position + Vector3.right * 2f;
                    Vector3 origen = leche.transform.position;
                    Debug.DrawRay(origen, direccion * 5, Color.red, 2f);
                    if (Physics.Raycast(origen, Vector3.down, out hit, 5f))
                    {

                        if (hit.collider.GetComponent<Carta>() != null)
                        {
                             Debug.Log("Carta v�lida detectada debajo del hijo.");
                            leche.transform.SetParent(hit.collider.transform);
                        }
                        else
                        {
                            Debug.Log("No es una carta");
                        }
                    }
                    else
                    {
                        Debug.Log("No se detect� nada debajo del hijo.");
                    }
                    leche.transform.position = leche.transform.position + Vector3.down * 1f;





                //Destruimos el objeto necesario para crear el otro(el heno en caso de la leche)
                Destroy(item);
                numHijos--;
            }
        }

        cartasAProcesar.Clear();
        fabricando = false;
    }


    public void CancelarFusion()
    {
        fabricando=false;
        // Detener la coroutine si est� en ejecuci�n
        if (fusionCoroutine != null)
        {
            StopCoroutine(fusionCoroutine);
            fusionCoroutine = null;
        }

        // Obtener los sliders activos
        slider.value = 0;
        slider.gameObject.SetActive(false);
        cartasEnFusion.Remove(gameObject);
        fusionCancelada = false;
        cartasAProcesar.Clear();
        numHijos = 1; 
    }
}
