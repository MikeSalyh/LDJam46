using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SpeechBubble : MonoBehaviour
{
  public string[] responses = { "No way", "Thank you", "Okay", "Alright", "Go on", "Are you sure?", "Shut up" };
  public Image maskImage, shadowImage;
  public TextMeshProUGUI label;
  public Sprite[] patterns;
  public Sprite[] bubbles;
  public Color[] colors;


  // Start is called before the first frame update
  void Start()
  {
    DoWiggle();
  }

  private void DoWiggle()
  {
    float tempo = 1f;
    float offset = Random.Range(5f, 10f);
    transform.localEulerAngles = new Vector3(0f, 0f, -offset);
    transform.DORotate(new Vector3(0f, 0f, offset), tempo).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
  }
}
