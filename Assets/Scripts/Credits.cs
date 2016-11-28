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

        if(!started)
        {
            started = true;
            StartCoroutine("waitThreeSeconds");
        }
    }

    void updateLine()
    {
        StartCoroutine("waitThreeSeconds");
    }

    IEnumerator waitThreeSeconds()
    {
        yield return new WaitForSeconds(3.0f);
        theText.text = "Project Leads:";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Nazely Hartoonian";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Ulises Perez";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Programmers:";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Ulises Perez";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Christopher Chu";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Yixuan (Angela) Li";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Artist:";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Victoria Barinova";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Audio & Design:";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Nazely Hartoonian";
        yield return new WaitForSeconds(3.0f);
        theText.text = "Joshua Rutledge";
        yield return new WaitForSeconds(3.0f);
        theText.text = " ";
    }
}
