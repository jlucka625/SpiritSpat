using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [Tooltip("The dead zone for joystick movement. Range [0,1]")]
    public float joystickDeadZone;

    public Sprite startSprite;
    public Sprite startSelectedSprite;
    public Sprite exitSprite;
    public Sprite exitSelectedSprite;

    public GameObject[] menuSelections;

    public AudioClip buttonHighlightSound;
    public AudioClip buttonSelectSound;

    private int currentSelectionIndex = 0;
    private int menuOptionCount;
    private GameObject currentSelection;
    private GameObject previousSelection;
    private bool readyToChangeSelections = true;

    private const float PAUSE_TIME = 0.75f;

    void Start()
    {
        currentSelection = menuSelections[currentSelectionIndex];
        menuOptionCount = menuSelections.Length;
        Transform currentTransform = currentSelection.gameObject.GetComponent<Transform>();
        currentTransform.position = new Vector3(currentTransform.position.x, currentTransform.position.y, -1);
        currentSelection.gameObject.GetComponent<SpriteRenderer>().sprite = startSelectedSprite;
        previousSelection = currentSelection;
    }

	// Use this for initialization
	void Update()
    {
        float rawHorizontalInput = Input.GetAxis("HorizontalJoystick");

        if (rawHorizontalInput < joystickDeadZone && rawHorizontalInput > -joystickDeadZone)
            readyToChangeSelections = true;
        
        if (readyToChangeSelections)
        {
            if (rawHorizontalInput > joystickDeadZone)
            {
                IncrementMenuIndex();
                currentSelection = menuSelections[currentSelectionIndex];
                readyToChangeSelections = false;
            }
            else if (rawHorizontalInput < -joystickDeadZone)
            {
                DecrementMenuIndex();
                currentSelection = menuSelections[currentSelectionIndex];
                readyToChangeSelections = false;
            }
        }

        if (Input.GetButton("Confirm"))
        {
            AudioSource.PlayClipAtPoint(buttonSelectSound, transform.position);
            //StartCoroutine(ConfirmSelection());
            Invoke("ConfirmSelection", PAUSE_TIME);
        }

        if (previousSelection != currentSelection)
        {
            AudioSource.PlayClipAtPoint(buttonHighlightSound, transform.position);
            if (currentSelection.name.Contains("start"))
            {
                Transform currentTransform = currentSelection.gameObject.GetComponent<Transform>();
                currentTransform.position = new Vector3(currentTransform.position.x, currentTransform.position.y, -1);
                currentSelection.gameObject.GetComponent<SpriteRenderer>().sprite = startSelectedSprite;

                Transform previousTransform = previousSelection.gameObject.GetComponent<Transform>();
                previousTransform.position = new Vector3(previousTransform.position.x, previousTransform.position.y, -1);
                previousSelection.gameObject.GetComponent<SpriteRenderer>().sprite = exitSprite;
            }
            else if (currentSelection.name.Contains("exit"))
            {
                Transform currentTransform = currentSelection.gameObject.GetComponent<Transform>();
                currentTransform.position = new Vector3(currentTransform.position.x, currentTransform.position.y, -1);
                currentSelection.gameObject.GetComponent<SpriteRenderer>().sprite = exitSelectedSprite;

                Transform previousTransform = previousSelection.gameObject.GetComponent<Transform>();
                previousTransform.position = new Vector3(previousTransform.position.x, previousTransform.position.y, -1);
                previousSelection.gameObject.GetComponent<SpriteRenderer>().sprite = startSprite;
            }
        }

        previousSelection = currentSelection;
    }

    private void IncrementMenuIndex()
    {
        currentSelectionIndex++;
        if (currentSelectionIndex >= menuOptionCount)
            currentSelectionIndex = menuOptionCount - 1;
    }

    private void DecrementMenuIndex()
    {
        currentSelectionIndex--;
        if (currentSelectionIndex < 0)
            currentSelectionIndex = 0;
    }

    void ConfirmSelection()
    { 
        if (currentSelection.name.Contains("startButton"))
            Application.LoadLevel("MainGame");
        else if (currentSelection.name.Contains("exitButton"))
            Application.Quit();
    }
}
