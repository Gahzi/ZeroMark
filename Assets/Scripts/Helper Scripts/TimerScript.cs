using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour
{

    /// <summary>
    /// Basic timer. Counts down.
    /// 
    /// </summary>

    //private float startTime;
    public const int numberOfTimers = 10;
    private float[] timerLength = new float[numberOfTimers];
    public float[] timeRemaining = new float[numberOfTimers];
    public bool[] isActive = new bool[numberOfTimers];

    /// <summary>
    /// Call this to start a timer
    /// </summary>
    /// <param name="_timerLength"> length of timer in seconds</param>
    /// <returns>The number of the timer you're using</returns>
    public int StartTimer(float _timerLength)
    {
        for (int i = 0; i < numberOfTimers; i++)
        {
            if (!IsTimerActive(i))
            {
                timerLength[i] = _timerLength;
                timeRemaining[i] = timerLength[i];
                isActive[i] = true;
                return i;
            }
        }
        return 99;
        Debug.LogError("Ran out of timers!");

    }

    public void EndTimerForcefully(int timerNumber) { }

    private void EndTimer(int timerNumber)
    {
        isActive[timerNumber] = false;
    }

    // Update is called once per frame
    public void Update()
    {
        for (int i = 0; i < numberOfTimers; i++)
        {
            if (timeRemaining[i] > 0)
            {
                timeRemaining[i] -= Time.deltaTime;
                isActive[i] = true;
            }
            else
            {
                EndTimer(i);
            }
        }

    }

    public bool IsTimerActive(int number)
    {
        if (isActive[number])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public float GetTimeRemaining(int which)
    {
        return timeRemaining[which];
    }
}
