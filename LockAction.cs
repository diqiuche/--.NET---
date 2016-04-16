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
    /// 锁动作枚举。
    /// </summary>
    /// <remarks>锁动作枚举。</remarks>
    [Serializable]
    public enum LockAction
    {
        /// <summary>
        /// 解锁动作。
        /// </summary>
        /// <remarks>解锁动作。</remarks>
        UnLock,
        /// <summary>
        /// 锁定动作。
        /// </summary>
        /// <remarks>锁定动作。</remarks>
        Lock,
        /// <summary>
        /// 销毁动作。
        /// </summary>
        /// <remarks>销毁动作。</remarks>
        Destruction,
        /// <summary>
        /// 超时动作。
        /// </summary>
        /// <remarks>超时动作。</remarks>
        Timeout,
    }
}
