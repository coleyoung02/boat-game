using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowerController : MonoBehaviour
{
    [SerializeField] private Animator rower;
    [SerializeField] private Animator rowerArm;
    [SerializeField] private GameObject rowerArmObj;
    [SerializeField] private GameObject ikArm;
    [SerializeField] private GameObject oar;

    public void OnHandReturn()
    {
        Debug.LogError("they hit the pentagon");
        ikArm.SetActive(false);
        rowerArmObj.SetActive(true);
        oar.SetActive(false);
    }

    public void OnIkEnabled()
    {
        Debug.LogError("ENABLING FROM there");
        ikArm.SetActive(true);
        FindAnyObjectByType<CustomIK>().SetUsable(true);
        rowerArmObj.SetActive(false);
        oar.SetActive(true);

    }
}
