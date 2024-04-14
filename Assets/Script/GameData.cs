using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckHighScore(float currentScore)
    {
        float highScore = GetHighScore();
        print(highScore);

        if(currentScore > highScore)
        {
            SetScore(currentScore);
        }
    }

    public float GetHighScore()
    {
        return PlayerPrefs.GetFloat("highScore");
    }

    public void SetScore(float newHighScore)
    {
        PlayerPrefs.SetFloat("highScore", newHighScore);
    }
}
