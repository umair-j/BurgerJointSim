using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameInput gameInput;
    private bool isWalking;
    [SerializeField]private float speed;
    [SerializeField] private float rotationSpeed;
    private float playerRadius = 0.7f;
    private float playerHeight = 2f;
    private float playerRaycastDistance = 0.4f;
    private float interactionDistance = 2f;
    private Vector3 lastInteractDir;
    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }
    void HandleInteraction()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(InputVector.x, 0, InputVector.y);
        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }
        bool isInteracting = Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hitinfo, interactionDistance);
        if (isInteracting)
        {
            if (hitinfo.transform.TryGetComponent<Counter>(out Counter counter))
            {
                counter.Interact();
            }
        }
    }
    void HandleMovement()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(InputVector.x, 0, InputVector.y);
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, playerRaycastDistance);
        if (canMove)
        {
            transform.position += moveDir * speed * Time.deltaTime;
        }
        else
        {
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, playerRaycastDistance);
            if(canMove)
            {
                transform.position += moveDirX * speed * Time.deltaTime;
            }
            else
            {
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, playerRaycastDistance);
                if (canMove)
                {
                    transform.position += moveDirZ * speed * Time.deltaTime;
                }
                else
                {
                    Debug.Log("Player is stuck");
                }

            }
        }
        
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        isWalking = moveDir != Vector3.zero;
    }
    public bool IsWalking()
    {
        return isWalking;
    }
}
