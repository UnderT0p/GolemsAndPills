using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] public float moveSpeed=3f ;
    [SerializeField] public float runSpeed=6f ;
    [SerializeField] public float crouchSpeed = 1f;
    [SerializeField] private float stamina = 5f;
    public event System.Action<bool> movePlayer;
    public event System.Action stopPlayer;
    private Vector3 direction;
    private bool isGrounded;
    private bool isCrouch;
    private float xRotation;
    public float CurrentSpeed { get;  set; }
    private Rigidbody _rb;
    Vector3 _normal;
    private bool approveRun=false;
    private bool approveMove=true;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        CurrentSpeed = moveSpeed;
    }
    private void FixedUpdate()
    {
        Quaternion angleRot = Quaternion.Euler(Vector3.up* xRotation * Time.fixedDeltaTime);
        _rb.MoveRotation(_rb.rotation * angleRot);   // вызов метода, который прикладывает силу к обьекту 

        if (approveMove&&approveRun)
        {
            CurrentSpeed = runSpeed;
        }
        else if (approveMove && !approveRun&&!isCrouch)
        {
            CurrentSpeed = moveSpeed;
        }
        else if (approveMove && isCrouch)
        {
            CurrentSpeed = crouchSpeed;
        }
        else
        {
            CurrentSpeed = 0;
        }
        MovePlayer(CurrentSpeed);
    }
    private void Update()
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"))*CurrentSpeed ;      
        xRotation = Input.GetAxis("Mouse X") * GameBehavior.GetInstance().Sensetivity;
        isCrouch = GetComponentInChildren<PlayerCrouch>().isCrouch;
        isGrounded = GetComponentInChildren<PlayerGroundChecker>().isGrounded;
        if ((Input.GetAxis("Horizontal") != 0|| Input.GetAxis("Vertical") != 0)&& isGrounded&&Time.timeScale>=1)
        {
            approveMove = true;
            movePlayer?.Invoke(approveRun);
        }
        else if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && !isGrounded && Time.timeScale >= 1)
        {
            approveMove = true;
            direction.x /= 2;
            direction.z /= 2;
            stopPlayer?.Invoke();
        }
        else
        {
            approveMove = false;
            stopPlayer?.Invoke();
        }
        if (Input.GetKey(KeyCode.LeftShift)&& direction.z> 0&&!Input.GetKey(KeyCode.LeftControl) && Time.timeScale >= 1)
        {
            if (stamina>0)
            {
                approveRun = true;
                stamina -= Time.deltaTime;
            }
            else
            {
                approveRun = false;
            }
        }
        else
        {
            approveRun = false;
            if (stamina<5)
            {
                stamina += Time.deltaTime;
            }
        }
    }
    
    private void MovePlayer(float currentSpeed)
    {
        _rb.velocity= transform.rotation * new Vector3(direction.x, _rb.velocity.y, direction.z);
    }
    

}
