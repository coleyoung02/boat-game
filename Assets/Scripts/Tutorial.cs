using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    private enum Mode
    {
        Read,
        Char,
        Wait
    }

    [SerializeField] private List<string> dialogue;
    [SerializeField] private GameObject firstStory;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private float charDelay;
    [SerializeField] private float afterTextLength;

    private float timer;
    private int index;
    private Mode cmode;
    private float expectedLength;
    private float charClock;
    private bool charAddingMode = false;
    private bool matchGrabbed;
    private bool matchStruck;
    private bool lanternLit;

    private void OnEnable()
    {
        index = 0;
        cmode = Mode.Read;

    }

    private void Update()
    {
        if (index < dialogue.Count)
        {
            if (charAddingMode || timer <= 0)
            {
                ReadState();
                MainUpdateLoop();
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
        else if (timer >= 0)
        {
            timer -= Time.deltaTime;

        }
        else
        {
            OnEnd();
        }
        if (lanternLit)
        {
            textBox.text = "";
        }
    }

    public void GrabMatch()
    {
        matchGrabbed = true;
    }

    public void StrikeMatch()
    {
        matchStruck = true;
    }

    public void LightLantern()
    {
        lanternLit = true;
    }

    private void OnEnd()
    {

        gameObject.SetActive(false);
        firstStory.SetActive(true);

    }

    private IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(3f);
    }

    private void MainUpdateLoop()
    {
        if (cmode == Mode.Char)
        {
            if (charAddingMode)
            {
                textBox.text = dialogue[index].Substring(0, Mathf.Min(Mathf.RoundToInt(dialogue[index].Length * charClock / expectedLength), dialogue[index].Length));
                charClock += Time.deltaTime;

                if (textBox.text.Length == dialogue[index].Length)
                {
                    timer = afterTextLength;
                    charAddingMode = false;
                }
            }
            else if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else if (CheckAdvance())
            {
                cmode = Mode.Read;
                index++;
            }
        }
        else if (cmode == Mode.Wait)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                cmode = Mode.Read;
                index++;
            }
        }
    }

    private bool CheckAdvance()
    {
        if (FindAnyObjectByType<MenuManager>().GetPaused())
        {
            return false;
        }
        return (index == 0 && matchGrabbed) || (index == 1 && matchStruck) || (index == 2 && lanternLit);
    }

    private void ReadState()
    {
        if (cmode == Mode.Read)
        {
            if (dialogue[index] != null && dialogue[index].Length > 0)
            {
                textBox.text = "";
                cmode = Mode.Char;
                charClock = 0;
                expectedLength = charDelay * dialogue[index].Length;
                charAddingMode = true;
            }
            else
            {
                cmode = Mode.Wait;
            }
        }
    }
}
