using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejadorHalos : MonoBehaviour
{
    [SerializeField] private GameObject haloArdilla;
    [SerializeField] private GameObject haloIguana ;
    [SerializeField] private GameObject haloPechiche;
    [SerializeField] private GameObject haloConejo ;
    [SerializeField] private GameObject haloRtn1;
    [SerializeField] private GameObject haloRtn2;
    [SerializeField] private GameObject haloSalamandra;
    [SerializeField] private GameObject haloBalde;
    [SerializeField] private GameObject explicacionPistaCanva;
    private Dictionary<int, List<GameObject>> halosPorEstacion = new Dictionary<int, List<GameObject>>();
    private Coroutine corriendo;
    private KeyCode teclaPista = KeyCode.P;
    public float duracionEncendido = 9f;
    public float duracionMensajePista = 3f;


    private void Awake()
    {
        halosPorEstacion.Add(1, new List<GameObject> { haloArdilla, haloIguana,haloPechiche });
        halosPorEstacion.Add(2, new List<GameObject> { haloArdilla, haloIguana,haloPechiche });
        halosPorEstacion.Add(3, new List<GameObject> { haloConejo });
        halosPorEstacion.Add(4, new List<GameObject> { haloRtn1,haloRtn2,haloSalamandra });
        halosPorEstacion.Add(6,new List<GameObject> { haloBalde });
        halosPorEstacion.Add(7,new List<GameObject> { haloBalde });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(teclaPista))
        {
            int estacionActual = ObtenerEstacionActual();
            if (halosPorEstacion.ContainsKey(estacionActual))
            {
                StartCoroutine(MostrarMensajePista());
                DispararPistaEstacionActual(estacionActual);  
            }

        }
    }
    public void DispararPistaEstacionActual(int estacionActual)
    {

        if (estacionActual <= 0)
        {
            Debug.LogWarning("[ManejadorHalos] No se pudo determinar la estación actual.");
            return;
        }

        if (!halosPorEstacion.TryGetValue(estacionActual, out var lista) || lista == null || lista.Count == 0)
        {
            Debug.Log($"[ManejadorHalos] Sin halos registrados para estación {estacionActual}.");
            return;
        }

        if (corriendo != null) StopCoroutine(corriendo);
        
        corriendo = StartCoroutine(EncenderYApagar(halosPorEstacion[estacionActual]));
    }
    private int ObtenerEstacionActual()
    {

        int estacionActual= GameManager.instance != null ? GameManager.instance.currentStation : -1;
        Debug.Log(estacionActual);
        return estacionActual;
        
    }
    private IEnumerator MostrarMensajePista()
    {
        explicacionPistaCanva.SetActive(true);
        Debug.Log("mostrando la pista");
        yield return new WaitForSeconds(duracionMensajePista);
        explicacionPistaCanva.SetActive(false);
        Debug.Log("cerrando la pista");
    }
    private IEnumerator EncenderYApagar(List<GameObject> lista)
    {
        foreach (var halo in lista)
            if (halo != null) halo.SetActive(true);

        yield return new WaitForSeconds(duracionEncendido);

        foreach (var halo in lista)
            if (halo != null) halo.SetActive(false);
    }
}
