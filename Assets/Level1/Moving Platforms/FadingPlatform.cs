using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    [Header("Values to change the alpha")]
    [Tooltip("Upper range is the highest the alpha will rise to, can be set over 1 which will make the alpha 1 for a longer period of time")]
    public float upperRange = 1.1f;
    [Tooltip("Lower range is the lowest the alpha will lower to, can be set under 0 which will make the alpha 0 for a longer period of time")]
    public float lowerRange = -0.1f;
    [Tooltip("Threshold under which the Collider is disabled")]
    public float threshold = 0.6f;
    [Tooltip("Speed at which the alpha changes")]
    public float speed = 0.3f;

    public bool isActive = false;
    public bool randomStart = true;
    
    float phase = 1;
    float phaseDir = 1;

    MeshRenderer meshRenderer;
    Color origColor;
    MeshCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        origColor = meshRenderer.material.color;
        collider = gameObject.GetComponent<MeshCollider>();

        if (randomStart)
        {
            phase = Random.Range(0f, 1f);
        }
        meshRenderer.material.color = new Color(origColor.r, origColor.g, origColor.b, phase);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            UpdatePhase();
            meshRenderer.material.color = new Color(origColor.r, origColor.g, origColor.b, phase);
            if (phase < threshold)
            {
                collider.enabled = false;
            } else 
            {
                collider.enabled = true;
            }
        }
    }

    private void UpdatePhase()
    {
        phase += speed * phaseDir * Time.deltaTime;

        if (phase >= upperRange)
        {
            phaseDir = -1;
        }
        else if (phase <= lowerRange)
        {
            phaseDir = 1;
        }
    }
}
