using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public event System.Action Jumped;
    private bool approveJump;
    [SerializeField]private float jumpVelocity = 300f;
    
    private void FixedUpdate()
    {
        if (approveJump)
        {
            GetComponent<Rigidbody>().AddForce((Vector3.up * jumpVelocity), ForceMode.Impulse);
            Jumped?.Invoke();
            approveJump = false;
        }
        
    }
    
    void Update()
    {
        if (Input.GetButtonDown("Jump") && GetComponentInChildren<PlayerGroundChecker>().isGrounded)
        {
            approveJump = true;

        }
        else
        {
            approveJump = false;
        }
    }
}
