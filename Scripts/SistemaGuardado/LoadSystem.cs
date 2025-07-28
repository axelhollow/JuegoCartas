using UnityEngine;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.UI;
 
public class LoadSystem : MonoBehaviour
{
    public string saveFileName = "saveData.json";

    // Lista de prefabs disponibles. Asegúrate de asignarlos desde el Inspector.
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

        if (!File.Exists(path))
        {
            Debug.LogWarning("No se encontró archivo de guardado.");
            return;
        }

        // 1. Leer JSON
        string json = File.ReadAllText(path);

        // 2. Convertir JSON a objetos
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // 3. Limpiar escena si es necesario (opcional)
        EliminarObjetosExistentes();

        // 4. Instanciar los objetos desde los datos guardados
        foreach (JsonCarta objData in data.listaCartas)
        {
            GameObject prefab = prefabs.Find(p => p.name == objData.prefabName);

            if (prefab != null)
            {
                Vector3 position = new Vector3(objData.posX, objData.posY, objData.posZ);
                GameObject newObj = Instantiate(prefab, position, Quaternion.identity);
                print(objData.prefabName + " cargado");
            }
            else
            {
                Debug.LogWarning("Prefab no encontrado: " + objData.prefabName);
            }
        }


        //PlayerPref
      
       monedas.text=PlayerPrefs.GetString("Monedas","0");
       
       diaNumero.text= PlayerPrefs.GetString("NumDia","1");

        Debug.Log("Carga completada.");
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
