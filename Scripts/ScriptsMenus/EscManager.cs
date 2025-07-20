using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscManager : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject canvasMenuEsc;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f;   // Detiene el tiempo del juego
        isPaused = true;
        // Aquí puedes activar un menú de pausa, por ejemplo:
        // pauseMenu.SetActive(true);
        Debug.Log("Juego pausado");
        canvasMenuEsc.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;   // Reanuda el tiempo del juego
        isPaused = false;

        Debug.Log("Juego reanudado");
        canvasMenuEsc.SetActive(false);
    }
}
