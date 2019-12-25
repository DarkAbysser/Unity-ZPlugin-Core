/**********************************************************************

***********************************************************************/

using OKZKX.UnityExtension;
using System;
using UnityEngine;


namespace OKZKX.UnityTool
{
    /// <summary>
    /// AutoSet 特性辅助类
    /// </summary>
    public static class AutoLoadTool
    {
        /// <summary>
        /// 为所有 AutoSet 特性的字段设置引用
        /// </summary>
        /// <param name="origin"></param>
        public static void LoadFields(this Component origin)
        {
            RefrectionTool.EachFieldWithAttr<AutoLoadAttribute>(origin, (fieldInfo, attr) =>
             {
                 string name = AutoSetTool.FormatName(attr.Name, fieldInfo.Name);
                 object value = Load(attr.Path, attr.LoadFrom, name);
                 fieldInfo.SetValue(origin, value);
             });
        }

        private static object Load(string path, LoadFrom loadFrom, string name)
        {
            switch (loadFrom)
            {
                case LoadFrom.Resources:
                    return Resources.Load($"{path}{name}");
                default:
                    break;
            }
            return null;
        }
    }
}