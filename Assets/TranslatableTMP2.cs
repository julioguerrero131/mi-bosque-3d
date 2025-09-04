using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslatableTMP2 : MonoBehaviour
{
    public string clave;
    private TextMeshProUGUI textoTMP;

    // Start is called before the first frame update
    void Awake()
    {
        textoTMP = GetComponent<TextMeshProUGUI>();
        if (textoTMP == null)
        {
            Debug.LogError("No se encontró componente TextMeshProUGUI en el objeto: " + gameObject.name);
            return;
        }
        Debug.Log(textoTMP.text);
    }

    void OnEnable()
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
            textoTMP.text = LanguageManager.Instancia.ObtenerTexto(clave);
        }
    }

    private IEnumerator EsperarLanguageManager()
    {
        while (LanguageManager.Instancia == null)
            yield return null;

        LanguageManager.Instancia.OnIdiomaCambiado += ActualizarTexto;
        ActualizarTexto();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
