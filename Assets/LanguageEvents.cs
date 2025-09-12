using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageEvents : MonoBehaviour
{
    public static event Action<string> OnLanguageChanged;

    public static void TriggerLanguageChanged(string nuevoIdioma)
    {
        OnLanguageChanged?.Invoke(nuevoIdioma);
    }
}
