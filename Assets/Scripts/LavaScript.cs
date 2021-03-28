using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    Vector3 orig;
    
    public bool rising = false;

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

    private void ResetToHeight(float y)
    {
        transform.position = new Vector3(0, y, 0);
    }
}
