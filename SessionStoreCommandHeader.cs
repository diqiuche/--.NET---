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
    /// 会话操作指令头枚举。
    /// </summary>
    /// <remarks>会话操作指令头枚举。</remarks>
    [Serializable]
    public enum SessionStoreCommandHeader : byte
    {
        /// <summary>
        /// 更新会话数据存储区中的项的到期日期和时间。
        /// </summary>
        /// <remarks>更新会话数据存储区中的项的到期日期和时间。</remarks>
        ResetItemTimeout,
        /// <summary>
        /// 将新的会话状态项添加到数据存储区中。
        /// </summary>
        /// <remarks>将新的会话状态项添加到数据存储区中。</remarks>
        CreateUninitializedItem,
        /// <summary>
        /// 释放对会话数据存储区中项的锁定。
        /// </summary>
        /// <remarks>释放对会话数据存储区中项的锁定。</remarks>
        ReleaseItemExclusive,
        /// <summary>
        /// 从会话数据存储区中返回只读会话状态数据。
        /// </summary>
        /// <remarks>从会话数据存储区中返回只读会话状态数据。</remarks>
        GetItemExclusive,
        /// <summary>
        /// 从会话数据存储区中返回只读会话状态数据。
        /// </summary>
        /// <remarks>从会话数据存储区中返回只读会话状态数据。</remarks>
        GetItem,
        /// <summary>
        /// 使用当前请求中的值更新会话状态数据存储区中的会话项信息，并清除对数据的锁定。
        /// </summary>
        /// <remarks>使用当前请求中的值更新会话状态数据存储区中的会话项信息，并清除对数据的锁定。</remarks>
        SetAndReleaseItemExclusive,
        /// <summary>
        /// 删除会话数据存储区中的项数据。
        /// </summary>
        /// <remarks>删除会话数据存储区中的项数据。</remarks>
        RemoveItem,
        /// <summary>
        /// 初始化。
        /// </summary>
        /// <remarks>初始化。</remarks>
        Initialize,
        /// <summary>
        /// 关闭。
        /// </summary>
        /// <remarks>关闭。</remarks>
        Close,
    }
}
