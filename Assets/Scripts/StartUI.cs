using UnityEngine;
using System.Collections;

public class StartUI : MonoBehaviour {
    public GameObject UI;
    public GameObject timer;
	// Use this for initialization
	void Start () {
        GameOver.paused = true;
	}

    void Update()
    {
        if(Input.GetKeyDown("joystick button 7"))
        {
            GameOver.paused = false;
            UI.GetComponent<CanvasGroup>().alpha = 1;
            timer.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
