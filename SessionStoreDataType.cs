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
namespace SessionsStore
{
    #region 会话数据值类型枚举
    /// <summary>
    /// 会话存储数据类型枚举。
    /// </summary>
    /// <remarks>会话存储数据类型枚举。</remarks>
    [Serializable]
    public enum SessionStoreDataType : byte
    {
        /// <summary>
        /// 空值类型。
        /// </summary>
        /// <remarks>空值类型。</remarks>
        Null = 0,
        /// <summary>
        /// Bool值类型。
        /// </summary>
        /// <remarks>Bool值类型。</remarks>
        Bool = 1,
        /// <summary>
        /// Byte值类型。
        /// </summary>
        /// <remarks>Byte值类型。</remarks>
        Byte = 2,
        /// <summary>
        /// SByte值类型。
        /// </summary>
        /// <remarks>SByte值类型。</remarks>
        SByte = 3,
        /// <summary>
        /// Short值类型。
        /// </summary>
        /// <remarks>Short值类型。</remarks>
        Short = 4,
        /// <summary>
        /// UShort值类型。
        /// </summary>
        /// <remarks>UShort值类型。</remarks>
        UShort = 5,
        /// <summary>
        /// Int值类型。
        /// </summary>
        /// <remarks>Int值类型。</remarks>
        Int = 6,
        /// <summary>
        /// UInt值类型。
        /// </summary>
        /// <remarks>UInt值类型。</remarks>
        UInt = 7,
        /// <summary>
        /// Long值类型。
        /// </summary>
        /// <remarks>Long值类型。</remarks>
        Long = 8,
        /// <summary>
        /// ULong值类型。
        /// </summary>
        /// <remarks>ULong值类型。</remarks>
        ULong = 9,
        /// <summary>
        /// Float值类型。
        /// </summary>
        /// <remarks>Float值类型。</remarks>
        Float = 10,
        /// <summary>
        /// Decimal值类型。
        /// </summary>
        /// <remarks>Decimal值类型。</remarks>
        Decimal = 11,
        /// <summary>
        /// Double值类型。
        /// </summary>
        /// <remarks>Double值类型。</remarks>
        Double = 12,
        /// <summary>
        /// String值类型。
        /// </summary>
        /// <remarks>String值类型。</remarks>
        String = 13,
        /// <summary>
        /// Guid值类型。
        /// </summary>
        /// <remarks>Guid值类型。</remarks>
        Guid = 14,
        /// <summary>
        /// Buffer值类型。
        /// </summary>
        /// <remarks>Buffer值类型。</remarks>
        Buffer = 15,
        /// <summary>
        /// DateTime值类型。
        /// </summary>
        /// <remarks>DateTime值类型。</remarks>
        DateTime = 16,
        /// <summary>
        /// TimeSpan值类型。
        /// </summary>
        /// <remarks>TimeSpan值类型。</remarks>
        TimeSpan = 17,
        /// <summary>
        /// Object值类型。
        /// </summary>
        /// <remarks>Object值类型。</remarks>
        Object = 18,
        /// <summary>
        /// Icon值类型。
        /// </summary>
        /// <remarks>Icon值类型。</remarks>
        Icon = 19,
        /// <summary>
        /// Image值类型。
        /// </summary>
        /// <remarks>Image值类型。</remarks>
        Image = 20,
        /// <summary>
        /// Char值类型。
        /// </summary>
        /// <remarks>Char值类型。</remarks>
        Char = 21,
        /// <summary>
        /// Chars值类型。
        /// </summary>
        /// <remarks>Chars值类型。</remarks>
        Chars = 22
    }
    #endregion
}
