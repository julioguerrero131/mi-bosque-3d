using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownManager : MonoBehaviour
{
    public Dropdown dropdown;
    
    // Start is called before the first frame update
    void Start()
    {

        if (dropdown == null)
        {
            dropdown = GetComponent<Dropdown>();
        }

        // Opciones
        string nino = LanguageManager.Instancia.ObtenerTexto("menu_partidas.genero_h");
        string nina = LanguageManager.Instancia.ObtenerTexto("menu_partidas.genero_m");
        string otro = LanguageManager.Instancia.ObtenerTexto("menu_partidas.genero_otro");
        List<string> opciones = new List<string>()
        {
            nino,
            nina,
            otro
        };

        ActualizarOpciones(opciones);
    }

    public void ActualizarOpciones(List<string> nuevasOpciones)
    {
        dropdown.ClearOptions(); // Limpia las opciones actuales
        dropdown.AddOptions(nuevasOpciones); // Agrega las nuevas
    }
}
