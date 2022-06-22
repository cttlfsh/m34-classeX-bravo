using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Streamer : MonoBehaviour
{
    protected Vector2 inputMovement;
    [SerializeField] private float speed;
    private float turnSmoothTime = 0.01f;
    protected CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    public void Update()
    {
        
        Vector3 finalMovement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f).normalized;
        float turnSmoothVelocity = 0f;

        if (finalMovement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(finalMovement.x*-1, finalMovement.y) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            cc.Move(finalMovement * speed * Time.deltaTime);

        }
    }
}
