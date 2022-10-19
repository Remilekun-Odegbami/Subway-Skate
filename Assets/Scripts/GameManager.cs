using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private const int COIN_SCORE = 5;
    public static GameManager Instance { get; set; }
    public static bool mute = false; 

    public bool isDead { get; set; }

    private bool isGameStarted = false;

    private PlayerController playerController;

    //UI and UI fields
    public TextMeshProUGUI scoreText, coinText, modifierText, hiscoreText; 

    private float score, coinScore, modifierScore;

    private int lastScore;

    // play menu
    public Animator playMenuAnim;
    public Animator gameCanvasAnim, mainMenuAnim;

    public TextMeshProUGUI pointScoredText, coinCollectedText;

    private void Awake()
    {
        Instance = this;
        // update texts as soon as game starts
        modifierScore = 1;
        DefaultScores();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        hiscoreText.text = "High Score: " + PlayerPrefs.GetInt("Hiscore").ToString();
    }

    private void Update()
    {
        // if user does not tap screen do not do anything
        if(MobileInput.Instance.Tap && !isGameStarted)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            isGameStarted = true;
            playerController.StartRunning();
            FindObjectOfType<GlacierSpawner>().isScrolling = true;
            FindObjectOfType<CameraController>().isMoving = true;
            gameCanvasAnim.SetTrigger("Show");
            mainMenuAnim.SetTrigger("Hide");
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

    public void OnPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void OnDeath()
    {
        isDead = true;
        pointScoredText.text = "Score: " + score.ToString("0");
        coinCollectedText.text = "Coins: " + coinScore.ToString("0");
        playMenuAnim.SetTrigger("Play");
        gameCanvasAnim.SetTrigger("Hide");
        FindObjectOfType<GlacierSpawner>().isScrolling = false;

        //check if this is a highscore
        if (score > PlayerPrefs.GetInt("Hiscore"))
        {
            float s = score;
            if(s % 1 == 0)
            {
                s += 1;
            }
            PlayerPrefs.SetInt("Hiscore", (int)s);
        }
    }
}
