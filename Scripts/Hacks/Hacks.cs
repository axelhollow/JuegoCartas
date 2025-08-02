using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacks : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dia1() 
    {
        PlayerPrefs.SetString("NumDia","1");

        PlayerPrefs.SetFloat("DiaBarra", 0f);

    }
}
