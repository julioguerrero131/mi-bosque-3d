using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instancia { get; private set; }

    private Dictionary<string, string> textos = new Dictionary<string, string>();
    public event Action OnIdiomaCambiado;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        DontDestroyOnLoad(gameObject);

        // Comprobar si ya hay un idioma guardado
        string idioma;
        if (PlayerPrefs.HasKey("idioma"))
        {
            idioma = PlayerPrefs.GetString("idioma");
        }
        else
        {
            idioma = "textos_espanol"; // idioma por defecto
            PlayerPrefs.SetString("idioma", idioma);
        }

        Debug.Log("Idioma cargado desde PlayerPrefs: " + idioma);
        CargarIdioma(idioma);
    }

    void Start()
    {
        // Solo para pruebas: eliminar idioma guardado
        PlayerPrefs.DeleteKey("idioma");
        PlayerPrefs.Save();
    }

    public void CargarIdioma(string archivoIdioma)
    {
        PlayerPrefs.SetString("idioma", archivoIdioma);

        TextAsset archivo = Resources.Load<TextAsset>(archivoIdioma);
        if (archivo == null)
        {
            Debug.LogError("Archivo de idioma no encontrado: " + archivoIdioma);
            return;
        }

        // Limpiar el diccionario antes de cargar uno nuevo
        textos.Clear();

        // Deserializar usando JsonUtility
        DatosIdioma datos = JsonUtility.FromJson<DatosIdioma>(archivo.text);
        if (datos == null)
        {
            Debug.LogError("No se pudo deserializar el archivo de idioma.");
            return;
        }

        // Cargar textos del JSON al diccionario plano
        CargarTextos(datos);

        // Notificar a los elementos que el idioma cambió
        OnIdiomaCambiado?.Invoke();
    }

    public string ObtenerTexto(string clave)
    {
        if (textos.TryGetValue(clave, out string texto))
            return texto;
        else
            return $"[{clave}]";
    }

    private void CargarTextos(DatosIdioma datos)
    {
        if (datos.menu_inicial != null)
        {
            textos["menu_inicial.boton_jugar"] = datos.menu_inicial.boton_jugar;
            textos["menu_inicial.boton_explorar"] = datos.menu_inicial.boton_explorar;
            textos["menu_inicial.boton_salir"] = datos.menu_inicial.boton_salir;
        }

        if (datos.menu_partidas != null && datos.menu_partidas.nueva_partida != null)
        {
            var np = datos.menu_partidas.nueva_partida;
            textos["menu_partidas.title_avatar"] = np.title_avatar;
            textos["menu_partidas.title_datos"] = np.title_datos;
            textos["menu_partidas.label_nombre"] = np.label_nombre;
            textos["menu_partidas.label_edad"] = np.label_edad;
            textos["menu_partidas.label_genero"] = np.label_genero;
            textos["menu_partidas.genero_h"] = np.genero_h;
            textos["menu_partidas.genero_m"] = np.genero_m;
            textos["menu_partidas.genero_otro"] = np.genero_otro;
            textos["menu_partidas.label_pin"] = np.label_pin;
            textos["menu_partidas.pin_invitado"] = np.pin_invitado;
            textos["menu_partidas.invitado_1"] = np.invitado_1;
            textos["menu_partidas.invitado_2"] = np.invitado_2;
        }

        if (datos.carteles_bosque != null)
        {
            textos["carteles_bosque.entrada_bosque"] = datos.carteles_bosque.entrada_bosque;
            textos["carteles_bosque.primer_nivel"] = datos.carteles_bosque.primer_nivel;
            textos["carteles_bosque.segundo_nivel"] = datos.carteles_bosque.segundo_nivel;
            textos["carteles_bosque.llegada_lago"] = datos.carteles_bosque.llegada_lago;
            textos["carteles_bosque.antes_nivel_4"] = datos.carteles_bosque.antes_nivel_4;
        }
    }

    // Estructuras que representan el JSON
    [Serializable]
    public class DatosIdioma
    {
        public MenuInicial menu_inicial;
        public MenuPartidas menu_partidas;
        public CartelesBosque carteles_bosque;
    }

    [Serializable]
    public class MenuInicial
    {
        public string boton_jugar;
        public string boton_explorar;
        public string boton_salir;
    }

    [Serializable]
    public class MenuPartidas
    {
        public NuevaPartida nueva_partida;
    }

    [Serializable]
    public class NuevaPartida
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
        public string invitado_1;
        public string invitado_2;
    }

    [Serializable]
    public class CartelesBosque
    {
        public string entrada_bosque;
        public string primer_nivel;
        public string segundo_nivel;
        public string llegada_lago;
        public string antes_nivel_4;
    }
}
