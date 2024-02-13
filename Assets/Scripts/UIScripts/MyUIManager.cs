using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    private void Start()
    {
        GameManager.Instance.OnSwitchTimeScale += SwitchPauseMenuActive;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnSwitchTimeScale -= SwitchPauseMenuActive;
    }
    private void SwitchPauseMenuActive()
    {
        if (GameManager.Instance.IsPaused)
        {
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.SetActive(false);
        }
    }
}
