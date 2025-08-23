using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoDisabled : MonoBehaviour
{
    MinimizarVideos minimizador;
    private void Start()
    {
        minimizador = GameObject.FindObjectOfType<MinimizarVideos>();
    }
    public void OnDisable()
    {
        Debug.Log("video minimizado000000000");
        minimizador.minimizadorVideos();
        MenuPausa.instance.Reanudar();
    }
}
