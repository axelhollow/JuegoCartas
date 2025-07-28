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

        //PlayerPref

        PlayerPrefs.SetString("Monedas", monedas.text);
        PlayerPrefs.SetString("NumDia", diaNumero.text);
        float procentajeDia = barraDiaValor.value;
        PlayerPrefs.SetFloat("DiaBarra", procentajeDia);
        PlayerPrefs.Save();
    }
}
