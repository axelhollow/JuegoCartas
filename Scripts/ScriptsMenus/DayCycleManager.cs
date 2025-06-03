using System;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static Carta;

public class DayCycleManager : MonoBehaviour
{
    [Header("Duración del día en segundos")]
    public float dayDuration;

    [Header("Referencias UI")]
    public Slider dayProgressSlider;
    public TextMeshProUGUI dayCounterText;

    [Header("Resumen del día")]
    public GameObject panelResumenDia;
    public TextMeshProUGUI textMonedas;
    public TextMeshProUGUI textObjetivo;
    public TextMeshProUGUI textResultado;
    public TextMeshProUGUI textoObjetivoActual; // Contador en tiempo real
    public TextMeshProUGUI monedasActuales;

    [Header("Canvas UI")]
    public Canvas canvasUI;

    private float timer = 0f;
    public int currentDay;
    private bool cumpliaObjetivoAntes = false; // <- Nuevo flag dinámico

    public event Action OnDayEnded;

    [Header("Objetivo del dia")]
    private int objetivo;

    [Header("Partículas")]
    public GameObject particulaDestruccionPrefab;

    [Header("Configuración")]
    public int maxDays = 100;
    public AnimationCurve curvaObjetivo;

    void Start()
    {
        UpdateDayUI();

        //le damos un valor al mx del slider
        if (dayProgressSlider != null)
            dayProgressSlider.maxValue = dayDuration;

        //desactivamos el panel del final del dia
        if (panelResumenDia != null)
            panelResumenDia.SetActive(false);
        //calcular el objevtivo del día 1
        CalcularObjetivoDelDia();
    }

    void Update()
    {
        if (GameManager.EstaPausado != true)
        {
            timer += Time.deltaTime;

            if (timer >= dayDuration)
            {
                timer = 0f;
                currentDay++;
                UpdateDayUI();
                OnDayEnded?.Invoke();
                ShowEndOfDaySummary();
                //OBJETIVO DEL DIA
                CalcularObjetivoDelDia();
            }

            if (dayProgressSlider != null)
                dayProgressSlider.value = timer;
        }
    }
    //Incrementar Dia
    void UpdateDayUI()
    {
        if (dayCounterText != null)
            dayCounterText.text = "Día " + currentDay;
    }
    void ObjetivoDelDia() 
    {
        switch (currentDay) 
        {
            case 1:
                {
                    objetivo = currentDay;
                    textoObjetivoActual.text = objetivo.ToString();
                    break;
                }
            case 2:
                {
                    objetivo = currentDay * 2;
                    textoObjetivoActual.text = objetivo.ToString(); 
                    break;
                }
        }
        
    
    }

    public void CalcularObjetivoDelDia()
    {

        float t = (float)(currentDay - 1) / (maxDays - 1); // Normaliza entre 0 y 1

        float valor = curvaObjetivo.Evaluate(t); ; // Evaluar la curva
        print(valor);
        objetivo = Mathf.RoundToInt(valor);
       
        textoObjetivoActual.text = objetivo.ToString();
    }

    void ShowEndOfDaySummary()
    {
        Time.timeScale = 0f;
        canvasUI.gameObject.SetActive(false);
        panelResumenDia.SetActive(true);


        //Carta[] todasLasCartas = GameObject.FindObjectsOfType<Carta>(true);

        //var cartasTierra = todasLasCartas
        //    .Where(c => c.cartaEnum == CardEnum.TierraCultivo && c.gameObject.activeInHierarchy)
        //    .ToList();

        int cantidadMonedas = int.Parse(monedasActuales.text);
        textMonedas.text = "Monedas: " + cantidadMonedas;
        textObjetivo.text = "Objetivo: " + objetivo + " monedas";

        if (cantidadMonedas >= objetivo)
        {
            textResultado.text = "Objetivo cumplido";

          
           int monedasAct= int.Parse(monedasActuales.text)-objetivo;
            monedasActuales.text=monedasAct.ToString();
        }
        //No cumples el objetivo diario PIERDES
        else
        {
            textResultado.text = "No cumpliste el objetivo";


        }



    }


    public void ContinuarJuego()
    {
        Time.timeScale = 1f;
        canvasUI.gameObject.SetActive(true);
        if (panelResumenDia != null)
            panelResumenDia.SetActive(false);
    }
}
