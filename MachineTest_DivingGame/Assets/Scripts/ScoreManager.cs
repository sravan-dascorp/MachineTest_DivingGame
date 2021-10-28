using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public Text scoreText;
    public Text highScoreText;
    private int _score = 0;
    private int _highScore = 0;


    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        _highScore = PlayerPrefs.GetInt("highScore", 0);
        scoreText.text = _score.ToString() + " POINTS";
        highScoreText.text = "HIGHSCORE: " + _highScore.ToString();
    }


    public void AddPoint(int point)
    {
        _score += point;
        scoreText.text = _score.ToString() + " POINTS";
        
        if (_highScore < _score)
        {
            PlayerPrefs.SetInt("highScore", _score);
        }
    }


    public void AddBonusPoint(int bonusPoint)
    {
        _score += 10;
        scoreText.text = _score.ToString() + " POINTS";
        
        if (_highScore < _score)
        {
            PlayerPrefs.SetInt("highScore", _score);
        }
    }

}
