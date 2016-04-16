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
    /// <summary>
    /// 字符编码枚举。
    /// </summary>
    /// <remarks>字符编码枚举。</remarks>
    [Serializable]
    public enum CharacterEncoding : byte
    {
        /// <summary>
        /// UTF-8 格式的编码。
        /// </summary>
        /// <remarks>UTF-8 格式的编码。</remarks>
        UTF8Encoding,
        /// <summary>
        /// UTF-7 格式的编码。
        /// </summary>
        /// <remarks>UTF-7 格式的编码。</remarks>
        UTF7Encoding,
        /// <summary>
        /// Little-Endian 字节顺序的 UTF-32 格式的编码。
        /// </summary>
        /// <remarks>Little-Endian 字节顺序的 UTF-32 格式的编码。</remarks>
        UTF32Encoding,
        /// <summary>
        /// Little-Endian 字节顺序的 UTF-16 格式的编码。
        /// </summary>
        /// <remarks>Little-Endian 字节顺序的 UTF-16 格式的编码。</remarks>
        UnicodeEncoding,
        /// <summary>
        /// ASCII（7 位）字符集的编码。
        /// </summary>
        /// <remarks>ASCII（7 位）字符集的编码。</remarks>
        ASCIIEncoding,
        /// <summary>
        /// Big Endian 字节顺序的 UTF-16 格式的编码。
        /// </summary>
        /// <remarks>Big Endian 字节顺序的 UTF-16 格式的编码。</remarks>
        BigEndianUnicode,
    }
}
