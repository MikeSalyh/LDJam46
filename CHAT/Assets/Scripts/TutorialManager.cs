using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
  public GameObject[] tutorials;
  private int currentSlide = 0;
  public TextMeshProUGUI label;


  // Start is called before the first frame update
  void OnEnable()
  {
    currentSlide = 0;
    Advance();
  }

  public void Advance()
  {
    if (currentSlide == tutorials.Length)
    {
      MetagameManager.instance.GoToGameplay();
      return;
    }

    for (int i = 0; i < tutorials.Length; i++)
    {
      tutorials[i].gameObject.SetActive(false);
    }
    tutorials[currentSlide].gameObject.SetActive(true);
    currentSlide++;

    if (currentSlide == tutorials.Length)
    {
      label.text = "PLAY";
    }
    else
    {
      label.text = "NEXT>";
    }
  }
}
