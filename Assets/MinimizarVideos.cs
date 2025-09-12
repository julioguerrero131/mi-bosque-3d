using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MinimizarVideos : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject videoCanvas;    
    [SerializeField] private VideoPlayer videoPlayer;   
    [SerializeField] private AudioSource audioSource;   
    [SerializeField] private GameObject videoCanvas2;    
    [SerializeField] private VideoPlayer videoPlayer2;   
    [SerializeField] private AudioSource audioSource2;   
    private bool VideoUnoMinimizado;

    private void Start()
    {
        VideoUnoMinimizado = false;
    }
    
    public void OnMinimizar()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop(); 
        }

        if (audioSource != null)
        {
            audioSource.Stop(); 
        }

        if (videoCanvas != null)
        {
            videoCanvas.SetActive(false); 
        }

        Debug.Log("Video1 detenido y minimizado.");
        VideoUnoMinimizado = true;
    }

    public void OnMinimizarVideo2()
    {
        if (videoPlayer2 != null)
        {
            videoPlayer2.Stop(); 
        }

        if (audioSource2 != null)
        {
            audioSource2.Stop();
        }

        if (videoCanvas2 != null)
        {
            videoCanvas2.SetActive(false); 
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

