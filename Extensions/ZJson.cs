using System;
using System.Collections.Generic;
using System.Reflection;

namespace ConsoleApp1
{
    class ZJson
    {
        //static void Main(string[] args)
        //{
        //    //string name = "dsfa";
        //    //List<object> list = new List<object>();
        //    //object[] objs = new object[2];
        //    //float f = 1;
        //    //Dictionary<string, object> dict = new Dictionary<string, object>();
        //    //object obj = dict;
        //    //Type t = obj.GetType();
        //    //Console.WriteLine(t.IsArray);
        //    //Console.WriteLine(t.IsClass);
        //    //Console.WriteLine(t.IsValueType);

        //    Dictionary<string, object> dict = new Dictionary<string, object>();
        //    dict.Add("str", "123123");
        //    dict.Add("fnum", 3123);
        //    dict.Add("list", new List<object>() { 1f, 2, "哈撒给", new B(), 5f });

        //    Dictionary<string, object> dictb = new Dictionary<string, object>();
        //    dictb.Add("str", "dict b");
        //    dict.Add("b", dictb);
        //    A a = new A();

        //    ObjectToClass(dict, a);
        //    object obj = ClassToObject(a);
        //    Console.WriteLine(a);
        //}


        /// <summary>
        /// c是集合或者类会自动更改，
        /// 返回值是为了返回值类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static object ObjectToClass(object obj, object c)
        {
            if (c is List<object> c_list)
            {
                if (obj is List<object> obj_list)
                {
                    for (int i = 0; i < obj_list.Count; i++)
                    {
                        if (i < c_list.Count)
                        {
                            c_list[i] = ObjectToClass(obj_list[i], c_list[i]);
                        }
                        else
                        {
                            if (obj_list[i].GetType().IsValueType)
                                c_list.Add(ObjectToClass(obj_list[i], 0f));
                            else
                                c_list.Add(ObjectToClass(obj_list[i], ""));
                        }
                    }
                    return c;
                }
            }

            if (c is string)
            {
                return obj.ToString();
            }

            if (obj is Dictionary<string, object> obj_dict)
            {
                foreach (var obj_item in obj_dict)
                {
                    var fi = c.GetType().GetField(obj_item.Key);
                    fi.SetValue(c, ObjectToClass(obj_item.Value, fi.GetValue(c)));
                }
                return c;
            }

            return obj;
        }

        public static object ClassToObject(object c)
        {
            if (c is string)
            {
                return c;
            }

            if (c is List<object> c_list)
            {
                List<object> list = new List<object>();
                foreach (var item in c_list)
                {
                    list.Add(ClassToObject(item));
                }
                return list;
            }

            Type c_type = c.GetType();
            if (c_type.IsValueType)
            {
                return c;
            }

            FieldInfo[] fs = c_type.GetFields();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (var fi in fs)
            {
                dict.Add(fi.Name, ClassToObject(fi.GetValue(c)));
            }
            return dict;
        }

        //public class A
        //{
        //    public string str = "dasfa";
        //    public float fnum = 321;
        //    //public List<object> list = new List<object>() { 3, 4f, 3f, 2f, 4f };
        //    public List<object> list = new List<object>();
        //    public B b = new B();
        //    //public float fnum = 321;
        //}

        //public class B
        //{
        //    public string str = "";
        //}
    }
}
