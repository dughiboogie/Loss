using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;
    public PlayerInput playerInput;
    public InputActionAsset actions;

    public InputAction jumpAction;

    public bool gameIsPaused = false;

    private void Awake()
    {
        #region Singleton

        if(instance != null) {
            Debug.LogWarning("Multiple instances of GameManager found!");
            return;
        }
        instance = this;

        #endregion

        playerInput = player.GetComponent<PlayerInput>();

        jumpAction = playerInput.actions.FindAction("Jump");

    }
}
