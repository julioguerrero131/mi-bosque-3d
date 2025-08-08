using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueOnClick : MonoBehaviour
{
    public float rayDistance = 3f; // distancia máxima para "clic"

    // Update is called once per frame
    void Update()
    {
        // Solo procesa clic izquierdo si no hay diálogo activo
        if (!DialogueManager.instance.isDialogueActive && Input.GetMouseButtonDown(0))
        {
            // Raycast desde el centro de la pantalla (FPS)
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // Busca si el objeto clickeado tiene DialogueTrigger
                DialogueTrigger trigger = hit.collider.GetComponent<DialogueTrigger>();
                if (trigger != null)
                {
                    // Lanza el diálogo
                    trigger.TriggerDialogue();
                }
            }
        }
    }
}
