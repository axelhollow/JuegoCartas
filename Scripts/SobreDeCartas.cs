using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SobreDeCartas : MonoBehaviour
{

    [SerializeField]
    List<GameObject> listaCartas = new List<GameObject>();
    private Vector3 puntoBase;          // Punto base fijo (usaremos Y y Z de este)
    public float rangoX;          // Rango máximo para aleatorizar X (+- rangoX)
    public float rangoZ;

    private float tiempoUltimoClick = 0f;
    public float tiempoEntreClicks = 0.3f; // Tiempo máximo entre clics para considerarlo doble clic
    void OnMouseDown()
    {
        if (Time.time - tiempoUltimoClick < tiempoEntreClicks)
        {
            // Doble clic detectado
            puntoBase = gameObject.transform.position;
            int tamanoLista = listaCartas.Count;
            for (int i = 1; i < 4; i++)
            {
                int numeroAleatorio = UnityEngine.Random.Range(0, tamanoLista);
                GameObject carta = listaCartas[numeroAleatorio];
                if (carta != null)
                {
                    InstanciarEnXaleatorio(carta);
                }

            }
            Destroy(gameObject);
        }

        tiempoUltimoClick = Time.time;
    }

    void InstanciarEnXaleatorio(GameObject carta)
    {
        // Genera un valor aleatorio en X entre -rangoX y +rangoX
        float xAleatorio = UnityEngine.Random.Range(puntoBase.x - rangoX, puntoBase.x + rangoX);
        float zAleatorio = UnityEngine.Random.Range(puntoBase.z - rangoZ, puntoBase.z + rangoZ);

        Vector3 posicion = new Vector3(xAleatorio, puntoBase.y, zAleatorio);

        // Instancia el prefab en la posición con rotación por defecto
        Instantiate(carta, posicion, Quaternion.identity);
    }
}
