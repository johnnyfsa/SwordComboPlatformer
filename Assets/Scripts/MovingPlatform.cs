using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> points;
    private Vector3 nextPosition;
    private Vector3 startingPoint;
    public float moveSpeed = 2.0f;

    private int locationIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.position;
        nextPosition = points[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        //moves to the next location
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
        if (transform.position == nextPosition)
        {
            locationIndex++;
            if (locationIndex >= points.Count)
            {
                locationIndex = 0;
            }
            //once it reaches the location
            nextPosition = points[locationIndex].position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }
}
