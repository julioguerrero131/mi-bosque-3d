using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorModoJuego : MonoBehaviour
{


    [SerializeField] private Text relojTxt;
    [SerializeField] private GameObject relojBox;
    [SerializeField] private GameObject explicacionContrarreloj;
    [SerializeField] private GameObject despedidaContrarreloj;
    [SerializeField] private MenuPausa menuPausa;
    [SerializeField] private GameManager gameManagerRespawn;
    [SerializeField] private AudioScript sonidoReloj;
    public int minutosBase ;
    public float segundosBase;
    private int minutos;
    private float segundos;
    public bool IsContrarreloj = false;
    public bool PerdioContrarreloj;
    public bool IsExplicacionActive;
    public Color colorPeligro;
    public static bool IsSonidoContrarrelojActive;



    // Start is called before the first frame update
    void Start()
    {
        segundos = segundosBase;
        minutos = minutosBase;
        if (menuPausa == null)
        {
            Debug.Log("menupausa no existe");
        }
        if (relojBox != null)
                relojBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsContrarreloj)
        {
            //actualizarTextoContador();
            contadorMinutos();
            
        }
    }
    public void activarTemporizador()
    {
        
        Debug.Log("activar timer para contrarreloj");
        if (relojBox != null)
        {
            Debug.Log("encontré RelojBox");

            relojBox.SetActive(true);
        }
        IsContrarreloj = true;
        PerdioContrarreloj = false;
        explicacionContrarreloj.SetActive(false);

    }
    private void contadorMinutos()
    {

        segundos = segundos - Time.deltaTime;
        actualizarTextoContador();
        if (segundos < 0)
        {
            minutos = minutos - 1;
            segundos = 59;
            if (minutos < 3)
            {
                Debug.Log("estoy avisando que el tiempo se acaba");
                accionesDeAviso();
            }
            if (minutos < 0)
            {

                activarDespedida();
                perdidaModoContrarreloj();
                               
                Debug.Log(minutos);
                Debug.Log(segundos);

            }
        }
    }
    public void actualizarTextoContador()
    {
        if (!PerdioContrarreloj)
        {
            
            if (segundos < 9.5f)
            { 
                relojTxt.text = minutos.ToString() + ":0" + segundos.ToString("f0");
            }
            else
            {
                relojTxt.text = minutos.ToString() + ":" + segundos.ToString("f0");

            }
        }
        else
        {
            relojTxt.text = "TIEMPOO";
            minutos = minutosBase;
            segundos = segundosBase;
        }

    }
    public void desactivarCronometro()
    {
        Debug.Log("Desactivar timer, volviendo al modo normal");
        if (relojBox != null)
        {
            relojBox.SetActive(false);
        }
        IsContrarreloj = false;
        actualizarTextoContador();
    }
    public void activarExplicacion()
    {
        explicacionContrarreloj.SetActive(true);
    }
    public void activarDespedida()
    {
        despedidaContrarreloj.SetActive(true);
    }
    public void desactivarDespedida()
    {
        Debug.Log("lo siento el temporizador se acabó, vuelve al principio");
        despedidaContrarreloj.SetActive(false);
    }
    public void perdidaModoContrarreloj()
    {
        minutos = minutosBase;
        segundos = segundosBase;
        PerdioContrarreloj = true;
        IsSonidoContrarrelojActive = false;
        desactivarCronometro();
        sonidoReloj.detener();
        Debug.Log("Lo siento, el temporizador se acabó, volverás al inicio");
        gameManagerRespawn.TeletransportarJugador();
    }
    private void accionesDeAviso()
    {
        Debug.Log("antes de camiar color");
        relojTxt.color = colorPeligro;
        Debug.Log("despues change");
        IsSonidoContrarrelojActive = true;
        sonidoReloj.reproducir();

    }

}
