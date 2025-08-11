using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class guestlogin : MonoBehaviour
{
    public InputField codigo;
    public int guest=4747;
    public void OnMouseDown()
    {
        Debug.Log("clic");
        codigo.text = ""+guest;

        string text_1 = LanguageManager.Instancia.ObtenerTexto("menu_partidas.invitado_1");
        string text_2 = LanguageManager.Instancia.ObtenerTexto("menu_partidas.invitado_2");

        this.GetComponent<Text>().text = "" + text_1 + guest + text_2;

        Debug.Log("Cambio Realizado: " + text_1 + guest + text_2);
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
