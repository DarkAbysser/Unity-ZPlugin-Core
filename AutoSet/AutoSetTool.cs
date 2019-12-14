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

using OKZKX.UnityExtension;
using System;
using UnityEngine;


namespace OKZKX.UnityTool
{


    /// <summary>
    /// AutoSet 特性辅助类
    /// </summary>
    public static class AutoSetTool
    {
        /// <summary>
        /// 为所有 AutoSet 特性的字段设置引用
        /// </summary>
        /// <param name="origin"></param>
        public static void SetFileds(Component origin)
        {
            RefrectionTool.EachFieldWithAttr<AutoSetAttribute>(origin, (fieldInfo, arr) =>
             {
                 string name = FormatName(arr.Name, fieldInfo.Name);
                 object value = GetComp(origin, arr.SetBy, fieldInfo.FieldType, name);
                 fieldInfo.SetValue(origin, value);
             });
        }

        private static object GetComp(Component origin, SetBy setBy, Type fieldType, string name)
        {
            switch (setBy)
            {
                case SetBy.Children:
                    return origin.transform.GetComponentInChildren(fieldType, name);
                case SetBy.Parent:
                    return origin.transform.GetComponentInParent(fieldType, name);
                case SetBy.SceneObject:
                    return origin.transform.GetComponentInScene(fieldType, name);
                default:
                    return null;
            }
        }

        private static string FormatName(string name, string fiName)
        {
            if (name == null) return null;
            return FirstCharToUpper(name == "" ? fiName : name);
        }

        private static string FirstCharToUpper(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return name[0].ToString().ToUpper() + name.Substring(1);
        }
    }
}