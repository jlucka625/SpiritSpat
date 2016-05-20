using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
    public static bool paused = false;
    public static int LydiaCount = 0;
    public static int GusCount = 0;
    public static void endGame()
    {
        GameObject.FindGameObjectWithTag("Gus").GetComponent<Rigidbody>().velocity = Vector3.zero;
        GameObject.FindGameObjectWithTag("Lydia").GetComponent<Rigidbody>().velocity = Vector3.zero;
        paused = true;
        LydiaCount = GameObject.FindGameObjectsWithTag("LydiaMarker").Length;
        GusCount = GameObject.FindGameObjectsWithTag("GusMarker").Length;
        Application.LoadLevel("GameOver");
    }
}
