/**********************************************************************
* 一些通用的 Unity 拓展方法
***********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

public static class UniExtension
{
    /// <summary>
    /// 得到类中所有具有 T属性的字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    public static void EachFieldWithAttr<T>(this object obj, Action<FieldInfo, T> action) where T : Attribute
    {
        Type type = obj.GetType();
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        foreach (var fieldInfo in fieldInfos)
        {
            object[] objs = fieldInfo.GetCustomAttributes(typeof(T), true);
            foreach (var o in objs)
            {
                T attr = o as T;
                action(fieldInfo, attr);
            }
        }

    }

    /// <summary>
    /// 得到所有具有 T属性的类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    public static void EachClassWithAttr<T>(this object obj, Type assemblyType, Action<Type, T> action) where T : Attribute
    {
        //Assembly asm = Assembly.GetExecutingAssembly(); 只能得到UniExtension程序集
        //Assembly asm = Assembly.GetCallingAssembly();
        Assembly asm = Assembly.GetAssembly(assemblyType);
        Type[] types = asm.GetTypes();
        foreach (var type in types)
        {
            System.Object[] attrs = type.GetCustomAttributes(typeof(T), true);
            if (attrs != null && attrs.Length > 0)
            {
                foreach (var attr in attrs)
                {
                    T tAttr = attr as T;
                    //如果这个类包含有 tAttr 特性
                    if (tAttr != null)
                    {
                        action(type, tAttr);
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// (测试) 每帧遍历一个迭代器对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="monoBehaviour"></param>
    /// <param name="enumerator"></param>
    /// <param name="action"></param>
    public static void FrameForeach<T>(MonoBehaviour monoBehaviour, IEnumerator<T> enumerator, Action<object> action)
    {
        monoBehaviour.StartCoroutine(FrameForeachCoro(enumerator, action));
    }

    static IEnumerator FrameForeachCoro(IEnumerator enumerator, Action<object> action)
    {
        while (enumerator.MoveNext())
        {
            action(enumerator.Current);
            yield return null;
        }
    }

    /// <summary>
    /// (测试)自动输出错误信息
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="action"></param>
    public static void Try(this object obj, Action<List<object>> action)
    {
        List<object> list = new List<object>();
        try
        {
            action?.Invoke(list);
        }
        catch (Exception)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in list)
            {
                stringBuilder.Append("{" + item + "}");
            }

            Debug.LogError(stringBuilder);
        }
    }

    /// <summary>
    /// 删除本物体
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="delay"></param>
    public static void Destroy(this MonoBehaviour mono, float delay = 0)
    {
        GameObject.Destroy(mono.gameObject, delay);
    }

    /// <summary>
    /// 根据key得到value,失败返回空
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="Tvalue"></typeparam>
    /// <param name="dict"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static Tvalue GetValue<Tkey, Tvalue>(this Dictionary<Tkey, Tvalue> dict, Tkey key)
    {
        Tvalue tvalue = default(Tvalue);
        dict.TryGetValue(key, out tvalue);
        return tvalue;
    }
}
