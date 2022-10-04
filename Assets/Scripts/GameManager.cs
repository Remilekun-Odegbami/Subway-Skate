using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE = 5;
    public static GameManager Instance { get; set; }

    public bool isDead { get; set; }

    private bool isGameStarted = false;

    private PlayerController playerController;

    //UI and UI fields
    public TextMeshProUGUI scoreText, coinText, modifierText; 

    private float score, coinScore, modifierScore;

    private int lastScore;


    private void Awake()
    {
        Instance = this;
        // update texts as soon as game starts
        modifierScore = 1;
        DefaultScores();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Update()
    {
        // if user does not tap screen do not do anything
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            isGameStarted = true;
            playerController.StartRunning();
        }

        if (isGameStarted && !isDead)
        {
            score+=(Time.deltaTime * modifierScore);
            if(lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = "Score: " + score.ToString("0");
            }
            
        }
    }

    public void GetCoin()
    {
        coinScore++;
        coinText.text = "Coins: " + coinScore.ToString("0");
        score += COIN_SCORE;
        scoreText.text = scoreText.text = "Score: " + score.ToString("0");
    }
    public void DefaultScores()
    {
        scoreText.text = "Score: " + score.ToString("0");
        modifierText.text = "Speed: x" + modifierScore.ToString("0.0");// this cuts the extra 0 at the end
        coinText.text = "Coins: " + coinScore.ToString("0");
    }

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1.0f + modifierAmount;
        modifierText.text = "Speed: x" + modifierScore.ToString("0.0");// this cuts the extra 0 at the end
    }
}
