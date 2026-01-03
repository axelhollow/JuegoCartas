using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class NuevaPartida : MonoBehaviour
{
        public string saveFileName = "saveData.json";
    // Start is called before the first frame update

    public void Init()
    {
        string path = Application.persistentDataPath + "/" + saveFileName;
        print(path);
        //si el archivo de guardado exite se sobreescribe (se borra que es mas facil)
        if (File.Exists(path))
        {   
                 File.Delete(path);
                 print("archivo de guardado eliminado");
        }

    }

}
