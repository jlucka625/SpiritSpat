using UnityEngine;
using System.Collections;

public class ProjectileAbility : Ability
{
    [Tooltip("The object that will be instantiated and launched.")]
    public GameObject projectile;

    [Tooltip("The vertical and horizontal force that will be applied to the object.")]
    public Vector2 force;

    [Tooltip("A multiplier for easily scaling the force components.")]
    public float weakForceMultiplier = 1;
    public float mediumForceMultiplier = 2;
    public float strongForceMultiplier = 3;

    public AudioClip standardProjectileSound;
    public AudioClip chargeShotSound;

    private GameObject chargeGlow;
    public GameObject chargeParticle;

    private float forceMultiplier;

    public float rechargeTime;
    public int maxProjectileCount;

    private int projectileCount;
    private bool shooting = false;
    private float startChargeTime;
    private float startRechargeTime;
    private float proportionalRechargeTime;

    public float mediumChargeTime;
    public float longChargeTime;

    private int projectilesLost = 1;
    private bool soundPlayed = false;

    void Start()
    {
        projectileCount = maxProjectileCount;
        forceMultiplier = weakForceMultiplier;
        proportionalRechargeTime = rechargeTime;
    }

    void Update()
    {
        if (shooting && !soundPlayed)
        {
            float elapsedTime = Time.time - startChargeTime;
            if (elapsedTime >= mediumChargeTime)
            {
                soundPlayed = true;
                GetComponent<AudioSource>().Play();
            }
        }

        //Determines how large of a force to shoot the projectile at based on how long input was held down
        if (Deactivate() && shooting)
        {
            float elapsedTime = Time.time - startChargeTime;
            Debug.Log("ELAPSED TIME: " + elapsedTime);
            projectilesLost = 1;
            if(elapsedTime >= mediumChargeTime && projectileCount >= 2)
            {
                projectilesLost = 2;
                forceMultiplier = mediumForceMultiplier;
            }
            if(elapsedTime >= longChargeTime && projectileCount >= 3)
            {
                projectilesLost = 3;
                forceMultiplier = strongForceMultiplier;
            }

            projectileCount -= projectilesLost;

            if(projectilesLost < 2)
                AudioSource.PlayClipAtPoint(standardProjectileSound, transform.position);
            else
                AudioSource.PlayClipAtPoint(chargeShotSound, transform.position);

            //spawn projectile
            GameObject projectileInstance = 
                Instantiate(projectile, transform.position, Quaternion.Euler(Vector3.forward)) as GameObject;
            projectileInstance.GetComponent<ProjectileContact>().setPower(projectilesLost);
            projectileInstance.GetComponent<Rigidbody>().AddForce(new Vector3(
                force.x * forceMultiplier * GetComponent<PlayerController>().GetDirection().x,
                force.y * forceMultiplier,
                0.0f));
            chargeGlow.transform.parent = projectileInstance.transform;

            if (projectileCount < maxProjectileCount)
            {
                startRechargeTime = Time.time;
                proportionalRechargeTime = rechargeTime * (maxProjectileCount - projectileCount);
            }

            forceMultiplier = weakForceMultiplier;
            shooting = false;
            GetComponent<AudioSource>().Stop();
            soundPlayed = false;
        }
        if (Activate() && projectileCount != 0 && projectileCount != 0 && !shooting)
        {
            //begin charging and checking time
            chargeGlow = Instantiate(chargeParticle, transform.position, Quaternion.Euler(Vector3.zero)) as GameObject;
            chargeGlow.transform.parent = transform;
            startChargeTime = Time.time;
            shooting = true;
        }
        if (Time.time - startRechargeTime >= proportionalRechargeTime)
        {
            recharge();
        }
    }

    void recharge()
    {
        projectileCount = maxProjectileCount;
        startRechargeTime = Time.time;
    }

    public int GetCount()
    {
        return projectileCount;
    }

    public float getRechargeValue()
    {
            return ((Time.time - startRechargeTime) / proportionalRechargeTime) * 5;
    }
    public int getChargedProjectiles()
    {
        if (shooting)
        {
            return Mathf.Clamp((int)(((Time.time - startChargeTime) / longChargeTime) * 3),0, projectileCount);
        }
        else return 0;
    }

    public void DestroyProjectileData()
    {
        Destroy(chargeGlow);
        recharge();
        shooting = false;
        forceMultiplier = weakForceMultiplier;
    }
}
