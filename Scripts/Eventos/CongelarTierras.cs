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
        if(textoEstacion.GetComponent<TextMeshProUGUI>().text=="Invierno")
        {
      
             GameObject[] todos = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in todos)
            {
                if (obj.name.Contains("FarmLand"))
                {

                    Transform invierno = obj.transform.Find("Canvas/Invierno");
                    if (invierno != null)
                    {
                      
                            invierno.gameObject.SetActive(true);

                    }
                }
            }
        }
        if(textoEstacion.GetComponent<TextMeshProUGUI>().text!="Invierno")
        {
      
             GameObject[] todos = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in todos)
            {
                if (obj.name.Contains("FarmLand"))
                {

                    Transform invierno = obj.transform.Find("Canvas/Invierno");
                    if (invierno != null)
                    {
                            invierno.gameObject.SetActive(false);

                    }
                }
            }
        }
        
    }
}
