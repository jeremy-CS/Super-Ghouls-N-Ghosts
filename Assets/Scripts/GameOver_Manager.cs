using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver_Manager : MonoBehaviour
{
    [SerializeField] private GameObject gameover_screen;
    [SerializeField] private int scene_number;
    [SerializeField] private GameObject level_theme;

    private AudioSource[] gameover_sounds;
    private AudioSource player_death_sound;
    private AudioSource gameover_sound;
    private AudioSource continue_sound;

    [SerializeField] private Button continueButton;
    [SerializeField] private Image numberOfLives;
    [SerializeField] private Image[] numbers;
    private int numberOfContinues = 2;
    //private float time = 0f;

    private void Awake()
    {
        //DontDestroyOnLoad(numberOfContinues);

        if(numberOfContinues == 1)
        {
            numberOfLives.sprite = numbers[1].sprite;
        }
        else if (numberOfContinues == 0)
        {
            numberOfLives.sprite = numbers[0].sprite;
        }
    }

    public void SetGameOver()
    {
        gameover_sounds = this.gameObject.GetComponents<AudioSource>();
        player_death_sound = gameover_sounds[0];
        gameover_sound = gameover_sounds[1];
        continue_sound = gameover_sounds[2];

        //Stop level music
        AudioSource[] level_themes = level_theme.GetComponents<AudioSource>();
        foreach (AudioSource audio in level_themes)
        {
            if(audio.isPlaying)
            {
                audio.mute = true;
            }
        }

        gameover_screen.SetActive(true);

        //Restrict number of continues
        if(numberOfContinues == 0)
        {
            continueButton.enabled = false;
        }

        //Play game over sounds
        player_death_sound.Play();
        gameover_sound.PlayDelayed(player_death_sound.clip.length-1.748f);
        continue_sound.PlayDelayed(gameover_sound.clip.length+player_death_sound.clip.length-2.748f);
    }

    public void RestartLevel()
    {
        numberOfContinues -= 1;
        SceneManager.LoadScene(scene_number);
    }
}
