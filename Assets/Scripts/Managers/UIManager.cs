using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healTextPrefab;
    public Canvas gameCanvas;

    public GameObject FinishText;
    public WinCondition winCondition;

    void Awake()
    {
        gameCanvas = GameObject.FindObjectOfType<Canvas>();
        winCondition.OnWin += ShowFinishLevelText;

    }

    private void OnEnable()
    {
        CharacterEvents.OnCharacterDamaged += ShowDamageText;
        CharacterEvents.OnCharacterHealed += ShowHealText;
    }

    private void OnDisable()
    {
        CharacterEvents.OnCharacterDamaged -= ShowDamageText;
        CharacterEvents.OnCharacterHealed -= ShowHealText;
        winCondition.OnWin -= ShowFinishLevelText;

    }

    public void ShowDamageText(Transform characterToDamage, int damage)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(characterToDamage.position);
        TMP_Text tmpText = Instantiate(damageTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = damage.ToString();
    }

    public void ShowHealText(Transform characterToHeal, int heal)
    {
        Vector3 spawnPosition = Camera.main.WorldToScreenPoint(characterToHeal.position);
        TMP_Text tmpText = Instantiate(healTextPrefab, spawnPosition, Quaternion.identity, gameCanvas.transform).GetComponent<TMP_Text>();
        tmpText.text = heal.ToString();
    }

    private void ShowFinishLevelText()
    {
        FinishText.SetActive(true);
        winCondition.enabled = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Finish");
    }
}
