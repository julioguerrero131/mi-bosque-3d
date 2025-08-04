using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextCharger : MonoBehaviour
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
            CartelesWrapper data = JsonUtility.FromJson<CartelesWrapper>(jsonFile.text);
            if (data != null && data.carteles_bosque != null)
            {
                string[] textos = new string[]
                {
                    data.carteles_bosque.entrada_bosque,
                    data.carteles_bosque.primer_nivel,
                    data.carteles_bosque.segundo_nivel,
                    data.carteles_bosque.llegada_lago,
                    data.carteles_bosque.antes_nivel_4
                };

                for (int i = 0; i < textos.Length; i++)
                {
                    string tag = "cartel_" + (i + 1);
                    GameObject cartel = GameObject.FindWithTag(tag);
                    if (cartel != null)
                    {
                        TextMeshPro tmp = cartel.GetComponent<TextMeshPro>();
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

    void AsignarTexto(string clave, string nombreObjeto, Dictionary<string, object> textos)
    {
        if (textos.ContainsKey(clave))
        {
            GameObject cartel = GameObject.Find(nombreObjeto);
            if (cartel != null)
            {
                TextMeshPro tmp = cartel.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = textos[clave].ToString();
                }
                else
                {
                    Debug.LogWarning($"No se encontró TextMeshPro en {nombreObjeto}");
                }
            }
            else
            {
                Debug.LogWarning($"No se encontró GameObject con nombre {nombreObjeto}");
            }
        }
        else
        {
            Debug.LogWarning($"No se encontró clave '{clave}' en el JSON.");
        }
    }
}


// CLASES

[System.Serializable]
public class CartelesBosque
{
    public string entrada_bosque;
    public string primer_nivel;
    public string segundo_nivel;
    public string llegada_lago;
    public string antes_nivel_4;
}

[System.Serializable]
public class CartelesWrapper
{
    public CartelesBosque carteles_bosque;
}