/**********************************************************************
* OnGUI 快速工具
* 封装了一些OnGUI 方法
* 主要是实时在屏幕上显示一些信息
* 一般供测试使用
***********************************************************************/
using UnityEngine;

public enum GUIType
{
    Monitor,
    Tip
}

public class OnGUITool : MonoBehaviour
{
    static OnGUITool onGUITool;

    static string strToShow;

    static GUIType GUIType;

    public static void Show<T>(T str, GUIType GUIType = GUIType.Monitor)
    {
        if (onGUITool == null)
        {
            GameObject temp = new GameObject("OnGUITool");
            onGUITool = temp.AddComponent<OnGUITool>();
        }
        OnGUITool.GUIType = GUIType;
        strToShow = str.ToString();
    }

    public static void Stop(float t = 0)
    {
        Destroy(onGUITool.gameObject,t);
    }

    private void OnGUI()
    {
        switch (GUIType)
        {
            case GUIType.Monitor:
                GUI.Box(new Rect(10, 10, 100, 100), strToShow);
                break;
            case GUIType.Tip:
                GUI.Box(new Rect(10, 10, 600, 50), strToShow);
                Stop(0.5f);
                break;
            default:
                break;
        }
    }
}
