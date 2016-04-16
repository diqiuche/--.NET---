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
    /// 会话传输节点类型枚举。
    /// </summary>
    /// <remarks>会话传输节点类型枚举。</remarks>
    [Serializable]
    public enum SessionTransferNodeTypes : byte
    {
        /// <summary>
        /// ASP_NET_FORM类型会话传送。
        /// </summary>
        /// <remarks>ASP_NET_FORM类型会话传送。</remarks>
        ASP_NET_FORM,
        /// <summary>
        /// WCF类型会话传送。
        /// </summary>
        /// <remarks>WCF类型会话传送。</remarks>
        WCF,
        /// <summary>
        /// APP类型会话传送。
        /// </summary>
        /// <remarks>APP类型会话传送。</remarks>
        APP,
        /// <summary>
        /// ASP_NET_API类型会话传送。
        /// </summary>
        /// <remarks>ASP_NET_API类型会话传送。</remarks>
        ASP_NET_API,
        /// <summary>
        /// ASP_NET_MVC类型会话传送。
        /// </summary>
        /// <remarks>ASP_NET_MVC类型会话传送。</remarks>
        ASP_NET_MVC,
        /// <summary>
        /// NET_WIN_FORM类型会话传送。
        /// </summary>
        /// <remarks>NET_WIN_FORM类型会话传送。</remarks>
        NET_WIN_FORM,
        /// <summary>
        /// PHP类型会话传送。
        /// </summary>
        /// <remarks>PHP类型会话传送。</remarks>
        PHP,
        /// <summary>
        /// JSP类型会话传送。
        /// </summary>
        /// <remarks>JSP类型会话传送。</remarks>
        JSP,
        /// <summary>
        /// Python类型会话传送。
        /// </summary>
        /// <remarks>Python类型会话传送。</remarks>
        Python,
        /// <summary>
        /// Node.Js类型会话传送。
        /// </summary>
        /// <remarks>Node.Js类型会话传送。</remarks>
        NodeJs,
    }
}