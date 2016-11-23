using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Credits : MonoBehaviour
{
    public GameObject textBox;
    public Text theText;


    public TextAsset textFile;
    public string[] textLines;


    public int currentLine;
    public int endAtLine;

    public bool started;

    // Use this for initialization
    void Start()
    {
        started = false;

        if (textFile != null)
        {
            textLines = (textFile.text.Split('\n'));
        }


        if (endAtLine == 0)
        {
            endAtLine = textLines.Length - 1;
        }
    }

     void Update()
     {
        theText.text = textLines[currentLine];

        while (currentLine < 10)
        {
            print("Is this working");
            updateLine();
        }

        if(!started)
        {
            started = true;
            StartCoroutine("waitThreeSeconds");
            updateLine();
            currentLine += 1;
            theText.text = textLines[currentLine];
        }
    }

    void updateLine()
    {
        StartCoroutine("waitThreeSeconds");
    }

    IEnumerator waitThreeSeconds()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
