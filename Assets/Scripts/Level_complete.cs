using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_complete : MonoBehaviour
{
    [SerializeField] private GameObject level_theme;
    [SerializeField] private AudioSource level_complete_theme;

    public void SetLevelComplete()
    {
        Time.timeScale = 0;
        this.gameObject.SetActive(true);

        //Stop level music
        AudioSource[] level_themes = level_theme.GetComponents<AudioSource>();
        foreach (AudioSource audio in level_themes)
        {
            if (audio.isPlaying)
            {
                audio.mute = true;
            }
        }

        level_complete_theme.Play();
    }
}
