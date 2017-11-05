using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnergyBar))]
public class EnergyUI : MonoBehaviour
{
    public EnergyBar bar;

    private void Awake()
    {
        bar = GetComponent<EnergyBar>();
    }

    public void Reset()
    {
        if (bar == null)
        {
            bar = GetComponent<EnergyBar>();
        }
        bar.ChargeToMin(0f);
    }

    public void GetReady(float duration)
    {
        bar.ChargeToMax(duration);
    }
}
