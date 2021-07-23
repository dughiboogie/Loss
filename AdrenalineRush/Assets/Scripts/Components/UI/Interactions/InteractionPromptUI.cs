using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public TextMeshProUGUI interactionType;
    public TextMeshProUGUI interactableName;

    #region Singleton

    public static InteractionPromptUI instance;

    private void Awake()
    {
        if(instance != null) {
            Debug.LogWarning("Multiple instances of InteractionPromptUI found!");
            return;
        }
        instance = this;

        gameObject.SetActive(false);
    }

    #endregion

    public void EnableInteractionPrompt()
    {
        gameObject.SetActive(true);
        // Disable jump action (same key as interaction action)
        GameManager.instance.jumpAction.Disable();
    }

    public void DisableInteractionPrompt()
    {
        gameObject.SetActive(false);
        // Re-enable jump action
        GameManager.instance.jumpAction.Enable();
    }

}
