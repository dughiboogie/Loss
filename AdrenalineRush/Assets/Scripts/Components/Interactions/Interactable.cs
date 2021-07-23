using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour {

    [SerializeField] private float radius = 3f;
    [SerializeField] private Transform interactionTransform;

    public InteractionPromptUI interactionPromptUI;

    protected virtual void Awake()
    {
        interactionPromptUI = InteractionPromptUI.instance;
    }

    public virtual void Interact()
    {
        // This method is meant to be overritten
        Debug.Log("Interacting with: " + transform.name);

        interactionPromptUI.DisableInteractionPrompt();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            interactionPromptUI.EnableInteractionPrompt();
            interactionPromptUI.interactableName.text = gameObject.name;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player") {
            interactionPromptUI.DisableInteractionPrompt();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(interactionTransform == null)
            interactionTransform = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
