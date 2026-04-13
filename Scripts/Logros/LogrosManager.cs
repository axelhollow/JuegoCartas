using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogrosManager : MonoBehaviour
{
    SteamIntegration si;
    public TextMeshProUGUI monedas;

    private void Start()
    {
        si = GetComponent<SteamIntegration>();
    }

    void Update()
    {
        int moneditas= int.Parse(monedas.text);
        GameObject vaca = GameObject.Find("Vaca(Clone)");
        GameObject vacuna = GameObject.Find("Medicina(Clone)");
        GameObject heno = GameObject.Find("Heno(Clone)");
        GameObject basura = GameObject.Find("Basura(Clone)");
        GameObject carne = GameObject.Find("Carne(Clone)");
        GameObject huevo = GameObject.Find("Huevo(Clone)");

        if (vaca != null)
        {
           si.Unlock("vaca");
        }
        if (moneditas >= 100)
        {
            si.Unlock("100Monedas");
        }
        if (vacuna != null)
        {
            si.Unlock("Vacuna");
        }
        if (heno != null)
        {
            si.Unlock("Heno");
        }
        if (basura != null)
        {
            si.Unlock("Basura");
        }
        if (carne != null)
        {
        si.Unlock("Carne");
        }
        if (huevo != null)
        {
            si.Unlock("Huevo");
        }
        

    }




    
}
