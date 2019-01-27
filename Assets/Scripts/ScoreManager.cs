using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{

    public int Score;
    public int PlanetScore;

    public int getTotalScore() => Score;

    public void setTotalScore(int newScore) => Score = newScore;

    public void addTotalScore(int change) => Score += change;
    public int getPlanetScore() => PlanetScore;

    public void setPlanetScore(int newScore) => PlanetScore = newScore;

    public void addPlanetScore(int change) => PlanetScore += change;

}
