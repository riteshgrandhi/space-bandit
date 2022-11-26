using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int score;
    public void AddScore(int increment)
    {
        score += increment;
    }
}