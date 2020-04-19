using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeechBubble : MonoBehaviour
{
  public Image shapeImage, shadowImage, patternImage, suitImage;
  public GameObject flippingCard;
  public TextMeshProUGUI label;

  private static int responseIndex = 0;
  public bool tickClockwise = true;
  public Vector3 defaultLocalScale;
  public Vector3 defaultLocalPosition;

  private int _color, _pattern, _shape, _suit;
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
  public int Suit
  {
    get { return _suit; }
  }


  // Start is called before the first frame update
  void Start()
  {
    defaultLocalScale = transform.localScale;
    defaultLocalPosition = transform.localPosition;
    DoWiggle();
  }

  private void DoWiggle()
  {
    float tempo = 1f;
    float offset = Random.Range(5f, 10f);
    if (tickClockwise)
      offset *= -1f;

    transform.localEulerAngles = new Vector3(0f, 0f, -offset);
    transform.DORotate(new Vector3(0f, 0f, offset), tempo).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
  }

  private void Configure(int pattern, int shape, int color, int suit)
  {
    _pattern = pattern;
    _shape = shape;
    _color = color;
    _suit = suit;

    patternImage.sprite = GameManager.instance.possiblePatterns[Pattern];
    patternImage.color = GameManager.instance.possibleColors[Color];
    shapeImage.sprite = GameManager.instance.possibleShapes[Shape];
    shadowImage.sprite = GameManager.instance.possibleShapes[Shape];
    suitImage.color = GameManager.instance.possibleColors[Color] + UnityEngine.Color.grey;
    suitImage.sprite = GameManager.instance.possibleSuits[Suit];

    //Update the response text.
    label.text = GameManager.instance.possibleResponses[responseIndex];
    responseIndex++;
    if (responseIndex >= GameManager.instance.possibleResponses.Length)
      responseIndex = 0;
  }

  public void ConfigurePrompt()
  {
    int color = 0, pattern = 0, suit = 0, shape = 0;
    if (GameManager.instance.randomizeColors)
      color = Random.Range(0, GameManager.instance.possibleColors.Length);
    if (GameManager.instance.randomizePatterns)
      pattern = Random.Range(0, GameManager.instance.possiblePatterns.Length);
    if (GameManager.instance.randomizeSuits)
      suit = Random.Range(0, GameManager.instance.possibleSuits.Length);
    if (GameManager.instance.randomizeShapes)
      shape = Random.Range(0, GameManager.instance.possibleShapes.Length);

    Configure(pattern, shape, color, suit);
  }

  private void ConfigureWrongAnswer(int correctPattern, int correctShape, int correctColor, int correctSuit)
  {
    Configure(GetDistractorPattern(correctPattern), GetDistractorShape(correctShape), GetDistractorColor(correctColor), GetDistractorSuit(correctSuit));
  }

  public void ConfigureWrongAnswer(SpeechBubble correctAnswer)
  {
    ConfigureWrongAnswer(correctAnswer.Pattern, correctAnswer.Shape, correctAnswer.Color, correctAnswer.Suit);
  }

  private void ConfigureCorrectAnswer(int correctPattern, int correctShape, int correctColor, int correctSuit)
  {
    //Find out which properties can be randomized
    List<int> possibleCategories = new List<int>();
    if (GameManager.instance.randomizePatterns)
      possibleCategories.Add(0);
    if (GameManager.instance.randomizeShapes)
      possibleCategories.Add(1);
    if (GameManager.instance.randomizeColors)
      possibleCategories.Add(2);
    if (GameManager.instance.randomizeSuits)
      possibleCategories.Add(3);

    if (possibleCategories.Count == 0)
      throw new System.Exception("At least 1 category must be randomized in the game manager");

    int pickedCategory = possibleCategories[Random.Range(0, possibleCategories.Count)];
    if(pickedCategory == 0)
      Configure(correctPattern, GetDistractorShape(correctShape), GetDistractorColor(correctColor), GetDistractorSuit(correctSuit));
    else if(pickedCategory == 1)
      Configure(GetDistractorPattern(correctPattern), correctShape, GetDistractorColor(correctColor), GetDistractorSuit(correctSuit));
    else if(pickedCategory == 2)
      Configure(GetDistractorPattern(correctPattern), GetDistractorShape(correctShape), correctColor, GetDistractorSuit(correctSuit));
    else if(pickedCategory == 3)
      Configure(GetDistractorPattern(correctPattern), GetDistractorShape(correctShape), GetDistractorColor(correctColor), correctSuit);
  }

  public void ConfigureCorrectAnswer(SpeechBubble correctAnswer)
  {
    ConfigureCorrectAnswer(correctAnswer.Pattern, correctAnswer.Shape, correctAnswer.Color, correctAnswer.Suit);
  }

  public bool IsMatch(SpeechBubble other)
  {
    if (other.Shape == this.Shape)
      return true;
    if (other.Color == this.Color)
      return true;
    if (other.Pattern == this.Pattern)
      return true;
    if (other.Suit == this.Suit)
      return true;
    return false;
  }

  private int GetDistractorShape(int correctAnswer)
  {
    if (GameManager.instance.randomizeShapes)
      return GetRandomIntExcluding(correctAnswer, GameManager.instance.possibleShapes.Length);
    else return correctAnswer == 1 ? 0 : 1;
  }

  private int GetDistractorColor(int correctAnswer)
  {
    if(GameManager.instance.randomizeColors)
      return GetRandomIntExcluding(correctAnswer, GameManager.instance.possibleColors.Length);
    else return correctAnswer == 1 ? 0 : 1;
  }

  private int GetDistractorPattern(int correctAnswer)
  {
    if(GameManager.instance.randomizePatterns)
      return GetRandomIntExcluding(correctAnswer, GameManager.instance.possiblePatterns.Length);
    else return correctAnswer == 1 ? 0 : 1;
  }

  private int GetDistractorSuit(int correctAnswer)
  {
    if(GameManager.instance.randomizeSuits)
      return GetRandomIntExcluding(correctAnswer, GameManager.instance.possibleSuits.Length);
    else return correctAnswer == 1 ? 0 : 1;
  }

  public void Reveal(string text)
  {
    StopAllCoroutines();
    StartCoroutine(RevealCoroutine(text, 0.25f));
  }

  private IEnumerator RevealCoroutine(string text, float turnTime)
  {
    flippingCard.transform.DORotate(new Vector3(0f, 180f, 0f), turnTime).SetEase(Ease.InOutSine);
    yield return new WaitForSeconds(turnTime / 2f);
    suitImage.enabled = false;
    label.text = text;
    label.gameObject.SetActive(true);
    label.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); 
    label.transform.DOScale(new Vector3(-1f, 1f, 1f), 0.15f).SetEase(Ease.OutBack);
    yield break;
  }

  public void Hide(float hideTime)
  {
    StopAllCoroutines();
    StartCoroutine(HideCoroutine(hideTime));
  }

  private IEnumerator HideCoroutine(float hideTime = 0.25f)
  {
    transform.DOScale(0f, hideTime).SetEase(Ease.InBack);
    yield return new WaitForSeconds(hideTime);
    gameObject.SetActive(false);
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
