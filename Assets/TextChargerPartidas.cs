using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChargerPartidas : MonoBehaviour
{
    public GameObject panelMenuPartidas;
    public GameObject panelDatos;
    public GameObject panelPin;

    public bool mostrarDebugDetallado = true;

    // Cache para almacenar los textos cargados
    private Dictionary<string, string> textosCacheAvatar = new Dictionary<string, string>();
    private Dictionary<string, string> textosCacheDatos = new Dictionary<string, string>();
    private Dictionary<string, string> textosCachePin = new Dictionary<string, string>();

    private Dictionary<string, string> opcionesGenero = new Dictionary<string, string>();
    public Dropdown genderDropdown;

    private bool panelAvatarEstabaActivoAntes = false;
    private bool panelDatosEstabaActivoAntes = false;
    private bool panelPinEstabaActivoAntes = false;

    private bool textosYaCargados = false;

    void Start()
    {
        DiagnosticarJSON();

        if (textosYaCargados)
        {
            // Para Cargar/Nueva Partida
            if (panelMenuPartidas != null && panelMenuPartidas.activeInHierarchy)
            {
                StartCoroutine(AplicarTextosPanelAvatar());
            }
        }
    }

    void Update()
    {
        // Detectar cuando el panel se activa
        if (panelMenuPartidas != null && textosYaCargados)
        {
            bool estaActivoAhora = panelMenuPartidas.activeInHierarchy;

            if (!panelAvatarEstabaActivoAntes && estaActivoAhora)
            {
                Debug.Log("PANEL 1 ACTIVADO - Aplicando textos...");
                StartCoroutine(AplicarTextosPanelAvatar());
            }

            panelAvatarEstabaActivoAntes = estaActivoAhora;
        }

        if (panelDatos != null && textosYaCargados)
        {
            bool estaActivoAhora = panelDatos.activeInHierarchy;

            if (!panelDatosEstabaActivoAntes && estaActivoAhora)
            {
                Debug.Log("PANEL 2 ACTIVADO - Aplicando textos...");
                StartCoroutine(AplicarTextosPanelDatos());
            }

            panelDatosEstabaActivoAntes = estaActivoAhora;
        }

        if (panelPin != null && textosYaCargados)
        {
            bool estaActivoAhora = panelPin.activeInHierarchy;

            if (!panelPinEstabaActivoAntes && estaActivoAhora)
            {
                Debug.Log("PANEL 3 ACTIVADO - Aplicando textos...");
                StartCoroutine(AplicarTextosPanelPin());
            }

            panelPinEstabaActivoAntes = estaActivoAhora;
        }
    }

    private void DiagnosticarJSON()
    {
        Debug.Log("=== INICIANDO DIAGNÓSTICO DEL JSON ===");

        TextAsset jsonFile = Resources.Load<TextAsset>("all_texts");
        if (jsonFile == null)
        {
            Debug.LogError("❌ No se encontró el archivo JSON 'all_texts' en Resources.");
            return;
        }

        Debug.Log("✅ Archivo JSON encontrado. Contenido completo:");
        Debug.Log("==================== JSON RAW ====================");
        Debug.Log(jsonFile.text);
        Debug.Log("==================== FIN JSON ====================");

        try
        {
            // Intentar parsear el JSON completo como un objeto genérico primero
            var jsonObject = JsonUtility.FromJson<System.Object>(jsonFile.text);
            Debug.Log("✅ JSON es válido sintácticamente");

            // Ahora intentar con nuestra estructura corregida
            JsonCompleto data = JsonUtility.FromJson<JsonCompleto>(jsonFile.text);

            Debug.Log("=== DIAGNÓSTICO DE ESTRUCTURA CORREGIDA ===");
            Debug.Log("data != null: " + (data != null));

            if (data != null)
            {
                Debug.Log("data.menu_partidas != null: " + (data.menu_partidas != null));

                if (data.menu_partidas != null)
                {
                    Debug.Log("data.menu_partidas.nueva_partida != null: " + (data.menu_partidas.nueva_partida != null));

                    if (data.menu_partidas.nueva_partida != null)
                    {
                        var nuevaPartida = data.menu_partidas.nueva_partida;
                        Debug.Log("Contenido Nueva_Partida:" + nuevaPartida);

                        // Cargar Cache
                        textosCacheAvatar["title_avatar"] = nuevaPartida.title_avatar;
                        textosCacheDatos["title_datos"] = nuevaPartida.title_datos;
                        textosCacheDatos["label_nombre"] = nuevaPartida.label_nombre;
                        textosCacheDatos["label_edad"] = nuevaPartida.label_edad;
                        textosCacheDatos["label_genero"] = nuevaPartida.label_genero;
                        opcionesGenero["genero_h"] = nuevaPartida.genero_h;
                        opcionesGenero["genero_m"] = nuevaPartida.genero_m;
                        opcionesGenero["genero_otro"] = nuevaPartida.genero_otro;
                        textosCachePin["label_pin"] = nuevaPartida.label_pin;
                        textosCachePin["pin_invitado"] = nuevaPartida.pin_invitado;

                        int suma_textos = textosCacheAvatar.Count + textosCacheDatos.Count + textosCachePin.Count;

                        textosYaCargados = suma_textos > 0;
                        Debug.Log("✅ Total de textos cargados: " + suma_textos);
                    }
                    else
                    {
                        Debug.LogError("❌ nueva_partida es NULL");
                    }
                }
                else
                {
                    Debug.LogError("❌ menu_partidas es NULL");
                }
            }
            else
            {
                Debug.LogError("❌ No se pudo parsear el JSON con nuestra estructura corregida");

                // Intentar diagnóstico alternativo
                Debug.Log("=== INTENTANDO DIAGNÓSTICO ALTERNATIVO ===");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("❌ Error al parsear JSON: " + e.Message);
            Debug.LogError("Stack trace: " + e.StackTrace);
        }
    }

    private IEnumerator AplicarTextosPanelAvatar()
    {
        yield return new WaitForSeconds(0.0001f);

        Debug.Log("=== APLICANDO TEXTOS AVATAR ===");

        foreach (KeyValuePair<string, string> par in textosCacheAvatar)
        {
            Debug.Log("Aplicando: " + par.Key + " = '" + par.Value + "'");

            GameObject objeto = GameObject.FindWithTag(par.Key);
            if (objeto != null)
            {
                Text comp = objeto.GetComponent<Text>();
                if (comp != null)
                {
                    string textoAntes = comp.text;
                    comp.text = par.Value;
                    Debug.Log("✅ Aplicado en " + objeto.name + ": '" + textoAntes + "' → '" + comp.text + "'");
                }
                else
                {
                    Debug.LogWarning("❌ No hay componente Text en: " + objeto.name);
                }
            }
            else
            {
                Debug.LogWarning("❌ No se encontró objeto con tag: " + par.Key);
            }
        }
    }

    private IEnumerator AplicarTextosPanelDatos()
    {
        yield return new WaitForSeconds(0.0001f);

        Debug.Log("=== APLICANDO TEXTOS DATOS ===");

        // labels
        foreach (KeyValuePair<string, string> par in textosCacheDatos)
        {
            Debug.Log("Aplicando: " + par.Key + " = '" + par.Value + "'");

            GameObject objeto = GameObject.FindWithTag(par.Key);
            if (objeto != null)
            {
                Text comp = objeto.GetComponent<Text>();
                if (comp != null)
                {
                    string textoAntes = comp.text;
                    comp.text = par.Value;
                    Debug.Log("✅ Aplicado en " + objeto.name + ": '" + textoAntes + "' → '" + comp.text + "'");
                }
                else
                {
                    Debug.LogWarning("❌ No hay componente Text en: " + objeto.name);
                }
            }
            else
            {
                Debug.LogWarning("❌ No se encontró objeto con tag: " + par.Key);
            }
        }

        // dropdown
        genderDropdown.ClearOptions();
        List<string> visibleOptions = new List<string>();

        foreach (KeyValuePair<string, string> par in opcionesGenero)
        {
            visibleOptions.Add(par.Value);
        }

        genderDropdown.AddOptions(visibleOptions);
    }

    private IEnumerator AplicarTextosPanelPin()
    {
        yield return new WaitForSeconds(0.0001f);

        Debug.Log("=== APLICANDO TEXTOS PIN ===");

        foreach (KeyValuePair<string, string> par in textosCachePin)
        {
            Debug.Log("Aplicando: " + par.Key + " = '" + par.Value + "'");

            GameObject objeto = GameObject.FindWithTag(par.Key);
            if (objeto != null)
            {
                Text comp = objeto.GetComponent<Text>();
                if (comp != null)
                {
                    string textoAntes = comp.text;
                    comp.text = par.Value;
                    Debug.Log("✅ Aplicado en " + objeto.name + ": '" + textoAntes + "' → '" + comp.text + "'");
                }
                else
                {
                    Debug.LogWarning("❌ No hay componente Text en: " + objeto.name);
                }
            }
            else
            {
                Debug.LogWarning("❌ No se encontró objeto con tag: " + par.Key);
            }
        }
    }
}


// CLASES 
[System.Serializable]
public class TextosNuevaPartida
{
    public string title_avatar;
    public string title_datos;
    public string label_nombre;
    public string label_edad;
    public string label_genero;
    public string genero_h;
    public string genero_m;
    public string genero_otro;
    public string label_pin;
    public string pin_invitado;
}

[System.Serializable]
public class CrearCargarPartida
{
    // vacio
}

[System.Serializable]
public class MenuPartidas
{
    public CrearCargarPartida crear_cargar_partida;
    public TextosNuevaPartida nueva_partida;
}

[System.Serializable]
public class JsonCompleto
{
    public MenuPartidas menu_partidas;
}