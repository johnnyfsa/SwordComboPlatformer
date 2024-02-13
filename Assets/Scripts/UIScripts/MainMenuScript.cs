using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuScript : MonoBehaviour
{
    Button stage1, stage2, stage3;
    private void OnEnable()
    {
        GameManager.Instance.ResumeGame();
        var rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
        stage1 = rootVisualElement.Q<Button>("Stage1");
        stage2 = rootVisualElement.Q<Button>("Stage2");
        stage3 = rootVisualElement.Q<Button>("Stage3");

        stage1.RegisterCallback<ClickEvent>(OnStage1Clicked);
        stage2.RegisterCallback<ClickEvent>(OnStage2Clicked);
        stage3.RegisterCallback<ClickEvent>(OnStage3Clicked);
    }

    private void OnStage3Clicked(ClickEvent evt)
    {
        GameManager.Instance.LoadStage(3);
    }

    private void OnStage2Clicked(ClickEvent evt)
    {
        GameManager.Instance.LoadStage(2);
    }

    private void OnStage1Clicked(ClickEvent evt)
    {
        GameManager.Instance.LoadStage(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
