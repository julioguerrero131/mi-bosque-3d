using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TranslatableTMP2 : MonoBehaviour
{
    public string clave;
    private TextMeshProUGUI textoUI;   // Para UI (Canvas)
    private TextMeshPro texto3D;       // Para textos 3D
    private TextMesh textoMesh;        // Para el TextMesh clásico

    // Start is called before the first frame update
    private void Awake()
    {
        // Intentar obtener ambos, pero solo uno será válido
        textoUI = GetComponent<TextMeshProUGUI>();
        texto3D = GetComponent<TextMeshPro>();
        textoMesh = GetComponent<TextMesh>();

        if (textoUI == null && texto3D == null && textoMesh == null)
        {
            Debug.LogError("No se encontró ni TextMeshProUGUI, TextMeshPro ni TextMesh en el objeto: " + gameObject.name);
            return;
        }
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
            LanguageManager.Instancia.OnIdiomaCambiado -= ActualizarTexto;
    }

    private void ActualizarTexto()
    {
        if (LanguageManager.Instancia != null && !string.IsNullOrEmpty(clave))
        {
            string nuevoTexto = LanguageManager.Instancia.ObtenerTexto(clave);

            if (textoUI != null)
                textoUI.text = nuevoTexto;

            if (texto3D != null)
                texto3D.text = nuevoTexto;

            if (textoMesh != null)
                textoMesh.text = nuevoTexto;
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
