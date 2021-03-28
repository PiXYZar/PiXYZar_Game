using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockShot : MonoBehaviour
{
    float currentAge = 0f;
    public float maxAge = 5f;
    public float ageRate = 1f;

    public float power = 5f;


    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        currentAge = Utils.UpdateEnergyCapped(currentAge, maxAge, ageRate);

        if (currentAge >= maxAge) 
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        //gameObject.GetComponent<Rigidbody>().mass = 0.1f;
        Destroy(gameObject.GetComponent<Collider>());
        currentAge = 4;
        
    }
}