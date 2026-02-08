using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Esta en el boton del menu continuar juego del menu fin del dia
/// </summary>
public class Enfermedad : MonoBehaviour
{
    public bool estaEnfermo = false;
    public GameObject logoEnfermo;


    private void Update()
    {
        if (estaEnfermo == true)
        {
            logoEnfermo.gameObject.SetActive(true);

            var scriptMaquina = gameObject.GetComponent<MaquinaProduccion>();
            if (scriptMaquina != null)
            {
                print("Deja de producir");
                Destroy(scriptMaquina);

            }
        }
    }

    public void PonerEnfermo()
    {

        // Generar un número aleatorio entre 1 y 5 (incluye el 5)
        int numero = Random.Range(1, 6);

        print("El número generado es: " + numero);



        if (numero == 1)
        {
            print("¡Salió el 1!");
            estaEnfermo = true;

        }
        else
        {

            print("¡Salió el " + numero);
        }

    }





}
