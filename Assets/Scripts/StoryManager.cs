using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> acts;
    private int index = -1;

    public void StartNextAct()
    {
        index++;
        if (index < acts.Count)
        {
            acts[index].SetActive(true);
        }
    }
}
