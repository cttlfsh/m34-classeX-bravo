using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorExplosion : MonoBehaviour
{
    [SerializeField] private GameObject FracturedObj;
    [SerializeField] private float maxExplosionForce;
    [SerializeField] private float minExplosionForce;
    [SerializeField] private float explosionRange;

    [SerializeField] private List<GameObject> FracturedObjList;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject thisFractures = Instantiate(FracturedObj, transform.position, Quaternion.identity);
            print(FracturedObj.transform.childCount);
            //Explode();
            Destroy(gameObject);  
        }

        void Explode()
        {
            //foreach(Transform t in FracturedObj.transform)
            //{
            //    var rb = t.GetComponent<Rigidbody>();
            //    if(rb != null)
            //    {
            //        rb.AddExplosionForce(Random.Range(minExplosionForce, maxExplosionForce), transform.position, explosionRange);
            //    }
            //}
            for(int i = 0; i < FracturedObj.transform.childCount; i++)
            {
                Rigidbody rb = FracturedObj.transform.GetChild(i).GetComponent<Rigidbody>();
                //if (rb != null)
                //{
                //rb.AddForce(new Vector3 (Random.Range(minExplosionForce, maxExplosionForce), Random.Range(minExplosionForce, maxExplosionForce), Random.Range(minExplosionForce, maxExplosionForce)), ForceMode.Impulse);
               // }
              
            }
        }
    }
}
