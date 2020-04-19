using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

  public static GameManager instance;

  [Header("Variables")]
  public Sprite[] possiblePatterns;
  public Sprite[] possibleShapes;
  public Color[] possibleColors;
  public string[] possibleResponses = { "No way", "Thank you", "Okay", "Alright", "Go on", "Are you sure?", "Shut up" };

  public SpeechBubble source;
  public SpeechBubble[] answers;



  private void Start()
  {
    instance = this;

    foreach (SpeechBubble answer in answers)
    {
      answer.GetComponent<Button>().onClick.AddListener(delegate { Evaluate(answer); });
    }
    Shuffle();
  }

  private void Shuffle()
  {
    source.ConfigureRandom();
    int correctAnswerIndex = UnityEngine.Random.Range(0, answers.Length);
    for (int i = 0; i < answers.Length; i++)
    {
      if (i == correctAnswerIndex)
        answers[i].ConfigureCorrectAnswer(source);
      else
        answers[i].ConfigureWrongAnswer(source);
    }
  }

  public void Evaluate(SpeechBubble origin)
  {
    if (origin.IsMatch(source))
    {
      Debug.Log("Match");
      Shuffle();
    }
  }
}
