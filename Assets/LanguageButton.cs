using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
    public Button botonIdioma;
    public Text textoBoton;

    private string[] idiomas = { "Español", "English", "Português" };
    private string[] archivos = { "textos_espanol", "textos_english", "textos_portugues" };
    private int idiomaActual = 0;

    void Start()
    {
        if (botonIdioma != null && textoBoton != null)
        {
            // Leer idioma guardado y sincronizar índice
            string idiomaGuardado = PlayerPrefs.GetString("idioma", "textos_espanol");

            for (int i = 0; i < archivos.Length; i++)
            {
                if (archivos[i] == idiomaGuardado)
                {
                    idiomaActual = i;
                    break;
                }
            }

            ActualizarTextoBoton();

            // Asignar función al botón
            botonIdioma.onClick.AddListener(CambiarIdioma);
        }
        else
        {
            Debug.LogError("Faltan referencias en el Inspector.");
        }
    }

    private void ActualizarTextoBoton()
    {
        textoBoton.text = idiomas[idiomaActual];
    }

    public void CambiarIdioma()
    {
        // Cambiar al siguiente idioma
        idiomaActual = (idiomaActual + 1) % idiomas.Length;

        // Actualizar visualmente
        ActualizarTextoBoton();

        // Guardar y aplicar idioma
        string archivoIdioma = archivos[idiomaActual];
        PlayerPrefs.SetString("idioma", archivoIdioma);

        if (LanguageManager.Instancia != null)
        {
            LanguageManager.Instancia.CargarIdioma(archivoIdioma);
            Debug.Log("Idioma cambiado a: " + idiomas[idiomaActual]);
        }
        else
        {
            Debug.LogError("LanguageManager no está inicializado.");
        }
    }
}
