using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score_manager : MonoBehaviour
{
    private int score = 0;
    private int score_calculator = 0;
    private int score_temp;

    [SerializeField] private Image[] numbers;
    [SerializeField] private Image[] score_numbers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddPoints(int points)
    {
        score += points;
        score_calculator = score;
        int i = 3;

        while (score_calculator > 0 || i >= 0)
        {
            score_temp = score_calculator % 10;
            ChangeSprite(i, score_temp);

            //Update values to get the other digits
            score_calculator /= 10;
            i--;
        }
    }

    public void ChangeSprite(int sprite_placement, int sprite_num)
    {
        score_numbers[sprite_placement].sprite = numbers[sprite_num].sprite;
    }

}
