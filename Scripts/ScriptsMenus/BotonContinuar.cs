using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonContinuar : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(PlayerPrefs.HasKey("PrimerArranque"))
        {
        gameObject.GetComponent<Button>().interactable=true;
        }
    }
}
