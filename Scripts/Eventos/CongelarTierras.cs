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

                    Transform canvasInvierno = obj.transform.Find("Invierno");
                    if (canvasInvierno != null)
                    {
                        Canvas c = canvasInvierno.GetComponent<Canvas>();
                        if (c != null)
                        {
                            print("activar canvas");
                            c.gameObject.SetActive(true);
                        }
                            
                    }
                }
            }
        }
        
    }
}
