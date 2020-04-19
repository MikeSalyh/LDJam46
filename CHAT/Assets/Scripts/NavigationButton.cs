using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationButton : MonoBehaviour
{
  private Button btn;
  public enum NextScene
  {
    Menu,
    Gameplay,
    Finale
  }
  public NextScene nav;

  // Start is called before the first frame update
  void Start()
  {
    btn = GetComponent<Button>();
    btn.onClick.AddListener(HandleClick);
  }

  // Update is called once per frame
  void HandleClick()
  {
    if (nav == NextScene.Gameplay)
      MetagameManager.instance.GoToGameplay();
    else if (nav == NextScene.Menu)
      MetagameManager.instance.GoToMenu();
  }
}
