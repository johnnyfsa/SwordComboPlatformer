using System;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public event Action OnWin;
    [Serializable]
    public enum WinConditionType
    {
        DestroyTarget,
        ReachPlatform,
        None
    }

    public GameObject targetToDestroy, platformToReach;

    private bool isPlatformReached = false;
    public bool IsPlatformReached
    {
        get
        {
            return isPlatformReached;
        }
        set
        {
            isPlatformReached = value;
        }
    }

    public WinConditionType winConditionType;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (winConditionType)
        {
            case WinConditionType.DestroyTarget:
                if (targetToDestroy == null)
                {
                    OnWin?.Invoke();
                }
                break;
            case WinConditionType.ReachPlatform:
                if (IsPlatformReached)
                {
                    OnWin?.Invoke();
                }
                break;
            case WinConditionType.None:
                return;
        }
    }
}
