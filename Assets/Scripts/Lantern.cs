using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    [SerializeField] private GameObject lightsource;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAA");
        lightsource.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAA");
        lightsource.SetActive(true);

    }
}
