using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : MonoBehaviour
{
    private bool isSpinStart = false;

    private void Update()
    {
        if(isSpinStart)
        {
            float spinAddAmount = 360.0f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        }
    }

    public void Spin()
    {
        isSpinStart = true;
    }
}
