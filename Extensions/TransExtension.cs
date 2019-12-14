/**********************************************************************
* Transform 拓展方法
* 最常用 拓展脚本查找方法,
* 可以更方便的查找组件
***********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransExtension
{
    #region 拓展脚本查找方法

    /// <summary>
    /// 得到场景中的一个未隐藏的脚本实例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static T GetComponentInScene<T>(this Component component, string name = null) where T : Component
    {
        return component.GetComponentInScene(typeof(T), name) as T;
    }

    /// <summary>
    /// 得到场景中的一个未隐藏的脚本实例
    /// </summary>
    /// <param name="component"></param>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Component GetComponentInScene(this Component component, Type target, string name = null)
    {
        UnityEngine.Object[] objects = GameObject.FindObjectsOfType(target);
        foreach (var obj in objects)
        {
            if (name == null || obj.name == name)
            {
                return obj as Component;
            }
        }
        return null;
    }

    /// <summary>
    /// 根据 name 从后代物体中得到脚本
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Component GetComponentInChildren(this Component source, Type target, string name)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(source.transform);
        while (queue.Count != 0)
        {
            Transform transform = queue.Dequeue();
            if (name == null || transform.name == name)
            {
                Component t = transform.GetComponent(target);
                if (t != null)
                {
                    return t;
                }
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                queue.Enqueue(transform.GetChild(i));
            }
        }
        return null;
    }

    /// <summary>
    /// 根据 name 从后代物体中得到脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetComponentInChildren<T>(this Component source, string name) where T : Component
    {
        return (T)source.GetComponentInChildren(typeof(T), name);
    }

    /// <summary>
    /// 根据 name 从先祖物体中得到脚本
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Component GetComponentInParent(this Component source, Type target, string name)
    {
        Transform current = source.transform;
        for (; current != null; current = current.parent)
        {
            if (name == null || current.name == name)
            {
                Component comp = current.GetComponent(target);
                if (comp != null)
                {
                    return comp;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 根据 name 从先祖物体中得到脚本
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T GetComponentInParent<T>(this Component source, string name) where T : Component
    {
        return (T)source.GetComponentInParent(typeof(T), name);
    }

    /// <summary>
    /// 得到所有子物体
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Transform[] GetChilds(this Component source)
    {
        Transform trans = source.transform;
        Transform[] childs = new Transform[trans.childCount];
        for (int i = 0; i < trans.childCount; i++)
        {
            childs[i] = trans.GetChild(i);
        }
        return childs;
    }

    /// <summary>
    /// 得到所有具有 T 脚本的子物体
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Component[] GetChilds(this Component source, Type type)
    {
        Transform trans = source.transform;
        List<Component> comps = new List<Component>();
        for (int i = 0; i < trans.childCount; i++)
        {
            Transform child = trans.GetChild(i);
            Component comp = child.GetComponent(type);
            if (comp != null)
            {
                comps.Add(comp);
            }

        }
        return comps.ToArray();
    }

    #endregion

    #region 其他方法
    //static Vector3 relaV3;
    //public static void Follow(this Transform transform,Transform target)
    //{
    //    relaV3 = target.position - transform.position;

    //}
    #endregion

    #region 拓展 Transform 操作方法, 暂时不使用

    public static float X(this Transform transform)
    {
        return transform.position.x;
    }
    public static void X(this Transform transform, float value)
    {
        transform.position = new Vector3(value, transform.position.y, transform.position.z);
    }

    public static float Y(this Transform transform)
    {
        return transform.position.y;
    }
    public static void Y(this Transform transform, float value)
    {
        transform.position = new Vector3(transform.position.x, value, transform.position.z);
    }

    public static float Z(this Transform transform)
    {
        return transform.position.z;
    }
    public static void Z(this Transform transform, float value)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, value);
    }

    #endregion
}
