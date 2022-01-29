using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteMusic : MonoBehaviour
{
    public void MuteMusicToggle(bool muted)
    {
        if (muted)
        {
            AudioListener.volume = 0; // Mute the music
        }

        else
        {
            AudioListener.volume = 1;
        }
    }
}
