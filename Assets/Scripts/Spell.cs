using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Animator spellAnimator;

    private void Awake()
    {
        spellAnimator = GetComponent<Animator>();
    }
}
