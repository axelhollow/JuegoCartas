using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSystem : MonoBehaviour
{
    public string saveFileName = "saveData.json";
    public TextMeshProUGUI monedas;
    public TextMeshProUGUI diaNumero;
    public Slider barraDiaValor;
    public TextMeshProUGUI textoEstacion;

    public GameObject cicloDiaOBJ;

    public Image ColorEstacion;
    public void Save()
    {
        SaveData data = new SaveData();

        GameObject[] draggable = GameObject.FindGameObjectsWithTag("Draggable");
        GameObject[] sobres = GameObject.FindGameObjectsWithTag("Sobre");

        List<GameObject> allObjects = new List<GameObject>();
        allObjects.AddRange(draggable);
        allObjects.AddRange(sobres);

        foreach (GameObject obj in allObjects)
        {
            JsonCarta datosCarta = new JsonCarta();
            datosCarta.prefabName = obj.name.Replace("(Clone)", "");
            datosCarta.posX = obj.transform.position.x;
            datosCarta.posY = obj.transform.position.y;
            datosCarta.posZ = obj.transform.position.z;

            data.listaCartas.Add(datosCarta);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/" + saveFileName, json);
        print(Application.persistentDataPath + "/" + saveFileName);
        //PlayerPref

        PlayerPrefs.SetString("Monedas", monedas.text);
        PlayerPrefs.SetString("NumDia", diaNumero.text);
        float procentajeDia = barraDiaValor.value;
        PlayerPrefs.SetFloat("DiaBarra", procentajeDia);
        PlayerPrefs.SetString("Estacion", textoEstacion.text);
        string hex = "#" + ColorUtility.ToHtmlStringRGBA(ColorEstacion.color);
        PlayerPrefs.SetString("ColorEstacion", hex);
        print("Color: "+ColorEstacion.color.ToString());

        PlayerPrefs.SetInt("DiaEstacion", cicloDiaOBJ.GetComponent<DayCycleManager>().diaEstacion);
        PlayerPrefs.SetInt("EstacionActual", cicloDiaOBJ.GetComponent<DayCycleManager>().estacionActual);

        PlayerPrefs.Save();
    }
}
