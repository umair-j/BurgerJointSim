using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameInput gameInput;
    private bool isWalking;
    [SerializeField]private float speed;
    [SerializeField] private float rotationSpeed;
    void Update()
    {
        Vector2 InputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(InputVector.x, 0, InputVector.y);
        transform.position += moveDir * speed * Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        isWalking = moveDir != Vector3.zero;
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
