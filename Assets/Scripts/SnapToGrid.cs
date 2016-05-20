using UnityEngine;
using System.Collections;

public class SnapToGrid : MonoBehaviour
{
    public float gridSize;
    public float gridSections = 1;

    public bool snapToXAxis = true;
    public bool snapToYAxis = false;
    public bool snapToZAxis = false;

    public bool twoDimensionalSpace = true;

    private float snapMultiplier;

	void Awake ()
    {
        // Validate grid sections, cannot divide by zero, should not be negative
        if (gridSections <= 0)
            gridSections = 1;

        snapMultiplier = gridSize / gridSections;

        float xPos = transform.position.x;
        if (snapToXAxis)
            SnapToAxis(ref xPos);

        float yPos = transform.position.y;
        if (snapToYAxis)
            SnapToAxis(ref yPos);

        float zPos = 0;
        if (!twoDimensionalSpace)
        {
            zPos = transform.position.z;
            if (snapToZAxis)
                SnapToAxis(ref zPos);
        }

        transform.position = new Vector3(xPos, yPos, zPos);
	}

    void SnapToAxis(ref float val)
    {
        val = Mathf.Round(val) * snapMultiplier;
    }
}
