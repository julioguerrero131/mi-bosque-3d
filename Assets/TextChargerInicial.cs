using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChargerInicial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("all_texts");
        if (jsonFile == null)
        {
            Debug.LogError("No se encontró el archivo JSON 'all_texts.json' en Resources.");
            return;
        }
        Debug.Log("JSON cargado correctamente:\n" + jsonFile.text);

        if (jsonFile != null)
        {
            BotonesWrapper data = JsonUtility.FromJson<BotonesWrapper>(jsonFile.text);
            if (data != null && data.menu_inicial != null)
            {
                string[] textos = new string[]
                {
                    data.menu_inicial.boton_jugar,
                    data.menu_inicial.boton_explorar,
                    data.menu_inicial.boton_salir
                };

                string[] botones = new string[]
                {
                    "boton_jugar",
                    "boton_explorar",
                    "boton_salir"
                };

                for (int i = 0; i < textos.Length; i++)
                {
                    GameObject texto_boton = GameObject.FindWithTag(botones[i]);
                    if (texto_boton != null)
                    {
                        Text tmp = texto_boton.GetComponent<Text>();
                        if (tmp != null)
                        {
                            tmp.text = textos[i];
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


// CLASES

[System.Serializable]
public class BotonMenu
{
    public string boton_jugar;
    public string boton_explorar;
    public string boton_salir;
}

[System.Serializable]
public class BotonesWrapper
{
    public BotonMenu menu_inicial;
}