using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DetectTargets : MonoBehaviour
{
    Collider2D hitBox;
    public List<Collider2D> targets = new List<Collider2D>();
    private void Awake()
    {
        hitBox = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        targets.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        targets.Remove(collision);
    }
}
