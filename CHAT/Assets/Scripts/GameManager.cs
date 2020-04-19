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

  public delegate void ChangeStateDelegate(GameState newState);
  public static ChangeStateDelegate OnChangeState;
  public delegate void AnswerDelegate(bool correct);
  public static AnswerDelegate OnAnswer;

  private bool speakerIsLeft = true;

  public enum GameState
  {
    Init,
    Answering,
    Reveal,
    GameOver
  }

  private GameState _currentState;
  public GameState CurrentState
  {
    get { return _currentState; }
    set { SwitchState(value); }
  }

  public void SwitchState(GameState newState)
  {
    if (_currentState == newState)
      return;

    _currentState = newState;
    if (OnChangeState != null)
      OnChangeState(newState);
  }

  private void Start()
  {
    instance = this;
    LifeManager.OnGameOver += HandleGameOver;
    foreach (SpeechBubble answer in answers)
    {
      answer.GetComponentInChildren<Button>().onClick.AddListener(delegate { Evaluate(answer); });
    }
    StartNewRound();
  }

  private void Update()
  {
    for (int i = 0; i < answers.Length; i++)
    {
      if (Input.GetKeyDown(answers[i].keyboardKey))
        Evaluate(answers[i]);
    }
  }

  private void HandleGameOver()
  {
    if (CurrentState == GameState.Answering)
    {
      source.Hide(0.5f);
      for (int i = 0; i < answers.Length; i++)
        answers[i].Hide(0.5f + i / 10f);
    }
    SwitchState(GameState.GameOver);
  }

  private void StartNewRound()
  {
    SwitchState(GameState.Answering);

    speakerIsLeft = !speakerIsLeft;
    if (speakerIsLeft)
      source.transform.SetAsFirstSibling();
    else
      source.transform.SetAsLastSibling();

    source.ConfigurePrompt();
    source.Appear();
    int correctAnswerIndex = UnityEngine.Random.Range(0, answers.Length);
    for (int i = 0; i < answers.Length; i++)
    {
      if (i == correctAnswerIndex)
        answers[i].ConfigureCorrectAnswer(source);
      else
        answers[i].ConfigureWrongAnswer(source);

      answers[i].Appear((i+1)/16f);
    }
  }

  public void Evaluate(SpeechBubble clickedBubble)
  {
    if (CurrentState == GameState.Answering)
    {
      StartCoroutine(DoReveal(clickedBubble, clickedBubble.IsMatch(source)));
    }
  }

  private IEnumerator DoReveal(SpeechBubble clickedBubble, bool success)
  {
    SwitchState(GameState.Reveal);
    if (OnAnswer != null)
      OnAnswer(success);

    foreach (SpeechBubble answer in answers)
      if (answer != clickedBubble)
        answer.Hide(0.25f);

    clickedBubble.transform.SetAsLastSibling();
    clickedBubble.transform.localScale = clickedBubble.defaultLocalScale * 0.8f;
    clickedBubble.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutSine);
    clickedBubble.keycodeCard.SetActive(false);

    yield return new WaitForSeconds(0.25f);
    clickedBubble.transform.DOMove(centerPosition.position, 0.25f).SetEase(Ease.InOutSine);
    clickedBubble.Reveal(success ? "Right!" : "Wrong!");

    if (!success)
    {
      clickedBubble.transform.DOShakeRotation(1f);
    }

    yield return new WaitForSeconds(1.15f);
    clickedBubble.Hide(0.4f);
    source.Hide(0.6f);

    //WIP. This shouldn't be here, should it?
    yield return new WaitForSeconds(1f);
    if(CurrentState == GameState.Reveal)
      StartNewRound();
  }
}
