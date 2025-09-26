using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalirJuego : MonoBehaviour
{
    public void Salir()
    {
        //PlayerPrefs.DeleteAll(); 
        // Esto cierra el juego compilado
        Application.Quit();

        // Solo para comprobar en el editor de Unity (no se cierra el editor, pero muestra un log)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }



}
