using UnityEngine;
using System.Collections;

public class ProjectileContact : MonoBehaviour
{
    public GameObject projectile;
    public GameObject opponent;

    private float distToWall;
    private int power = 1;

    void Start()
    {
        distToWall = GetComponent<Collider>().bounds.extents.x;
    }

    public void setPower(int value)
    {
        power = value;
        if (value > 3)
            power = 3;
        if (value < 1)
            power = 1;
    }
    public int getPower()
    {
        return power;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!GameOver.paused)
        {
            //Projectile just gets destroyed if it hits a wall
            if(other.tag == "Wall")
            {
                Destroy(gameObject);
            }

            //Spawn a marker (fire/flower) if the projectile hits the ground
            if (other.tag == "Environment")
            {
                GameObject marker = Instantiate(projectile, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
                Vector3 contactPoint = transform.position;
                contactPoint.y = other.transform.position.y + other.bounds.extents.y + marker.GetComponent<Collider>().bounds.extents.y;
                marker.transform.position = contactPoint;
                Destroy(gameObject);
            }

            //Damage enemy if the projectile hits them
            if (other.tag == opponent.tag)
            {
                Destroy(gameObject);
                other.gameObject.GetComponent<PlayerController>().TakeDamage(power);
            }

            if (gameObject.tag == "Fireball")
            {
                if (other.tag == "GusMarker")
                {
                    // Increment elemental buildup for Gus' tile
                    other.gameObject.GetComponent<ElementalBuildup>().IncrementElementalBuildup();
                    Destroy(gameObject);
                }
                else if (other.tag == "LydiaMarker")
                {
                    // Destroy Lydia's marker
                    Destroy(other.gameObject);
                }
                else if (other.tag == "Elemental" && other.name.Contains("PlantElemental"))
                {
                    //Destroy opponent's elemental
                    Destroy(other.gameObject);
                }
            }
            else if (gameObject.tag == "Watershot")
            {
                if (other.tag == "GusMarker")
                {
                    // Destroy Gus' marker
                    Destroy(other.gameObject);
                }
                else if (other.tag == "LydiaMarker")
                {
                    // Increment elemental buildup for Lydia's tile
                    other.gameObject.GetComponent<ElementalBuildup>().IncrementElementalBuildup();
                    Destroy(gameObject);
                }
                else if (other.tag == "Elemental" && other.name.Contains("FireElemental"))
                {
                    //Destroy opponent's elemental
                    Destroy(other.gameObject);
                }
            }
        }
    }
}
