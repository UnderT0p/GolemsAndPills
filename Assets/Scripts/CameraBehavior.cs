using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float sensitivity=2;
    [SerializeField] private float smoothing = 0.2f;
    [SerializeField] private GameObject menu;
    Vector2 velocity;
    Vector2 frameVelocity;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor to the game screen.
    }
    private void Update()
    {
        if (Time.timeScale!=0f)
        {
            Vector2 mouseDelta = new Vector2(0, Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);
            transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        }
    }
}

    

