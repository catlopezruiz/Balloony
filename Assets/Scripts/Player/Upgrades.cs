using UnityEngine;

public class Upgrades : MonoBehaviour
{
    // These are your "Master Records". 
    // I made them public so you can actually watch them change in the Unity Inspector!
    public int currentForce = 1;
    public float currentSpeed = 5f;
    public bool hasBoots = false; 

    public int GetCurrentForce()
    {
        return currentForce;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public bool CheckIfHasBoots()
    {
        return hasBoots;
    }
    public void CollectPowerUpgrade()
    {
        currentForce++;
    }

    public void CollectSpeedUpgrade(float amountToAdd)
    {
        currentSpeed += amountToAdd; 
    }

    public void CollectBootsUpgrade()
    {
        hasBoots = true;
        
    }
}