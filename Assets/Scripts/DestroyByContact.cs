using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
    public int lifeCount;
    public string weaknessTag;
    private GameObject Gus;
    private GameObject Lydia;

    void Start()
    {
        Gus = GameObject.FindGameObjectWithTag("Gus");
        Lydia = GameObject.FindGameObjectWithTag("Lydia");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == weaknessTag)
        {
            int power = other.gameObject.GetComponent<ProjectileContact>().getPower();
            lifeCount -= power;
            Destroy(other.gameObject);
            if (lifeCount < 1)
            {
                if(gameObject.tag == "Spire"){
                    Gus.GetComponent<SpireAbility>().spirePunched(transform);
                }
                else if(gameObject.tag == "Vine")
                {
                    Lydia.GetComponent<VineAbility>().vineRemoved(transform);
                }
                Destroy(gameObject);
            }
        }
    }
}
