using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Match : MonoBehaviour
{
    [SerializeField] private GameObject lit;
    [SerializeField] private Light2D litLight;
    [SerializeField] private AudioSource source;

    // Update is called once per frame
    private void OnEnable()
    {
        lit.SetActive(false);
        lit.GetComponent<BoxCollider2D>().enabled = true;
        FindAnyObjectByType<CustomIK>().SetUsable(true);
    }

    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        if (mouseY < -.5f)
        {
            Debug.Log(mouseY);
            if (!lit.activeSelf)
            {
                Strike();
            }
        }
    }

    public void PutOut()
    {
        StartCoroutine(Outify());
    }

    private IEnumerator Outify()
    { 
        lit.GetComponent<BoxCollider2D>().enabled = false;
        for (float f = litLight.intensity; f >= 0; f -= Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            litLight.intensity = f;
        }
        lit.SetActive(false);
        litLight.intensity = 0f;
        gameObject.SetActive(false);
    }

    private IEnumerator Inify()
    {
        for (float f = 0f; f <= .25f; f += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            litLight.intensity = f * 2;
        }
        litLight.intensity = .5f;
    }

    private void Strike()
    {
        lit.SetActive(true);
        source.Play();
        StartCoroutine(Inify());
        if (FindAnyObjectByType<Tutorial>() != null && FindAnyObjectByType<Tutorial>().gameObject.activeSelf)
        {
            FindAnyObjectByType<Tutorial>().StrikeMatch();
        }
    }
}
