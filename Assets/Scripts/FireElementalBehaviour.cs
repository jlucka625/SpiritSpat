using UnityEngine;
using System.Collections;

public class FireElementalBehaviour : MonoBehaviour {
    public string playerName;
    public GameObject fireProjectile;
    public float lifetime;
    public float speed;
    public float dropTime;
    public bool inSafeZone;

    private bool isAffectedBySpores = false;
    private int direction;
    private float spawnTime;
    private float lastDrop;
    private GameObject player;

    // Use this for initialization
    void Start () {
        inSafeZone = false;
        spawnTime = Time.time;
        lastDrop = Time.time;
        player = GameObject.FindGameObjectWithTag(playerName);
        direction = (int)Mathf.Sign(player.GetComponent<PlayerController>().GetDirection().x);
        if(direction > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }

    public void switchDirection()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        direction = -direction;
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        velocity.x = -velocity.x;
        GetComponent<Rigidbody>().velocity = velocity;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Elemental will move horizontally for it's lifetime and drop projectiles along the way
        if (Time.time - spawnTime < lifetime)
        {
            //only drop projectiles if elemental is in close proximity to Gus
            Vector3 translateX = new Vector3(direction * speed, 0, 0);
            transform.position += translateX;
            float distBetweenPlayerAndElemental = Vector3.Distance(this.transform.position, player.transform.position);

            if (Time.time - lastDrop > dropTime)
            {
                if (distBetweenPlayerAndElemental < 100 && !inSafeZone)
                {
                    GameObject fireProjectileInstance =
                    Instantiate(fireProjectile, transform.position, Quaternion.Euler(Vector3.forward)) as GameObject;
                }
                lastDrop = Time.time;
            }
        }
        else
        {
            Destroy(gameObject);
            player.GetComponent<PlayerController>().DecrementSpawnCount();
        }
	}

    public void ActivateSporeBehavior()
    {
        isAffectedBySpores = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    public bool IsAffectedBySpores()
    {
        return isAffectedBySpores;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Elemental" && other.name.Contains("FireElemental") 
            && other.GetComponent<FireElementalBehaviour>().IsAffectedBySpores())
        {
            Destroy(other.gameObject);
            player.GetComponent<PlayerController>().DecrementSpawnCount();
        }
    }
}
