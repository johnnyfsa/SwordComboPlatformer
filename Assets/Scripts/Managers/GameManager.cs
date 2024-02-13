using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action OnSwitchTimeScale;
    [SerializeField]
    private bool isGameOver = false;

    private bool isPaused = false;

    public bool IsPaused
    {
        get { return isPaused; }
        set { isPaused = value; }
    }
    public bool IsGameOver
    {
        get { return isGameOver; }
        set { isGameOver = value; }
    }
    //Game manager is a singleton that cannot be destroyed on load
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

    }

    private void Update()
    {
        if (IsGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    public void PauseGame()
    {
        Time.timeScale = 0;
        IsPaused = true;
        OnSwitchTimeScale?.Invoke();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        IsPaused = false;
        OnSwitchTimeScale?.Invoke();
    }

    public void GameOver()
    {
        StartCoroutine(PlayVictorySFX());
        Time.timeScale = 0;
        IsPaused = true;
    }


    IEnumerator PlayVictorySFX()
    {
        yield return new WaitForSeconds(0f);
        AudioManager.Instance.PlaySFX(SoundType.Victory);
    }
    public void LoadStage(int stageNumber)
    {
        switch (stageNumber)
        {
            case 1:
                SceneManager.LoadScene("Stage1");
                break;
            case 2:
                SceneManager.LoadScene("Stage2");
                break;
            case 3:
                SceneManager.LoadScene("Stage3");
                break;
            default:
                SceneManager.LoadScene("MainMenu");
                break;
        }
    }
}
