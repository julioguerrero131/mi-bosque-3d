﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour
{

    public RawImage rawimage;
    public VideoPlayer videoPlayer;
    public AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
    
        StartCoroutine(PlayVideo());
    

    }

    // Update is called once per frame

    


    IEnumerator PlayVideo() {

        videoPlayer.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared) {
            yield return waitForSeconds;
            break;
            
        }
        
        rawimage.texture = videoPlayer.texture;
        videoPlayer.Play();
        audioSource.Play();
        SceneChanger.videoreproducido =true;
        }

}
