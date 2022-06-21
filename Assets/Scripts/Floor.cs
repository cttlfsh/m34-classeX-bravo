using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public string PlayerNickname { get; set; }
    public int Move { get; set; }
    public int Points { get; set; }

    [Range(0, 1)] public int towerIndex;


    #region UNITY_METHODS
    #endregion
}
