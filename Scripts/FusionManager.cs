using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FusionManager : MonoBehaviour
{
    public static FusionManager Instance { get; private set; }
    public Slider slider;
    public float duration = 3f;
    GameObject _obj1;
    GameObject _obj2;
    public GameObject morado;

    Vector3 posicionCartaNueva;
    private void Awake()
    {
        // Si ya existe una instancia y no somos nosotros, destruimos este objeto
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }



    public void fusionarCartas(GameObject obj1, GameObject obj2)
    {
        _obj1 = obj1;
        _obj2 = obj2;
        Carta card1 = obj1.GetComponent<Carta>();
        Carta card2 = obj2.GetComponent<Carta>();
        card1.slider.gameObject.SetActive(true);
        slider= card1.slider;
        StartCoroutine("ProgressSlider");
    }

    IEnumerator ProgressSlider()
    {
        float elapsed = 0f;
        slider.value = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            slider.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        slider.value = 1f; // Asegurar que termine exactamente en 1
        posicionCartaNueva= _obj1.transform.position;
        Destroy(_obj1);
        Destroy(_obj2);
        Instantiate(morado);
        morado.transform.position = posicionCartaNueva;
    }

    public void TryFusion() 
    {
        
    
    }
}
