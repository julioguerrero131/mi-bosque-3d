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
        if(datos.botones != null)
        {
            textos["botones.retroceder"] = datos.botones.retroceder;
            textos["botones.continuar"] = datos.botones.continuar;
            textos["botones.informacion"] = datos.botones.informacion;
        }
        
        if (datos.menu_inicial != null)
        {
            textos["menu_inicial.boton_jugar"] = datos.menu_inicial.boton_jugar;
            textos["menu_inicial.boton_explorar"] = datos.menu_inicial.boton_explorar;
            textos["menu_inicial.boton_salir"] = datos.menu_inicial.boton_salir;
        }

        if (datos.menu_partidas != null)
        {
            if(datos.menu_partidas.nueva_partida != null)
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

            if(datos.menu_partidas.crear_cargar_partida != null)
            {
                var cnp = datos.menu_partidas.crear_cargar_partida;
                textos["menu_partidas.cargar_partida"] = cnp.cargar_partida;
                textos["menu_partidas.nueva_partida"] = cnp.nueva_partida;
            }
        }

        if(datos.tutorial != null)
        {
            textos["tutorial.dialogo_0"] = datos.tutorial.dialogo_0;
            textos["tutorial.dialogo_1"] = datos.tutorial.dialogo_1;
            textos["tutorial.dialogo_2"] = datos.tutorial.dialogo_2;
            textos["tutorial.dialogo_3"] = datos.tutorial.dialogo_3;
            textos["tutorial.dialogo_4"] = datos.tutorial.dialogo_4;
            textos["tutorial.dialogo_5"] = datos.tutorial.dialogo_5;
            textos["tutorial.dialogo_6"] = datos.tutorial.dialogo_6;
            textos["tutorial.dialogo_7"] = datos.tutorial.dialogo_7;
            textos["tutorial.dialogo_8"] = datos.tutorial.dialogo_8;
            textos["tutorial.dialogo_9"] = datos.tutorial.dialogo_9;
            textos["tutorial.dialogo_10"] = datos.tutorial.dialogo_10;
            textos["tutorial.dialogo_11"] = datos.tutorial.dialogo_11;
            textos["tutorial.dialogo_12"] = datos.tutorial.dialogo_12;
        }

        if (datos.lobby != null)
        {
            if (datos.lobby.bruno != null)
            {
                var br = datos.lobby.bruno;
                textos["lobby.bruno.title_0"] = br.title_0;
                textos["lobby.bruno.title_1"] = br.title_1;
                textos["lobby.bruno.title_2"] = br.title_2;
                textos["lobby.bruno.sentence_0"] = br.sentence_0;
                textos["lobby.bruno.sentence_1"] = br.sentence_1;
                textos["lobby.bruno.sentence_2"] = br.sentence_2;
            }

            if (datos.lobby.cabanas_info != null)
            {
                var ci = datos.lobby.cabanas_info;
                textos["lobby.cabanas_info.title_animales"] = ci.title_animales;
                textos["lobby.cabanas_info.sentence_animales_0"] = ci.sentence_animales_0;
                textos["lobby.cabanas_info.sentence_animales_1"] = ci.sentence_animales_1;
                textos["lobby.cabanas_info.sentence_animales_2"] = ci.sentence_animales_2;

                textos["lobby.cabanas_info.title_aves"] = ci.title_aves;
                textos["lobby.cabanas_info.sentence_aves_0"] = ci.sentence_aves_0;
                textos["lobby.cabanas_info.sentence_aves_1"] = ci.sentence_aves_1;

                textos["lobby.cabanas_info.title_flora"] = ci.title_flora;
                textos["lobby.cabanas_info.sentence_flora_0"] = ci.sentence_flora_0;
                textos["lobby.cabanas_info.sentence_flora_1"] = ci.sentence_flora_1;
                textos["lobby.cabanas_info.sentence_flora_2"] = ci.sentence_flora_2;
            }

            if (datos.lobby.excursionistas != null)
            {
                var exc = datos.lobby.excursionistas;
                textos["lobby.excursionistas.title_grupo"] = exc.title_grupo;
                textos["lobby.excursionistas.sentence_grupo_0"] = exc.sentence_grupo_0;
                textos["lobby.excursionistas.sentence_grupo_1"] = exc.sentence_grupo_1;
                textos["lobby.excursionistas.sentence_grupo_2"] = exc.sentence_grupo_2;
            }

            if (datos.lobby.cabana_animal != null)
            {
                var ca = datos.lobby.cabana_animal;
                textos["cabana_animal.title_0"] = ca.title_0;
                textos["cabana_animal.title_1"] = ca.title_1;
                textos["cabana_animal.title_2"] = ca.title_2;

                textos["cabana_animal.animal_0_sentence_0"] = ca.animal_0_sentence_0;
                textos["cabana_animal.animal_0_sentence_1"] = ca.animal_0_sentence_1;
                textos["cabana_animal.animal_0_sentence_2"] = ca.animal_0_sentence_2;

                textos["cabana_animal.animal_1_sentence_0"] = ca.animal_1_sentence_0;
                textos["cabana_animal.animal_1_sentence_1"] = ca.animal_1_sentence_1;
                textos["cabana_animal.animal_1_sentence_2"] = ca.animal_1_sentence_2;

                textos["cabana_animal.animal_2_sentence_0"] = ca.animal_2_sentence_0;
                textos["cabana_animal.animal_2_sentence_1"] = ca.animal_2_sentence_1;
                textos["cabana_animal.animal_2_sentence_2"] = ca.animal_2_sentence_2;

                textos["cabana_animal.animal_3_sentence_0"] = ca.animal_3_sentence_0;
                textos["cabana_animal.animal_3_sentence_1"] = ca.animal_3_sentence_1;
                textos["cabana_animal.animal_3_sentence_2"] = ca.animal_3_sentence_2;

                textos["cabana_animal.animal_4_sentence_0"] = ca.animal_4_sentence_0;
                textos["cabana_animal.animal_4_sentence_1"] = ca.animal_4_sentence_1;
                textos["cabana_animal.animal_4_sentence_2"] = ca.animal_4_sentence_2;

                textos["cabana_animal.animal_5_sentence_0"] = ca.animal_5_sentence_0;
                textos["cabana_animal.animal_5_sentence_1"] = ca.animal_5_sentence_1;
                textos["cabana_animal.animal_5_sentence_2"] = ca.animal_5_sentence_2;

                textos["cabana_animal.animal_6_sentence_0"] = ca.animal_6_sentence_0;
                textos["cabana_animal.animal_6_sentence_1"] = ca.animal_6_sentence_1;
                textos["cabana_animal.animal_6_sentence_2"] = ca.animal_6_sentence_2;

                textos["cabana_animal.animal_7_sentence_0"] = ca.animal_7_sentence_0;
                textos["cabana_animal.animal_7_sentence_1"] = ca.animal_7_sentence_1;
                textos["cabana_animal.animal_7_sentence_2"] = ca.animal_7_sentence_2;

                textos["cabana_animal.animal_8_sentence_0"] = ca.animal_8_sentence_0;
                textos["cabana_animal.animal_8_sentence_1"] = ca.animal_8_sentence_1;
                textos["cabana_animal.animal_8_sentence_2"] = ca.animal_8_sentence_2;

                textos["cabana_animal.animal_9_sentence_0"] = ca.animal_9_sentence_0;
                textos["cabana_animal.animal_9_sentence_1"] = ca.animal_9_sentence_1;
                textos["cabana_animal.animal_9_sentence_2"] = ca.animal_9_sentence_2;
            }

            if (datos.lobby.cabana_aves != null)
            {
                var cav = datos.lobby.cabana_aves;
                textos["cabana_aves.title_0"] = cav.title_0;
                textos["cabana_aves.title_1"] = cav.title_1;
                textos["cabana_aves.title_2"] = cav.title_2;

                textos["cabana_animal.ave_0_sentence_0"] = cav.ave_0_sentence_0;
                textos["cabana_animal.ave_0_sentence_1"] = cav.ave_0_sentence_1;
                textos["cabana_animal.ave_0_sentence_2"] = cav.ave_0_sentence_2;

                textos["cabana_animal.ave_1_sentence_0"] = cav.ave_1_sentence_0;
                textos["cabana_animal.ave_1_sentence_1"] = cav.ave_1_sentence_1;
                textos["cabana_animal.ave_1_sentence_2"] = cav.ave_1_sentence_2;

                textos["cabana_animal.ave_2_sentence_0"] = cav.ave_2_sentence_0;
                textos["cabana_animal.ave_2_sentence_1"] = cav.ave_2_sentence_1;
                textos["cabana_animal.ave_2_sentence_2"] = cav.ave_2_sentence_2;

                textos["cabana_animal.ave_3_sentence_0"] = cav.ave_3_sentence_0;
                textos["cabana_animal.ave_3_sentence_1"] = cav.ave_3_sentence_1;
                textos["cabana_animal.ave_3_sentence_2"] = cav.ave_3_sentence_2;

                textos["cabana_animal.ave_4_sentence_0"] = cav.ave_4_sentence_0;
                textos["cabana_animal.ave_4_sentence_1"] = cav.ave_4_sentence_1;
                textos["cabana_animal.ave_4_sentence_2"] = cav.ave_4_sentence_2;

                textos["cabana_animal.ave_5_sentence_0"] = cav.ave_5_sentence_0;
                textos["cabana_animal.ave_5_sentence_1"] = cav.ave_5_sentence_1;
                textos["cabana_animal.ave_5_sentence_2"] = cav.ave_5_sentence_2;

                textos["cabana_animal.ave_6_sentence_0"] = cav.ave_6_sentence_0;
                textos["cabana_animal.ave_6_sentence_1"] = cav.ave_6_sentence_1;
                textos["cabana_animal.ave_6_sentence_2"] = cav.ave_6_sentence_2;

                textos["cabana_animal.ave_7_sentence_0"] = cav.ave_7_sentence_0;
                textos["cabana_animal.ave_7_sentence_1"] = cav.ave_7_sentence_1;
                textos["cabana_animal.ave_7_sentence_2"] = cav.ave_7_sentence_2;

                textos["cabana_animal.ave_8_sentence_0"] = cav.ave_8_sentence_0;
                textos["cabana_animal.ave_8_sentence_1"] = cav.ave_8_sentence_1;
                textos["cabana_animal.ave_8_sentence_2"] = cav.ave_8_sentence_2;

                textos["cabana_animal.ave_9_sentence_0"] = cav.ave_9_sentence_0;
                textos["cabana_animal.ave_9_sentence_1"] = cav.ave_9_sentence_1;
                textos["cabana_animal.ave_9_sentence_2"] = cav.ave_9_sentence_2;

                textos["cabana_animal.ave_10_sentence_0"] = cav.ave_10_sentence_0;
                textos["cabana_animal.ave_10_sentence_1"] = cav.ave_10_sentence_1;
                textos["cabana_animal.ave_10_sentence_2"] = cav.ave_10_sentence_2;

                textos["cabana_animal.ave_11_sentence_0"] = cav.ave_11_sentence_0;
                textos["cabana_animal.ave_11_sentence_1"] = cav.ave_11_sentence_1;
                textos["cabana_animal.ave_11_sentence_2"] = cav.ave_11_sentence_2;

                textos["cabana_animal.ave_12_sentence_0"] = cav.ave_12_sentence_0;
                textos["cabana_animal.ave_12_sentence_1"] = cav.ave_12_sentence_1;
                textos["cabana_animal.ave_12_sentence_2"] = cav.ave_12_sentence_2;

                textos["cabana_animal.ave_13_sentence_0"] = cav.ave_13_sentence_0;
                textos["cabana_animal.ave_13_sentence_1"] = cav.ave_13_sentence_1;
                textos["cabana_animal.ave_13_sentence_2"] = cav.ave_13_sentence_2;

                textos["cabana_animal.ave_14_sentence_0"] = cav.ave_14_sentence_0;
                textos["cabana_animal.ave_14_sentence_1"] = cav.ave_14_sentence_1;
                textos["cabana_animal.ave_14_sentence_2"] = cav.ave_14_sentence_2;
            }

            if (datos.lobby.cabana_flora != null) 
            {
                var cf = datos.lobby.cabana_flora;
                textos["cabana_flora.title_0"] = cf.title_0;
                textos["cabana_flora.title_1"] = cf.title_1;

                textos["cabana_flora.flora_0_sentence_0"] = cf.flora_0_sentence_0;
                textos["cabana_flora.flora_0_sentence_1"] = cf.flora_0_sentence_1;

                textos["cabana_flora.flora_1_sentence_0"] = cf.flora_1_sentence_0;
                textos["cabana_flora.flora_1_sentence_1"] = cf.flora_1_sentence_1;

                textos["cabana_flora.flora_2_sentence_0"] = cf.flora_2_sentence_0;
                textos["cabana_flora.flora_2_sentence_1"] = cf.flora_2_sentence_1;

                textos["cabana_flora.flora_3_sentence_0"] = cf.flora_3_sentence_0;
                textos["cabana_flora.flora_3_sentence_1"] = cf.flora_3_sentence_1;

                textos["cabana_flora.flora_4_sentence_0"] = cf.flora_4_sentence_0;
                textos["cabana_flora.flora_4_sentence_1"] = cf.flora_4_sentence_1;
            }
        }

        if (datos.controles != null)
        {
            textos["controles.caja_ardilla"] = datos.controles.caja_ardilla;
            textos["controles.caja_iguana"] = datos.controles.caja_iguana;
            textos["controles.caja_pechiche"] = datos.controles.caja_pechiche;
        }

        if (datos.first_challenge != null)
        {
            var fc = datos.first_challenge;
            textos["first_challenge.title_0"] = fc.title_0;
            textos["first_challenge.title_1"] = fc.title_1;
            textos["first_challenge.title_2"] = fc.title_2;
            textos["first_challenge.title_3"] = fc.title_3;
            textos["first_challenge.title_4"] = fc.title_4;

            textos["first_challenge.sentence_0"] = fc.sentence_0;
            textos["first_challenge.sentence_1"] = fc.sentence_1;
            textos["first_challenge.sentence_2"] = fc.sentence_2;
            textos["first_challenge.sentence_3"] = fc.sentence_3;
            textos["first_challenge.sentence_4"] = fc.sentence_4;
        }

        if (datos.conseguir_libro != null)
        {
            var cl = datos.conseguir_libro;
            textos["conseguir_libro.title_0"] = cl.title_0;
            textos["conseguir_libro.title_1"] = cl.title_1;
            textos["conseguir_libro.title_2"] = cl.title_2;

            textos["conseguir_libro.sentence_0"] = cl.sentence_0;
            textos["conseguir_libro.sentence_1"] = cl.sentence_1;
            textos["conseguir_libro.sentence_2"] = cl.sentence_2;
        }

        if(datos.second_challenge != null)
        {
            var sc = datos.second_challenge;
            textos["second_challenge.title_0"] = sc.title_0;
            textos["second_challenge.title_1"] = sc.title_1;
            textos["second_challenge.title_2"] = sc.title_2;

            textos["second_challenge.sentence_0"] = sc.sentence_0;
            textos["second_challenge.sentence_1"] = sc.sentence_1;
            textos["second_challenge.sentence_2"] = sc.sentence_2;

            textos["second_challenge.title_completado"] = sc.title_completado;
            textos["second_challenge.sentence_completado"] = sc.sentence_completado;
            textos["second_challenge.title_pendiente"] = sc.title_pendiente;
            textos["second_challenge.sentence_pendiente"] = sc.sentence_pendiente;
        }

        if (datos.third_challenge != null)
        {
            var tc = datos.third_challenge;
            textos["third_challenge.title_0"] = tc.title_0;
            textos["third_challenge.title_1"] = tc.title_1;

            textos["third_challenge.sentence_0"] = tc.sentence_0;
            textos["third_challenge.sentence_1"] = tc.sentence_1;

            textos["third_challenge.title_completado"] = tc.title_completado;
            textos["third_challenge.sentence_completado"] = tc.sentence_completado;
            textos["third_challenge.title_pendiente"] = tc.title_pendiente;
            textos["third_challenge.sentence_pendiente"] = tc.sentence_pendiente;

            textos["third_challenge.recordatorio"] = tc.recordatorio;
        }

        if (datos.fourth_challenge != null)
        {
            var fc = datos.fourth_challenge;
            textos["fourth_challenge.title_suelo"] = fc.title_suelo;
            textos["fourth_challenge.sentence_suelo"] = fc.sentence_suelo;

            textos["fourth_challenge.title_0"] = fc.title_0;
            textos["fourth_challenge.title_1"] = fc.title_1;
            textos["fourth_challenge.title_2"] = fc.title_2;

            textos["fourth_challenge.sentence_0"] = fc.sentence_0;
            textos["fourth_challenge.sentence_1"] = fc.sentence_1;
            textos["fourth_challenge.sentence_2"] = fc.sentence_2;

            textos["fourth_challenge.title_completado"] = fc.title_completado;
            textos["fourth_challenge.sentence_completado"] = fc.sentence_completado;
            textos["fourth_challenge.title_pendiente"] = fc.title_pendiente;
            textos["fourth_challenge.sentence_pendiente"] = fc.sentence_pendiente;
        }

        if (datos.fifth_challenge != null)
        {
            var ffc = datos.fifth_challenge;
            textos["fifth_challenge.title_vidrio"] = ffc.title_vidrio;
            textos["fifth_challenge.sentence_vidrio"] = ffc.sentence_vidrio;

            textos["fifth_challenge.title_basura"] = ffc.title_basura;
            textos["fifth_challenge.sentence_basura"] = ffc.sentence_basura;

            textos["fifth_challenge.title_0"] = ffc.title_0;
            textos["fifth_challenge.title_1"] = ffc.title_1;

            textos["fifth_challenge.sentence_0"] = ffc.sentence_0;
            textos["fifth_challenge.sentence_1"] = ffc.sentence_1;

            textos["fifth_challenge.title_completado"] = ffc.title_completado;
            textos["fifth_challenge.sentence_completado"] = ffc.sentence_completado;
            textos["fifth_challenge.title_pendiente"] = ffc.title_pendiente;
            textos["fifth_challenge.sentence_pendiente"] = ffc.sentence_pendiente;
        }

        if (datos.sixth_challenge != null)
        {
            var sxc = datos.sixth_challenge;
            textos["sixth_challenge.title_aire"] = sxc.title_aire;
            textos["sixth_challenge.sentence_aire"] = sxc.sentence_aire;

            textos["sixth_challenge.title_0"] = sxc.title_0;
            textos["sixth_challenge.title_1"] = sxc.title_1;

            textos["sixth_challenge.sentence_0"] = sxc.sentence_0;
            textos["sixth_challenge.sentence_1"] = sxc.sentence_1;

            textos["sixth_challenge.title_completado"] = sxc.title_completado;
            textos["sixth_challenge.sentence_completado"] = sxc.sentence_completado;
            textos["sixth_challenge.title_pendiente"] = sxc.title_pendiente;
            textos["sixth_challenge.sentence_pendiente"] = sxc.sentence_pendiente;
        }

        if (datos.seventh_challenge != null)
        {
            var svc = datos.seventh_challenge;
            textos["seventh_challenge.title_0"] = svc.title_0;
            textos["seventh_challenge.title_1"] = svc.title_1;

            textos["seventh_challenge.sentence_0"] = svc.sentence_0;
            textos["seventh_challenge.sentence_1"] = svc.sentence_1;

            textos["seventh_challenge.title_completado_0"] = svc.title_completado_0;
            textos["seventh_challenge.title_completado_1"] = svc.title_completado_1;
            textos["seventh_challenge.sentence_completado_0"] = svc.sentence_completado_0;
            textos["seventh_challenge.sentence_completado_1"] = svc.sentence_completado_1;
            textos["seventh_challenge.title_pendiente_0"] = svc.title_pendiente_0;
            textos["seventh_challenge.title_pendiente_1"] = svc.title_pendiente_1;
            textos["seventh_challenge.sentence_pendiente_0"] = svc.sentence_pendiente_0;
            textos["seventh_challenge.sentence_pendiente_1"] = svc.sentence_pendiente_1;
            textos["seventh_challenge.title_fallado"] = svc.title_fallado;
            textos["seventh_challenge.sentence_fallado"] = svc.sentence_fallado;

            textos["seventh_challenge.title_wellzone_0"] = svc.title_wellzone_0;
            textos["seventh_challenge.title_wellzone_1"] = svc.title_wellzone_1;
            textos["seventh_challenge.title_wellzone_2"] = svc.title_wellzone_2;

            textos["seventh_challenge.sentence_wellzone_0"] = svc.sentence_wellzone_0;
            textos["seventh_challenge.sentence_wellzone_1"] = svc.sentence_wellzone_1;
            textos["seventh_challenge.sentence_wellzone_2"] = svc.sentence_wellzone_2;
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

    // CLASES

    // Contenedor de todas las escenas
    [Serializable]
    public class DatosIdioma
    {
        public Botones botones;
        public MenuInicial menu_inicial;
        public MenuPartidas menu_partidas;
        public Tutorial tutorial;
        public CartelesBosque carteles_bosque;
        public Lobby lobby;
        public Controles controles;
        public FirstChallenge first_challenge;
        public ConseguirLibro conseguir_libro;
        public SecondChallenge second_challenge;
        public ThirdChallengeD third_challenge;
        public FourthChallenge fourth_challenge;
        public FifthChallenge fifth_challenge;
        public SixthChallenge sixth_challenge;
        public SeventhChallenge seventh_challenge;
    }

    // Botones
    [Serializable]
    public class Botones
    {
        public string continuar;
        public string retroceder;
        public string informacion;
    }

    // Menu inicial
    [Serializable]
    public class MenuInicial
    {
        public string boton_jugar;
        public string boton_explorar;
        public string boton_salir;
    }

    // Menu partidas
    [Serializable]
    public class MenuPartidas
    {
        public NuevaPartida nueva_partida;
        public CrearCargarPartida crear_cargar_partida;
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
    public class CrearCargarPartida
    {
        public string cargar_partida;
        public string nueva_partida;
    }

    // Tutorial
    [Serializable]
    public class Tutorial
    {
        public string dialogo_0;
        public string dialogo_1;
        public string dialogo_2;
        public string dialogo_3;
        public string dialogo_4;
        public string dialogo_5;
        public string dialogo_6;
        public string dialogo_7;
        public string dialogo_8;
        public string dialogo_9;
        public string dialogo_10;
        public string dialogo_11;
        public string dialogo_12;
    }

    // Lobby
    [Serializable]
    public class Lobby
    {
        public LobbyBruno bruno;
        public CabanasInfo cabanas_info;
        public Excursionistas excursionistas;
        public CabanaAnimal cabana_animal;
        public CabanaAves cabana_aves;
        public CabanaFlora cabana_flora;
    }

    [Serializable]
    public class LobbyBruno
    {
        public string title_0;
        public string title_1;
        public string title_2;
        public string sentence_0;
        public string sentence_1;
        public string sentence_2;
    }

    [Serializable]
    public class CabanasInfo
    {
        public string title_animales;
        public string sentence_animales_0;
        public string sentence_animales_1;
        public string sentence_animales_2;

        public string title_aves;
        public string sentence_aves_0;
        public string sentence_aves_1;

        public string title_flora;
        public string sentence_flora_0;
        public string sentence_flora_1;
        public string sentence_flora_2;
    }

    [Serializable]
    public class Excursionistas
    {
        public string title_grupo;
        public string sentence_grupo_0;
        public string sentence_grupo_1;
        public string sentence_grupo_2;
    }

    [Serializable]
    public class CabanaAnimal
    {
        public string title_0;
        public string title_1;
        public string title_2;
        
        public string animal_0_sentence_0;
        public string animal_0_sentence_1;
        public string animal_0_sentence_2;

        public string animal_1_sentence_0;
        public string animal_1_sentence_1;
        public string animal_1_sentence_2;

        public string animal_2_sentence_0;
        public string animal_2_sentence_1;
        public string animal_2_sentence_2;

        public string animal_3_sentence_0;
        public string animal_3_sentence_1;
        public string animal_3_sentence_2;

        public string animal_4_sentence_0;
        public string animal_4_sentence_1;
        public string animal_4_sentence_2;

        public string animal_5_sentence_0;
        public string animal_5_sentence_1;
        public string animal_5_sentence_2;

        public string animal_6_sentence_0;
        public string animal_6_sentence_1;
        public string animal_6_sentence_2;

        public string animal_7_sentence_0;
        public string animal_7_sentence_1;
        public string animal_7_sentence_2;
        
        public string animal_8_sentence_0;
        public string animal_8_sentence_1;
        public string animal_8_sentence_2;

        public string animal_9_sentence_0;
        public string animal_9_sentence_1;
        public string animal_9_sentence_2;
    }

    [Serializable]
    public class CabanaAves
    {
        public string title_0;
        public string title_1;
        public string title_2;

        public string ave_0_sentence_0;
        public string ave_0_sentence_1;
        public string ave_0_sentence_2;

        public string ave_1_sentence_0;
        public string ave_1_sentence_1;
        public string ave_1_sentence_2;

        public string ave_2_sentence_0;
        public string ave_2_sentence_1;
        public string ave_2_sentence_2;

        public string ave_3_sentence_0;
        public string ave_3_sentence_1;
        public string ave_3_sentence_2;

        public string ave_4_sentence_0;
        public string ave_4_sentence_1;
        public string ave_4_sentence_2;

        public string ave_5_sentence_0;
        public string ave_5_sentence_1;
        public string ave_5_sentence_2;

        public string ave_6_sentence_0;
        public string ave_6_sentence_1;
        public string ave_6_sentence_2;

        public string ave_7_sentence_0;
        public string ave_7_sentence_1;
        public string ave_7_sentence_2;

        public string ave_8_sentence_0;
        public string ave_8_sentence_1;
        public string ave_8_sentence_2;

        public string ave_9_sentence_0;
        public string ave_9_sentence_1;
        public string ave_9_sentence_2;

        public string ave_10_sentence_0;
        public string ave_10_sentence_1;
        public string ave_10_sentence_2;

        public string ave_11_sentence_0;
        public string ave_11_sentence_1;
        public string ave_11_sentence_2;

        public string ave_12_sentence_0;
        public string ave_12_sentence_1;
        public string ave_12_sentence_2;

        public string ave_13_sentence_0;
        public string ave_13_sentence_1;
        public string ave_13_sentence_2;

        public string ave_14_sentence_0;
        public string ave_14_sentence_1;
        public string ave_14_sentence_2;
    }

    [Serializable]
    public class CabanaFlora
    {
        public string title_0;
        public string title_1;
        public string title_2;

        public string flora_0_sentence_0;
        public string flora_0_sentence_1;

        public string flora_1_sentence_0;
        public string flora_1_sentence_1;

        public string flora_2_sentence_0;
        public string flora_2_sentence_1;

        public string flora_3_sentence_0;
        public string flora_3_sentence_1;

        public string flora_4_sentence_0;
        public string flora_4_sentence_1;
    }

    // Bosque
    [Serializable]
    public class Controles
    {
        public string caja_ardilla;
        public string caja_iguana;
        public string caja_pechiche;
    }

    [Serializable]
    public class FirstChallenge
    {
        public string title_0;
        public string title_1;
        public string title_2;
        public string title_3;
        public string title_4;

        public string sentence_0;
        public string sentence_1;
        public string sentence_2;
        public string sentence_3;
        public string sentence_4;
    }

    [Serializable]
    public class ConseguirLibro
    {
        public string title_0;
        public string title_1;
        public string title_2;

        public string sentence_0;
        public string sentence_1;
        public string sentence_2;
    }

    [Serializable]
    public class SecondChallenge
    {
        public string title_0;
        public string title_1;
        public string title_2;

        public string sentence_0;
        public string sentence_1;
        public string sentence_2;

        public string title_completado;
        public string sentence_completado;
        public string title_pendiente;
        public string sentence_pendiente;
    }

    [Serializable]
    public class ThirdChallengeD
    {
        public string title_0;
        public string title_1;

        public string sentence_0;
        public string sentence_1;

        public string title_completado;
        public string sentence_completado;
        public string title_pendiente;
        public string sentence_pendiente;

        public string recordatorio;
    }

    [Serializable]
    public class FourthChallenge
    {
        public string title_suelo;
        public string sentence_suelo;

        public string title_0;
        public string title_1;
        public string title_2;

        public string sentence_0;
        public string sentence_1;
        public string sentence_2;

        public string title_completado;
        public string sentence_completado;
        public string title_pendiente;
        public string sentence_pendiente;
    }

    [Serializable]
    public class FifthChallenge
    {
        public string title_vidrio;
        public string sentence_vidrio;

        public string title_basura;
        public string sentence_basura;

        public string title_0;
        public string title_1;

        public string sentence_0;
        public string sentence_1;

        public string title_completado;
        public string sentence_completado;
        public string title_pendiente;
        public string sentence_pendiente;
    }

    [Serializable]
    public class SixthChallenge
    {
        public string title_aire;
        public string sentence_aire;

        public string title_0;
        public string title_1;

        public string sentence_0;
        public string sentence_1;

        public string title_completado;
        public string sentence_completado;
        public string title_pendiente;
        public string sentence_pendiente;
    }

    [Serializable]
    public class SeventhChallenge
    {
        public string title_0;
        public string title_1;

        public string sentence_0;
        public string sentence_1;

        public string title_completado_0;
        public string title_completado_1;
        public string sentence_completado_0;
        public string sentence_completado_1;
        public string title_pendiente_0;
        public string title_pendiente_1;
        public string sentence_pendiente_0;
        public string sentence_pendiente_1;
        public string title_fallado;
        public string sentence_fallado;

        public string title_wellzone_0;
        public string title_wellzone_1;
        public string title_wellzone_2;

        public string sentence_wellzone_0;
        public string sentence_wellzone_1;
        public string sentence_wellzone_2;
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
