using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCamara : MonoBehaviour
{
    public float velocidadMovimiento = 0.5f;

    public Vector2 limiteX = new Vector2(-20f, 20f);
    public Vector2 limiteZ = new Vector2(-20f, 20f);

    private Vector3 posicionAnterior;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Botón derecho del ratón
        {
            posicionAnterior = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            Vector3 desplazamiento = Camera.main.ScreenToViewportPoint(Input.mousePosition - posicionAnterior);

            // Movimiento en plano XZ (horizontal)
            Vector3 movimiento = new Vector3(-desplazamiento.x * velocidadMovimiento, 0, -desplazamiento.y * velocidadMovimiento);
            transform.position += movimiento;

            // Limitar posición
            Vector3 posicionLimitada = transform.position;
            posicionLimitada.x = Mathf.Clamp(posicionLimitada.x, limiteX.x, limiteX.y);
            posicionLimitada.z = Mathf.Clamp(posicionLimitada.z, limiteZ.x, limiteZ.y);
            transform.position = posicionLimitada;

            posicionAnterior = Input.mousePosition;
        }
    }
}
