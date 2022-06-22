using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    public string PlayerNickname { get; set; }
    public int Move { get; set; }
    public int Points { get; set; }
    public bool isInvincible;
    public List<Floor> adiacentFloors = new List<Floor>();

    [Range(0, 1)] public int towerIndex;

    private void OnCollisionEnter(Collision collision)
    {
        if(adiacentFloors.Count < 2)
        {
            adiacentFloors.Add(collision.gameObject.GetComponent<Floor>());
        }
    }

    #region UNITY_METHODS
    #endregion
}
