using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiffycrewFlyCamera : MonoBehaviour {

    [Range(0,0.1f)]
    public float Speed = 0.05f;

    private Camera cam;
    private float yValue = 0.0f;
    

    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        Application.targetFrameRate = 500;
        QualitySettings.vSyncCount = 0;
        
    }


    void Update()
    {
        UpdateCameraMovement();
    }

    void UpdateCameraMovement()
    {
        // WASD QE
        float xAxisValue = Input.GetAxis("Horizontal") * Speed;
        float zAxisValue = Input.GetAxis("Vertical") * Speed;

        if (Input.GetKey(KeyCode.Q))
            yValue -= Speed * 0.05f;
        else if (Input.GetKey(KeyCode.E))
            yValue += Speed * 0.05f;
        else if (yValue != 0)
            yValue += yValue * -0.25f;

        if (cam != null)
        {
            cam.transform.Translate(new Vector3(xAxisValue, yValue, zAxisValue));
        }

        // ROTATE
        if (Input.GetMouseButton(1)) // mouse right-click
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            Vector3 targetRot = transform.rotation.eulerAngles;
            targetRot.y += mouseX;
            targetRot.x -= mouseY;

            float rotationX = targetRot.x < 180 ? targetRot.x : targetRot.x - 360;
            rotationX = Mathf.Clamp(rotationX, -60f, 60f);
            rotationX = rotationX >= 0 ? rotationX : rotationX + 360f;
            targetRot.x = rotationX;

            transform.rotation = Quaternion.Euler(targetRot);
        }
    }

}
