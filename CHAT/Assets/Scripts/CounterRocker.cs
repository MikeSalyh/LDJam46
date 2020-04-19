using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterRocker : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
      transform.localEulerAngles = -transform.parent.localEulerAngles;
    }
}
