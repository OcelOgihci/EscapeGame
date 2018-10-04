using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_script : MonoBehaviour
{

    public Text score;
    public Text Timer;

    // Update is called once per frame
    void Update()
    {
        score.text = "Score: " + ((uint)_MGR_Score.Instance.getScore()).ToString();
        Timer.text = "Timer: " + ((uint)Time.time).ToString();
    }
}
