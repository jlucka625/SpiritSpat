using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float cameraPadding;
    public float minCameraSize;
    public float maxCameraSize;
    public Transform player1;
    public Transform player2;

    private Camera cam;

	// Use this for initialization
	void Start ()
    {
        cam = GetComponent<Camera>();
	}

    // Update is called once per frame
    void Update()
    {
        float distanceBetweenPlayers = Mathf.Abs(Vector3.Distance(
            player1.position, player2.position));
        cam.orthographicSize = Mathf.Clamp((distanceBetweenPlayers + cameraPadding), minCameraSize, maxCameraSize);
        Vector3 directionVector = Vector3.Normalize(player2.position - player1.position) * (distanceBetweenPlayers / 2);

        cam.transform.position = new Vector3(player1.position.x + directionVector.x, player1.position.y + directionVector.y, -10.0f);
    }
}
