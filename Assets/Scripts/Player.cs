using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public float velocity = 1 * Time.deltaTime;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        characterController.Move(transform.forward * velocity);
    }
}
