using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FinalManager : MonoBehaviour
{
    public GameObject camera;
    public GameObject polo;
    public GameObject NPCs;
    public GameObject ballons;
    public GameObject confetti1;
    public GameObject confetti2;
    public AudioSource audioClap;
    public Canvas canvas;
    public Transform target;

    // Velocidades y control
    public int currentSpeed = 6;                 // velocidad de la cámara
    public float diplomaScaleTarget = 0.5f;      // tamaño final del diploma
    public float diplomaScaleSpeed = 3f;         // rapidez del escalado
    public float pauseBeforeCredits = 1.0f;      // pausa antes de créditos

    public GameObject miniCertificado;
    public Text playerName;
    public PlayerData playerData;
    public GameObject diploma;

    bool onDiplome = true;
    GameObject background;
    public GameObject credits;

    // Flags de control
    private bool fadingToCredits = false;
    private bool creditsLaunched = false;

    // Start is called before the first frame update
    void Start()
    {
        loadData();
        background = canvas.transform.GetChild(0).gameObject;
        diploma = canvas.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 2)
        {
            cameraAutoMove();
            statePolo();
        }

        if (onDiplome)
        {
            activeDiplome();
        }

        if (fadingToCredits)
        {
            transitionToCredits();
        }
    }

    void cameraAutoMove()
    {
        Vector3 nextPos = Vector3.MoveTowards(camera.transform.position, target.position, currentSpeed * Time.deltaTime);
        camera.GetComponent<Rigidbody>().MovePosition(nextPos);
    }

    void statePolo()
    {
        if (!audioClap.GetComponent<AudioSource>().isPlaying)
        {
            polo.GetComponent<Animator>().SetInteger("state", 1);
        }
    }

    void activeDiplome()
    {
        if (camera.transform.position.z <= target.position.z)
        {
            miniCertificado.SetActive(false);
            diploma.SetActive(true);

            RectTransform rt = diploma.GetComponent<RectTransform>();
            Vector3 targetScale = Vector3.one * diplomaScaleTarget;
            rt.localScale = Vector3.Lerp(rt.localScale, targetScale, diplomaScaleSpeed * Time.deltaTime);

            if (Vector3.Distance(rt.localScale, targetScale) < 0.01f)
            {
                onDiplome = false;

                if (!creditsLaunched)
                {
                    creditsLaunched = true;
                    StartCoroutine(StartCreditsAfter(pauseBeforeCredits));
                }
            }
        }
    }

    IEnumerator StartCreditsAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        fadingToCredits = true;
    }

    void transitionToCredits()
    {
        audioClap.GetComponent<AudioSource>().Stop();

        Color colorDiplome = diploma.GetComponent<Image>().color;
        colorDiplome.a = Mathf.Lerp(colorDiplome.a, 0.0f, 2 * Time.deltaTime);
        diploma.GetComponent<Image>().color = colorDiplome;

        if (colorDiplome.a < 0.5f)
        {
            diploma.transform.GetChild(0).gameObject.SetActive(false);
        }

        background.SetActive(true);
        Color colorBackground = background.GetComponent<Image>().color;
        colorBackground.a = Mathf.Lerp(colorBackground.a, 1.0f, 2 * Time.deltaTime);
        background.GetComponent<Image>().color = colorBackground;

        if (colorBackground.a > 0.99f)
        {
            offElements();
            credits.SetActive(true);
            playCredits();
            fadingToCredits = false; // evitar que siga llamando
        }
    }

    void playCredits()
    {
        credits.GetComponent<Animator>().Play("Base Layer.Credits", 0, 0);
    }

    void offElements()
    {
        NPCs.SetActive(false);
        ballons.SetActive(false);
        confetti1.SetActive(false);
        confetti2.SetActive(false);
        polo.SetActive(false);
    }

    void loadData()
    {
        playerData = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().playerData;
        playerName.text = playerData.nombre;
    }
}
