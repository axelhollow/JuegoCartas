using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedMaterial : MonoBehaviour
{
    public Renderer targetRenderer;

    [System.Serializable]
    public class MaterialByLanguage
    {
        public Language language;
        public Material material;
       
    }

    public MaterialByLanguage[] localizedMaterials;

    private void Start()
    {
        LanguageManager.Instance.OnLanguageChanged += UpdateMaterial;
        UpdateMaterial();
    }

    private void OnDestroy()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.OnLanguageChanged -= UpdateMaterial;
    }

    private void UpdateMaterial()
    {
        foreach (var item in localizedMaterials)
        {
            if (item.language == LanguageManager.Instance.CurrentLanguage)
            {
                targetRenderer.material = item.material;
                break;
            }
        }
    }
}
