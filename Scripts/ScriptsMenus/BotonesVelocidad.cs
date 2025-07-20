using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class BotonesVelocidad : MonoBehaviour
{
    private Button botonPausa;
    private Button botonNormal;
    private Button botonRapido;

    public Color colorActivo;
    public Color colorInactivo = Color.white;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // Acción para la tecla 1
            Debug.Log("Pulsaste 1");
            JuegoEnPausa(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // Acción para la tecla 2
            Debug.Log("Pulsaste 2");
            SetTiempo(1f);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Acción para la tecla 3
            Debug.Log("Pulsaste 3");
            SetTiempo(2f);
        }
    }

    void Start()
    {
        botonPausa = GameObject.Find("BottonPause").GetComponent<Button>();
        botonNormal = GameObject.Find("BottonPlay").GetComponent<Button>();
        botonRapido = GameObject.Find("BottonX2").GetComponent<Button>();



        botonPausa.onClick.AddListener(() => JuegoEnPausa(true));
        botonNormal.onClick.AddListener(() => SetTiempo(1f));
        botonRapido.onClick.AddListener(() => SetTiempo(2f));

        ActualizarColores();
    }

    void SetTiempo(float escala)
    {
        JuegoEnPausa(false);
        Time.timeScale = escala;
        ActualizarColores();
    }

    public void JuegoEnPausa(bool pausa)
    {
        var textoPausa = botonPausa.GetComponentInChildren<TextMeshProUGUI>();
        GameManager.PausarJuego(pausa);

        if (pausa == true)
        {
            textoPausa.color = colorActivo;
            ActualizarColores();
        }
        else
        {
            textoPausa.color = colorInactivo;
            ActualizarColores();
        }
    }

    public void ActualizarColores()
    {
        // Obtener referencias al TextMeshProUGUI de cada botón
        var textoNormal = botonNormal.GetComponentInChildren<TextMeshProUGUI>();
        var textoRapido = botonRapido.GetComponentInChildren<TextMeshProUGUI>();

        // Colorear según la escala de tiempo actual
        if (Time.timeScale == 1f) 
        {
            textoNormal.color= colorActivo;
            textoRapido.color = colorInactivo;
        }
        if (Time.timeScale == 2f)
        {
            textoRapido.color =colorActivo;
            textoNormal.color = colorInactivo;
        }
        if (GameManager.EstaPausado == true) 
        {
            textoNormal.color = colorInactivo;
            textoRapido.color = colorInactivo;
        }
       
    }
}
