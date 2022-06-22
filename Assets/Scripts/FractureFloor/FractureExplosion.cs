using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class FractureExplosion : MonoBehaviour
{
    public Vector3 fatherPosition;
    private Rigidbody thisRigidBody;
    private float minForce;
    [SerializeField]private float maxForce;
    [SerializeField] private float minTorque;
    [SerializeField] private float maxTorque;
    private Vector3 currentScale;
  
    //[SerializeField] private float radius;
    

    void Start()
    {
        minForce = maxForce * -1f;
        currentScale = transform.localScale;
        
        thisRigidBody = GetComponent<Rigidbody>();
        thisRigidBody.AddForce(Random.Range(minForce, maxForce), Random.Range(minForce, maxForce), Random.Range(minForce, maxForce), ForceMode.Impulse);
        thisRigidBody.AddTorque(Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), Random.Range(minTorque, maxTorque), ForceMode.Impulse);
        StartCoroutine(Shrink(currentScale));
        Destroy(gameObject, 5f);
    }

    
    void Update()
    {
        transform.localScale = currentScale;
    }

    IEnumerator Shrink (Vector3 newScale)
    {
        yield return new WaitForSeconds(.5f);
        while (currentScale.x > 0)
        {
            newScale = newScale * .99f;
            yield return newScale;
            currentScale = newScale;
        }
             

    }
}


