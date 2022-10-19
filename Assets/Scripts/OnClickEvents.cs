using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnClickEvents : MonoBehaviour
{
    public TextMeshProUGUI soundText;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.mute)
        {
            soundText.text = "/";
        } else
        {
            soundText.text = "";
        }
    }

   public void ToggleMute()
    {
        if(GameManager.mute)
        {
            GameManager.mute = false;
            soundText.text = "";
        } else
        {
            GameManager.mute = true;
            soundText.text = "/";
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Ended");
    }
}

// To make the sound mute

/*
 where you are calling the sound to play write

if(GameMananger.mute){
return;
}
 
 */