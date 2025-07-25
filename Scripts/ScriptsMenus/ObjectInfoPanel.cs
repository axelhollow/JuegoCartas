using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI levelText;

    public void ShowInfo(InspectableObject obj)
    {
        if (LanguageManager.Instance.idiomaActual == "es")
        {

            nameText.text = obj.objectName;
        }
        if (LanguageManager.Instance.idiomaActual == "en")
        {

            nameText.text = obj.objectNameENG;
        }

        if (LanguageManager.Instance.idiomaActual == "es") 
        {
            descriptionText.text = obj.description;
        }
        if (LanguageManager.Instance.idiomaActual == "en")
        {
            descriptionText.text = obj.descriptionENG;
        }

        levelText.text = "" + obj.valor;
        gameObject.SetActive(true);
    }

    public void HideInfo()
    {
        gameObject.SetActive(false);
    }
}
