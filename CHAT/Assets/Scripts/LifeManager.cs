using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
  public float life = 1f;
  public float lifeDecreasePerSecond = 0.1f;
  public float correctAnswerBonus = 0.05f;
  public float wrongAnswerPenalty = 0.2f;
  public float speedIncreasePerCA = 0.01f;
  public int score = 0;

  public delegate void GameEndDelegate();
  public static GameEndDelegate OnGameOver;

  // Start is called before the first frame update
  void Start()
  {
    GameManager.OnAnswer += HandleAnswer;
  }

  void HandleAnswer(bool correct)
  {
    if (!correct)
    {
      ReduceLife(wrongAnswerPenalty);
    }
    else
    {
      life += correctAnswerBonus;
      lifeDecreasePerSecond += speedIncreasePerCA;
      correctAnswerBonus += speedIncreasePerCA;
      score++;
    }
    life = Mathf.Clamp01(life);
  }

  // Update is called once per frame
  void Update()
  {
    if (GameManager.instance.CurrentState == GameManager.GameState.Answering)
      ReduceLife(lifeDecreasePerSecond * Time.deltaTime);
  }

  private void ReduceLife(float damage)
  {
    bool wasAlive = life > 0;
    life -= damage;
    if (life <= 0 && wasAlive)
    {
      if (OnGameOver != null)
        OnGameOver();
    }
  }
}
