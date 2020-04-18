using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Shadow))]
public class RotatingShadow : MonoBehaviour
{
  // Start is called before the first frame update
  private Vector2 _effectDistanceAtZero;
  private Shadow shadow;

  void Start()
  {
    shadow = GetComponent<Shadow>();
    _effectDistanceAtZero = new Vector2(shadow.effectDistance.x, shadow.effectDistance.y);
  }

  // Update is called once per frame
  void Update()
  {
    shadow.effectDistance = Rotate(_effectDistanceAtZero, -transform.eulerAngles.z);    
  }

  public Vector2 Rotate(Vector2 v, float degrees)
  {
    float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
    float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

    float tx = v.x;
    float ty = v.y;
    v.x = (cos * tx) - (sin * ty);
    v.y = (sin * tx) + (cos * ty);
    return v;
  }
}
