using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] private float distanceToGroundForMove = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    public event System.Action Grounded;
    private CapsuleCollider _col;
    public bool isGrounded;
    private void Start()
    {
        _col = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (IsGroundedNow(distanceToGroundForMove) && !isGrounded)
        {
            Grounded?.Invoke();
        }
        isGrounded = IsGroundedNow(distanceToGroundForMove);
    }
    private bool IsGroundedNow(float distanceToGround)
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x, _col.bounds.min.y, _col.bounds.center.z);
        bool grounded = Physics.CheckCapsule(_col.bounds.center, capsuleBottom, distanceToGround, groundLayer, QueryTriggerInteraction.Ignore); // радиус капсулы -это
                                                                                                                                // радиус полусфер                                                                                                                                  
        return grounded;
    }
}
