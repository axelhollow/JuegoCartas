using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComprarSobre : MonoBehaviour
{
    public GameObject SobrePrefab;
    public Vector3 posicion = new Vector3(-4.01999998f, -0.910000026f, 3.18000007f);

    public float radioDeteccion = 3f;      // Radio para detectar objetos cercanos
    public float distanciaEmpuje = 2f;     // Cuánto queremos empujar
    public float duracionEmpuje = 0.5f;    // Duración del empuje animado

    public TextMeshProUGUI monedas;
    public int costeSobre;
    private int cantidadMonedas;

    [Header("Partículas")]
    public GameObject particulaDestruccionPrefab;


    private void Start()
    {
        cantidadMonedas = int.Parse(monedas.text);
    }
    void OnMouseDown()
    {
        if (TryCompra()==true) 
        {
            //cobramos el coste
            restarDinero();

            // Empujar si hay un SOBRE en el lugar exacto
            Collider[] objetosEnPosicion = Physics.OverlapSphere(posicion, 0.5f);
            foreach (Collider col in objetosEnPosicion)
            {
                if (col.CompareTag("Sobre"))
                {
                    Vector3 direccionEmpuje = (col.transform.position - posicion).normalized;
                    if (direccionEmpuje == Vector3.zero) direccionEmpuje = Vector3.right;
                    StartCoroutine(EmpujarSuave(col.transform, direccionEmpuje * distanciaEmpuje, duracionEmpuje));
                }
            }

            // Empujar si hay un DRAGGABLE justo en la posición
            foreach (Collider col in objetosEnPosicion)
            {
                if (col.CompareTag("Draggable"))
                {
                    Vector3 direccionEmpuje = (col.transform.position - posicion).normalized;
                    if (direccionEmpuje == Vector3.zero) direccionEmpuje = Vector3.right;
                    StartCoroutine(EmpujarSuave(col.transform, direccionEmpuje * distanciaEmpuje, duracionEmpuje));
                }
            }

            // Instanciar el objeto en la posición deseada
            AudioManager.Instance.PlaySFX("Dinero");

            LeanTween.scale(monedas.gameObject, Vector3.one * 1.2f, 0.15f).setEaseOutBack().setOnComplete(() =>
            {
                LeanTween.scale(monedas.gameObject, Vector3.one, 0.15f).setEaseInBack();
            });

            GenerarParticula(posicion);
            var sobre = Instantiate(SobrePrefab);
            sobre.transform.position = posicion;


            // Empujar objetos DRAGGABLE cercanos (como antes)
            Collider[] objetosCercanos = Physics.OverlapSphere(posicion, radioDeteccion);
            foreach (Collider col in objetosCercanos)
            {
                if (col.CompareTag("Draggable") && col.gameObject != sobre)
                {
                    Vector3 direccionEmpuje = (col.transform.position - posicion).normalized;
                    StartCoroutine(EmpujarSuave(col.transform, direccionEmpuje * distanciaEmpuje, duracionEmpuje));
                }
            }
        }
    }
    void GenerarParticula(Vector3 posicion)
    {
        if (particulaDestruccionPrefab != null)
        {
            var instancia = Instantiate(particulaDestruccionPrefab, posicion, Quaternion.identity);
            Destroy(instancia, 2f);
        }
    }
    public void restarDinero() 
    {
        cantidadMonedas=cantidadMonedas-costeSobre;
        monedas.text=cantidadMonedas.ToString();
    }

    public bool TryCompra() 
    {
        cantidadMonedas = int.Parse(monedas.text);
        if (cantidadMonedas - costeSobre > -1)
        {
            return true;
        }
        else 
        {
            print("FALSE");
            AudioManager.Instance.PlaySFX("Error");
            return false;
        }
     

    }

    IEnumerator EmpujarSuave(Transform objeto, Vector3 desplazamiento, float duracion)
    {
        Vector3 posicionInicial = objeto.position;
        Vector3 posicionObjetivo = posicionInicial + desplazamiento;

        float tiempo = 0f;
        while (tiempo < duracion)
        {
            objeto.position = Vector3.Lerp(posicionInicial, posicionObjetivo, tiempo / duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }
        objeto.position = posicionObjetivo;
    }
}
