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
    public float dayDuration = 60f;

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
    private int currentDay = 1;
    private bool cumpliaObjetivoAntes = false; // <- Nuevo flag dinámico

    public event Action OnDayEnded;



    [Header("Partículas")]
    public GameObject particulaDestruccionPrefab;

    void Start()
    {
        UpdateDayUI();

        //le damos un valor al mx del slider
        if (dayProgressSlider != null)
            dayProgressSlider.maxValue = dayDuration;

        //desactivamos el panel del final del dia
        if (panelResumenDia != null)
            panelResumenDia.SetActive(false);
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
            }

            if (dayProgressSlider != null)
                dayProgressSlider.value = timer;

            ActualizarObjetivoActual();
        }
    }

    void UpdateDayUI()
    {
        if (dayCounterText != null)
            dayCounterText.text = "Día " + currentDay;
    }

    void ActualizarObjetivoActual()
    {
        if (textoObjetivoActual == null) return;

        var cartas = GameObject.FindObjectsOfType<Carta>(true);

        int tierras = cartas.Count(c => c.cartaEnum == CardEnum.TierraCultivo && c.gameObject.activeInHierarchy);
        int monedas = int.Parse(monedasActuales.text);

        textoObjetivoActual.text = $"{monedas} / {tierras}";

        bool cumple = monedas >= tierras;

        textoObjetivoActual.color = cumple ? Color.green : Color.red;

        // Activar animación solo si se acaba de cumplir el objetivo
        if (cumple && !cumpliaObjetivoAntes)
        {
            LeanTween.scale(textoObjetivoActual.gameObject, Vector3.one * 1.2f, 0.15f).setEaseOutBack().setOnComplete(() =>
            {
                LeanTween.scale(textoObjetivoActual.gameObject, Vector3.one, 0.15f).setEaseInBack();
            });
        }

        // Guardar el estado para la siguiente comparación
        cumpliaObjetivoAntes = cumple;
    }

    void ShowEndOfDaySummary()
    {
        canvasUI.gameObject.SetActive(false);
        Time.timeScale = 0f;

        Carta[] todasLasCartas = GameObject.FindObjectsOfType<Carta>(true);

        var cartasTierra = todasLasCartas
            .Where(c => c.cartaEnum == CardEnum.TierraCultivo && c.gameObject.activeInHierarchy)
            .ToList();

        int cantidadMonedas = int.Parse(monedasActuales.text);
        int objetivo = cartasTierra.Count;

        textMonedas.text = "Monedas: " + cantidadMonedas;
        textObjetivo.text = "Objetivo: " + objetivo + " monedas";

        if (cantidadMonedas >= objetivo)
        {
            textResultado.text = "Objetivo cumplido";

          
           int monedasAct= int.Parse(monedasActuales.text)-objetivo;
            monedasActuales.text=monedasAct.ToString();
        }
        else
        {
            textResultado.text = "No cumpliste el objetivo";

            monedasActuales.text = "0";

            int diferencia = objetivo - cantidadMonedas;

            var tierrasParaEliminar = cartasTierra.OrderBy(x => UnityEngine.Random.value).Take(diferencia).ToList();

            foreach (var tierra in tierrasParaEliminar)
            {
                GenerarParticula(tierra.transform.position);
                Destroy(tierra.gameObject);
            }
        }

        panelResumenDia.SetActive(true);

        // --- Chequeo de derrota ---
        // Esperamos un frame para que se actualice la escena, para esto usamos StartCoroutine

        StartCoroutine(VerificarDerrotaAlFinalDelDia());
    }

    IEnumerator VerificarDerrotaAlFinalDelDia()
    {
        // Esperar al siguiente frame para que se actualicen los objetos destruidos
        yield return null;

        // Buscar si quedan tierras activas
        var tierrasRestantes = GameObject.FindObjectsOfType<Carta>(true)
            .Where(c => c.cartaEnum == CardEnum.TierraCultivo && c.gameObject.activeInHierarchy)
            .ToList();

        if (tierrasRestantes.Count == 0)
        {
            // Aquí tu lógica de derrota, por ejemplo:
            Debug.Log("¡Has perdido! No quedan tierras.");

            // Puedes mostrar un panel de Game Over o reiniciar el juego
            MostrarPanelDerrota();
        }
    }

    void MostrarPanelDerrota()
    {
        // Ejemplo simple: mostrar un panel con mensaje de derrota
        // Puedes adaptar esto a tu UI

        // Por ejemplo, si tienes un panel específico:
        // panelDerrota.SetActive(true);

        // O simplemente pausar el juego
        Time.timeScale = 0f;

        // Puedes hacer más cosas aquí...
    }


    void GenerarParticula(Vector3 posicion)
    {
        if (particulaDestruccionPrefab != null)
        {
            var instancia = Instantiate(particulaDestruccionPrefab, posicion, Quaternion.identity);
            Destroy(instancia, 2f);
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
