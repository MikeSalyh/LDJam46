﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeechBubble : MonoBehaviour
{
  public Image shapeImage, shadowImage, patternImage;
  public TextMeshProUGUI label;

  private static int responseIndex = 0;

  private int _color, _pattern, _shape;
  public int Color
  {
    get { return _color; }
  }
  public int Pattern
  {
    get { return _pattern; }
  }
  public int Shape
  {
    get { return _shape; }
  }


  // Start is called before the first frame update
  void Start()
  {
    DoWiggle();
  }

  private void DoWiggle()
  {
    float tempo = 1f;
    float offset = Random.Range(5f, 10f);
    if (Random.value > 0.5f)
      offset *= -1f;

    transform.localEulerAngles = new Vector3(0f, 0f, -offset);
    transform.DORotate(new Vector3(0f, 0f, offset), tempo).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
  }

  private void Configure(int pattern, int shape, int color)
  {
    _pattern = pattern;
    _shape = shape;
    _color = color;

    patternImage.sprite = GameManager.instance.possiblePatterns[Pattern];
    patternImage.color = GameManager.instance.possibleColors[Color];
    shapeImage.sprite = GameManager.instance.possibleShapes[Shape];
    shadowImage.sprite = GameManager.instance.possibleShapes[Shape];

    //Update the response text.
    label.text = GameManager.instance.possibleResponses[responseIndex];
    responseIndex++;
    if (responseIndex >= GameManager.instance.possibleResponses.Length)
      responseIndex = 0;
  }

  public void ConfigureRandom()
  {
    Configure(Random.Range(0, GameManager.instance.possiblePatterns.Length), Random.Range(0, GameManager.instance.possibleShapes.Length), Random.Range(0, GameManager.instance.possibleColors.Length));
  }

  public void ConfigureWrongAnswer(int correctPattern, int correctShape, int correctColor)
  {
    Configure(GetRandomIntExcluding(correctPattern, GameManager.instance.possiblePatterns.Length), GetRandomIntExcluding(correctShape, GameManager.instance.possibleShapes.Length), GetRandomIntExcluding(correctColor, GameManager.instance.possibleColors.Length));
  }

  public void ConfigureWrongAnswer(SpeechBubble correctAnswer)
  {
    ConfigureWrongAnswer(correctAnswer.Pattern, correctAnswer.Shape, correctAnswer.Color);
  }

  public void ConfigureCorrectAnswer(int correctPattern, int correctShape, int correctColor)
  {
    int correctCategory = Random.Range(0, 3);
    if(correctCategory == 0)
      Configure(correctPattern, GetRandomIntExcluding(correctShape, GameManager.instance.possibleShapes.Length), GetRandomIntExcluding(correctColor, GameManager.instance.possibleColors.Length));
    else if(correctCategory == 1)
      Configure(GetRandomIntExcluding(correctPattern, GameManager.instance.possiblePatterns.Length), correctShape, GetRandomIntExcluding(correctColor, GameManager.instance.possibleColors.Length));
    else if(correctCategory == 2)
      Configure(GetRandomIntExcluding(correctPattern, GameManager.instance.possiblePatterns.Length), GetRandomIntExcluding(correctShape, GameManager.instance.possibleShapes.Length), correctColor);
  }

  public void ConfigureCorrectAnswer(SpeechBubble correctAnswer)
  {
    ConfigureCorrectAnswer(correctAnswer.Pattern, correctAnswer.Shape, correctAnswer.Color);
  }

  public bool IsMatch(SpeechBubble other)
  {
    if (other.Shape == this.Shape)
      return true;
    if (other.Color == this.Color)
      return true;
    if (other.Pattern == this.Pattern)
      return true;

    return false;
  }


  //This is super inefficient.
  private int GetRandomIntExcluding(int exclusion, int length)
  {
    int output;
    output = Random.Range(0, length);
    while (output == exclusion)
    {
      output = Random.Range(0, length);
    }
    return output;
  }
}
