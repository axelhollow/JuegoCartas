using System;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static CartasJson;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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
    public TextMeshProUGUI objetivoCartaTXT;
    public TextMeshProUGUI textResultado;
    public TextMeshProUGUI textoObjetivoActual; // Contador en tiempo real
    public TextMeshProUGUI monedasActuales;
    RectTransform recResultadoText;
    private Vector2 posicionGuardada;
    RectTransform recCartasText;


    [Header("Canvas UI")]
    public Canvas canvasUI;

    private float timer = 0f;
    public int currentDay;


    public event Action OnDayEnded;

    [Header("Objetivo del dia")]
    private int objetivo;

    [Header("Partículas")]
    public GameObject particulaDestruccionPrefab;

    [Header("Configuración")]
    public int maxDays = 100;
    public AnimationCurve curvaObjetivo;

    [Header("CartaOjetivo")]
    public Image imagenUI;           // Asigna en el Inspector
    private Sprite nuevoSprite;       // Asigna en el Inspector
    public CardEnum objetivoCarta;
    public CartaObjetivo[] listaCartasObjetivo;

    [Header("Botones")]
    public GameObject BottonPause;
    public GameObject BottonPlay;
    public GameObject BotonX2;


    float tiempodediausado;

    void Start()
    {

        currentDay=int.Parse(PlayerPrefs.GetString("NumDia", "1").Replace("Día","").Trim());

        tiempodediausado = PlayerPrefs.GetFloat("DiaBarra", 0f);

        recResultadoText = textResultado.GetComponent<RectTransform>();
        posicionGuardada = textResultado.GetComponent<RectTransform>().anchoredPosition;
        recCartasText = objetivoCartaTXT.GetComponent<RectTransform>();


        objetivoCartaTXT.gameObject.SetActive(false);
        imagenUI.gameObject.SetActive(false);
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

            if (timer >= dayDuration || timer+tiempodediausado>=dayDuration)
            {
                tiempodediausado = 0f;
                timer = 0f;
                currentDay++;
                UpdateDayUI();
                OnDayEnded?.Invoke();
                ShowEndOfDaySummary();
                //OBJETIVO DEL DIA
                CalcularObjetivoDelDia();
            }

            if (dayProgressSlider != null)
                dayProgressSlider.value = tiempodediausado+timer;

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
        objetivo = Mathf.RoundToInt(valor);
        textoObjetivoActual.text = objetivo.ToString();

        if (currentDay >= 2 && currentDay <= 3)
        {
            objetivoCarta = CardEnum.Vaca;
            nuevoSprite = Resources.Load<Sprite>($"Sprites/UI/CartasObjetivo/{objetivoCarta.ToString()}");
            imagenUI.sprite = nuevoSprite;
            imagenUI.gameObject.SetActive(true);
        }
        else 
        {
            objetivoCarta = CardEnum.Rojo;
            imagenUI.gameObject.SetActive(false);
        }


    }

    public bool ComprobarSiTieneLaCarta(CardEnum cartaObjetivoEnum) 
    {

        listaCartasObjetivo = GameObject.FindObjectsOfType<CartaObjetivo>(true);
        var cartasObjetivo = listaCartasObjetivo.Where(c => c.carta == cartaObjetivoEnum && c.gameObject.activeInHierarchy).ToList();
        print("Carta objetivo: " + cartaObjetivoEnum);
        if (cartasObjetivo != null)
        {
            print("tiene la carta");
            return true;
        }
        return false;
    }

    void ShowEndOfDaySummary()
    {
        Time.timeScale = 0f;
        canvasUI.gameObject.SetActive(false);
        panelResumenDia.SetActive(true);


        int cantidadMonedas = int.Parse(monedasActuales.text);
        textMonedas.text = "Objetivos:";
        textObjetivo.text = objetivo + " monedas";

        if(objetivoCarta != CardEnum.Rojo) 
        {

            if (ComprobarSiTieneLaCarta(objetivoCarta)==true && cantidadMonedas >= objetivo)
            {
                print("ganaste");
                printVictoria(true);
            }
            else 
            {
                textResultado.text = "No cumpliste el objetivo";
                recResultadoText.anchoredPosition = posicionGuardada;

                objetivoCartaTXT.gameObject.SetActive(true);
                objetivoCartaTXT.text = $"1 carta de {objetivoCarta}";

            }
        }
        if(objetivoCarta == CardEnum.Rojo)
        {
            if (cantidadMonedas >= objetivo)
            {
                printVictoria(false);
            }
            else 
            {
                objetivoCartaTXT.gameObject.SetActive(false);
                textResultado.text = "No cumpliste el objetivo";
                recResultadoText.anchoredPosition = recCartasText.anchoredPosition;

            }
        }
    }

    void printVictoria(bool carta) 
    {
        if (carta==false)
        {
            objetivoCartaTXT.gameObject.SetActive(false);
            textResultado.text = "Objetivo cumplido";
            recResultadoText.anchoredPosition = recCartasText.anchoredPosition;
        }
        else
        {
            recResultadoText.anchoredPosition = posicionGuardada;
            textResultado.text = "Objetivo cumplido";
            objetivoCartaTXT.gameObject.SetActive(true);
            objetivoCartaTXT.text = $"1 carta de {objetivoCarta}";
        }
    }

    public void ContinuarJuego()
    {
        //Dejar como boton activo el x1

        Color colorVerdeClaro = new Color(142f / 255f, 230f / 255f, 142f / 255f);


        BottonPause.GetComponentInChildren<TextMeshProUGUI>().color=Color.white;
        BottonPlay.GetComponentInChildren<TextMeshProUGUI>().color = colorVerdeClaro;
        BotonX2.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;


        Time.timeScale = 1f;
        canvasUI.gameObject.SetActive(true);
        if (panelResumenDia != null)
            panelResumenDia.SetActive(false);
    }
}
