using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejadorModoJuego : MonoBehaviour
{


    /*public Text relojTxt;
    public GameObject relojBox;*/
    [SerializeField] private Text relojTxt;
    [SerializeField] private GameObject relojBox;
    private int minutos;
    private float segundos;
    public bool IsContrarreloj = false;



    // Start is called before the first frame update
    void Start()
    {
        if (relojBox != null)
                relojBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsContrarreloj)
        {
            actualizarTextoContador();
        }
    }
    public void activarTemporizador()
    {
        Debug.Log("activar timer para contrarreloj");
        IsContrarreloj = true;
        if (relojBox != null)
        {
            Debug.Log("encontré RelojBox");

            relojBox.SetActive(true);
        }
    }
    private void contadorMinutos()
    {
        segundos = segundos + Time.deltaTime;
        if (segundos >= 60)
        {
            segundos = 0;
            minutos = minutos + 1;

        }
    }
    public void actualizarTextoContador()
    {
        contadorMinutos();
        if(segundos < 9.5f)
        {
            relojTxt.text = minutos.ToString() + ":0" + segundos.ToString("f0");
        }
        else
        {
            relojTxt.text = minutos.ToString() + ":" + segundos.ToString("f0");

        }
    }
    public void modoNormal()
    {
        Debug.Log("Desactivar timer, volviendo al modo normal");
        IsContrarreloj = false;
        if (relojBox != null)
        {
            relojBox.SetActive(false);
        }
        minutos = 0;
        segundos = 0;
        actualizarTextoContador();
    }
}
