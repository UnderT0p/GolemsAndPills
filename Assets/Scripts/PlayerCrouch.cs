using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    private float heightUp = 2;
    private float heightSit = 1;
    public bool isCrouch = false;
    private bool isCrouchNow = false;
    public event System.Action CrouchStart, CrouchEnd;
    private void FixedUpdate()
    {
        if (isCrouch)
        {
            gameObject.GetComponent<CapsuleCollider>().height = heightSit;
        }
        else
        {
            if (gameObject.GetComponent<CapsuleCollider>().height < 2)
            {
                gameObject.GetComponent<CapsuleCollider>().height = Mathf.Lerp(gameObject.GetComponent<CapsuleCollider>().height, heightUp, 0.1f);
            }
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouchNow = true;
        }
        else
        {
            isCrouchNow = false;
        }
        if (!isCrouch&& isCrouchNow)
        {
            CrouchStart?.Invoke();
        }
        else if (isCrouch && !isCrouchNow)
        {
            CrouchEnd?.Invoke();
        }
        isCrouch = isCrouchNow;
    }
}
