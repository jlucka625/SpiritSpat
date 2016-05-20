using UnityEngine;
using System.Collections;

public class VineAbility : Ability
{
    public GameObject vine;
    public AudioClip vineGrowAudio;
    public float spawnOffset = 13f;

    private ArrayList activeVines;
    private int vineCount = 0;

    private PlayerController controller;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        activeVines = new ArrayList();
    }

    void Update()
    {
        if (Activate() && !controller.OnSpire())
        {
            if (vineCount == 3)
            {
                GameObject vineToRemove = (GameObject)activeVines[0];
                activeVines.RemoveAt(0);
                Destroy(vineToRemove);
                vineCount--;
                Debug.Log("NUMBER OF VINESSSSSS: " + vineCount);
            }
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit, 70.0f);
            if (hit.transform.gameObject.tag == "Environment")
            {
                AudioSource.PlayClipAtPoint(vineGrowAudio, transform.position);
                GameObject vineInstance = Instantiate(vine, transform.position, Quaternion.Euler(Vector3.forward)) as GameObject;
                Vector3 spawnPos = hit.transform.position;
                spawnPos.y += spawnOffset;
                vineInstance.transform.position = spawnPos;
                activeVines.Add(vineInstance);
                vineCount++;
                Debug.Log("NUMBER OF VINESSSSSS: " + vineCount);
            }
        }
    }

    public void vineRemoved(Transform hitVine)
    {
        activeVines.Remove(hitVine.gameObject);
        Destroy(hitVine.gameObject);
        vineCount--;
        Debug.Log("NUMBER OF VINESSSSSS: " + vineCount);
    }

    public int getVineCount()
    {
        return vineCount;
    }
}
