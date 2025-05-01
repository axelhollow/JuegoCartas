using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcomodarHijos : MonoBehaviour
{
    public float alturaEntreHijos = 0.1f;
    private float escaleraOffset = -0.3f;

    void Update()
    {
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform hijo = transform.GetChild(i);
                Vector3 nuevaPos = transform.position;

                nuevaPos.y += alturaEntreHijos * (i + 1);           // Apila en altura
                nuevaPos.z += escaleraOffset * (i + 1);             // Desplaza hacia atrás (como escalera)

                hijo.position = nuevaPos;
            }
        }
    }
}
