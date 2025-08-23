using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MinimizarVideos : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject videoCanvas;    // El panel/canvas que contiene el video
    [SerializeField] private VideoPlayer videoPlayer;   // El componente VideoPlayer
    [SerializeField] private AudioSource audioSource;   // El AudioSource del VideoPlayer (si lo usas separado)
    [SerializeField] private GameObject videoCanvas2;    // El panel/canvas que contiene el video
    [SerializeField] private VideoPlayer videoPlayer2;   // El componente VideoPlayer
    [SerializeField] private AudioSource audioSource2;   // El AudioSource del VideoPlayer (si lo usas separado)
    private bool VideoUnoMinimizado;

    private void Start()
    {
        VideoUnoMinimizado = false;
    }
    // Llamar este método en el botón Minimizar
    public void OnMinimizar()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); // detiene video y audio vinculados
        }

        if (audioSource != null)
        {
            audioSource.Stop(); // por si el audio sigue en loop separado
        }

        if (videoCanvas != null)
        {
            videoCanvas.SetActive(false); // oculta el panel/canvas del video
        }

        Debug.Log("Video1 detenido y minimizado.");
        VideoUnoMinimizado = true;
    }

    public void OnMinimizarVideo2()
    {
        if (videoPlayer2 != null)
        {
            videoPlayer2.Stop(); // detiene video y audio vinculados
        }

        if (audioSource2 != null)
        {
            audioSource2.Stop(); // por si el audio sigue en loop separado
        }

        if (videoCanvas2 != null)
        {
            videoCanvas2.SetActive(false); // oculta el panel/canvas del video
        }

        Debug.Log("Video2 detenido y minimizado.");
    }
    public  void minimizadorVideos()
    {
        if (!VideoUnoMinimizado)
        {
            OnMinimizar();
        }
        else
        {
            OnMinimizarVideo2();
        }
    }
}

