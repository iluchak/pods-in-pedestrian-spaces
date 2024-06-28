using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//source: https://vionixstudio.com/2022/07/30/unity-stopwatch/ stopwatch (even though it's called Timer here)
public class Timer : MonoBehaviour
{

    float val;
    bool srt;
    public Text disvar;

    void Start()
        {
            val=0;
            srt=false;
        }

    void Update() 

    { 

        if(srt)     

            {         
                val += Time.deltaTime;    
            } 

        double b = System.Math.Round (val, 2);     
    
        disvar.text = b.ToString() + " seconds";    
    }
    public void stopbutton()
    {
        srt=false;
    }   
    public void resetbutton()
    {
        srt=false;
        val=0;
    }
    public void startbutton()
    {
    srt=true;
    }
    // public Text timerMinutes;
    // public Text timerSeconds;
    // public Text timerSeconds100;

    // private float startTime;
    // private float stopTime;
    // private float timerTime;
    // private bool isRunning = false;
    // // Start is called before the first frame update
    // void Start()
    // {
    //        TimerReset();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     timerTime = stopTime + (Time.time - startTime);
    //     int minutesInt = (int)timerTime / 60;
    //     int secondsInt = (int)timerTime % 60;
    //     int seconds100Int = (int)(Mathf.Floor((timerTime - (secondsInt + minutesInt * 60)) * 100));

    //     if (isRunning)
    //     {
    //         timerMinutes.text = (minutesInt < 10) ? "0" + minutesInt : minutesInt.ToString();
    //         timerSeconds.text = (secondsInt < 10) ? "0" + secondsInt : secondsInt.ToString();
    //         timerSeconds100.text = (seconds100Int < 10) ? "0" + seconds100Int : seconds100Int.ToString();
    //     }
    // }
    // public void TimerStart() {
    //     if (!isRunning) {
    //         print("START");
    //         isRunning = true;
    //         startTime = Time.time;       
    //     }
    // }
    // public void TimerStop()
    // {
    //     if (isRunning)
    //     {
    //         print("STOP");
    //         isRunning = false;
    //         stopTime = timerTime;
    //     }
    // }

    // public void TimerReset()
    // {
    //     print("RESET");
    //     stopTime = 0;
    //     isRunning = false;
    //     timerMinutes.text = timerSeconds.text = timerSeconds100.text = "00";
    // }
}
