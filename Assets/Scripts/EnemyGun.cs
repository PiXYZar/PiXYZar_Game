using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public GameObject projectile;

    // Ability variables
    public float maxEnergy = 1000f;
    public float energyCost = 150f;
    public float energyRegenRate = 50f;
    public float currentEnergy = 1000f;
    public float projectileSpeed = 60f;
    public float projectileMaxAge = 20f;
    public float projectilePower = 5f;
    
    public float icd = 1f;
    float currenticd = 0f;

    public bool weaponActive = false;

    // Update is called once per frame
    void Update()
    {
        if (weaponActive && currentEnergy > energyCost && currenticd == icd)
        {
            // Gen projectile and pass attributes
            GameObject block = Instantiate(projectile, transform) as GameObject;
            BlockShot temp = block.GetComponent<BlockShot>();
            temp.maxAge = projectileMaxAge;
            temp.power = projectilePower;

            Rigidbody rb = block.GetComponent<Rigidbody>();
            rb.velocity = transform.forward * projectileSpeed;
            currentEnergy -= energyCost;
            currenticd = 0f;
        }

        currentEnergy = Utils.UpdateEnergyCapped(currentEnergy,  maxEnergy, energyRegenRate);
        currenticd = Utils.UpdateEnergyCapped(currenticd, icd, 1f);
    }

    public void setActive(bool state)
    {
        weaponActive = state;
    }
}
