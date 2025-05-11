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
    public Slider slider;
    public bool fabricando=false;
    public List<GameObject> cartasAProcesar = new List<GameObject>();

    //Cancelar Fusion
    public bool fusionCancelada = false;
    public Coroutine fusionCoroutine = null;  // Referencia a la coroutine activa para poder cancelarla
    public HashSet<GameObject> cartasEnFusion = new HashSet<GameObject>();

    //Nivel De Hijos
    public int profundidadMaxima = 99;

    private void Awake()
    {
        numHijos = transform.childCount;
        print("Hijos vaca: "+ numHijos);
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(numHijos<1)numHijos = 1;
        if (transform.childCount > numHijos && fabricando==false) 
        {
            
            
            fusionCoroutine = StartCoroutine("procesarHijos");

        }
        if(transform.childCount < numHijos && fabricando==true) 
        {
            print("cancelar fusion");
            CancelarFusion();
        }

        
    }



    IEnumerator procesarHijos()
    {
        fabricando = true;

        // Guardamos una copia de los hijos originales antes de modificar la jerarquía
        List<Transform> hijosOriginales = new List<Transform>();
        foreach (Transform hijo in transform)
        {
            hijosOriginales.Add(hijo);
        }

        foreach (Transform hijo in hijosOriginales)
        {
            // Hijo correcto
            if (hijo.GetComponent<Carta>() != null && hijo.GetComponent<Carta>().cartaEnum == CardEnum.Heno)
            {
                print("heno padre: " + hijo.name + " num hijos: " + hijo.transform.childCount);
                Queue<(Transform nodo, int nivel)> cola = new Queue<(Transform, int)>();
                cola.Enqueue((hijo, 0));
                cartasAProcesar.Insert(0, hijo.gameObject);

                while (cola.Count > 0)
                {
                    var (actual, nivel) = cola.Dequeue();

                    if (nivel > profundidadMaxima)
                        continue;

                    // Copia temporal de hijos para evitar problemas al cambiar jerarquía
                    List<Transform> nietos = new List<Transform>();
                    foreach (Transform n in actual)
                    {
                        nietos.Add(n);
                    }

                    foreach (Transform nieto in nietos)
                    {
                        if (nieto.TryGetComponent<Carta>(out Carta carta))
                        {
                            if (carta.cartaEnum == CardEnum.Heno)
                            {
                                cartasAProcesar.Insert(0, nieto.gameObject);
                                nieto.gameObject.transform.SetParent(transform);
                                print(nieto.gameObject.name);
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
            else 
            {
                if (hijo.GetComponent<Canvas>() != null)
                {

                    print("EL CANVAS SE IGNORA");
                }
                else 
                {

                    hijo.SetParent(null);
                    hijo.transform.position = transform.position + Vector3.up * 1f;
                    hijo.transform.position = hijo.transform.position + Vector3.left * 1f;
                    Vector3 direccion = Vector3.down;
                    RaycastHit hit;
                    Vector3 origen = hijo.position;
                    Debug.DrawRay(origen, direccion * 5, Color.red, 2f);
                    if (Physics.Raycast(origen, Vector3.down, out hit, 5f))
                    {
                        
                        if (hit.collider.GetComponent<Carta>() != null)
                        {
                            Debug.Log("Carta válida detectada debajo del hijo.");
                            hijo.SetParent(hit.collider.transform);
                        }
                        else
                        {
                            Debug.Log("No es una carta");
                        }
                    }
                    else
                    {
                        Debug.Log("No se detectó nada debajo del hijo.");
                    }
                    hijo.transform.position = hijo.transform.position + Vector3.up * -1f;
                    numHijos--;
                }
                
            }
            


            foreach (GameObject item in cartasAProcesar)
            {
 

                float duration = 1f;
                float elapsed = 0f;
                slider.gameObject.SetActive(true);

                while (elapsed < duration && fabricando)
                {
                    elapsed += Time.deltaTime;
                    slider.value = Mathf.Clamp01(elapsed / duration);
                    yield return null;
                }

                slider.gameObject.SetActive(false);

                //Instanciamos la leche
                GameObject leche = Instantiate(lecheOBJ);

                    Vector3 direccion = Vector3.down;
                    RaycastHit hit;
                    
                    leche.transform.position = transform.position + Vector3.up * 1f;
                    leche.transform.position = leche.transform.position + Vector3.right * 1f;
                    Vector3 origen = leche.transform.position;
                    Debug.DrawRay(origen, direccion * 5, Color.red, 2f);
                    if (Physics.Raycast(origen, Vector3.down, out hit, 5f))
                    {
                        
                        if (hit.collider.GetComponent<Carta>() != null)
                        {
                            Debug.Log("Carta válida detectada debajo del hijo.");
                            leche.transform.SetParent(hit.collider.transform);
                        }
                        else
                        {
                            Debug.Log("No es una carta");
                        }
                    }
                    else
                    {
                        Debug.Log("No se detectó nada debajo del hijo.");
                    }
                    leche.transform.position = leche.transform.position + Vector3.down * 1f;





                //Destruimos el Heno
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
        // Detener la coroutine si está en ejecución
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
