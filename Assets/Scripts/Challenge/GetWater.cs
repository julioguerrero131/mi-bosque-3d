using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetWater : MonoBehaviour
{
    public GameObject panelwater;
    public GameObject panelbalde;
    public Text time;
    public Text recordatorio;
    public GameObject agua;
    public GameObject pendienteGO;


    private void OnMouseDown()
    {
        LanguageManager lm = LanguageManager.Instancia;
        string txt0 = lm.ObtenerTexto("recordatorios.help_fogata_0");
        string txt1 = lm.ObtenerTexto("recordatorios.help_fogata_1");
        string txt2 = lm.ObtenerTexto("recordatorios.help_fogata_2");

        if (time.text != "0" && (panelbalde.activeSelf==true) && !(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas))
        {
            agua.SetActive(true);
            panelbalde.SetActive(false);
            recordatorio.text = txt0;
            panelwater.SetActive(true);
            pendienteGO.GetComponent<DialogueTrigger>().dialogue.sentences[0] = txt1;
        }
        else if (time.text != "0" && (panelbalde.activeSelf == false) && (panelwater.activeSelf == false) && !(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas))
        {
            recordatorio.text = txt2;
        }
        
       
    }
    public void recogerAgua()
    {
        LanguageManager lm = LanguageManager.Instancia;
        string txt0 = lm.ObtenerTexto("recordatorios.help_fogata_0");
        string txt1 = lm.ObtenerTexto("recordatorios.help_fogata_1");
        string txt2 = lm.ObtenerTexto("recordatorios.help_fogata_2");

        if (time.text != "0" && (panelbalde.activeSelf == true) && !(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas))
        {
            agua.SetActive(true);
            panelbalde.SetActive(false);
            recordatorio.text = txt0;
            panelwater.SetActive(true);
            pendienteGO.GetComponent<DialogueTrigger>().dialogue.sentences[0] = txt1;
        }
        else if (time.text != "0" && (panelbalde.activeSelf == false) && (panelwater.activeSelf == false) && !(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas))
        {
            recordatorio.text = txt2;
        }
    }

    /*private void OnTriggerEnter(Collider other)
    {
        Debug.Log("algo en el aguaaaaaaaaaaaaaaaa");
        //if (other.CompareTag("Player"))
        {
            if (time.text != "0" && (panelbalde.activeSelf == true) && !(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas))
            {
                agua.SetActive(true);
                panelbalde.SetActive(false);
                recordatorio.text = "Rápido, corre a la fogata y apágala!";
                panelwater.SetActive(true);
                pendienteGO.GetComponent<DialogueTrigger>().dialogue.sentences[0] = "No hay tiempo que perder! Apaga la fogata ahora que tienes el agua!";
            }
            else if (time.text != "0" && (panelbalde.activeSelf == false) && (panelwater.activeSelf == false) && !(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas))
            {
                recordatorio.text = "Busca algo en que llevar agua!";
            }
        }
        
    }*/
}