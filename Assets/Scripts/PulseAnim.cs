using System;
using UnityEngine;

public class PulseAnim : MonoBehaviour
{
    [SerializeField] GameObject targetObj;

    [SerializeField] float expandDuration = 1f;
    private float currentTime = 0f;
    [SerializeField] Vector3 breathIn;
    [SerializeField] Vector3 breathOut;
    private bool breathingIn = true;

    [SerializeField] bool pulsing = false;

    private void Awake()
    {
        if (!targetObj)
        {
            targetObj = this.gameObject;
        }
    }

    private void Update()
    {
        PulseUpdate();
    }

    private void PulseUpdate()
    {
        if (pulsing)
        {
            Vector3 targetScale = breathingIn ? breathIn : breathOut;
            Vector3 startScale = breathingIn ? breathOut : breathIn;

            currentTime = Time.deltaTime;
            float lerpFactor = currentTime/expandDuration;

            targetObj.transform.localScale = Vector3.Lerp(startScale, targetScale, lerpFactor);

            if(lerpFactor >= 1f)
            {
                breathingIn = !breathingIn;
                currentTime = 0f;
            }
        }
    }
}
