using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private Interactable interactable = null;
    private bool isInInteractionRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactable = collision.gameObject.GetComponent<Interactable>();

        if(interactable != null) {
            isInInteractionRange = true;
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed) {
            if(isInInteractionRange) {
                interactable.Interact();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = null;
        isInInteractionRange = false;
    }

}
