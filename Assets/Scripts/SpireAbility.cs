using UnityEngine;
using System.Collections;

public class SpireAbility : Ability {
    [Tooltip("The object that will be instantiated and launched.")]
    public GameObject spire;
    public float spawnOffset;

    public float spireCoolDownTime;
    public AudioClip spireGrowAudio;
    public AudioClip spireDestroyAudio;
    public float spireSpawnOffsetFromGround = 8f;

    private int spireCount = 0;
    private ArrayList activeSpires;
    private bool spireActive = false;
    private GameObject spireInstance;
    private float timeUntilSpireSpawn = 0.0f;
    private bool onSpire = false;
    private float joystickThreshold = 0.55f;
    private PlayerController Gus;

    void Start()
    {
        Gus = GetComponent<PlayerController>();
        activeSpires = new ArrayList();
    }

    void Update()
    {
        if (Activate() && !spireActive && isReady() && !Gus.OnSpire())
        {
            //Can only have 3 spires at a time on the map, destroy the oldest existing spire
            if (spireCount == 3)
            {
                GameObject spireToRemove = (GameObject)activeSpires[0];
                activeSpires.RemoveAt(0);
                Destroy(spireToRemove);
                spireCount--;
            }

            //raycasting below player
            spireActive = true;
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, 70.0f);
            Vector3 spawnPos = hit.transform.position;
            spawnPos.y += spireSpawnOffsetFromGround;

            //set spawn position of spire slightly to left or right of player if they are moving horizontally
            float horizontalInput = Input.GetAxis("GusHorizontal");
            if (horizontalInput >= joystickThreshold || horizontalInput <= -joystickThreshold)
            {
                spawnPos.x += Mathf.Sign(horizontalInput) * spawnOffset;
            }

            //if raycast hit the ground
            if (hit.transform.gameObject.tag == "Environment")
            {
                spireInstance = Instantiate(spire, spawnPos, Quaternion.Euler(Vector3.zero)) as GameObject;
                AudioSource.PlayClipAtPoint(spireGrowAudio, transform.position);
                activeSpires.Add(spireInstance);
                spireCount++;
            }

            timeUntilSpireSpawn = Time.time + spireCoolDownTime;
            Debug.Log("SPIRE COUNT: " + spireCount);
        }
        else if(spireActive && (Deactivate() || Gus.UnderPlatform()))
        {
            spireActive = false;
            if (spireInstance != null)
                spireInstance.GetComponent<Animator>().enabled = false;
        }
    }

    //Cooldown is done? Gus can spawn more spires
    bool isReady()
    {
        return Time.time > timeUntilSpireSpawn;
    }

    public int getSpireCount()
    {
        return spireCount;
    }

    public void toggleOnSpire()
    {
        onSpire = !onSpire;
    }

    public void spirePunched(Transform hitSpire)
    {
        spireActive = false;
        onSpire = false;
        activeSpires.Remove(hitSpire.gameObject);
        spireCount--;
        AudioSource.PlayClipAtPoint(spireDestroyAudio, transform.position);
        Destroy(hitSpire.gameObject);
    }

    //Don't spawn a spire if there is already one in your location
    public bool spireOverlap(Vector3 spawnPos)
    {
        RaycastHit hit;
        float rayDistance = spawnPos.x;
        spawnPos.x = transform.position.x;
        float horizontalInput = Input.GetAxis("GusHorizontal");
        if (Physics.Raycast(spawnPos, GetComponent<PlayerController>().GetDirection(), out hit, spawnOffset))
        {
            if (hit.transform.gameObject.tag == "Spire")
            {
                return true;
            }
        }
        return false;
    }

}
