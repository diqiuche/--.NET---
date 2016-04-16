/*
 * 此软件名称：全局会话之ASP.NET会话节点模块
 * 此软件开源遵循GPL标准执行
 * 此软件版本：1.0.0.0
 * 开发者：郭树灿
 * QQ：27048384
 * QQ交流群：29044972
 * 手机：13715848993
 * 邮箱：27048384@qq.com
 * 博客：http://www.cnblogs.com/itcabb/
 * 地址：广东省潮州市潮安区庵埠镇刘陇村新沟生活片区
 * 版权所有：广东省潮州市潮安区庵埠镇刘陇村{郭树灿}
 */
using System;
using System.Collections.Generic;
namespace SessionsStore
{
    /// <summary>
    /// 枚举扩展。
    /// </summary>
    /// <remarks>枚举扩展。</remarks>
    internal static class EnumExpand
    {
        /// <summary>
        /// 扩展获取枚举类型名称字符列表。
        /// </summary>
        /// <remarks>扩展获取枚举类型名称字符列表。</remarks>
        /// <param name="@enum">设置要转换的枚举类型。</param>
        /// <returns>返回枚举数组名称列表。</returns>
        public static List<string> EnumToList(this Type @enum)
        {
            List<string> d = new List<string>();
            Array enumValues = Enum.GetValues(@enum);
            foreach (Enum value in enumValues)
            {
                d.Add(value.ToString());
            }
            return d;
        }
    }
}
