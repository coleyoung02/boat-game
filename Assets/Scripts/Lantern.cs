using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lantern : MonoBehaviour
{
    [SerializeField] private GameObject lightsource;
    [SerializeField] private Light2D lightComp;
    [SerializeField] private float fadeInRate;

    private bool isOn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOn)
        {
            lightsource.SetActive(true);
            FindAnyObjectByType<StoryManager>().StartNextAct();
            lightComp.intensity = 0;
            isOn = true;
        }
    }

    private void Update()
    {
        if (isOn)
        {
            if (lightComp.intensity < 1.5)
            {
                lightComp.intensity = Mathf.Min(1.5f, lightComp.intensity + fadeInRate * Time.deltaTime);
                lightComp.pointLightOuterRadius = lightComp.intensity * 4;
            }
        }
    }

    public void Extinguish()
    {
        if (isOn)
        {
            lightsource.SetActive(false);
            isOn = false;
        }
    }
}
