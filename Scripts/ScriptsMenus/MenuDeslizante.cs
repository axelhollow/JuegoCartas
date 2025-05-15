using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDeslizante : MonoBehaviour
{
    public RectTransform menuPanel;
    public float velocidad = 5f;
    public Vector2 posicionOculta = new Vector2(-200f, 0f);
    public Vector2 posicionVisible = new Vector2(0f, 0f);

    private bool estaAbierto = false;

    void Start()
    {
        menuPanel.anchoredPosition = posicionOculta;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            AlternarMenu();
        }

        Vector2 destino = estaAbierto ? posicionVisible : posicionOculta;
        menuPanel.anchoredPosition = Vector2.Lerp(menuPanel.anchoredPosition, destino, Time.deltaTime * velocidad);
    }

    public void AlternarMenu()
    {
        estaAbierto = !estaAbierto;
    }
}
