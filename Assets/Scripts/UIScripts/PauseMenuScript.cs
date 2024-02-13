using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuScript : MonoBehaviour
{
    public event Action OnResumeGame;
    Button returnToTitle, resume;
    private void OnEnable()
    {
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        returnToTitle = rootVisualElement.Q<Button>("returnToTitle");
        resume = rootVisualElement.Q<Button>("resume");

        returnToTitle.RegisterCallback<ClickEvent>(OnReturnToTitleClicked);
        resume.RegisterCallback<ClickEvent>(OnResumeClicked);
    }

    private void OnResumeClicked(ClickEvent evt)
    {
        OnResumeGame?.Invoke();
    }

    private void OnReturnToTitleClicked(ClickEvent evt)
    {
        GameManager.Instance.LoadStage(0);
    }
}
