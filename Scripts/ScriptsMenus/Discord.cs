using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discord : MonoBehaviour
{

    public string discordURL = "https://discord.gg/7rCqyn9V3H";

    public void OpenDiscord()
    {
        Application.OpenURL(discordURL);
    }

}
