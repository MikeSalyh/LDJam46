using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeMeter : MonoBehaviour
{
  public CanvasGroup fire;
  public Image fireBar;

  private void Start()
  {
    GameManager.OnChangeState += HandleStateChange;
  }

  private void HandleStateChange(GameManager.GameState newState)
  {
    Debug.Log("Handling state change");
    if (newState == GameManager.GameState.Answering)
      StartBurning();
    else
      StopBurning();
  }

  public void StartBurning()
  {
    fire.alpha = 1f;
    fire.transform.localScale = Vector3.zero;
    fire.transform.DOScale(1f, 0.15f).SetEase(Ease.OutBack);
    fireBar.DOColor(Color.red, 0.15f);
  }

  public void StopBurning()
  {
    fire.DOFade(0f, 0.25f);
    fireBar.DOColor(Color.gray, 0.35f);
  }

  public void Update()
  {
    if (Input.GetKeyDown(KeyCode.Alpha1))
      StartBurning();
    if (Input.GetKeyDown(KeyCode.Alpha2))
      StopBurning();
  }
}
