using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombDebuff : MonoBehaviour, IIModifiers
{
   public void ApplyEffect(Floor floorAffected)
    {
        Destroy(floorAffected.gameObject);
        for(int i = 0; i < floorAffected.adiacentFloors.Count; i++)
        {
            print(floorAffected.adiacentFloors[i]);
            Destroy(floorAffected.adiacentFloors[i].gameObject);
        }

    }
}
