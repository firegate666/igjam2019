using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public int Score;

    public int getScore() => Score;

    public void setScore(int newScore) => Score = newScore;

    public void addScore(int change) => Score += change;
}
