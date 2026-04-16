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

public bool logroVaca = false;
public bool logro100 = false;
public bool logroVacuna = false;
public bool logroHeno = false;
public bool logroBasura = false;
public bool logroCarne = false;
public bool logroHuevo = false;

void Update()
{
    int moneditas;
    if (!int.TryParse(monedas.text, out moneditas)) return;

    if (!logroVaca)
    {
        GameObject vaca = GameObject.Find("Vaca(Clone)");
        if (vaca != null)
        {
            si.Unlock("vaca");
            logroVaca = true;
        }
    }

    if (!logro100 && moneditas >= 100)
    {
        si.Unlock("100Monedas");
        logro100 = true;
    }

    if (!logroVacuna)
    {
        GameObject vacuna = GameObject.Find("Medicina(Clone)");
        if (vacuna != null)
        {
            si.Unlock("Vacuna");
            logroVacuna = true;
        }
    }

    if (!logroHeno)
    {
        GameObject heno = GameObject.Find("Heno(Clone)");
        if (heno != null)
        {
            si.Unlock("Heno");
            logroHeno = true;
        }
    }

    if (!logroBasura)
    {
        GameObject basura = GameObject.Find("Basura(Clone)");
        if (basura != null)
        {
            si.Unlock("Basura");
            logroBasura = true;
        }
    }

    if (!logroCarne)
    {
        GameObject carne = GameObject.Find("Carne(Clone)");
        if (carne != null)
        {
            si.Unlock("Carne");
            logroCarne = true;
        }
    }

    if (!logroHuevo)
    {
        GameObject huevo = GameObject.Find("Huevo(Clone)");
        if (huevo != null)
        {
            si.Unlock("Huevo");
            logroHuevo = true;
        }
    }
}
        

    }




    

