using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int movementSpeed = 5; 
    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //transform.Rotate( new Vector3(0, 0, Input.GetAxis("Mouse X")*4));

        float h = horizontalSpeed * Input.GetAxis("Mouse X"); // Gets x position of the mouse
        // float v = verticalSpeed * Input.GetAxis("Mouse Y");
        transform.Rotate(0, h, 0); //Rotates object around its y axis


        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * Time.deltaTime * movementSpeed; // Translate the object in the forward direction 
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += transform.right * -1 * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.position += transform.forward * -1 * Time.deltaTime * movementSpeed;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * Time.deltaTime * movementSpeed;
        }
        /*  transform.Rotate(new Vector3(-90, 0, 180));
        */
        if (Input.GetKey(KeyCode.Escape)) // 
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // Locks cursor to center of screen
        } }
    }


