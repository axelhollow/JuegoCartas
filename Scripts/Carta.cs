using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Carta : MonoBehaviour
{

    public CardEnum cartaEnum;
    public Slider slider;
    public HashSet<GameObject> cartasEnFusion = new HashSet<GameObject>();
    public Coroutine fusionCoroutine = null;  // Referencia a la coroutine activa para poder cancelarla
    public bool fusionCancelada = false;

    public GameObject ResultadoFusion;
    public int numHijos;
    public GameObject hijo;


    private void Awake()
    {
        numHijos = gameObject.transform.childCount;
    }
    private void Update()
    {

            //Fusion
            if (gameObject.transform.childCount > numHijos)
            {
                numHijos = gameObject.transform.childCount;
                foreach (Transform child in transform)
                {
                    hijo = child.gameObject;

                    if (hijo.tag == "Draggable" && !cartasEnFusion.Contains(hijo))
                    {
                        cartasEnFusion.Add(gameObject); // Añadir las cartas al HashSet
                        cartasEnFusion.Add(hijo);
                        fusionCoroutine = StartCoroutine(FusionarConSlider(gameObject, hijo)); // Iniciar la fusión
                        break;
                    }

                }
            }
        

        //CancelarFusion

        if(gameObject.transform.childCount < numHijos) 
        {
            CancelarFusion();
        }

        if (transform.childCount > 1 && transform.parent!=null) 
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Carta>() != null) 
                {
                    child.transform.SetParent(transform.parent.transform);
                
                }
                
            }
        }
      
    }
    IEnumerator FusionarConSlider(GameObject carta1, GameObject carta2)
    {

        float duration = 1f;
        float elapsed = 0f;
        Carta cartaComp1 = carta1.GetComponent<Carta>();
        Carta cartaComp2 = carta2.GetComponent<Carta>();
        Slider slider = cartaComp1.slider;
        CardEnum? tipoFusion=null;
        //comprobar que fusion es
        if (cartaComp1.cartaEnum == CardEnum.Azul &&  cartaComp2.cartaEnum == CardEnum.Rojo || cartaComp1.cartaEnum == CardEnum.Rojo && cartaComp2.cartaEnum == CardEnum.Azul)
        {
            tipoFusion=CardEnum.Morado;
            ResultadoFusion = cartaComp2.ResultadoFusion;
        }
        if (cartaComp1.cartaEnum == CardEnum.TierraCultivo && cartaComp2.cartaEnum == CardEnum.Semilla)
        {
            tipoFusion = CardEnum.Heno;
            duration = 5f;
            ResultadoFusion = cartaComp2.ResultadoFusion;
        }

        if (tipoFusion != null)
        {
            slider.value = 0f;
            slider.gameObject.SetActive(true); // Aseguramos que esté visible

            while (elapsed < duration)
            {

                    #region FusionCancelada
                    if (fusionCancelada) // Si la fusión ha sido cancelada, detenemos la coroutine
                    {
                        // Restaurar el slider y las cartas
                        slider.value = 0f;
                        slider.gameObject.SetActive(false); // Hacemos invisible el slider
                        carta1.SetActive(true); // Reactivamos la carta 1
                        carta2.SetActive(true); // Reactivamos la carta 2

                        cartasEnFusion.Remove(carta1); // Eliminar las cartas del HashSet si se canceló la fusión
                        cartasEnFusion.Remove(carta2);

                        yield break; // Salir de la coroutine si se cancela
                    }
                #endregion

                if (GameManager.EstaPausado != true)
                {
                    elapsed += Time.deltaTime;
                    slider.value = Mathf.Clamp01(elapsed / duration);
                }
                    yield return null;
                
            }

            slider.value = 1f;

            #region fusiones

            //guardar la posicion
            Vector3 posicion = carta1.transform.position;

                Destroy(carta1);
                Destroy(carta2);

                GameObject nuevaCarta = Instantiate(ResultadoFusion);
                nuevaCarta.transform.position = posicion;

                cartasEnFusion.Remove(carta1);
                cartasEnFusion.Remove(carta2);

            #endregion
        }
        else 
        {
            //NO HAY FUSION PARA ESA CONVINACION
        }


    }

    public void CancelarFusion()
    {
        fusionCancelada = true;

        // Detener la coroutine si está en ejecución
        if (fusionCoroutine != null)
        {
            StopCoroutine(fusionCoroutine);
            fusionCoroutine = null;
        }

        // Obtener los sliders activos
        if (cartasEnFusion != null && cartasEnFusion.Count() > 0)
        {
            foreach (GameObject carta in cartasEnFusion)
            {
                try
                {
                    Carta cartaComp = carta.GetComponent<Carta>();
                    if (cartaComp != null && cartaComp.slider != null)
                    {
                        cartaComp.slider.value = 0f;
                        cartaComp.slider.gameObject.SetActive(false);
                    }

                    // Volver a activar las cartas para que estén disponibles para futuras fusiones
                    carta.SetActive(true);
                }
                catch(MissingReferenceException ex) 
                {
                    print("Carta: " + gameObject.name + " destruida "+ex);
                }
            }
        }

        numHijos = numHijos-1;
        cartasEnFusion.Remove(hijo);
        cartasEnFusion.Remove(gameObject);
        hijo = null;    
        fusionCancelada = false;
    }
    public enum CardEnum
    {
        Rojo,
        Azul,
        Morado,
        Heno,
        Leche,
        Queso,
        Semilla,
        TierraCultivo,
        Moneda,
        Vaca
    }
}
