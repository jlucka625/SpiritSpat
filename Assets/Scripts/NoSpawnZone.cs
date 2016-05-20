using UnityEngine;
using System.Collections;

public class NoSpawnZone : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Fireball" || collider.gameObject.tag == "Watershot")
        {
            Vector3 velocity = collider.gameObject.GetComponent<Rigidbody>().velocity;
            //collider.gameObject.GetComponent<Rigidbody>().AddForce(-velocity.x * 100, -velocity.y * 100,0);
            velocity.x = 0;
            collider.gameObject.GetComponent<Rigidbody>().velocity = velocity;
        }
        if (collider.gameObject.tag == "Elemental")
        {
            collider.gameObject.GetComponent<FireElementalBehaviour>().switchDirection();
            collider.gameObject.GetComponent<FireElementalBehaviour>().inSafeZone = true;
        }
        if (collider.gameObject.tag == "Gus")
        {
            collider.gameObject.GetComponent<SpireAbility>().enabled = false;
        }
        if (collider.gameObject.tag == "Lydia")
        {
            collider.gameObject.GetComponent<VineAbility>().enabled = false;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Elemental")
        {
            collider.gameObject.GetComponent<FireElementalBehaviour>().inSafeZone = false;
        }
        if (collider.gameObject.tag == "Gus")
        {
            collider.gameObject.GetComponent<SpireAbility>().enabled = true;
        }
        if (collider.gameObject.tag == "Lydia")
        {
            collider.gameObject.GetComponent<VineAbility>().enabled = true;
        }
    }
}
