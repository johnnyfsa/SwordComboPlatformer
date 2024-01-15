using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public event Action<Transform> OnCheckPointReached;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out Player player))
        {
            //signal StageManager that the checkpoint has been reached
            OnCheckPointReached?.Invoke(transform);
        }
    }
}
