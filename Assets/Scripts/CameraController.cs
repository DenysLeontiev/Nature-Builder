using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Look Sensitivity")]
    [SerializeField] private float sensitivityX;
    [SerializeField] private float sensitivityY;

    [Header("Look Bordes")]
    [SerializeField] private float minY = -90f;
    [SerializeField] private float maxY = 90f;

    [Header("Speed")]
    [SerializeField] private float moveCameraSpeed;

    private float rotationX;
    private float rotationY;

    private bool isCursorVisible;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleCursorState();
    }

    private void LateUpdate()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        if(isCursorVisible == true) 
            transform.rotation = Quaternion.Euler(-rotationY, rotationX, 0);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float y = 0;

        if (Input.GetKey(KeyCode.E))
        {
            y = -1;
        }
        else if(Input.GetKey(KeyCode.Q))
        {
            y = 1;
        }

        Vector3 moveDir = transform.right * x + transform.up * y + transform.forward * z;
        transform.position += moveDir * moveCameraSpeed * Time.deltaTime;
    }

    private void HandleCursorState()
    {
        if (Input.GetKeyUp(KeyCode.L))
        {
            isCursorVisible = !isCursorVisible;

            if (isCursorVisible)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
