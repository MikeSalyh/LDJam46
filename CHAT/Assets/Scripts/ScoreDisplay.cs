using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {
    GetComponent<TextMeshProUGUI>().text = MetagameManager.instance.score.ToString("00");
  }
}
