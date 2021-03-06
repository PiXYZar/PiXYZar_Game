using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    Vector3 orig;
    
    public bool rising = false;
    public float resetDistance = 20.0f;
    public float raiseSpeed = 1;
    public float speedMultiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
        orig = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (rising)
        {
            transform.position += transform.up * raiseSpeed * speedMultiplier * Time.deltaTime;
        }
    }

    public void ResetToHeight(float y)
    {
        if (y < transform.position.y)
        {
            transform.position = new Vector3(orig.x, y, orig.z);
        }
    }
}
