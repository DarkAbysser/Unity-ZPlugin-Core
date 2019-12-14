/**********************************************************************
* 以 StateEvent 为基础, 
* 整体的状态机事件管理解决方案
* 
* 遍历 animator 中的所有 StateEvent，通过名字执行相应的方法。
***********************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

public class StateEventTool
{
    #region 字典的解决方案 已经弃用

    private static Dictionary<string, StateEvent> stateEventDictionary = new Dictionary<string, StateEvent>();
    public static StateEvent GetStateEvent(string name) => stateEventDictionary[name];

    public static void AddAnimator(Animator animator, string preName = null)
    {
        if (preName != null)
        {
            preName = preName + ":";
        }
        else
        {
            preName = "";
        }

        StateEvent[] stateEvents = animator.GetBehaviours<StateEvent>();
        foreach (var item in stateEvents)
            stateEventDictionary.Add(preName + item.name, item);
    }

    public static void ClearStateEvents()
    {
        stateEventDictionary.Clear();
    }

    #endregion

    #region 遍历的解决方案

    /// <summary>
    /// 遍历动画状态机中的所有 StateEvent ,并执行相应方法
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="action"></param>
    public static void ForEach(Animator animator, Action<StateEvent, string> action)
    {
        StateEvent[] stateEvents = animator.GetBehaviours<StateEvent>();
        foreach (var stateEvent in stateEvents)
        {
            action(stateEvent, stateEvent.name);
        }
    }
    #endregion
}
