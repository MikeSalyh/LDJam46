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

  private AudioSource audioSrc;
  public AudioClip[] declarativeSFX;
  public AudioClip[] interogativeSFX;
  public float speakDelay = 0.1f;

  // Start is called before the first frame update
  void Start()
  {
    audioSrc = GetComponent<AudioSource>();
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

  public void DoTalk(string spokenWords, bool isQuestion)
  {
    SwitchState(State.talking);
    StartCoroutine(TalkCoroutuine(spokenWords.Length / 2, isQuestion));
  }

  private IEnumerator TalkCoroutuine(int length, bool isQuestion)
  {
    int counter = length;
    while (counter > 0)
    {
      counter--;
      if(isQuestion)
        audioSrc.PlayOneShot(interogativeSFX[Random.Range(0, interogativeSFX.Length)]);
      else
        audioSrc.PlayOneShot(declarativeSFX[Random.Range(0, declarativeSFX.Length)]);
      yield return new WaitForSeconds(speakDelay);
    }
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
