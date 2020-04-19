using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

  public static GameManager instance;

  [Header("Variables")]
  public Sprite[] possiblePatterns;
  public Sprite[] possibleShapes;
  public Color[] possibleColors;
  public Sprite[] possibleSuits;
  public string[] possibleResponses = { "No way", "Thank you", "Okay", "Alright", "Go on", "Are you sure?", "Shut up" };

  public SpeechBubble source;
  public SpeechBubble[] answers;
  public Transform centerPosition;

  public bool randomizeColors;
  public bool randomizeShapes;
  public bool randomizeSuits;
  public bool randomizePatterns;

  public enum GameState
  {
    Answering,
    Reveal
  }
  public GameState currentState;



  private void Start()
  {
    instance = this;
    foreach (SpeechBubble answer in answers)
    {
      answer.GetComponentInChildren<Button>().onClick.AddListener(delegate { Evaluate(answer); });
    }
    StartNewRound();
  }

  private void StartNewRound()
  {
    currentState = GameState.Answering;

    source.ConfigurePrompt();
    source.Appear();
    int correctAnswerIndex = UnityEngine.Random.Range(0, answers.Length);
    for (int i = 0; i < answers.Length; i++)
    {
      if (i == correctAnswerIndex)
        answers[i].ConfigureCorrectAnswer(source);
      else
        answers[i].ConfigureWrongAnswer(source);

      answers[i].Appear((i+1)/8f);
    }
  }

  public void Evaluate(SpeechBubble clickedBubble)
  {
    if (currentState == GameState.Answering)
    {
      StartCoroutine(DoReveal(clickedBubble, clickedBubble.IsMatch(source)));
    }
  }

  private IEnumerator DoReveal(SpeechBubble clickedBubble, bool success)
  {
    currentState = GameState.Reveal;

    foreach (SpeechBubble answer in answers)
      if (answer != clickedBubble)
        answer.Hide(0.25f);

    clickedBubble.transform.SetAsLastSibling();
    clickedBubble.transform.localScale = clickedBubble.defaultLocalScale * 0.8f;
    clickedBubble.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutSine);

    yield return new WaitForSeconds(0.25f);
    clickedBubble.transform.DOMove(centerPosition.position, 0.25f).SetEase(Ease.InOutSine);
    clickedBubble.Reveal(success ? "Right!" : "Wrong!");
    if (success)
      Debug.Log("Point");

    yield return new WaitForSeconds(1.15f);
    clickedBubble.Hide(0.4f);
    source.Hide(0.6f);

    //WIP. This shouldn't be here, should it?
    yield return new WaitForSeconds(1f);
    StartNewRound();
  }

}
