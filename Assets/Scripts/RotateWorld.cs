using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using System;

public class RotateWorld : MonoBehaviour, IMixedRealityInputHandler<Vector2>
{
    public float degrees = 45f;
    public float speed;

    string _inputName = "Rotate Map";
    float _deadzone = 0.1f;
    float _xRot = 0;

    void Update()
    {
        //Constantly rotate
        if (_xRot != 0)
            transform.Rotate(0, degrees * _xRot * Time.deltaTime, 0, Space.Self);
    }

    // Rotates Around Y Axis
    void IMixedRealityInputHandler<Vector2>.OnInputChanged(InputEventData<Vector2> eventData)
    {
        //If is not the input that we want, return
        if (eventData.MixedRealityInputAction.Description != _inputName)
            return;

        //X is the only thing that we care
        _xRot = eventData.InputData.x;

        //If is inside the deadzone, make it 0
        if (_xRot <= _deadzone && _xRot >= -_deadzone)
            _xRot = 0;
    }

    //Register and unregister callbacks
    void OnEnable()
    {
        Microsoft.MixedReality.Toolkit.CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler<Vector2>>(this);
    }

    void OnDisable()
    {
        Microsoft.MixedReality.Toolkit.CoreServices.InputSystem?.UnregisterHandler<IMixedRealityInputHandler<Vector2>>(this);
    }
}
