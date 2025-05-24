using UnityEngine;

public class HoverDetector : MonoBehaviour
{
    public Camera mainCamera;
    public ObjectInfoPanel infoPanel;
    private InspectableObject lastHovered;

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var obj = hit.collider.GetComponent<InspectableObject>();
            if (obj != null)
            {
                if (obj != lastHovered)
                {
                    lastHovered = obj;
                    infoPanel.ShowInfo(obj);
                }
                return;
            }
        }

        // Si no se encuentra nada o el objeto ya no es el mismo
        if (lastHovered != null)
        {
            lastHovered = null;
            infoPanel.HideInfo();
        }
    }
}