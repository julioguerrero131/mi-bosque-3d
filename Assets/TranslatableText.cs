using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TranslatableText : MonoBehaviour
{
    public string clave;

    private Text textoUI;

    private void Awake()
    {
        textoUI = GetComponent<Text>();
        if (textoUI == null)
        {
            Debug.LogError("No se encontró componente Text en el objeto: " + gameObject.name);
            return;
        }
        Debug.Log(textoUI.text);
    }

    private void OnEnable()
    {
        if (LanguageManager.Instancia != null)
        {
            LanguageManager.Instancia.OnIdiomaCambiado += ActualizarTexto;
            ActualizarTexto();
        }
        else
        {
            StartCoroutine(EsperarLanguageManager());
        }
    }

    private void OnDisable()
    {
        if (LanguageManager.Instancia != null)
        {
            LanguageManager.Instancia.OnIdiomaCambiado -= ActualizarTexto;
        }
    }

    private void ActualizarTexto()
    {
        if (LanguageManager.Instancia != null && !string.IsNullOrEmpty(clave))
        {
            textoUI.text = LanguageManager.Instancia.ObtenerTexto(clave);
        }
    }

    private IEnumerator EsperarLanguageManager()
    {
        while (LanguageManager.Instancia == null)
            yield return null;

        LanguageManager.Instancia.OnIdiomaCambiado += ActualizarTexto;
        ActualizarTexto();
    }
}
