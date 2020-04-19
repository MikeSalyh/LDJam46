using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Raccoon : MonoBehaviour
{
  public enum State
  {
    init,
    idle,
    talking,
    sad
  }
  private State myState;
  public Sprite[] talkingSprites;
  public Sprite[] sadSprites;
  public Sprite[] idleSprites;
  private int counter;
  private Sprite[] currentSequence;
  public Image head;
  public float fps;
  private bool looping = false;
  public bool tickCCW;

  // Start is called before the first frame update
  void Start()
  {
    currentSequence = idleSprites;
    StartCoroutine(Animate());
    DoWiggle();
  }

  private void DoWiggle()
  {
    float tempo = 1f;
    float offset = 5f;
    if (tickCCW)
      offset *= -1f;

    //transform.localEulerAngles = new Vector3(0f, 0f, -offset);
    transform.DOLocalMove(new Vector3(0f, offset, 0f), tempo).SetRelative(true).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutBack);
  }

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
      SwitchState(State.idle);
    if (Input.GetKeyDown(KeyCode.Alpha2))
      SwitchState(State.sad);
    if (Input.GetKeyDown(KeyCode.Alpha3))
      SwitchState(State.talking);

  }

  public void SwitchState(State newState)
  {
    if (myState == newState)
      return;

    Debug.Log("Playing " + newState);
    myState = newState;
    switch (newState) {
      case State.idle:
        currentSequence = idleSprites;
        looping = true;
        break;
      case State.sad:
        currentSequence = sadSprites;
        looping = false;
        break;
      case State.talking:
        looping = true;
        currentSequence = talkingSprites;
        break;
    }
    counter = 0;
  }

  private IEnumerator Animate()
  {
    while (true)
    {
      counter++;
      if (counter >= currentSequence.Length)
      {
        if (looping)
          counter = 0;
      }
      if (counter < currentSequence.Length)
      {
        head.sprite = currentSequence[counter];
        //head.GetComponent<RectTransform>().sizeDelta = new Vector2(currentSequence[counter].rect.width, currentSequence[counter].rect.height);
      }
      yield return new WaitForSeconds(1f / fps);
    }
  }
}
