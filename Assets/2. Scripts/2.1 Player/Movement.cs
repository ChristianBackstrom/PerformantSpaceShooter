using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float thrustSpeed;
    [SerializeField] private float rotationSpeed;
    
    private PlayerInputs playerInputs;
    private InputAction moveAction;
    
    private Rigidbody2D rigidbody2D;
    private Vector2 input;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        moveAction = playerInputs.Player.Move;
        moveAction.Enable();

        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        GetInputs();
        
        transform.rotation *= Quaternion.Euler(0, 0, input.x * rotationSpeed * Time.deltaTime);

        rigidbody2D.AddForce(transform.right * (thrustSpeed * input.y));
    }

    private void GetInputs()
    {
        input = moveAction.ReadValue<Vector2>();
        input.y = Mathf.Clamp(input.y, 0, 1);
    }
}
