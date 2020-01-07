using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSizeToScale : MonoBehaviour
{
    public bool adjustSize;
    public bool adjustSpeed;

    // Adjust variables based on the lossy scale of x
    void Start()
    {
        //Get the modules
        var system = GetComponent<ParticleSystem>();
        var mainModule = system.main;

        //Modify the size
        if (adjustSize)
            mainModule.startSize = AdjustCurve(mainModule.startSize);

        //Modify the speed
        if (adjustSpeed)
            mainModule.startSpeedMultiplier *= transform.lossyScale.x;
    }

    //Adjust the variable to the new scale
    ParticleSystem.MinMaxCurve AdjustCurve(ParticleSystem.MinMaxCurve curve)
    {
        switch (curve.mode)
        {
            case ParticleSystemCurveMode.Constant:
                curve.constant *= transform.lossyScale.x;
                break;
            case ParticleSystemCurveMode.TwoConstants:
                curve.constantMin *= transform.lossyScale.x;
                curve.constantMax *= transform.lossyScale.x;
                break;

            case ParticleSystemCurveMode.Curve:
            case ParticleSystemCurveMode.TwoCurves:
                curve.curveMultiplier = transform.lossyScale.x;
                break;
        }

        return curve;
    }
}
