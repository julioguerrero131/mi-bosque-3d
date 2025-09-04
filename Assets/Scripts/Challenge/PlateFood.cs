using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateFood : MonoBehaviour
{
    public bool lleno = false;
    public GameObject feedback;
    public Text message;
    public int recogidos=0;

    private void OnMouseDown()
    {
        if(recogidos == 3 && !(MenuPausa.IsPaused || MenuPausa.IsPausedByOtherCanvas))
        {
            lleno = true;
            StartCoroutine(ShowFeedback());
        }
    }

    IEnumerator ShowFeedback()
    {
        //message.text = "Gran trabajo, avanza al final de la estación";
        message.text = LanguageManager.Instancia.ObtenerTexto("recordatorios.feedback");
        feedback.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        feedback.SetActive(false);
    }
}
