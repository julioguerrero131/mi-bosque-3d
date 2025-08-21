using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorModoJuego : MonoBehaviour
{


    [SerializeField] private Text relojTxt;
    [SerializeField] private GameObject relojBox;
    [SerializeField] private GameObject explicacionContrarrelojCanva;
    [SerializeField] private GameObject despedidaContrarrelojCanva;
    [SerializeField] private GameObject perdidaContrarrelojCanva;
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
        explicacionContrarrelojCanva.SetActive(false);

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
                accionesDeAviso();
            }
            if (minutos < 0)
            {

                perdidaModoContrarreloj();
                activarDespedida();
                               
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
        if (relojBox != null)
        {
            relojBox.SetActive(false);
        }
        IsContrarreloj = false;
        actualizarTextoContador();
    }
    public void activarExplicacion()
    {
        explicacionContrarrelojCanva.SetActive(true);
    }
    public void activarDespedida()
    {
        if (PerdioContrarreloj)
        {
            perdidaContrarrelojCanva.SetActive(true);
        }
        else
        {
            despedidaContrarrelojCanva.SetActive(true);
        }
    }
    public void desactivarDespedida()
    {
        if (PerdioContrarreloj)
        {
            perdidaContrarrelojCanva.SetActive(false);
            PerdioContrarreloj = false;
        }
        else
        {
            despedidaContrarrelojCanva.SetActive(false);
        }
    }
    public void perdidaModoContrarreloj()
    {
        minutos = minutosBase;
        segundos = segundosBase;
        PerdioContrarreloj = true;
        IsSonidoContrarrelojActive = false;
        desactivarCronometro();
        sonidoReloj.detener();
        gameManagerRespawn.TeletransportarJugador();
    }
    private void accionesDeAviso()
    {
        relojTxt.color = colorPeligro;
        IsSonidoContrarrelojActive = true;
        sonidoReloj.reproducir();

    }

}
