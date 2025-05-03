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
    public float alturaEntreHijos = 0.1f;
    public float escaleraOffset = -0.3f;
    public GameObject morado;
    public int numHijos;
    public GameObject hijo;


    private void Awake()
    {
        numHijos = gameObject.transform.childCount;
    }
    private void Update()
    {
        //Acomodarr Hijos
        if (gameObject.transform.childCount > 1)
        {

            //Ordenar a los hijos
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform hijo = transform.GetChild(i);
                Vector3 nuevaPos = transform.position;

                nuevaPos.y += alturaEntreHijos * (i + 1);           // Apila en altura
                nuevaPos.z += escaleraOffset * (i + 1);             // Desplaza hacia atrás (como escalera)

                hijo.position = nuevaPos;
            }
        }


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

        
    }
    IEnumerator FusionarConSlider(GameObject carta1, GameObject carta2)
    {

        float duration = 9f;
        float elapsed = 0f;

        Carta cartaComp1 = carta1.GetComponent<Carta>();
        Carta cartaComp2 = carta2.GetComponent<Carta>();

        Slider slider = cartaComp1.slider;
        slider.value = 0f;
        slider.gameObject.SetActive(true); // Aseguramos que esté visible

        while (elapsed < duration)
        {
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

            elapsed += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        slider.value = 1f;

        // Si no se canceló, fusionamos las cartas
        Vector3 posicion = carta1.transform.position;

        Destroy(carta1);
        Destroy(carta2);

        GameObject nuevaCarta = Instantiate(morado);
        nuevaCarta.transform.position = posicion;

        cartasEnFusion.Remove(carta1);
        cartasEnFusion.Remove(carta2);


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
                Carta cartaComp = carta.GetComponent<Carta>();
                if (cartaComp != null && cartaComp.slider != null)
                {
                    cartaComp.slider.value = 0f;
                    cartaComp.slider.gameObject.SetActive(false);
                }

                // Volver a activar las cartas para que estén disponibles para futuras fusiones
                carta.SetActive(true);
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
        Morado
    }
}
