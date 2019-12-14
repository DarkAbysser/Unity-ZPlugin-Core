/**********************************************************************
* UI 管理器
* 挂载在Canvas下
* 
* 1.方便呼出和隐藏UI面板
* 2.UI面板挂载UIPanelBase
* 3.UI面板放在Resources文件夹相应目录下面
* 4.多实例使用 Show和 Hide 控制
* 5.单实例使用 Switch 控制
*   单实例依类名为键存储在字典中,根据情况调用 Show和 Hide
***********************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    Canvas canvas;

    public string ResourcePath = "UIPanel/";
    Dictionary<Type, UIPanelBase> uiPanelDict;

    private void Awake()
    {
        Instance = this;
        canvas = GetComponent<Canvas>();
        uiPanelDict = new Dictionary<Type, UIPanelBase>();
    }

    public T Show<T>() where T : UIPanelBase
    {
        T tempT = Resources.Load<T>(ResourcePath + typeof(T).Name);
        return Instantiate(tempT, canvas.transform);
    }

    public void Hide(UIPanelBase UIPanelBase)
    {
        Destroy(UIPanelBase.gameObject);
    }

    public bool Switch<T>() where T : UIPanelBase
    {
        if (uiPanelDict.TryGetValue(typeof(T), out var uiPanel))
        {
            Hide(uiPanel);
            return false;
        }
        else
        {
            T uiPanel1 = Show<T>();
            uiPanelDict.Add(typeof(T), uiPanel1);
            return true;
        }
    }

    public bool Switch<T>(out T uiPanel1) where T : UIPanelBase
    {
        if (uiPanelDict.TryGetValue(typeof(T), out var uiPanel))
        {
            Hide(uiPanel);
            uiPanel1 = null;
            return false;
        }
        else
        {
            uiPanel1 = Show<T>();
            uiPanelDict.Add(typeof(T), uiPanel1);
            return true;
        }

    }

    public bool TryGet<T>(out T uiPanel) where T : UIPanelBase
    {
        if (uiPanelDict.TryGetValue(typeof(T), out var uIPanel))
        {
            uiPanel = uIPanel as T;
            return true;
        }
        else
        {
            uiPanel = null;
            return false;
        }
    }
}
