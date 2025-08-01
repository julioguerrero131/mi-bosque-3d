using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using System;


public class ClickMouse : MonoBehaviour
{
    private Collider cameraBlocker;

    public GameObject Panel;
    public GameObject Galeria;
    private Galery GaleryScript;
    public GameObject Panel3;
    //private MouseController mouseController;
    private MouseController mouseController;
    public static bool IsGalery = false;
    public GameObject CuadroChallengeDos;
    public bool isAnimal;
    public bool isPlant;
    public bool isKnown;
    public GameObject logroSist;
    public GameObject fpscontroller;
    public GameObject canvasJoy = null;
    public GameObject actionLogger;

    private GameObject puntero;

    private bool tempResult;
    private NotificarLogros NL;

    public string specieName;

    private void Awake()
    {
        //mouseController = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseController>();
        //cameraBlocker = GameObject.FindGameObjectWithTag("Blocker").GetComponent<Collider>();
        
        //GaleryScript = Galeria.GetComponent<Galery>();
    }
    void Start()
    {
        puntero = GameObject.Find("Crosshair/Image");
        actionLogger = GameObject.Find("ActionLogger");
        tempResult = false;
        cameraBlocker = ConstantObjects.instance.cameraBlocker;
        mouseController = ConstantObjects.instance.mouseController;
        Panel.SetActive(false);
        isKnown = false;
        NL = GameObject.Find("NotifLogros").GetComponent<NotificarLogros>();
    }
    private void OnMouseEnter()
    {
        puntero.GetComponent<Puntero>().puntero();
    }
    private void OnMouseExit()
    {
        puntero.GetComponent<Puntero>().mira();
    }
    public void ShowGallery()
    {
        Debug.Log("**********************en el show galery ");
#if UNITY_ANDROID || UNITY_IOS
        canvasJoy.SetActive(false);
#endif
        cameraBlocker = ConstantObjects.instance.cameraBlocker;
        mouseController = ConstantObjects.instance.mouseController;
        
        MenuPausa.instance.Pausar();
        mouseController.enabled = false;
        Panel.SetActive(true);
        Galeria.SetActive(true);
        Panel3.SetActive(false);
        if (GaleryScript==null)
        {
            GaleryScript = Galeria.GetComponent<Galery>();
        }
        GaleryScript.name = specieName;
        GaleryScript.visible = true;
        //La siguiente linea se encarga de registrar un elemento en el libro.
        registrarEspecieId();
        CerrarCuadroChallengeDos();
        IsGalery = true;
        //Time.timeScale = 0f;


        NL.cerrar();
    }

    private void registrarEspecieId()
    {
        Debug.Log("Desde el script ClickMouse de la especie " + specieName + " se lanzo la funcion registrarEspecieId");
        if (gameObject.tag == "Bird")
        {
            string estacionPajaro="";
            try
            {
                estacionPajaro = gameObject.transform.parent.parent.parent.parent.gameObject.GetComponent<Estacion>().ID.ToString();
            }catch(Exception e)
            {
                estacionPajaro = "1";
            }
            BookPages.instance.registrarEspecie(specieName, estacionPajaro);
            return;
        }
        //OBTENER ESTACION ACTUAL AQUI
        string estacion = gameObject.transform.parent.parent.parent.gameObject.GetComponent<Estacion>().ID.ToString();
        //OBTENER ESTACION ACTUAL AQUI
        BookPages.instance.registrarEspecie(specieName, estacion);
        //Tambien se deberia agregar la estacion

    }
    private void OnMouseDown()
    {
        tempResult = false;
        Debug.Log("********************empezando en click mouse" + tempResult);
        try
        {

            if (!isKnown)
            {
                if (isAnimal)
                {
                    if (specieName != "")
                    {
                        Debug.Log("**********************se manda el nombre " + specieName);
                        if (logroSist.GetComponent<LogrosGlobales>().misiones[6].requisitos.Contains(specieName))
                        {
                            tempResult = logroSist.GetComponent<LogrosGlobales>().ProgresarLogro(6);
                            fpscontroller.GetComponent<Player>().gainEXP(1);
                        }
                        logroSist.GetComponent<LogrosGlobales>().ProgresarMision(0, specieName);
                        logroSist.GetComponent<LogrosGlobales>().ProgresarMision(6, specieName);
                    }

                    /*
                    Mision mision = (GameObject.FindGameObjectWithTag("sistema").GetComponent<LogrosGlobales>()).misiones[6];
                    Peticiones.instance.registerPlayerMission(mision.nombre, Player.instance.playerData, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                    Player.instance.playerData.logros[6] = DateTime.Now.ToString();
                    Peticiones.instance.registerPlayerPrize((GameObject.FindGameObjectWithTag("sistema").GetComponent<LogrosGlobales>()).logros[6].nombre, Player.instance.playerData);
                    */
                    //Debug.Log("********************Se volvio" + tempResult);
                }
                else if (isPlant)
                {
                    if (specieName != "")
                    {
                        Debug.Log("**********************se manda el nombre " + specieName);
                        if (logroSist.GetComponent<LogrosGlobales>().misiones[7].requisitos.Contains(specieName))
                        {
                            fpscontroller.GetComponent<Player>().gainEXP(1);
                            tempResult = logroSist.GetComponent<LogrosGlobales>().ProgresarLogro(7);
                        }
                        logroSist.GetComponent<LogrosGlobales>().ProgresarMision(0, specieName);
                        logroSist.GetComponent<LogrosGlobales>().ProgresarMision(7, specieName);
                    }
                    /*
                    Mision mision = (GameObject.FindGameObjectWithTag("sistema").GetComponent<LogrosGlobales>()).misiones[7];
                    Peticiones.instance.registerPlayerMission(mision.nombre, Player.instance.playerData, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                    Player.instance.playerData.logros[7] = DateTime.Now.ToString();
                    Peticiones.instance.registerPlayerPrize((GameObject.FindGameObjectWithTag("sistema").GetComponent<LogrosGlobales>()).logros[7].nombre, Player.instance.playerData);
                    */
                    
                    //Debug.Log("********************Se volvio" + tempResult);
                }
                isKnown = true;
            }

            actionLogger.GetComponent<ActionLogger>().actionLogger.agregarAccion("Interact especie", specieName);
        }
        catch (Exception e)
        {
        }
        //Debug.Log("**********************antes del if es " + tempResult);
        if (!(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas || tempResult))
        {
            //Debug.Log("**********************antes show galery ");
            ShowGallery();
            cameraBlocker.enabled = true;
        }

    }


    public void Continuar()
    {
#if UNITY_ANDROID || UNITY_IOS
        canvasJoy.SetActive(true);
#endif
        cameraBlocker = ConstantObjects.instance.cameraBlocker;
        mouseController = ConstantObjects.instance.mouseController;
        Time.timeScale = 1f;
        GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>().enabled = true;
        Galeria.GetComponent<Galery>().visible = false;
        MenuPausa.instance.Reanudar();
        mouseController.enabled = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Panel.SetActive(false);
        Panel3.SetActive(true);
        Galeria.SetActive(false);
        IsGalery = false;
        cameraBlocker.enabled = false;
    }
    
    public void CerrarCuadroChallengeDos()
    {
        if (CuadroChallengeDos != null)
        {
            CuadroChallengeDos.SetActive(false);
        }


    }


}
