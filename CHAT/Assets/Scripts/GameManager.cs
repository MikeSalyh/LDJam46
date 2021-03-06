﻿using System;
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
  public Dialogue[] conversations;

  public SpeechBubble source;
  public SpeechBubble[] answers;
  public Transform centerPosition;

  private int categoryDifficulty = 0;
  public bool randomizeColors;
  public bool randomizeShapes;
  public bool randomizeSuits;
  public bool randomizePatterns;

  public delegate void ChangeStateDelegate(GameState newState);
  public static ChangeStateDelegate OnChangeState;
  public delegate void NewQuestionDelegate();
  public static NewQuestionDelegate OnNewQuestion;
  public delegate void AnswerDelegate(bool correct);
  public static AnswerDelegate OnAnswer;

  private bool speakerIsLeft = true;
  private Dialogue currentDialogue;

  public AudioClip correctAnswerSFX, wrongAnswerSFX, selectSFX;
  private AudioSource audioSrc;

  public enum GameState
  {
    Init,
    Prompt,
    Answer,
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

    if (newState == GameState.GameOver)
    {
      if (MetagameManager.instance != null)
        MetagameManager.instance.GoToFinale();
    }
  }

  public Raccoon leftRaccoon, rightRaccon;

  private void Start()
  {
    instance = this;
    audioSrc = GetComponent<AudioSource>();
    HandleIncreaseCategoryDifficulty();
    LifeManager.OnGameOver += HandleGameOver;
    foreach (SpeechBubble answer in answers)
    {
      answer.GetComponentInChildren<Button>().onClick.AddListener(delegate { Evaluate(answer); });
    }
    StartCoroutine(StartNewRound(1.5f));
  }

  private void OnDisable()
  {
    LifeManager.OnGameOver -= HandleGameOver;
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
    audioSrc.PlayOneShot(wrongAnswerSFX);
    if (CurrentState == GameState.Answer)
    {
      source.Hide(0.5f);
      for (int i = 0; i < answers.Length; i++)
        answers[i].Hide(0.5f + i / 10f);
    }
    SwitchState(GameState.GameOver); 
  }

  private IEnumerator StartNewRound(float delay)
  {
    yield return new WaitForSeconds(delay);

    SwitchState(GameState.Prompt);
    if (OnNewQuestion != null)
      OnNewQuestion();

    speakerIsLeft = !speakerIsLeft;
    currentDialogue = conversations[UnityEngine.Random.Range(0, conversations.Length)];

    if (speakerIsLeft)
    {
      source.transform.SetAsFirstSibling();
      leftRaccoon.DoTalk(currentDialogue.intro, true);
    }
    else
    {
      source.transform.SetAsLastSibling();
      rightRaccon.DoTalk(currentDialogue.intro, true);
    }

    source.ConfigurePrompt();
    source.Appear(currentDialogue.intro);


    yield return new WaitForSeconds(1.5f);
    SwitchState(GameState.Answer);

    source.HideLabel();
    leftRaccoon.SwitchState(Raccoon.State.idle);
    rightRaccon.SwitchState(Raccoon.State.idle);


    yield return new WaitForSeconds(0.1f);

    int correctAnswerIndex = UnityEngine.Random.Range(0, answers.Length);
    for (int i = 0; i < answers.Length; i++)
    {
      yield return new WaitForSeconds(0.06f);
      if (i == correctAnswerIndex)
        answers[i].ConfigureCorrectAnswer(source);
      else
        answers[i].ConfigureWrongAnswer(source);

      answers[i].Appear();
    }
    yield break;
  }

  public void Evaluate(SpeechBubble clickedBubble)
  {
    if (CurrentState == GameState.Answer)
    {
      StartCoroutine(DoReveal(clickedBubble, clickedBubble.IsMatch(source)));
    }
  }

  private IEnumerator DoReveal(SpeechBubble clickedBubble, bool success)
  {
    SwitchState(GameState.Reveal);
    if (OnAnswer != null)
      OnAnswer(success);

    if (success)
    {
      HandleIncreaseCategoryDifficulty();
    }

    audioSrc.PlayOneShot(selectSFX);

    foreach (SpeechBubble answer in answers)
      if (answer != clickedBubble)
        answer.Hide(0.25f);

    clickedBubble.transform.SetAsLastSibling();
    clickedBubble.transform.localScale = clickedBubble.defaultLocalScale * 0.8f;
    clickedBubble.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.InOutSine);
    clickedBubble.keycodeCard.SetActive(false);

    if(!success)
      audioSrc.PlayOneShot(wrongAnswerSFX);

    yield return new WaitForSeconds(0.25f);
    clickedBubble.transform.DOMove(centerPosition.position, 0.25f).SetEase(Ease.InOutSine);

    if (success)
    {
      clickedBubble.Flip(currentDialogue.success);
      if (speakerIsLeft)
      {
        rightRaccon.DoTalk(currentDialogue.success, false);
      }
      else
      {
        leftRaccoon.DoTalk(currentDialogue.success, false);
      }
    }
    else
    {
      clickedBubble.Flip(currentDialogue.failure + "\n<size=0.5em>(Bad Match)</size>");
    }

    if (!success)
    {
      clickedBubble.transform.DOShakeRotation(1f);
    }

    yield return new WaitForSeconds(1.5f);
    clickedBubble.Hide(0.4f);
    source.Hide(0.6f);

    leftRaccoon.SwitchState(Raccoon.State.idle);
    rightRaccon.SwitchState(Raccoon.State.idle);

    if(CurrentState == GameState.Reveal)
      StartCoroutine(StartNewRound(1f));
  }

  private void HandleIncreaseCategoryDifficulty()
  {
    categoryDifficulty++;
    if (categoryDifficulty > 0)
      randomizeColors = true;
    else
      randomizeColors = false;

    if (categoryDifficulty > 2)
      randomizeSuits = true;
    else
      randomizeSuits = false;

    if (categoryDifficulty > 4)
      randomizeShapes = true;
    else
      randomizeShapes = false;

    if (categoryDifficulty > 6)
      randomizePatterns = true;
    else
      randomizePatterns = false;
  }
}
