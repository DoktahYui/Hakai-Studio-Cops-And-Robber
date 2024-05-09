using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{//walkSpeed = 5f, diagonalSpeed = Mathf.Sqrt(50f)
    public Vector3 temp;
    public Vector3 velocity;
    public float walkSpeed;
    public float diagonalSpeed;
    public float playerSpeed;
    public Vector3 gravityVal;
}
