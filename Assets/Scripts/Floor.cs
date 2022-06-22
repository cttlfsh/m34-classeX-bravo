using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Floor : MonoBehaviour
{
    public string PlayerNickname { get; set; }
    public int Move { get; set; }
    public int Points { get; set; }
    public bool isNPC { get; set; }

    [Range(0, 1)] public int towerIndex;

    private Rigidbody rb;


    #region UNITY_METHODS
    private void Awake()
    {
        SetupRigidbodyParameters();
        isNPC = true;
    }

    private void SetupRigidbodyParameters()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ;
    }
    #endregion
}
