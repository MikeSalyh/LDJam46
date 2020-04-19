using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeMeter : MonoBehaviour
{
  private Slider slider;
  public CanvasGroup fire;
  public Image fireBar;
  public LifeManager lifeManager;
  public bool inverted = false;

  private void Start()
  {
    slider = GetComponentInChildren<Slider>();
    GameManager.OnAnswer += HandleAnswerGraphics;
    GameManager.OnChangeState += HandleStateChange;
  }

  private void HandleStateChange(GameManager.GameState newState)
  {
    Debug.Log("Handling state change");
    if (newState == GameManager.GameState.Answer)
      StartBurning();
  }

  private void HandleAnswerGraphics(bool correct)
  {
    if (!correct)
    {
      transform.DOShakePosition(1.5f, 10f);
      fire.DOFade(0f, 0.5f).SetDelay(0.5f);
    }
    else
    {
      StopBurning();
    }
  }

  public void StartBurning()
  {
    fire.DOKill();
    fire.transform.DOKill();
    fireBar.DOKill();
    fire.alpha = 1f;
    fire.transform.localScale = Vector3.zero;
    fire.transform.DOScale(1f, 0.15f).SetEase(Ease.OutBack);
  }

  public void StopBurning(float delay = 0.25f)
  {
    fire.DOKill();
    fire.transform.DOKill();
    fire.DOFade(0f, 0.25f).SetDelay(delay);
  }

  public void Update()
  {
    if (inverted)
      slider.value = Mathf.Lerp(1 - lifeManager.life, slider.value, 0.75f);
    else
      slider.value = Mathf.Lerp(lifeManager.life, slider.value, 0.75f);
  }
}
