using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    Animator wizardAnimator;

    private void Awake()
    {
        wizardAnimator = GetComponent<Animator>();
    }
}
