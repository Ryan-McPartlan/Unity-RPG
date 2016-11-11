using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Clock : MonoBehaviour {

    public Text clockText;
    public float clockTime;

    void Update()
    {
        clockTime += Time.deltaTime * 2;
        UpdateClockText();
    }

    void UpdateClockText()
    {

        int days = (int)clockTime / (24 * 60) + 1;
        int clockTime2 = (int)clockTime % (24 * 60);

        int minuets = (int)clockTime2 % 60;
        minuets -= minuets % 10;

        string minuetsString = minuets.ToString();
        if (minuets < 10)
        {
            minuetsString = "0" + minuetsString;
        }

        int hours = (int)clockTime2 / 60;
        string period = "AM";
        if (hours >= 12)
        {
            period = "PM";
        }
        if (hours == 0)
        {
            hours = 12;
        }
        if (hours > 12)
        {
            hours -= 12;
        }

        clockText.text = "Day " + days.ToString() + ", " + hours.ToString() + ":" + minuetsString + " " + period;
    }
}
