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

  private void Start()
  {
    slider = GetComponentInChildren<Slider>();
    GameManager.OnAnswer += HandleAnswerGraphics;
    GameManager.OnChangeState += HandleStateChange;
  }

  private void HandleStateChange(GameManager.GameState newState)
  {
    Debug.Log("Handling state change");
    if (newState == GameManager.GameState.Answering)
      StartBurning();
    //else
    //  StopBurning();
  }

  private void HandleAnswerGraphics(bool correct)
  {
    if (!correct)
    {
      transform.DOShakePosition(1.5f, 10f);
      fire.DOKill();
      fire.transform.DOKill();
      fireBar.DOKill();
      fire.alpha = 1f;
      fire.transform.localScale = Vector3.one * 1.25f;
      fire.transform.DOScale(1f, 0.15f).SetEase(Ease.OutBack);
      fire.DOFade(0f, 0.5f).SetDelay(1.5f);
      fireBar.color = Color.red;
      fireBar.DOColor(Color.gray, 0.5f).SetDelay(1.5f);
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
    fireBar.DOColor(Color.red, 0.15f);
  }

  public void StopBurning()
  {
    fire.DOKill();
    fire.transform.DOKill();
    fireBar.DOKill();
    fire.DOFade(0f, 0.25f);
    fireBar.DOColor(Color.gray, 0.35f);    
  }

  public void Update()
  {
    slider.value = Mathf.Lerp(1 - lifeManager.life, slider.value, 0.75f);
  }
}
