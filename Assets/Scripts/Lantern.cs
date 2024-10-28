using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class Lantern : MonoBehaviour
{
    [SerializeField] private GameObject lightsource;
    [SerializeField] private Light2D lightComp;
    [SerializeField] private float fadeInRate;
    [SerializeField] private Match match;
    [SerializeField] private GameObject matchButton;

    private bool isOn = false;

    private float lightTarget;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isOn)
        {
            lightsource.SetActive(true);
            FindAnyObjectByType<StoryManager>().StartNextAct();
            lightComp.intensity = 0;
            isOn = true;

            if (FindAnyObjectByType<Tutorial>() != null && FindAnyObjectByType<Tutorial>().gameObject.activeSelf)
            {
                FindAnyObjectByType<Tutorial>().LightLantern();
            }
            match.PutOut();
            FindAnyObjectByType<CustomIK>().SetUsable(false);
        }
    }

    private void Update()
    {
        if (isOn)
        {
            if (lightTarget < 1.5)
            {
                lightTarget = Mathf.Min(1.5f, lightTarget + fadeInRate * Time.deltaTime);
            }
            float noise = Mathf.PerlinNoise((Time.time * 1.5f), (Time.time * Mathf.Sqrt(2)));
            lightComp.intensity = noise * (lightTarget - .3f) + .3f;
            lightComp.pointLightOuterRadius = noise * lightTarget / 3 + 5 * lightTarget;
        }
        else
        {
            float noise = Mathf.PerlinNoise((Time.time * 1.5f), (Time.time * Mathf.Sqrt(2)));
            lightComp.intensity = noise * (lightTarget - .3f) + .3f * lightTarget / 1.5f;
            lightComp.pointLightOuterRadius = noise * lightTarget / 3 + 5 * lightTarget;
        }
    }

    private IEnumerator GoOut(bool isFinal)
    {
        for (float f = 1.5f; f >= 0; f -= Time.deltaTime * 4)
        {
            yield return new WaitForEndOfFrame();
            lightTarget = f;
        }
        lightsource.SetActive(false);
        if (isFinal)
        {
            SceneManager.LoadScene("End");
        }
        else
        {
            matchButton.SetActive(true);
            Debug.LogError("ENABLING FROM HERE");
            FindAnyObjectByType<RowerController>().OnIkEnabled();
        }
    }

    public void Extinguish(bool isFinal)
    {
        if (isOn)
        {
            isOn = false;
            StartCoroutine(GoOut(isFinal));
        }
    }
}
