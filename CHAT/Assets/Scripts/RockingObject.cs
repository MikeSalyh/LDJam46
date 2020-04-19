using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class RockingObject : MonoBehaviour
{
  public bool ccw;
  // Start is called before the first frame update
  void Start()
  {
    DoWiggle();
  }

  private void DoWiggle()
  {
    float tempo = 1f;
    float offset = Random.Range(5f, 10f);
    if (ccw)
      offset *= -1f;

    transform.localEulerAngles = new Vector3(0f, 0f, -offset);
    transform.DORotate(new Vector3(0f, 0f, offset), tempo).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
  }
}
