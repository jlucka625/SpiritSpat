using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Text winnerLabel;
    public Sprite GusWins;
    public Sprite LydiaWins;
    public Sprite Draw;
    public Image GameOverImage;
    private AudioSource audio;
    public AudioClip lydiaVictorySFX;
    public AudioClip GusVictorySFX;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        
        if (GameOver.LydiaCount > GameOver.GusCount)
        {
            audio.clip = lydiaVictorySFX;
            GameOverImage.sprite = LydiaWins;
            audio.Play();
        }
        else if (GameOver.LydiaCount < GameOver.GusCount)
        {
            audio.clip = GusVictorySFX;
            GameOverImage.sprite = GusWins;
            audio.Play();
        }
        else
        {
            GameOverImage.sprite = Draw;
        }
        winnerLabel.text += "Fire Markers: " + GameOver.GusCount + "\nFlower Markers: " + GameOver.LydiaCount;
    }

    void Update()
    {
        if (Input.GetKeyDown("joystick button 7"))
        {
            GameOver.paused = false;
            Application.LoadLevel("TitleScreen");
        }
    }
}