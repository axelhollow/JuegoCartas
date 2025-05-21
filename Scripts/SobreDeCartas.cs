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

    void OnMouseDown()
    {
        puntoBase=gameObject.transform.position;

        Debug.Log("¡Me clicaron!: " + gameObject.name);
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
