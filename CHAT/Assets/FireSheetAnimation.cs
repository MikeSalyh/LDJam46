using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSheetAnimation : MonoBehaviour
{
  public Sprite[] sprites;
  private int counter = 0;
  private Image img;
  public int fps;

    // Start is called before the first frame update
    void Start()
    {
    img = GetComponent<Image>();
    StartCoroutine(Animate());
    }

  private IEnumerator Animate()
  {
    while (true)
    {
      counter++;
      if (counter >= sprites.Length)
        counter = 0;

      img.sprite = sprites[counter];
      yield return new WaitForSeconds(1f / fps);
    }
  }
}
