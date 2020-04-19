using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
  private Button btn;

    // Start is called before the first frame update
    void Start()
    {
    btn = GetComponent<Button>();
    btn.onClick.AddListener(HandleClick);
    }

    // Update is called once per frame
    void HandleClick()
    {
    Debug.Log("Click!");
    MetagameManager.instance.GoToGameplay();
    }
}
