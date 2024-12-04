using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObject_to_cam : MonoBehaviour
{
    private bool hasLoggedError = false; // Flag to track if the error has been logged

    void Update()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
        else
        {
            if (!hasLoggedError) // Check if the error has already been logged
            {
                Debug.Log("lỗi chưa tối ưu code th :V kệ đi sau này update sau");
                hasLoggedError = true; // Set the flag to true after logging the error
            }
        }
    }
}

