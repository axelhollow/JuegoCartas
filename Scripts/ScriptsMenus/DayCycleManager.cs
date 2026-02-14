using System;
using System.Linq;
using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static CartasJson;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using System.Collections.Generic;

public class DayCycleManager : MonoBehaviour
{
    [Header("Duración del día en segundos")]
    public float dayDuration;

    [Header("Referencias UI")]
    public Slider dayProgressSlider;
    public TextMeshProUGUI dayCounterText;

    [Header("Resumen del día")]

    public TextMeshProUGUI textMonedas;
    public TextMeshProUGUI textObjetivo;
    public TextMeshProUGUI objetivoCartaTXT;
    public TextMeshProUGUI textResultado;
    public TextMeshProUGUI textoObjetivoActual; // Contador en tiempo real
    public TextMeshProUGUI monedasActuales;
    public bool objetivoActivado=false;
    RectTransform recResultadoText;
    private Vector2 posicionGuardada;
    RectTransform recCartasText;


    [Header("Canvas UI")]
    public Canvas canvasUI;
        [Header("Canvas Victoria")]
    public GameObject panelResumenDia;

            [Header("Canvas Derrota")]
    public Canvas canvasDerrota;
[Header("Dia")]
    public float timer = 0f;
    public int currentDay;

    [Header("Estacion")]
    public int estacionActual=1;
    public int diaEstacion=1;
    public GameObject textoEstacion;
    public List<GameObject> listacartasPrimavera;

    public List<GameObject> listacartasVerano;
    public List<GameObject> listacartasOtonyo;
   public List<GameObject> listacartasInvierno;

    public GameObject EstacionFill;

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



    public void ReseteDeMisionesYEstaciones()
    {
                imagenUI.gameObject.SetActive(false);
                currentDay=1;
                UpdateDayUI();
                CalcularObjetivoDelDia();
                timer=0;
                tiempodediausado=0;

    }

    void Start()
    {

        currentDay=int.Parse(PlayerPrefs.GetString("NumDia", "1").Replace("Día","").Trim());

        tiempodediausado = PlayerPrefs.GetFloat("DiaBarra", 0f);

       // recResultadoText = textResultado.GetComponent<RectTransform>();
        //posicionGuardada = textResultado.GetComponent<RectTransform>().anchoredPosition;
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

        if(canvasDerrota!=null)
        {
            canvasDerrota.gameObject.SetActive(false);
        }
        //calcular el objevtivo del día 1
        CalcularObjetivoDelDia();
    }

    void Update()
    {
        if (GameManager.EstaPausado != true)
        {
            timer += Time.deltaTime;

            if (timer >= dayDuration || timer + tiempodediausado >= dayDuration)
            {
                tiempodediausado = 0f;
                timer = 0f;
                currentDay++;
                UpdateDayUI();
                DiaEstacionUpdate();
                OnDayEnded?.Invoke();
                ShowEndOfDaySummary();
                //OBJETIVO DEL DIA
                 objetivoActivado=false;
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
            dayCounterText.text = ""+currentDay;
    }

    void DiaEstacionUpdate()
    {
        diaEstacion++;
        if (diaEstacion > 3)
        {
            diaEstacion = 1;
            EstacionUpdate();
        }
        if (estacionActual == 1)
        {
           // SpawnDeEstcion(listacartasPrimavera);
        }
        if (estacionActual == 2)
        {
            //SpawnDeEstcion(listacartasPrimavera);
        }
       
    }

    #region estaciones
    

    void EstacionUpdate()
    {
        estacionActual++;
        if (estacionActual > 4)
        {
            estacionActual = 1;
        }

        Image image = EstacionFill.GetComponent<Image>();
        Color color;
        switch (estacionActual)
        {
            case 1:
                if (ColorUtility.TryParseHtmlString("#8EE68E", out color))
                {
                    if(LanguageManager.Instance.idiomaActual == "es")
                    {
                        textoEstacion.GetComponent<TextMeshProUGUI>().text = "Primavera";
                    }
                    if(LanguageManager.Instance.idiomaActual == "en")
                    {
                         textoEstacion.GetComponent<TextMeshProUGUI>().text = "Spring";
                    }
                    image.color = color;
                }
                break;
            case 4:
                if (ColorUtility.TryParseHtmlString("#ffd445ff", out color))
                {
                    if(LanguageManager.Instance.idiomaActual == "es")
                    {
                        textoEstacion.GetComponent<TextMeshProUGUI>().text = "Verano";
                    }
                    if(LanguageManager.Instance.idiomaActual == "en")
                    {
                         textoEstacion.GetComponent<TextMeshProUGUI>().text = "Summer";
                    }
                    image.color = color;
                }
                break;
            case 3:
                if (ColorUtility.TryParseHtmlString("#ff932fff", out color))
                {
                    if(LanguageManager.Instance.idiomaActual == "es")
                    {
                        textoEstacion.GetComponent<TextMeshProUGUI>().text = "Otoño";
                    }
                    if(LanguageManager.Instance.idiomaActual == "en")
                    {
                         textoEstacion.GetComponent<TextMeshProUGUI>().text = "Autumn";
                    }
                    image.color = color;
                }
                break;
            case 2:

                if (ColorUtility.TryParseHtmlString("#4BB2D9", out color))
                {
                    if(LanguageManager.Instance.idiomaActual == "es")
                    {
                        textoEstacion.GetComponent<TextMeshProUGUI>().text = "Invierno";
                    }
                    if(LanguageManager.Instance.idiomaActual == "en")
                    {
                         textoEstacion.GetComponent<TextMeshProUGUI>().text = "Winter";
                    }
                    image.color = color;
                }
                break;
        }

    }
#endregion
    void SpawnDeEstcion(List<GameObject> listacartas)
    {
        int numero = UnityEngine.Random.Range(1, 3);
        print("Sale: " + numero);
        if (numero == 2)
        {
            print("Generando recurso");
            int longilist = listacartas.Count;
            int cartaElegida = UnityEngine.Random.Range(0, longilist);
            GameObject recursoEstcion = listacartas[cartaElegida];


            Vector3 posicion1 = new Vector3(-7.65f, -0.8450004f, -3.5F);
            Vector3 posicion2 = new Vector3(-6.99047f, -0.8150005f, 3.35f);
            Vector3 posicion3 = new Vector3(7.65f, -0.8150005f, 3.35f);
            Vector3 posicion4 = new Vector3(7.65f, -0.8150005f, -3.35f);
            Vector3 posicion5 = new Vector3(0.13f, -0.845f, 0.42f);

            Vector3[] posiciones = new Vector3[5];
            posiciones[0] = posicion1;
            posiciones[1] = posicion2;
            posiciones[2] = posicion3;
            posiciones[3] = posicion4;
            posiciones[4] = posicion5;
            int intPosicion = UnityEngine.Random.Range(1, 5);

            Instantiate(recursoEstcion, posiciones[intPosicion], recursoEstcion.transform.rotation);
        }
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

        #region Objetivos_diarios
        if (currentDay == 2)
        {
            objetivoCarta = CardEnum.Heno;
            if (LanguageManager.Instance.idiomaActual == "es")
            {
                nuevoSprite = Resources.Load<Sprite>($"Sprites/UI/CartasObjetivo/{objetivoCarta.ToString()}");
                print("Objetivo: "+objetivoCarta.ToString());
                objetivoActivado=true;
            }
           /* if (LanguageManager.Instance.idiomaActual == "en")
            {
                nuevoSprite = Resources.Load<Sprite>($"Sprites/UI/CartasObjetivoEng/{objetivoCarta.ToString()}");

            }*/
            imagenUI.sprite = nuevoSprite;
            imagenUI.gameObject.SetActive(true);
        }
        if (currentDay == 4)
        {
            objetivoCarta = CardEnum.Vaca;
            if (LanguageManager.Instance.idiomaActual == "es")
            {
                nuevoSprite = Resources.Load<Sprite>($"Sprites/UI/CartasObjetivo/{objetivoCarta.ToString()}");
                print("Objetivo: "+objetivoCarta.ToString());
                 objetivoActivado=true;
            }
           /* if (LanguageManager.Instance.idiomaActual == "en")
            {
                nuevoSprite = Resources.Load<Sprite>($"Sprites/UI/CartasObjetivoEng/{objetivoCarta.ToString()}");

            }*/
            imagenUI.sprite = nuevoSprite;
            imagenUI.gameObject.SetActive(true);
        }


       if(objetivoActivado==false)
        {
            objetivoCarta = CardEnum.Rojo;
            imagenUI.gameObject.SetActive(false);
        }

        #endregion
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

#region Victoria o derota
    void ShowEndOfDaySummary()
    {
        Time.timeScale = 0f;
        canvasUI.gameObject.SetActive(false);
        panelResumenDia.SetActive(true);


        int cantidadMonedas = int.Parse(monedasActuales.text);
        if (LanguageManager.Instance.idiomaActual == "es")
        {
            textObjetivo.text = objetivo + " monedas";
        }

        if (objetivoCarta != CardEnum.Rojo) 
        {

            if (ComprobarSiTieneLaCarta(objetivoCarta)==true && cantidadMonedas >= objetivo)
            {
                print("ganaste");
                printVictoria();
            }
            //PERDER
            else 
            {
                print("Perdiste");
                    printDerrota();

            }
        }
      /*  if(objetivoCarta == CardEnum.Rojo)
        {
            if (cantidadMonedas >= objetivo)
            {
                printVictoria(false);
            }
            else 
            {
                objetivoCartaTXT.gameObject.SetActive(false);
                if (LanguageManager.Instance.idiomaActual == "es")
                {
                    textResultado.text = "No cumpliste el objetivo";
                }
                if (LanguageManager.Instance.idiomaActual == "en")
                {
                    textResultado.text = "Objective failed";
                }
                    recResultadoText.anchoredPosition = recCartasText.anchoredPosition;

            }
        }*/
    }
#endregion
    void printVictoria() 
    {

            if (LanguageManager.Instance.idiomaActual == "es")
            {
                textResultado.text = "Objetivo cumplido";
            }
        
    }

   void printDerrota()
    {
        
                    if (LanguageManager.Instance.idiomaActual == "es")
            {
                textResultado.text = "Objetivo NO cumplido";
            }
    } 

/// <summary>
/// Boton continuar el juego en el boton del resumen del dia
/// </summary>
    public void ContinuarJuego()
    {
        //Dejar como boton activo el x1

        Color colorVerdeClaro = new Color(142f / 255f, 230f / 255f, 142f / 255f);


        BottonPause.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        BottonPlay.GetComponentInChildren<TextMeshProUGUI>().color = colorVerdeClaro;
        BotonX2.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;


        Time.timeScale = 1f;
        canvasUI.gameObject.SetActive(true);
        if (panelResumenDia != null)
            panelResumenDia.SetActive(false);
    }
}
