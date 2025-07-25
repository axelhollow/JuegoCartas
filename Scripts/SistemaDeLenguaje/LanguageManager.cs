using System;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageManager : MonoBehaviour
{

    public static LanguageManager Instance;

    public Language CurrentLanguage { get; private set; } = Language.Espa�ol;

    public string idiomaActual;

    public event Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        idiomaActual = "es";
    }

    public void SetLanguage(string language)
    {
        if (language=="Espa�ol" && Enum.TryParse<Language>("Espa�ol", out var result))
        {
            CurrentLanguage = result;
            OnLanguageChanged?.Invoke();
            print("Idioma cambiado a espa�ol");
            CambiarIdioma("es");
            idiomaActual = "es";

        }
        if (language == "Ingles" && Enum.TryParse<Language>("Ingles", out var resultEng))
        {
            CurrentLanguage = resultEng;
            OnLanguageChanged?.Invoke();
            print("Idioma cambiado a ingles");
            CambiarIdioma("en");
            idiomaActual = "en";
        }

    }


    public void CambiarIdioma(string codigo)
    {
        var locale = LocalizationSettings.AvailableLocales.Locales
            .Find(l => l.Identifier.Code == codigo);

        if (locale != null)
            LocalizationSettings.SelectedLocale = locale;
    }
}

