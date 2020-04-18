using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeechBubble : MonoBehaviour
{
  public const int Variance = 5;  //How many variants of each property exist

  public Image shapeImage, shadowImage, patternImage;
  public TextMeshProUGUI label;

  [Header("Premutations")]
  public Sprite[] possiblePatterns;
  public Sprite[] possibleShapes;
  public Color[] possibleColors;
  public string[] possibleResponses = { "No way", "Thank you", "Okay", "Alright", "Go on", "Are you sure?", "Shut up" };
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
    if (possibleColors.Length != Variance)
      throw new System.Exception("There must be exactly " + Variance + " colors defined");
    if (possiblePatterns.Length != Variance)
      throw new System.Exception("There must be exactly " + Variance + " patterns defined");
    if (possibleShapes.Length != Variance)
      throw new System.Exception("There must be exactly " + Variance + " shapes defined");

    DoWiggle();
    Configure(Random.Range(0, Variance), Random.Range(0, Variance), Random.Range(0, Variance)); //WIP. Randomized config.
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

    patternImage.sprite = possiblePatterns[Pattern];
    patternImage.color = possibleColors[Color];
    shapeImage.sprite = possibleShapes[Shape];
    shadowImage.sprite = possibleShapes[Shape];

    //Update the response text.
    label.text = possibleResponses[responseIndex];
    responseIndex++;
    if (responseIndex >= possibleResponses.Length)
      responseIndex = 0;
  }
}
