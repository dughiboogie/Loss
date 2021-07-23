using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject resumeButton;

    public void PauseButtonPressed(InputAction.CallbackContext context)
    {
        if(context.performed) {
            if(GameManager.instance.gameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Back(InputAction.CallbackContext context)
    {
        Resume();
    }


    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameManager.instance.gameIsPaused = true;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(resumeButton);

        /*
         * TODO Write on Unity forum for errors on playerInput.SwitchCurrentActionMap() 
         * Interaction index out of range, 
         * UnityEngine.InputSystem.PlayerInput:SwitchCurrentActionMap(String)
         */
        GameManager.instance.playerInput.SwitchCurrentActionMap("Menu");
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.gameIsPaused = false;

        GameManager.instance.playerInput.SwitchCurrentActionMap("Gameplay");
    }

    public void LoadMenu()
    {

    }
    
    public void QuitGame()
    {
        /*
         * TODO Works only in Unity Editor, in official release version call
         * Application.Quit();
         */
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
