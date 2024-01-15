using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform subject;

    //Starting position of the parallax object
    Vector2 startPos;

    //Stores the Z value of the parallax game object
    float startZ;

    //Camera movement since start of game
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position - startPos;

    //Distance from the target
    float zDistanceFromTarget => transform.position.z - subject.position.z;

    //clipping plane
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    //paralax Factor
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPos = startPos + camMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, startZ);
    }
}
