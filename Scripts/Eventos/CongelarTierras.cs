using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CongelarTierras : MonoBehaviour
{

    public GameObject textoEstacion;

    // Update is called once per frame
    void Update()
    {
        print("hola");
        if(textoEstacion.GetComponent<TextMeshProUGUI>().text=="Invierno")
        {
            print("ES INVIERNO");
             GameObject[] todos = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in todos)
            {
                if (obj.name.Contains("FarmLand"))
                {
                    print("Encontrado: " + obj.name);
                    Transform canvasInvierno = obj.transform.Find("Invierno");
                    if (canvasInvierno != null)
                    {
                        Canvas c = canvasInvierno.GetComponent<Canvas>();
                        if (c != null)
                            c.enabled = true; // activa solo el canvas llamado "Invierno"
                    }
                }
            }
        }
        
    }
}
