using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateOnInput : MonoBehaviour
{
    public Animator handAnimation;

    public InputActionProperty pinchAnimation;
    public InputActionProperty gripAnimation;    

    void Start()
    {
        
    }

    void Update()
    {
        float triggerValue = pinchAnimation.action.ReadValue<float>();
        handAnimation.SetFloat("Trigger", triggerValue);

        float gripValue = gripAnimation.action.ReadValue<float>();
        handAnimation.SetFloat("Grip", gripValue);
    }
}
