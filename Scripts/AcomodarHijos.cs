using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcomodarHijos : MonoBehaviour
{
    public float alturaEntreHijos = 0.1f;
    private float escaleraOffset = -0.3f;
    FusionManager fusionManager;


    private void Start()
    {
        
        if (FusionManager.Instance != null)
        {
             fusionManager = FusionManager.Instance;
        }
        else 
        {
             fusionManager = new();

        }
    }
    void Update()
    {
        if (transform.childCount > 0)
        {
            //Ordenar a los hijos
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform hijo = transform.GetChild(i);
                Vector3 nuevaPos = transform.position;

                nuevaPos.y += alturaEntreHijos * (i + 1);           // Apila en altura
                nuevaPos.z += escaleraOffset * (i + 1);             // Desplaza hacia atrás (como escalera)

                hijo.position = nuevaPos;
            }

            //Comprobamos si solo tienen 2 hijos (ten en cuenta que hay que sumar 1 de mas por el canvas que tiene la carta)
            if (transform.childCount == 2) 
            {
                //comprobar si hay que fusionar
                //fusionManager.TryFusion();
                GameObject hijo=null;
                foreach (Transform child in transform)
                {
                    if(child.tag== "Draggable") 
                    {
                        hijo=child.gameObject;
                    }
                }
                if (hijo != null) 
                {
                    fusionManager.fusionarCartas(gameObject, hijo);
                }

            }

        }
    }
}
