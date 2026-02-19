using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class NuevaPartida : MonoBehaviour
{
    public Button boton_continuar;
        public string saveFileName = "saveData.json";
    // Start is called before the first frame update

    public void Init()
    {
        boton_continuar.interactable=true;
        string path = Application.persistentDataPath + "/" + saveFileName;
        print(path);
        //si el archivo de guardado exite se sobreescribe (se borra que es mas facil)
        if (File.Exists(path))
        {   
                 File.Delete(path);
                 print("archivo de guardado eliminado");
        }
       
        if (!PlayerPrefs.HasKey("PrimerArranque"))
            {
                Debug.Log("Es la primera vez que se ejecuta en este equipo");

                PlayerPrefs.SetInt("PrimerArranque", 1);
                PlayerPrefs.Save();
            }
        else
            {
                Debug.Log("Ya se había ejecutado antes");
            }

    }

}
