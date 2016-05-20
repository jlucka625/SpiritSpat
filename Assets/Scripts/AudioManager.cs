using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] myMusic;   // List of all music

    private int currentSong = -1;    // Keep track of the current song playing

    public void PlayMusic(int index)
    {
        // Stop any music currently playing
        StopMusic();

        // Start playing the new song
        currentSong = index;
        if (index >= 0)
        {
            myMusic[index].enabled = true;
            myMusic[index].Play();
        }
    }


    // If current song is playing, stop and disable
    public void StopMusic()
    {
        if (currentSong >= 0)
        {
            myMusic[currentSong].Pause();
            myMusic[currentSong].enabled = false;
        }
    }

}
