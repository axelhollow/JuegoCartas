using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    public int profundidadMaxima = 99;

    private void Awake()
    {
        numHijos=transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.childCount > numHijos && fabricando==false) 
        {
            StartCoroutine("procesarHijos");

        }
        
    }

    IEnumerator procesarHijos() 
    {
        fabricando = true;
        foreach (Transform hijo in transform)
        {
            //Hijo correcto
            if (hijo.GetComponent<Carta>() != null && hijo.GetComponent<Carta>().cartaEnum == CardEnum.Heno)
            {
                Queue<(Transform nodo, int nivel)> cola = new Queue<(Transform, int)>();
                cola.Enqueue((hijo, 0));
                cartasAProcesar.Insert(0, hijo.gameObject);
                print(hijo.gameObject.name);
                while (cola.Count > 0)
                {
                    var (actual, nivel) = cola.Dequeue();

                    if (nivel > profundidadMaxima)
                        continue;

                    foreach (Transform nieto in actual)
                    {
                        // Verificamos si tiene el componente Carta
                        if (nieto.TryGetComponent<Carta>(out Carta carta))
                        {
                            if (carta.cartaEnum == CardEnum.Heno)
                            {
                                // Insertar al principio para que los últimos aparezcan primero
                                cartasAProcesar.Insert(0, nieto.gameObject);
                            }
                        }

                        if (nieto.childCount > 0 && nivel < profundidadMaxima)
                        {
                            cola.Enqueue((nieto, nivel + 1));
                        }
                    }
                }

            }


            foreach (GameObject item in cartasAProcesar)
            {

                Vector3 posicionLeche = item.transform.position;
                posicionLeche.x += 1;
                posicionLeche.z -= 1;


                //Slider
                float duration = 3f;
                float elapsed = 0f;
                slider.gameObject.SetActive(true);
                while (elapsed < duration)
                {
                    elapsed += Time.deltaTime;
                    slider.value = Mathf.Clamp01(elapsed / duration);
                    yield return null;
                }
                slider.gameObject.SetActive(false);

                //Destruccion y creacion
                Instantiate(lecheOBJ);
                lecheOBJ.transform.position = posicionLeche;
                Destroy(item);
            }
 

        }
        cartasAProcesar.Clear();
        fabricando = false;
    }
}
