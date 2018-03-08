using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

    public float speed = 10f;
    public float maxSpeed = 100f;
    public float incrementSpeedAtScore = 200f;
    public int score;
    public int startScore;
    public int life = 3;

    public Text scoreText;
    public Text lifeText;

    public GameObject gameOverPanel;

    void Update () {
        if(life > 0)
        {
            transform.Translate((Vector2.up / 50f) * Speed());
        }
        life = Mathf.Max(0, life);

        if (scoreText) scoreText.text = "SCORE\n" + score.ToString("D6");
        if (lifeText) lifeText.text = "LIFE\n" + life.ToString("D2");

        if(life <= 0 && !gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.GetComponentInParent<EventButton>().RequestBanner();
        }
    }

    public void AddScore(int point)
    {
        score += point;
    }

    public void LifeBonus(int point)
    {
        life += point;
    }

    public float Speed()
    {
        return Mathf.Min(speed + (score / incrementSpeedAtScore), maxSpeed) * Time.deltaTime;
    }

}
