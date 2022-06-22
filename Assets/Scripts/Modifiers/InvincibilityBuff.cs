using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibilityBuff : MonoBehaviour, IIModifiers
{
   public void ApplyEffect(Floor floorAffected)
    {
        print($"{floorAffected.gameObject.name} is invincible");
        floorAffected.isInvincible = true;
    }
}
