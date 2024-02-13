using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMapSwitcher : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private PauseMenuScript pauseMenu;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Pause"].performed += SetUIMapActive;
        playerInput.actions["Unpause"].performed += SetPlayerMapActive;
        pauseMenu.OnResumeGame += ResumeGame;
    }

    private void ResumeGame()
    {
        playerInput.SwitchCurrentActionMap("Player");
        GameManager.Instance.ResumeGame();
    }

    public void SetPlayerMapActive(InputAction.CallbackContext context)
    {
        playerInput.SwitchCurrentActionMap("Player");
        GameManager.Instance.ResumeGame();
    }

    void OnDisable()
    {
        playerInput.actions["Pause"].performed -= SetUIMapActive;
        playerInput.actions["Unpause"].performed -= SetPlayerMapActive;
    }

    public void SetUIMapActive(InputAction.CallbackContext context)
    {
        playerInput.SwitchCurrentActionMap("UI");
        GameManager.Instance.PauseGame();
    }


}
