using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptiveMusic : MonoBehaviour
{
  private AudioSource src;
  public float maxPitch = 1.5f;
  public float maxGameSpeed = 0.25f;
  public LifeManager life;

  // Start is called before the first frame update
  void Start()
  {
    src = GetComponent<AudioSource>();
    src.loop = false;
    src.Play();
  }

  // Update is called once per frame
  void Update()
  {
    if (!src.isPlaying)
    {
      src.pitch = Mathf.Lerp(1f, maxPitch, Mathf.InverseLerp(0f, maxGameSpeed, life.lifeDecreasePerSecond));
      src.Play();
    }
  }
}
