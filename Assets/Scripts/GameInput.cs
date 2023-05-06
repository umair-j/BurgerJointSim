using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 InputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        
        ////////////////////// Discarted legacy input system
        
        //if (Input.GetKey(KeyCode.W))
        //{
        //    InputVector.y = 1;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    InputVector.y = -1;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    InputVector.x = -1;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    InputVector.x = 1;
        //}
        InputVector = InputVector.normalized;
        return InputVector;
    }
}
