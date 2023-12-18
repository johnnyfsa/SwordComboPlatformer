using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InputManager inputManager;
    public float speed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = inputManager.GetMovementVectorNormalized();
        Vector3 movementVector = new Vector3(inputVector.x, 0, 0);
        transform.position += movementVector * Time.deltaTime * speed;
    }


}
