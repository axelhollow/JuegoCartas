using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GestionDeEnfermedad : MonoBehaviour
{
    public GameObject textoEstacion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivarEnfermedad()
    {
        // Busca todos los objetos en la escena, si es otoño
        string estcion = textoEstacion.GetComponent<TextMeshProUGUI>().text;
        if (estcion == "Otoño")
        {
            GameObject[] todos = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in todos)
            {
                if (obj.name.Contains("Vaca") || obj.name.Contains("Gallina"))
                {
                    print("Encontrado: " + obj.name);
                    // Aquí puedes hacer algo con el objeto
                    obj.GetComponent<Enfermedad>().PonerEnfermo();

                }
            }
        }


    }
}
