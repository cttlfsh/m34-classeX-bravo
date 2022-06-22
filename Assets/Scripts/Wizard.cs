using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : MonoBehaviour
{
    public Animator wizardAnimator;
    public GameObject spell;
    public Transform magicSpawnPosition;
    private void Awake()
    {
        wizardAnimator = GetComponent<Animator>();
    }
}
