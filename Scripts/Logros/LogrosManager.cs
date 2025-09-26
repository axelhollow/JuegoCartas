using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogrosManager : MonoBehaviour
{
    SteamIntegration si;

    private void Start()
    {
        si = GetComponent<SteamIntegration>();
    }

    void Update()
    {
        GameObject vaca = GameObject.Find("Vaca(Clone)");


        if (vaca != null)
        {
           si.Unlock("vaca");
        }

    }




    
}
