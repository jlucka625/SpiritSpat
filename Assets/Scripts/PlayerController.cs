using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float defaultSpeed;
    public float jumpForce;
    public float jumpCooldownTimeInSeconds;
    public int startingLives;
    public int respawnTimeInSeconds;

    // The button names from the input manager
    public string horizontalAxis;
    public string jumpAxis;
    public string jumpButton;
    public float invulnerableTime;

    public int spawnLimit;
    public AudioClip takeDamageAudio;

    private int spawnCount = 0;

    private Rigidbody rigidBody;
    private float distToCollider;
    private float distToSpire;
    private float joystickHorizontalDeadZone = 0.35f;
    private float joystickVerticalDeadZone = 0.45f;
    private Vector3 direction = Vector3.right;
    private Vector3 previousDirection;
    private float jumpAvailable = 0.0f;
    private float speed;
    private Vector3 spawnPoint;
    private int livesRemaining = 0;
    private bool isAlive = true;
    private int markerCount = 0;
    private float timeLastTookDamage = 0.0f;

    private Animator animationController;

    public void Start()
    {
        animationController = GetComponent<Animator>();
        livesRemaining = startingLives;
        speed = defaultSpeed;
        rigidBody = GetComponent<Rigidbody>();
        distToCollider = GetComponent<Collider>().bounds.extents.y;
        distToSpire = GetComponent<Collider>().bounds.extents.x;
        spawnPoint = transform.position;
    }

    public void Update()
    {
        if (!GameOver.paused)
        {
        	if (!isAlive)
          	  return;

            previousDirection = direction;

            float rawHorizontalInput = Input.GetAxis(horizontalAxis);
            float rawVerticalInput = Input.GetAxis(jumpAxis);
            float horizontalInput = rawHorizontalInput;

            // Set the new direction
            if (rawHorizontalInput > joystickHorizontalDeadZone)
            {
                direction = Vector3.right;
            }
            else if (rawHorizontalInput < -joystickHorizontalDeadZone)
            {
                direction = Vector3.left;
            }
            else
            {
                // Horizontal input is less than the threshold, set to zero
                horizontalInput = 0;
            }

            if (tag == "Gus")
            {
                if (horizontalInput != 0)
                {
                    animationController.SetBool("Walking", true);
                }
                else
                {
                    animationController.SetBool("Walking", false);
                }
            }

            Vector3 movement = new Vector3(horizontalInput * speed, rigidBody.velocity.y, 0.0f);
            rigidBody.velocity = movement;

            if (direction != previousDirection)
            {
                // The direction has changed, update sprite
                FlipSpriteHorizontal();
            }

            if (CanJump())
            {
                // We are grounded, the vertical velocity should be zero in this case
                movement = new Vector3(horizontalInput * speed, 0.0f, 0.0f);
                rigidBody.velocity = movement;

                if ((Time.time > jumpAvailable) &&
                    (rawVerticalInput > joystickVerticalDeadZone ||
                    Input.GetButton(jumpButton)))
                {
                    // Jump
                    rigidBody.AddForce(Vector3.up * jumpForce);
                    jumpAvailable = Time.time + jumpCooldownTimeInSeconds;
                }
            }
        }
    }

    //Is the player touching the ground?
    public bool IsGrounded()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, distToCollider + 0.05f))
        {
            if(hit.transform.gameObject.tag == "Environment" || hit.transform.gameObject.tag == "Spire")
            {
                return true;
            }
        }
        return false;
    }


    public bool CanJump()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToCollider + 0.05f);
    }

    //Is the player currently on top of a spire?
    public bool OnSpire()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, distToCollider + 0.1f))
        {
            if(hit.transform.gameObject.tag == "Spire")
            {
                return true;
            }
        }
        return false;
    }

    //Used to make sure Gus doesn't levitate himself into a platform above him when he spawns a spire
    public bool UnderPlatform()
    {
        return Physics.Raycast(transform.position, Vector3.up, distToCollider + 6f);
    }

    //Checks if Gus is close enough to a spire when he goes to punch it
    public bool hitSpire()
    {
        RaycastHit hit;
        Vector3 punchPos = transform.position;
        punchPos.y -= 2*distToCollider/3;
        if(Physics.Raycast(punchPos, direction, out hit, distToSpire + 14f)){
            if (hit.transform.gameObject.tag == "Spire")
            {
                GetComponent<SpireAbility>().spirePunched(hit.transform.parent);
                return true;
            }
        }
        return false;
    }

    private void FlipSpriteHorizontal()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public float GetSpeed()
    {
        return this.speed;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void ResetSpeed()
    {
        this.speed = defaultSpeed;
    }

    public void TakeDamage(int power)
    {
        if (Time.time > timeLastTookDamage + invulnerableTime)
        {
            timeLastTookDamage = Time.time;
            livesRemaining -= power;
            Debug.Log(gameObject.name + " lives remaining: " + livesRemaining);
            AudioSource.PlayClipAtPoint(takeDamageAudio, transform.position);

            if (livesRemaining < 1)
            {
                // Player died, respawn at base
                livesRemaining = 0;
                Debug.Log(gameObject.name + " died!");
                isAlive = false;
                StartCoroutine(Respawn());
            }
        }
    }

    public int GetLives()
    {
        return livesRemaining;
    }

    public void IncrementSpawnCount()
    {
        spawnCount++;
        if (spawnCount > spawnLimit)
            spawnCount = spawnLimit;
    }

    public void DecrementSpawnCount()
    {
        spawnCount--;
        if (spawnCount < 0)
            spawnCount = 0;
    }

    public int getMarkerCount()
    {
        return markerCount;
    }

    public void IncrementMarkerCount()
    {
        markerCount++;
    }

    public void DecrementMarkerCount()
    {
        if(markerCount > 0)
            markerCount--;
    }


    public int GetSpawnCount()
    {
        return spawnCount;
    }

    IEnumerator Respawn()
    {
        // Dead, disable components
        GetComponent<ProjectileAbility>().DestroyProjectileData();
        GetComponent<Collider>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<ProjectileAbility>().enabled = false;
        if (tag == "Lydia")
        {
            GetComponent<VineAbility>().enabled = false;
        }
        else if (tag == "Gus")
        {
            GetComponent<SpireAbility>().enabled = false;
        }

        yield return new WaitForSeconds(respawnTimeInSeconds);

        // Respawn, enable components
        GetComponent<Collider>().enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<ProjectileAbility>().enabled = true;
        if (tag == "Lydia")
        {
            GetComponent<VineAbility>().enabled = true;
        }
        else if (tag == "Gus")
        {
            GetComponent<SpireAbility>().enabled = true;
        }

        isAlive = true;
        speed = defaultSpeed;
        transform.position = spawnPoint;
        rigidBody.velocity = Vector3.zero;
        livesRemaining = startingLives;
    }
}
