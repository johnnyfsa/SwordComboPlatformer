using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField]
    private Damageble playerDamageble;
    public GameObject enemyPrefab;

    public List<Checkpoint> checkpoints;
    public List<FallingPlatform> fallingPlatforms;
    private List<Vector3> fallingPlaftformsStartPositions = new List<Vector3>();
    public List<Enemy> enemies;

    private void Start()
    {
        if (checkpoints.Count > 0)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                checkpoint.OnCheckPointReached += SetNewCheckpoint;
            }
        }

        foreach (FallingPlatform platform in fallingPlatforms)
        {
            fallingPlaftformsStartPositions.Add(platform.transform.position);
        }
    }

    private void OnDestroy()
    {
        foreach (Checkpoint checkpoint in checkpoints)
        {
            checkpoint.OnCheckPointReached -= SetNewCheckpoint;
        }
    }

    private void SetNewCheckpoint(Transform transform)
    {
        /*iterate over the checkpoints, 
        once reached the checkpoint that has the same transform.position as the recieved transform, 
        swap the position of the 0 index in the checkpoint list*/
        for (int i = 0; i < checkpoints.Count; i++)
        {
            if (checkpoints[i].transform.position == transform.position)
            {
                //swap checkpoints[i] and chackpoints[0]
                Checkpoint aux = checkpoints[0];
                checkpoints[0] = checkpoints[i];
                checkpoints[i] = aux;
            }
        }

    }

    private void Update()
    {
        if (!playerDamageble.IsAlive)
        {
            //if the player is dead reset its position to the first chekpoint on the list
            StartCoroutine(ResetPlayerPositionAndHealth());
        }
    }

    private IEnumerator ResetPlayerPositionAndHealth()
    {
        yield return new WaitForSeconds(1.5f);
        playerDamageble.ResetHealth();
        playerDamageble.transform.position = checkpoints[0].transform.position;
        ResetFallingPlatforms();
        yield return null;
    }


    private void ResetFallingPlatforms()
    {
        foreach (FallingPlatform fallingPlatform in fallingPlatforms)
        {
            fallingPlatform.transform.position = fallingPlaftformsStartPositions[fallingPlatforms.IndexOf(fallingPlatform)];
            fallingPlatform.ResetFallingPlatform();
            fallingPlatform.gameObject.SetActive(true);
        }
    }
}
