using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

public class RequestInfo : MonoBehaviour
{
}
/// <summary>
/// 商品类型
/// </summary>
public enum ProdKind
{
    [Description("101")]
    ChuangLian,
    [Description("10101")]
    ChuangMan,
    [Description("10103")]
    ChuangShen,
    [Description("10104")]
    ChuangSha,
    [Description("10107")]
    GuiDao,
    [Description("0")]
    ChuangYing,
    [Description("1")]
    ZhangTu,
    [Description("2")]
    DaiShi,
    [Description("10102")]
    HuaBian,
    [Description("10110")]
    ChuangJin,
    [Description("10105")]
    CeGou,
    [Description("10109")]
    BengDai,
    [Description("5")]
    ChuangGou,
    [Description("10106")]
    ChuangDai,
    [Description("10108")]
    PeiZhong,
    [Description("-1")]
    Null,
    [Description("102")]
    ChuangHu,
}

public enum FuncID
{
    [Description("3D404641")]
    House,
    [Description("3D404637")]
    Scene,
    [Description("3D404638")]
    Louyed,
    [Description("MM404442")]
    AllCurtain,
    [Description("MM404445")]
    SingleCurtain,
    [Description("3D404605")]
    FengGe,
    [Description("3D404601")]
    SceneStyle,
    //[StringValue("3D404641")]
    //House,
}

public enum ActionID
{
    [Description("page")]
    House,
    [Description("page")]
    Scene,
    [Description("roomDetail")]
    Louyed,
    [Description("detail")]
    AllCurtain,
    [Description("page")]
    SingleCurtain,
    //[StringValue("3D404641")]
    //House,
    //[StringValue("3D404641")]
    //House,
}

public static class EnumToolV2
{
    /// <summary>
    /// 从枚举中获取Description
    /// 说明：
    /// 单元测试-->通过
    /// </summary>
    /// <param name="enumName">需要获取枚举描述的枚举</param>
    /// <returns>描述内容</returns>
    public static string GetDescription(this Enum enumName)
    {
        string _description = string.Empty;
        FieldInfo _fieldInfo = enumName.GetType().GetField(enumName.ToString());
        DescriptionAttribute[] _attributes = _fieldInfo.GetDescriptAttr();
        if (_attributes != null && _attributes.Length > 0)
            _description = _attributes[0].Description;
        else
            _description = enumName.ToString();
        return _description;
    }
    /// <summary>
    /// 获取字段Description
    /// </summary>
    /// <param name="fieldInfo">FieldInfo</param>
    /// <returns>DescriptionAttribute[] </returns>
    public static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
    {
        if (fieldInfo != null)
        {
            return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        }
        return null;
    }
    /// <summary>
    /// 根据Description获取枚举
    /// 说明：
    /// 单元测试-->通过
    /// </summary>
    /// <typeparam name="T">枚举类型</typeparam>
    /// <param name="description">枚举描述</param>
    /// <returns>枚举</returns>
    public static T GetEnumName<T>(string description)
    {
        Type _type = typeof(T);
        foreach (FieldInfo field in _type.GetFields())
        {
            DescriptionAttribute[] _curDesc = field.GetDescriptAttr();
            if (_curDesc != null && _curDesc.Length > 0)
            {
                if (_curDesc[0].Description == description)
                    return (T)field.GetValue(null);
            }
            else
            {
                if (field.Name == description)
                    return (T)field.GetValue(null);
            }
        }
        throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "Description");
    }
    /// <summary>
    /// 将枚举转换为ArrayList
    /// 说明：
    /// 若不是枚举类型，则返回NULL
    /// 单元测试-->通过
    /// </summary>
    /// <param name="type">枚举类型</param>
    /// <returns>ArrayList</returns>
    public static ArrayList ToArrayList(this Type type)
    {
        if (type.IsEnum)
        {
            ArrayList _array = new ArrayList();
            Array _enumValues = Enum.GetValues(type);
            foreach (Enum value in _enumValues)
            {
                _array.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
            }
            return _array;
        }
        return null;
    }
}



///// <summary>
//    /// 从枚举中获取Description
//    /// 说明：
//    /// 单元测试-->通过
//    /// </summary>
//    /// <param name="enumName">需要获取枚举描述的枚举</param>
//    /// <returns>描述内容</returns>
//    //public static string GetDescription(this Enum enumName)
//    //{
//    //  string _description = string.Empty;
//    //  FieldInfo _fieldInfo = enumName.GetType().GetField(enumName.ToString());
//    //  DescriptionAttribute[] _attributes = _fieldInfo.GetDescriptAttr();
//    //  if (_attributes != null && _attributes.Length > 0)
//    //    _description = _attributes[0].Description;
//    //  else
//    //    _description = enumName.ToString();
//    //  return _description;
//    //}


//    /// <summary>
//    /// 将枚举转换为ArrayList
//    /// 说明：
//    /// 若不是枚举类型，则返回NULL
//    /// 单元测试-->通过
//    /// </summary>
//    /// <param name="type">枚举类型</param>
//    /// <returns>ArrayList</returns>
//    //public static ArrayList ToArrayList(this Type type)
//    //{
//    //  if (type.IsEnum)
//    //  {
//    //    ArrayList _array = new ArrayList();
//    //    Array _enumValues = Enum.GetValues(type);
//    //    foreach (Enum value in _enumValues)
//    //    {
//    //      _array.Add(new KeyValuePair<Enum, string>(value, GetDescription(value)));
//    //    }
//    //    return _array;
//    //  }
//    //  return null;
//    //}




///// <summary>
///// 返回枚举对应的描述
///// </summary>
//    public static class StringEnum
//    {
//        /// <summary>
//        /// 传入枚举  返回对应的描述
//        /// </summary>
//        /// <param name="value"></param>
//        /// <returns></returns>
//        public static string GetStringValue(Enum value)
//        {
//            string output = null;
//            Type type = value.GetType();

//            FieldInfo fi = type.GetField(value.ToString());
//            StringValue[] attrs =
//               fi.GetCustomAttributes(typeof(StringValue),
//                                       false) as StringValue[];
//            if (attrs.Length > 0)
//            {
//                output = attrs[0].Value;
//            }

//            return output;
//        }
//        /// <summary>
//        /// 根据Description获取枚举
//        /// 说明：
//        /// 单元测试-->通过
//        /// </summary>
//        /// <typeparam name="T">枚举类型</typeparam>
//        /// <param name="description">枚举描述</param>
//        /// <returns>枚举</returns>
//        public static T GetEnumName<T>(string description)
//        {
//            Type _type = typeof(T);
//            foreach (FieldInfo field in _type.GetFields())
//            {
//                DescriptionAttribute[] _curDesc = field.GetDescriptAttr();
//                if (_curDesc != null && _curDesc.Length > 0)
//                {
//                    if (_curDesc[0].Description == description)
//                        return (T)field.GetValue(null);
//                }
//                else
//                {
//                    if (field.Name == description)
//                        return (T)field.GetValue(null);
//                }
//            }
//            throw new ArgumentException(string.Format("{0} 未能找到对应的枚举.", description), "Description");
//        }


//        /// <summary>
//        /// 获取字段Description
//        /// </summary>
//        /// <param name="fieldInfo">FieldInfo</param>
//        /// <returns>DescriptionAttribute[] </returns>
//        public static DescriptionAttribute[] GetDescriptAttr(this FieldInfo fieldInfo)
//        {
//            if (fieldInfo != null)
//            {
//                return (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
//            }
//            return null;
//        }
//    }



//    public class StringValue : System.Attribute
//    {
//        private string _value;

//        public StringValue(string value)
//        {
//            _value = value;
//        }

//        public string Value
//        {
//            get { return _value; }
//        }

//    }
