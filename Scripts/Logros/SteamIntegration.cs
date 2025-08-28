using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntegration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        inicio();

    }

    public void inicio()
    {
        try
        {
            Steamworks.SteamClient.Init(3934350);

        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    public void printName() 
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }
    public void Unlock(string id)
    {
        var arc = new Steamworks.Data.Achievement(id);
        arc.Trigger();

        print("Logro desbloqueado");

    }
    public  void LimpiarLogros(string id) 
    {
        var arc = new Steamworks.Data.Achievement(id);
        arc.Clear();
        print("Logro bloqueado");
    }


}
