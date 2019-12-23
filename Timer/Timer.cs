/**********************************************************************
* 使用 协程的方式进行一些异步的方法
* 
* Timer 计时器会不断地进行计时
* 
* 时间插值:
* To 方法会在每帧执行方法,并传入相应的线性插值
***********************************************************************/
using System;
using System.Collections;
using UnityEngine;

public class Timer
{
    public bool Running = true;
    public float Time;
    public Timer()
    {
        MonoBehaviour mono = GameObject.FindObjectOfType<MonoBehaviour>();
        mono.StartCoroutine(UpdateTimerCoro());
    }

    public static implicit operator float(Timer timer)
    {
        return timer.Time;
    }

    /// <summary>
    /// 重置计时器
    /// </summary>
    public void Reset() => Time = 0;

    private IEnumerator UpdateTimerCoro()
    {
        while (true)
        {
            if (Running)
            {
                Time += UnityEngine.Time.deltaTime;
            }
            yield return null;
        }
    }

    /// <summary>
    /// 延时执行
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delay"></param>
    public static void InvokeDelay(Action action, float delay)
    {
        MonoBehaviour mono = GameObject.FindObjectOfType<MonoBehaviour>();
        mono.StartCoroutine(InvokeDelayCoro(action, delay));
    }

    static IEnumerator InvokeDelayCoro(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    /// <summary>
    /// 时间插值
    /// </summary>
    /// <param name="x"></param>
    /// <param name="target"></param>
    /// <param name="time"></param>
    /// <param name="action"></param>
    public static void To(float x, float target, float time, Action<float> action)
    {
        MonoBehaviour mono = GameObject.FindObjectOfType<MonoBehaviour>();
        mono.StartCoroutine(ToCoro(x, target, time, action));
    }
    static IEnumerator ToCoro(float start, float target, float time, Action<float> action)
    {
        float timer = 0;
        float temp = start;
        while (timer < time)
        {
            temp = Mathf.Lerp(start, target, timer / time);
            action(temp);
            yield return null;
            timer += UnityEngine.Time.deltaTime;
        }
        action(target);
    }
}
