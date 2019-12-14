/**********************************************************************
* Author: 曾楷翔
* Create Date: 2018-12-07
* Description: 自动设置引用
* 将 AutoSet 特性放在字段上 调用 this.SetFileds(推荐在 Awake方法内)为其设置引用
* 将会自动查找对应字段的类
* 
* name 为引用的实例游戏物体名称
* 当 name == "", name 为字段的名称(自动首字母大写)
* 当 name == null, 代表匹配所有的名称
* 
* SetBy 支持 后代物体,祖先物体,Resource资源(使用 path),场景内唯一且激活的物体
* 
* SetFileds 可以配置进行查找的 Transform 对象 ,默认就为本对象
***********************************************************************/

using System;
using System.Reflection;
using UnityEngine;
public enum SetBy
{
    Children,
    Parent,
    SceneObject,
    Resource
}

/// <summary>
/// AutoSet 特性, 自动设置字段引用
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class AutoSetAttribute : Attribute
{
    public string Name;
    public SetBy SetBy;
    public string Path;

    public AutoSetAttribute(string Name = null, SetBy setBy = SetBy.Children, string Path = null)
    {
        this.Name = Name;
        this.SetBy = setBy;
        this.Path = Path;
    }
}

/// <summary>
/// AutoSet 特性辅助类
/// </summary>
public static class AutoSet
{
    /// <summary>
    /// 为所有 AutoSet 特性的字段设置引用
    /// </summary>
    /// <param name="origin"></param>
    public static void SetFileds(this object obj, Component origin = null)
    {
        obj.EachFieldWithAttr<AutoSetAttribute>((fieldInfo, autoSetAttribute) =>
        {
            origin = origin ?? obj as Component;
            if (!origin)
            {
                Debug.LogError("SetFileds没有参数是 Component 类型,自动设置字段出错");
            }

            object value = null;
            //使用名字的查找
            if (autoSetAttribute.Name != null)
            {
                #region name处理

                string name;
                if (autoSetAttribute.Name == "")
                {
                    name = fieldInfo.Name;
                }
                else
                {
                    name = autoSetAttribute.Name;
                }
                name = name[0].ToString().ToUpper() + name.Substring(1);

                #endregion

                switch (autoSetAttribute.SetBy)
                {
                    case SetBy.Children:
                        value = origin.transform.GetComponentInChildren(fieldInfo.FieldType, name);
                        break;
                    case SetBy.Parent:
                        value = origin.transform.GetComponentInParent(fieldInfo.FieldType, name);
                        break;
                    case SetBy.SceneObject:
                        value = origin.transform.GetComponentInScene(fieldInfo.FieldType, name);
                        break;
                    case SetBy.Resource:
                        string path = autoSetAttribute.Path;
                        if (path != null)
                        {
                            path += "/";
                        }
                        value = Resources.Load(path + name, fieldInfo.FieldType);
                        break;
                }
            }
            //不使用名字的查找
            else
            {
                switch (autoSetAttribute.SetBy)
                {
                    case SetBy.Children:
                        value = origin.transform.GetComponentInChildren(fieldInfo.FieldType);
                        break;
                    case SetBy.Parent:
                        value = origin.transform.GetComponentInParent(fieldInfo.FieldType);
                        break;
                    case SetBy.SceneObject:
                        value = origin.transform.GetComponentInScene(fieldInfo.FieldType);
                        break;
                    case SetBy.Resource:
                        string path = autoSetAttribute.Path;
                        if (path != null)
                        {
                            path += "/";
                        }
                        value = Resources.Load(path, fieldInfo.FieldType);
                        break;
                }
            }
            fieldInfo.SetValue(obj, value);
        });
    }
}
