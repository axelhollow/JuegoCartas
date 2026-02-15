using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
using System.Globalization;
using UnityEngine.UI;
 
public class LoadSystem : MonoBehaviour
{
    public string saveFileName = "saveData.json";

    // Lista de prefabs disponibles. Aseg�rate de asignarlos desde el Inspector.
    public List<GameObject> prefabs;
    public TextMeshProUGUI monedas;
    public TextMeshProUGUI diaNumero;
    public Slider barraDiaValor;

    

    private void Start()
    {

        Load();
    }
    public void Load()
    {
        string path = Application.persistentDataPath + "/" + saveFileName;
    print(path);
    //PARTIDA NUEVA
        if (!File.Exists(path))
        {
            //Datos en escena y player prefab
            Debug.LogWarning("No se encontr� archivo de guardado.");
            EliminarObjetosExistentes();
            monedas.text="0";
            diaNumero.text="1";
            barraDiaValor.value=0f;
            PlayerPrefs.SetString("Monedas", "0");
            PlayerPrefs.SetString("NumDia", "1");
            PlayerPrefs.SetFloat("DiaBarra", 0f);


            PlayerPrefs.Save();

            //Datos de misiones y estaciones (reseteo)
            DayCycleManager hijo = GetComponentInChildren<DayCycleManager>();
            hijo.ReseteDeMisionesYEstaciones();
            hijo. textoEstacion.GetComponent<TextMeshProUGUI>().text ="Primavera";
            Color color;
            ColorUtility.TryParseHtmlString("#8EE68E", out color);
            hijo. EstacionFill.GetComponent<Image>().color=color;
            PlayerPrefs.SetString("Estacion","Primavera");
            PlayerPrefs.SetString("ColorEstacion","#8EE68E");
            PlayerPrefs.SetString("DiaEstacion","1");
            PlayerPrefs.SetString("EstacionActual","1");
            hijo.estacionActual=1;
            hijo.diaEstacion=1;
            return;
        }

        // 1. Leer JSON
        string json = File.ReadAllText(path);

        // 2. Convertir JSON a objetos
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // 3. Limpiar escena si es necesario
        EliminarObjetosExistentes();

        // 4. Instanciar los objetos desde los datos guardados
        foreach (JsonCarta objData in data.listaCartas)
        {
            GameObject prefab = prefabs.Find(p => p.name == objData.prefabName);

            if (prefab != null)
            {
                Vector3 position = new Vector3(objData.posX, objData.posY, objData.posZ);
                GameObject newObj = Instantiate(prefab, position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Prefab no encontrado: " + objData.prefabName);
            }
        }


        //PlayerPref
        DayCycleManager hijo_AUX = GetComponentInChildren<DayCycleManager>();
        hijo_AUX. textoEstacion.GetComponent<TextMeshProUGUI>().text =PlayerPrefs.GetString("Estacion","0");
        Color color_aux;
        ColorUtility.TryParseHtmlString(PlayerPrefs.GetString("ColorEstacion"), out color_aux);
        hijo_AUX. EstacionFill.GetComponent<Image>().color=ParseRGBA(color_aux.ToString());
        monedas.text=PlayerPrefs.GetString("Monedas","0");
        diaNumero.text= PlayerPrefs.GetString("NumDia","1"); 
        hijo_AUX.diaEstacion= PlayerPrefs.GetInt("DiaEstacion",1);
        hijo_AUX.estacionActual= PlayerPrefs.GetInt("EstacionActual", 1);

        Debug.Log("Carga completada.");
    }

      Color ParseRGBA(string texto)
    {
        // Quita "RGBA(" y ")"
        texto = texto.Replace("RGBA(", "").Replace(")", "");

        string[] valores = texto.Split(',');

        float r = float.Parse(valores[0], CultureInfo.InvariantCulture);
        float g = float.Parse(valores[1], CultureInfo.InvariantCulture);
        float b = float.Parse(valores[2], CultureInfo.InvariantCulture);
        float a = float.Parse(valores[3], CultureInfo.InvariantCulture);

        return new Color(r, g, b, a);
    }

    private void EliminarObjetosExistentes()
    {
        // Opcional: elimina objetos con esas etiquetas antes de cargar
        GameObject[] draggable = GameObject.FindGameObjectsWithTag("Draggable");
        GameObject[] sobres = GameObject.FindGameObjectsWithTag("Sobre");

        foreach (GameObject obj in draggable) Destroy(obj);
        foreach (GameObject obj in sobres) Destroy(obj);
    }
}
