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
    /// 自定义会话Id。
    /// 配置文件需要删除旧的的会话Id管理类后添加自定义的会话Id管理类。
    /// <configuration>
    ///   <system.web>
    ///     <httpModules>
    ///       <remove name="SessionID" />
    ///       <add name="SessionID" type="SessionsStore.SessionIDManager" />
    ///     </httpModules>
    ///   </system.web>
    /// </configuration>
    /// </summary>
    /// <remarks>自定义会话Id。</remarks>
    public sealed class SessionIDManager : System.Web.SessionState.SessionIDManager
    {
        /// <summary>
        /// 创建字符会话Id。
        /// </summary>
        /// <remarks>创建字符会话Id。</remarks>
        /// <param name="context">设置Http内容对象。</param>
        /// <returns>string</returns>
        public override string CreateSessionID(System.Web.HttpContext context)
        {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 验证会话Id格式是否有效。
        /// </summary>
        /// <remarks>验证会话Id格式是否有效。</remarks>
        /// <param name="id">设置要验证的会话Id。</param>
        /// <returns>bool</returns>
        public override bool Validate(string id)
        {
            try
            {
                Guid testGuid = new Guid(id);
                if (id == testGuid.ToString())
                    return true;
            }
            catch { }
            return false;
        }
    }
}