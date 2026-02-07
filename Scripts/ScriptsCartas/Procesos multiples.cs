using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Es como el script de maquina produccion pero a este se le puede poner diferentes cartas para obtener diferentes resultados.
/// </summary>
public class Procesosmultiples : MonoBehaviour
{
    public int numHijos;
    public GameObject obj_producido;
    public List<CardEnum> ListaObjetosAceptados;
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

        // Guardamos una copia de los hijos originales antes de modificar la jerarquia
        List<Transform> hijosOriginales = new List<Transform>();
        foreach (Transform hijo in transform)
        {
            hijosOriginales.Add(hijo);
        }

        foreach (Transform hijo in hijosOriginales)
        {
            // Hijo correcto
            if (hijo.GetComponent<CartasJson>() != null)
            {
                if (ListaObjetosAceptados.Contains(hijo.GetComponent<CartasJson>().cartaEnum))
                {
                    Queue<(Transform nodo, int nivel)> cola = new Queue<(Transform, int)>();
                    cola.Enqueue((hijo, 0));
                    cartasAProcesar.Insert(0, hijo.gameObject);

                    while (cola.Count > 0)
                    {
                        var (actual, nivel) = cola.Dequeue();

                        if (nivel > profundidadMaxima)
                            continue;

                        // Copia temporal de hijos para evitar problemas al cambiar jerarquia
                        List<Transform> nietos = new List<Transform>();
                        foreach (Transform n in actual)
                        {
                            nietos.Add(n);
                        }

                        foreach (Transform nieto in nietos)
                        {
                            if (nieto.TryGetComponent<CartasJson>(out CartasJson carta))
                            {
                                if (ListaObjetosAceptados.Contains(carta.cartaEnum))
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

            


                foreach (GameObject item in cartasAProcesar)
                {
                    for(int i=0;i<2;i++)
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

                        slider.value = 0;
                        elapsed = 0;

                        //Instanciamos la leche
                        GameObject leche = Instantiate(obj_producido); //cambiar por una lista
                        AudioManager.Instance.PlaySFX("Listo");
                        Vector3 direccion = Vector3.down;
                            RaycastHit hit;
                            
                            leche.transform.position = transform.position + Vector3.up * 1f;
                            leche.transform.position = leche.transform.position + Vector3.right * 2f;
                            Vector3 origen = leche.transform.position;
                            Debug.DrawRay(origen, direccion * 5, Color.red, 2f);
                            if (Physics.Raycast(origen, Vector3.down, out hit, 5f))
                            {

                                if (hit.collider.GetComponent<CartasJson>() != null)
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
                            gameObject.GetComponent<BoxCollider>().enabled=false;   
                            item.SetActive(false);    
         
                        }
                                    
                numHijos--;
                Destroy(item);    
                gameObject.GetComponent<BoxCollider>().enabled=true;  

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

