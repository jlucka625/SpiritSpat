using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ElementalBuildup : MonoBehaviour
{
    public uint maxElementalBuildup;
    public GameObject uniqueSpawn;
    public GameObject player;
    private Vector3 defaultScale;
    private Vector3 defaultPosition;
    private uint elementalBuildup = 1;

    void Start()
    {
        defaultScale = transform.localScale;
        defaultPosition = transform.position;
        player = GameObject.FindGameObjectWithTag(player.tag);
    }

    public void IncrementElementalBuildup()
    {
        elementalBuildup++;

        //increase the size of the marker as you shoot projectiles into it
        Vector3 newScale = transform.localScale;
        newScale.y = defaultScale.y + (float)elementalBuildup/2.0f;
        transform.localScale = newScale;

        Vector3 newPosition = transform.position;
        newPosition.y = defaultPosition.y + (float)elementalBuildup / 4.0f;
        transform.position = newPosition;

        //spawn an elemental if the player has shot the right amount of projectiles onto the marker
        if (elementalBuildup >= maxElementalBuildup)
        {
            elementalBuildup = 0;
            UniqueSpawn();
        }
    }

    public void DecrementElementalBuildup()
    {
        elementalBuildup--;
        if (elementalBuildup < 1)
            elementalBuildup = 1;
    }

    void UniqueSpawn()
    {
        int spawnCount = player.GetComponent<PlayerController>().GetSpawnCount();
        int spawnLimit = player.GetComponent<PlayerController>().spawnLimit;
        if (spawnCount < spawnLimit)
        {
            Instantiate(uniqueSpawn, transform.position, Quaternion.Euler(Vector3.zero));
            player.GetComponent<PlayerController>().IncrementSpawnCount();
        }
    }
}
