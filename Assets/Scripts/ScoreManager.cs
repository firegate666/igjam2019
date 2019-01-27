using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{

    public int Score;

    public int getTotalScore() => Score;

    public void setTotalScore(int newScore) => Score = newScore;

    public void addTotalScore(int change) => Score += change;
    public int getPlanetScore() => Score;

    public void setPlanetScore(int newScore) => Score = newScore;

    public void addPlanetScore(int change) => Score += change;

    
    
}
