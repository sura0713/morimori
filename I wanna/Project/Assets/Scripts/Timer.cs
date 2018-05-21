using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    Text timeText;

    public static float time;
    
    // Use this for initialization
    void Start () {

        timeText = this.GetComponent<Text>();
        time = 0;

    }
	
	// Update is called once per frame
	void Update () {

        countTime();

    }

    void countTime()
    {
        time += Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        int mseconds = Mathf.FloorToInt((time - minutes * 60 - seconds) * 1000);
        string niceTime = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, mseconds);

        timeText.text = niceTime;
    }
}
