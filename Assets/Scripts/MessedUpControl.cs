using UnityEngine;
using System.Collections;

public class MessedUpControl : MonoBehaviour {
    public float recoverTime = 5.0f;
    private bool disoriented = false;
    public GameObject sporeCloud;
    public void disorient()
    {
        if (!disoriented)
        {
            disoriented = true;
            GetComponent<PlayerController>().horizontalAxis = "GusHorizontalInverted";
            GetComponent<PlayerController>().jumpAxis = "GusJumpInverted";
            sporeCloud.SetActive(true);
            GetComponent<SpriteRenderer>().color = Color.magenta;
            Invoke("recover", recoverTime);
        }
    }
    public void recover()
    {
        disoriented = false;
        GetComponent<PlayerController>().horizontalAxis = "GusHorizontal";
        GetComponent<PlayerController>().jumpAxis = "GusJump";
        GetComponent<SpriteRenderer>().color = Color.white;
        sporeCloud.SetActive(false);
    }
}
