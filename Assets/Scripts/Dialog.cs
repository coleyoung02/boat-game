using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Dialog : MonoBehaviour
{
    private enum Mode
    {
        Read,
        Char,
        Wait
    }

    [SerializeField] private TextAsset txtFile;
    [SerializeField] private List<string> names;
    [SerializeField] private List<string> dialogue;
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private float charDelay;
    [SerializeField] private float afterTextLength;

    private float timer;
    private int index;
    private Mode cmode;
    private float expectedLength;
    private float charClock;
    private bool charAddingMode = false;

    private void OnEnable()
    {
        index = 0;
        cmode = Mode.Read;

        string text = txtFile.text;
        string currentStr = "";
        bool readMode = false;
        dialogue = new List<string>();
        for (int i = 0; i < text.Length; ++i)
        {
            if (text[i] == '{')
            {
                readMode = true;
                Debug.Log("bracket open");
            }
            else if (text[i] == '}')
            {
                readMode = false;
                Debug.Log("bracket close");
            }
            else if (readMode)
            {
                Debug.Log("read name");
                if (currentStr.Length > 0)
                {
                    dialogue.Add(currentStr);
                }
                currentStr = names[(text[i] - '0') - 1] + ": ";
            }
            else
            {
                currentStr += text[i].ToString();
            }
        }
        dialogue.Add(currentStr.ToString());

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
    }

    private void OnEnd()
    {

        gameObject.SetActive(false);
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
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return);
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
