using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubblingText : MonoBehaviour
{
  private TextMeshProUGUI text;
  public float rate = 0.2f;
  private int charIndex = 0;

  // Start is called before the first frame update
  void Start()
  {
    text = GetComponent<TextMeshProUGUI>();
    StartCoroutine(BubbleText());
  }

  private IEnumerator BubbleText()
  {
    while (true)
    {
      char[] ch = text.text.ToCharArray();
      ch[charIndex] = char.IsUpper(ch[charIndex]) ? char.ToLower(ch[charIndex]) : char.ToUpper(ch[charIndex]);
      text.text = new string(ch);

      charIndex++;
      if (charIndex >= text.text.Length)
        charIndex = 0;
      if (char.IsWhiteSpace(text.text[charIndex]))
        charIndex++;

      yield return new WaitForSeconds(rate);
    }
  }
}
