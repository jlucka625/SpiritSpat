using UnityEngine;
using System.Collections;

public class VineBehavior : MonoBehaviour
{
    public float vineSlowdownSpeed;

    private float gusSpeedBeforeCollide;
    private float slowSpeed;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Gus")
        {
            other.gameObject.GetComponent<MessedUpControl>().disorient();
        }

        if (other.tag == "Elemental" && other.name.Contains("FireElemental"))
        {
            FireElementalBehaviour behaviorScript = other.GetComponent<FireElementalBehaviour>();
            behaviorScript.switchDirection();
            behaviorScript.ActivateSporeBehavior();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Gus")
        {
            other.gameObject.GetComponent<PlayerController>().SetSpeed(vineSlowdownSpeed);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Gus")
        {
            other.gameObject.GetComponent<PlayerController>().ResetSpeed();
        }
    }
}