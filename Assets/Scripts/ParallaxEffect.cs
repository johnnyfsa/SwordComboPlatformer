using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    private Transform subject;

    //Starting position of the parallax object
    Vector2 startPos;

    //Stores the Z value of the parallax game object
    float startZ;

    //Camera movement since start of game
    Vector2 camMoveSinceStart;

    //Distance from the target
    float zDistanceFromTarget;

    //clipping plane
    float clippingPlane;
    //paralax Factor
    float parallaxFactor;

    // Start is called before the first frame update
    void Start()
    {
        subject = GameObject.Find("Player").transform;
        if (!subject)
        {
            print("null subject");
        }
        startPos = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        camMoveSinceStart = (Vector2)cam.transform.position - startPos;
        zDistanceFromTarget = transform.position.z - subject.position.z;
        clippingPlane = cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane);
        parallaxFactor = Mathf.Abs(zDistanceFromTarget) / clippingPlane;
        Vector2 newPos = startPos + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
