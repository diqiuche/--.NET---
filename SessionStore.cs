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
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;
using System.Text;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
namespace SessionsStore
{
    /// <summary>
    /// 会话管理容器。
    /// </summary>
    /// <remarks>会话管理容器。</remarks>
    public class SessionStore : SessionStateStoreProviderBase
    {
        /// <summary>
        /// 获取或设置会话传输通道管理类。
        /// </summary>
        /// <remarks>获取或设置会话传输通道管理类。</remarks>
        /// <value>SessionsStore.SessionTransferChannelManager</value>
        internal SessionTransferChannelManager ChannelManager
        {
            private set;
            get;
        }
        /// <summary>
        /// 默认会话中心传输服务器端口。
        /// </summary>
        /// <remarks>默认会话中心传输服务器端口。</remarks>
        public const int DefaultSessionCenterServicePort = 5679;
        /// <summary>
        /// 默认会话中心传输服务器IP地址。
        /// </summary>
        /// <remarks>默认会话中心传输服务器IP地址。</remarks>
        public const string DefaultSessionCenterServiceIPAddresse = "127.0.0.1";
        /// <summary>
        /// 获取或设置会话字符编码。
        /// </summary>
        /// <remarks>获取或设置会话字符编码。</remarks>
        /// <value>System.Text.Encoding</value>
        internal Encoding Encoding
        {
            get;
            set;
        }
        /// <summary>
        /// 获取会话中心传输服务器IP地址。
        /// </summary>
        /// <remarks>获取会话中心传输服务器IP地址。</remarks>
        internal string SessionCenterServiceIPAddresse
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取会话中心传输服务器端口。
        /// </summary>
        /// <remarks>获取会话中心传输服务器端口。</remarks>
        /// <value>int</value>
        internal int SessionCenterServicePort
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取或设置最大会话传输通道数量，0表示无限制。
        /// </summary>
        /// <remarks>获取或设置最大会话传输通道数量，0表示无限制。</remarks>
        /// <value>int</value>
        internal int MaxSessionTransferChannel
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置安全连接会话中心验证内容。
        /// </summary>
        /// <remarks>获取或设置安全连接会话中心验证内容。</remarks>
        /// <value>string</value>
        internal string SessionCenterSecureConnectionContent
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置会话传输通道空闲超时时间。
        /// </summary>
        /// <remarks>获取或设置会话传输通道空闲超时时间。</remarks>
        /// <value>int</value>
        internal int SessionTransferChannelIdleTimeout
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置会话中心当前时间。
        /// </summary>
        /// <remarks>获取或设置会话中心当前时间。</remarks>
        /// <value>System.DateTime</value>
        internal DateTime SessionCenterCurrentDateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 获取或设置SSL证书密码。
        /// </summary>
        /// <remarks>获取或设置SSL证书密码。</remarks>
        /// <value>string</value>
        internal string SessionCenterServiceSSLCertificatePassword
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取或设置SSL证书名称。
        /// </summary>
        /// <remarks>获取或设置SSL证书名称。</remarks>
        /// <value>string</value>
        internal string SessionCenterServiceSSLCertificate
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取或设置SSL证书路径。
        /// </summary>
        /// <remarks>获取或设置SSL证书路径。</remarks>
        /// <value>string</value>
        internal string SessionCenterServiceSSLCertificatePath
        {
            get;
            private set;
        }
        /// <summary>
        /// SSL证书集合。
        /// </summary>
        /// <remarks>SSL证书集合。</remarks>
        internal X509CertificateCollection certificates;
        /// <summary>
        /// SSL证书。
        /// </summary>
        /// <remarks>SSL证书。</remarks>
        internal X509Certificate certificate;
        /// <summary>
        /// 创建要用于当前请求的新 SessionStateStoreData 对象。
        /// </summary>
        /// <remarks>创建要用于当前请求的新 SessionStateStoreData 对象。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="timeout">新 SessionStateStoreData 的会话状态 Timeout 值。</param>
        /// <returns>当前请求的新 SessionStateStoreData。</returns>
        public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
        {
            Debug.WriteLine("CreateNewStoreData[timeout:" + timeout.ToString() + "]", "SessionStore");
            return new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), timeout);
        }
        /// <summary>
        /// 将新的会话状态项添加到数据存储区中。
        /// </summary>
        /// <remarks>将新的会话状态项添加到数据存储区中。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的 SessionID。</param>
        /// <param name="timeout">当前请求的会话 Timeout。</param>
        public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
        {
            Debug.WriteLine("CreateUninitializedItem[会话Id:" + 
                (id == null ? "null" : id) + 
                ",超时:" + timeout.ToString() + 
                ",创建时间:" + DateTime.Now.ToString() +
                ",过期:" + DateTime.Now.AddMinutes((Double)timeout).ToString() +
                ",锁定时间:" + DateTime.Now.ToString() +
                ",锁定Id:0" +
                ",锁定:false" +
                ",标志:1" +
                "]", "SessionStore");
            this.ChannelManager.Write(new object[] {
                //指令
                (byte)SessionStoreCommandHeader.CreateUninitializedItem, 
                //会话Id
                id, 
                //创建时间
                DateTime.Now,
                //过期
                DateTime.Now.AddMinutes((Double)timeout), 
                //锁定时间
                DateTime.Now,
                //锁定Id
                (int)0,
                //超时
                timeout,
                //锁定
                false,
                //不需要通过调用代码来执行任何初始化操作。 
                (int)1
                });
        }
        /// <summary>
        /// 释放由 SessionStateStoreProviderBase 实现使用的所有资源。
        /// </summary>
        /// <remarks>释放由 SessionStateStoreProviderBase 实现使用的所有资源。</remarks>
        public override void Dispose()
        {
            Debug.WriteLine("Dispose");
            if (this.ChannelManager != null)
            {
                this.ChannelManager.Dispose();
            }
        }
        /// <summary>
        /// 在请求结束时由 SessionStateModule 对象调用。
        /// </summary>
        /// <remarks>在请求结束时由 SessionStateModule 对象调用。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        public override void EndRequest(HttpContext context)
        {
            Debug.WriteLine("EndRequest");
        }
        /// <summary>
        /// 从会话数据存储区中返回只读会话状态数据。
        /// </summary>
        /// <remarks>从会话数据存储区中返回只读会话状态数据。</remarks>
        /// <param name="lockRecord">锁记录。</param>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的 SessionID。</param>
        /// <param name="locked">当此方法返回时，如果请求的会话项在会话数据存储区被锁定，请包含一个设置为 true 的布尔值；否则请包含一个设置为 false 的布尔值。</param>
        /// <param name="lockAge">当此方法返回时，请包含一个设置为会话数据存储区中的项锁定时间的 TimeSpan 对象。</param>
        /// <param name="lockId">当此方法返回时，请包含一个设置为当前请求的锁定标识符的对象。有关锁定标识符的详细信息，请参见 SessionStateStoreProviderBase 类摘要中的“锁定会话存储区数据”。</param>
        /// <param name="actions">当此方法返回时，请包含 SessionStateActions 值之一，指示当前会话是否为未初始化的无 Cookie 会话。</param>
        /// <returns>使用会话数据存储区中的会话值和信息填充的 SessionStateStoreData。</returns>
        private SessionStateStoreData GetSessionStoreItem(bool lockRecord, HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            Debug.WriteLine("GetSessionStoreItem[锁记录:" + lockRecord.ToString() + ",会话id:" + (id == null ? "null" : id) + "]", "SessionStore");
            //声明
            //会话容器
            SessionStateStoreData item = null;
            //锁定时间
            lockAge = TimeSpan.Zero;
            //锁定Id
            lockId = null;
            //锁定
            locked = false;
            //动作
            actions = 0;
            //过期
            DateTime expires;
            //找到记录
            bool foundRecord = false;
            //返回过期删除
            bool deleteData = false;
            //超时时间
            int timeout = 0;
            //创建会话数据集合。
            SessionStateItemCollection ic = new SessionStateItemCollection();
            //声明会话传输通道。
            SessionTransferChannel channel = null;
            try
            {
                //获取会话传输通道对象，返回必须是被锁定的会话传输
                channel = this.ChannelManager.GetChannel();
                if (!channel.IsLock)
#if DEBUG
                    throw new Exception("获取会话传输通道没有被锁定错误！[Key:" + channel.Key.ToString() + "]");
#else
                    throw new Exception("获取会话传输通道没有被锁定错误！");  
#endif
                //
                channel.Write(new object[] { 
                    //指令
                    (byte)SessionStoreCommandHeader.GetItemExclusive,
                    //会话Id
                    id, 
                    //锁记录
                    lockRecord });
                //锁记录。
                if (lockRecord)
                {
                    channel.Write(new object[] { 
                        //锁定
                        true, 
                        //锁定时间
                        DateTime.Now,
                        //过期时间
                        DateTime.Now, });
                    //读取锁状态。
                    if (!channel.Reader.ReadBoolean())
                        //没有记录被更新，因为记录被锁定或没有找到。
                        locked = true;
                    else
                        //记录已更新。
                        locked = false;
                }
                //
                if (channel.Reader.ReadBoolean())
                {
                    //过期
                    expires = channel.ReadDateTime();
                    //验证是否过期
                    if (expires < DateTime.Now)
                    {
                        //该纪录是过期。将其标记为未锁定。
                        locked = false;
                        //会话已过期。标记为删除的数据。
                        deleteData = true;
                    }
                    else
                        //未过期
                        foundRecord = true;
                    //锁定Id
                    lockId = channel.Reader.ReadInt32();
                    //锁定经过时间
                    lockAge = channel.ReadTimeSpan();
                    //动作
                    actions = (SessionStateActions)channel.Reader.ReadInt32();
                    //超时
                    timeout = channel.Reader.ReadInt32();
                    //读取集合数据。
                    ReadItems(channel, ic);
                }
                //创建写入流打包对象。
                WriteStreamPackaging pack = channel.CreateWriteStreamPackaging(false);
                //是否删除
                pack.Write(deleteData);
                //记录没有被发现。确保锁定的是假的。
                if (!foundRecord)
                    //没有锁定
                    locked = false;
                //如果记录被发现，你获得了锁，然后设置
                //锁定，清除actionFlags，
                //并创建SessionStateStoreItem返回。
                if (foundRecord && !locked)
                {
                    //累加锁定Id
                    lockId = (int)lockId + 1;
                    //锁定
                    pack.Write(true);
                    //锁定Id
                    pack.Write((int)lockId);
                    //如果动作Flags参数未初始化项目。
                    //反序列化存储SessionStateItemCollection。
                    if (actions == SessionStateActions.InitializeItem)
                    {
                        item = new SessionStateStoreData(new SessionStateItemCollection(), SessionStateUtility.GetSessionStaticObjects(context), this.SessionStateConfig.Timeout.Minutes);
                    }
                    else
                    {
                        item = new SessionStateStoreData(ic, SessionStateUtility.GetSessionStaticObjects(context), this.SessionStateConfig.Timeout.Minutes);
                    }
                }
                else
                {
                    //没有锁定
                    pack.Write(false);
                }
                pack.Flush(channel.Stream);
            }
            finally
            {
                channel.UnLock();
            }
            return item;
        }
        /// <summary>
        /// 从会话数据存储区中返回只读会话状态数据。
        /// </summary>
        /// <remarks>从会话数据存储区中返回只读会话状态数据。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的 SessionID。</param>
        /// <param name="locked">当此方法返回时，如果请求的会话项在会话数据存储区被锁定，请包含一个设置为 true 的布尔值；否则请包含一个设置为 false 的布尔值。</param>
        /// <param name="lockAge">当此方法返回时，请包含一个设置为会话数据存储区中的项锁定时间的 TimeSpan 对象。</param>
        /// <param name="lockId">当此方法返回时，请包含一个设置为当前请求的锁定标识符的对象。有关锁定标识符的详细信息，请参见 SessionStateStoreProviderBase 类摘要中的“锁定会话存储区数据”。</param>
        /// <param name="actions">当此方法返回时，请包含 SessionStateActions 值之一，指示当前会话是否为未初始化的无 Cookie 会话。</param>
        /// <returns>使用会话数据存储区中的会话值和信息填充的 SessionStateStoreData。</returns>
        public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return this.GetSessionStoreItem(false, context, id, out locked, out lockAge, out lockId, out actions);
        }
        /// <summary>
        /// 从会话数据存储区中返回只读会话状态数据。
        /// </summary>
        /// <remarks>从会话数据存储区中返回只读会话状态数据。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的 SessionID。</param>
        /// <param name="locked">当此方法返回时，如果请求的会话项在会话数据存储区被锁定，请包含一个设置为 true 的布尔值；否则请包含一个设置为 false 的布尔值。</param>
        /// <param name="lockAge">当此方法返回时，请包含一个设置为会话数据存储区中的项锁定时间的 TimeSpan 对象。</param>
        /// <param name="lockId">当此方法返回时，请包含一个设置为当前请求的锁定标识符的对象。有关锁定标识符的详细信息，请参见 SessionStateStoreProviderBase 类摘要中的“锁定会话存储区数据”。</param>
        /// <param name="actions">当此方法返回时，请包含 SessionStateActions 值之一，指示当前会话是否为未初始化的无 Cookie 会话。</param>
        /// <returns>使用会话数据存储区中的会话值和信息填充的 SessionStateStoreData。</returns>
        public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actions)
        {
            return this.GetSessionStoreItem(true, context, id, out locked, out lockAge, out lockId, out actions);
        }
        /// <summary>
        /// 获取或设置会话状态配置。
        /// </summary>
        /// <remarks>获取或设置会话状态配置。</remarks>
        /// <value>System.Web.Configuration.SessionStateSection</value>
        internal SessionStateSection SessionStateConfig
        {
            get;
            private set;
        }
        /// <summary>
        /// 初始化提供程序。 （从 ProviderBase 继承。）
        /// </summary>
        /// <remarks>初始化提供程序。 （从 ProviderBase 继承。）</remarks>
        /// <param name="name">该提供程序的友好名称。</param>
        /// <param name="config">名称/值对的集合，表示在配置中为该提供程序指定的、提供程序特定的属性。</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            Debug.WriteLine("初始化", "SessionStore.Initialize");
            //验证配置对象无效。
            if (config == null)
                throw new ArgumentNullException("config");
            //重写基类初始化。
            base.Initialize(name, config);
            //获取虚拟路径。
            string appName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            //打开配置文件。
            Configuration cfg = WebConfigurationManager.OpenWebConfiguration(appName);
            //获取会话状态配置对象。
            this.SessionStateConfig = (SessionStateSection)cfg.GetSection("system.web/sessionState");
            //获取配置节点集合。
            NameValueCollection nvc = (NameValueCollection)ConfigurationManager.AppSettings;
            //获取配置会话中心IP地址。
            this.SessionCenterServiceIPAddresse = nvc.Get("SessionCenterServiceIPAddresse");
            if (string.IsNullOrEmpty(this.SessionCenterServiceIPAddresse)) this.SessionCenterServiceIPAddresse = DefaultSessionCenterServiceIPAddresse;
            //获取配置会话中心端口。
            this.SessionCenterServicePort = Convert.ToInt32(nvc.Get("SessionCenterServicePort"));
            if (this.SessionCenterServicePort <= 0) this.SessionCenterServicePort = DefaultSessionCenterServicePort;
            //获取配置SSL证书路径。
            this.SessionCenterServiceSSLCertificatePath = nvc.Get("SessionCenterServiceSSLCertificatePath");
            //获取配置SSL证书名称。
            this.SessionCenterServiceSSLCertificate = nvc.Get("SessionCenterServiceSSLCertificate");
            //获取配置文件字符编码。
            /*
            string ed = nvc.Get("SessionCenterServiceEncoding");
            if (string.IsNullOrEmpty(ed))
            {
                this.Encoding = System.Text.Encoding.UTF8;
            }
            else
            {
                this.Encoding = System.Text.Encoding.GetEncoding(ed);
            }
            */ 
            //获取配置文件安全连接会话中心验证内容。
            this.SessionCenterSecureConnectionContent = nvc.Get("SessionCenterSecureConnectionContent");
            //获取配置文件会话传输通道空闲超时时间。
            string ChannelIdleTimeout = nvc.Get("SessionTransferChannelIdleTimeout");
            if (!string.IsNullOrEmpty(ChannelIdleTimeout) && Convert.ToInt32(ChannelIdleTimeout) > 0)
            {
                this.SessionTransferChannelIdleTimeout = Convert.ToInt32(ChannelIdleTimeout);
            }
            //初始化SSL证书列表。
            if (this.certificates == null && this.certificate == null)
            {
                //验证使用文件或设置使用证书名称加载证书。
                if (!string.IsNullOrEmpty(this.SessionCenterServiceSSLCertificatePath) && string.IsNullOrEmpty(this.SessionCenterServiceSSLCertificate))
                {
                    //加载SSL文件。
                    //验证是否使用证书密码。
                    if (string.IsNullOrEmpty(this.SessionCenterServiceSSLCertificatePassword))
                        //使用没有密码的证书文件。
                        this.certificate = new X509Certificate2(this.SessionCenterServiceSSLCertificatePath);
                    else
                        //使用有密码的证书文件。
                        this.certificate = new X509Certificate2(this.SessionCenterServiceSSLCertificatePath, this.SessionCenterServiceSSLCertificatePassword);
                    Debug.WriteLine("连接时创建SSL证书信息完成将证书添加进证书容器", "SessionStore.Initialize");

                    this.certificates = new X509CertificateCollection();
                    this.certificates.Add(this.certificate);
                }
                else if (string.IsNullOrEmpty(this.SessionCenterServiceSSLCertificatePath) && !string.IsNullOrEmpty(this.SessionCenterServiceSSLCertificate))
                {
                    //加载SSL文件。
                    //验证是否使用证书密码。
                    if (string.IsNullOrEmpty(this.SessionCenterServiceSSLCertificatePassword))
                    {
                        X509Store store = null;
                        X509Certificate2Collection certs = null;
                        //检索证书。
                        foreach (string item in EnumExpand.EnumToList(typeof(StoreName)))
                        {
                            try
                            {
                                //创建证书列表。
                                store = new X509Store(((StoreName)Enum.Parse(typeof(StoreName), item)));
                                store.Open(OpenFlags.ReadOnly);
                                //检索证书。
                                certs = store.Certificates.Find(X509FindType.FindBySubjectName, this.SessionCenterServiceSSLCertificate, false);
                                //验证检索到的证书。
                                if (certs.Count > 0)
                                {
                                    //验证是否使用证书密码。
                                    if (string.IsNullOrEmpty(this.SessionCenterServiceSSLCertificatePassword))
                                    {
                                        //使用没有密码的证书。
                                        this.certificate = new X509Certificate(certs[0].RawData);
                                    }
                                    else
                                    {
                                        //使用有密码的证书。
                                        this.certificate = new X509Certificate(certs[0].RawData, this.SessionCenterServiceSSLCertificatePassword);
                                    }
                                    this.certificates = new X509CertificateCollection();
                                    this.certificates.Add(this.certificate);
                                    Debug.WriteLine("连接时创建SSL证书信息完成将证书添加进证书容器", "SessionStore.Initialize");
                                    //找到指定名称的证书退出。
                                    break;
                                }
                            }
                            finally
                            {
                                //释放证书资源。
                                if (store != null)
                                {
                                    store.Close();
                                    store = null;
                                }
                            }
                        }
                    }
                }
            }
            //
            Debug.WriteLine("初始化创建一条连接通道[SessionCenterServiceIPAddresse:" + this.SessionCenterServiceIPAddresse + 
                ",SessionCenterServicePort:" + this.SessionCenterServicePort.ToString() +
                ",SessionTransferChannelIdleTimeout:" + this.SessionTransferChannelIdleTimeout.ToString() +
                ",SessionCenterServiceSSLCertificatePath:" + (this.SessionCenterServiceSSLCertificatePath == null ? "" : this.SessionCenterServiceSSLCertificatePath) +
                ",SessionCenterServiceSSLCertificate:" + (this.SessionCenterServiceSSLCertificate == null ? "" : this.SessionCenterServiceSSLCertificate) +
                "]", "SessionStore.Initialize");
            //初始化通道管理对象。
            this.ChannelManager = new SessionTransferChannelManager(this);
            //初始化通道连接。
            this.ChannelManager.Connection();
            //验证初始化通道连接到会话中心是否正常。
            if (!this.ChannelManager.IsConnected)
            {
                throw new Exception("没有连接到全局会话中心初始化错误！");
            }
            Debug.WriteLine("初始化完成[SessionCenterServiceIPAddresse:" + this.SessionCenterServiceIPAddresse + ",SessionCenterServicePort:" + this.SessionCenterServicePort.ToString() + ",SessionCenterCurrentDateTime:" + this.SessionCenterCurrentDateTime.ToString() + "]", "SessionStore.Initialize");
        }
        /// <summary>
        /// 由 SessionStateModule 对象调用，以便进行每次请求初始化。
        /// </summary>
        /// <remarks>由 SessionStateModule 对象调用，以便进行每次请求初始化。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        public override void InitializeRequest(HttpContext context)
        {
            Debug.WriteLine("InitializeRequest");
        }
        /// <summary>
        /// 释放对会话数据存储区中项的锁定。
        /// </summary>
        /// <remarks>释放对会话数据存储区中项的锁定。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        /// <param name="lockId">当前请求的锁定标识符。</param>
        public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
        {
            Debug.WriteLine("ReleaseItemExclusive[会话Id:" + (id == null ? "null" : id) + ",过期:" + DateTime.Now.AddMinutes(this.SessionStateConfig.Timeout.TotalMinutes).ToString() + ",锁定Id:" + lockId.ToString() + "]", "SessionStore");
            this.ChannelManager.Write(new object[] { 
                //指令
                (byte)SessionStoreCommandHeader.ReleaseItemExclusive, 
                //会话Id
                id, 
                //过期
                DateTime.Now.AddMinutes(this.SessionStateConfig.Timeout.TotalMinutes),
                //锁定Id
                (int)lockId });
        }
        /// <summary>
        /// 删除会话数据存储区中的项数据。
        /// </summary>
        /// <remarks>删除会话数据存储区中的项数据。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        /// <param name="lockId">当前请求的锁定标识符。</param>
        /// <param name="item">表示将从数据存储区中删除的项的 SessionStateStoreData。</param>
        public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
        {
            Debug.WriteLine("RemoveItem[会话Id:" + (id == null ? "null" : id) + ",锁定Id:" + lockId.ToString() + "]", "SessionStore");
            this.ChannelManager.Write(new object[] {
                //指令
                (byte)SessionStoreCommandHeader.RemoveItem,
                //会话Id
                id,
                //锁定Id
                (int)lockId });
        }
        /// <summary>
        /// 更新会话数据存储区中的项的到期日期和时间。
        /// </summary>
        /// <remarks>更新会话数据存储区中的项的到期日期和时间。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        public override void ResetItemTimeout(HttpContext context, string id)
        {
            Debug.WriteLine("ResetItemTimeout[会话Id:" + (id == null ? "null" : id) + ",过期:" + DateTime.Now.AddMinutes(this.SessionStateConfig.Timeout.TotalMinutes).ToString() + "]", "SessionStore");
            this.ChannelManager.Write(new object[] { 
                //指令
                (byte)SessionStoreCommandHeader.ResetItemTimeout,
                //会话Id
                id, 
                //过期
                DateTime.Now.AddMinutes(this.SessionStateConfig.Timeout.TotalMinutes) });
        }
        /// <summary>
        /// 读取会话键集合内容。
        /// </summary>
        /// <remarks>读取会话键集合内容。</remarks>
        /// <param name="channel">设置读取通道对象。</param>
        /// <param name="itemCollection">设置会话容器键集合对象。</param>
        private static void ReadItems(SessionTransferChannel channel, SessionStateItemCollection itemCollection)
        {
            //声明获取会话存放键集合数量。
#if DEBUG
            int itemCount = channel.Reader.ReadInt32();
            Debug.WriteLine("ReadItems[itemCount:" + itemCount.ToString() + "]", "SessionStore");
            //验证要读取的会话容器键集合数量。
            if (itemCount > 0)
            {
                //遍历循环读取所有键与对象。
                for (int i = 0; i < itemCount; i++)
                {
                    //获取存储数据类型。
                    SessionStoreDataType dataType = (SessionStoreDataType)channel.Reader.ReadByte();
                    switch (dataType)
                    {
#else
                for (int i = 0; i < channel.Reader.ReadInt32(); i++)
                {
                    switch ((SessionStoreDataType)channel.Reader.ReadByte())
                    {
#endif
                        case SessionStoreDataType.Null:
                            //读取空值对象。
                            itemCollection[channel.ReadString()] = null;
                            break;
                        case SessionStoreDataType.Bool:
                            //读取Bool值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadBoolean();
                            break;
                        case SessionStoreDataType.Byte:
                            //读取Byte值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadByte();
                            break;
                        case SessionStoreDataType.SByte:
                            //读取SByte值对象。
                            itemCollection[channel.ReadString()] = unchecked((sbyte)channel.Reader.ReadByte());
                            break;
                        case SessionStoreDataType.Short:
                            //读取Short值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadInt16();
                            break;
                        case SessionStoreDataType.UShort:
                            //读取UShort值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadUInt16();
                            break;
                        case SessionStoreDataType.Int:
                            //读取Int值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadInt32();
                            break;
                        case SessionStoreDataType.UInt:
                            //读取UInt值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadUInt32();
                            break;
                        case SessionStoreDataType.Long:
                            //读取Long值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadInt64();
                            break;
                        case SessionStoreDataType.ULong:
                            //读取ULong值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadUInt64();
                            break;
                        case SessionStoreDataType.Float:
                            //读取Float值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadSingle();
                            break;
                        case SessionStoreDataType.Decimal:
                            //读取Decimal值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadDecimal();
                            break;
                        case SessionStoreDataType.Double:
                            //读取Double值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadDouble();
                            break;
                        case SessionStoreDataType.String:
                            //读取String值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadString();
                            break;
                        case SessionStoreDataType.Guid:
                            //读取Guid值对象。
                            itemCollection[channel.ReadString()] = channel.ReadGuid();
                            break;
                        case SessionStoreDataType.Buffer:
                            //读取Buffer值对象。
                            itemCollection[channel.ReadString()] = channel.ReadBytes();
                            break;
                        case SessionStoreDataType.DateTime:
                            //读取DateTime值对象。
                            itemCollection[channel.ReadString()] = channel.ReadDateTime();
                            break;
                        case SessionStoreDataType.TimeSpan:
                            //读取TimeSpan值对象。
                            itemCollection[channel.ReadString()] = channel.ReadTimeSpan();
                            break;
                        case SessionStoreDataType.Object:
                            //读取Object值对象。
                            itemCollection[channel.ReadString()] = channel.ReadObject();
                            break;
                        case SessionStoreDataType.Icon:
                            //读取Icon值对象。
                            using (MemoryStream outms = new MemoryStream(channel.ReadBytes()))
                            {
                                itemCollection[channel.ReadString()] = new Icon(outms);
                            }
                            break;
                        case SessionStoreDataType.Image:
                            //读取Image值对象。
                            using (MemoryStream outms = new MemoryStream(channel.ReadBytes()))
                            {
                                itemCollection[channel.ReadString()] = new Bitmap(outms);
                            }
                            break;
                        case SessionStoreDataType.Char:
                            //读取Char值对象。
                            itemCollection[channel.ReadString()] = channel.Reader.ReadChar();
                            break;
                        case SessionStoreDataType.Chars:
                            //读取Chars值对象。
                            itemCollection[channel.ReadString()] = channel.ReadChars();
                            break;
                    }
                }
#if DEBUG
            }
#endif
        }
        /// <summary>
        /// 写入会话键集合内容。
        /// </summary>
        /// <remarks>写入会话键集合内容。</remarks>
        /// <param name="storeData">设置会话键集合容器对象。</param>
        /// <param name="pack">设置写入流打包对象。</param>
        static void WriteItems(SessionStateStoreData storeData, WriteStreamPackaging pack)
        {
#if DEBUG
            if (storeData != null && storeData.Items != null)
            {
                Debug.WriteLine("WriteItems[itemCount:" + storeData.Items.Count.ToString() + "]", "SessionStore");
            }
#endif
            //遍历会话容器键集合。
            foreach (string key in storeData.Items.Keys)
            {
                //获取存放键对象。
                object item = storeData.Items[key];
                if (item == null)
                {
                    //写入空值对象。
                    pack.Write((byte)SessionStoreDataType.Null);
                    pack.Write(key);
                }
                else if (item is bool)
                {
                    //写入bool值对象。
                    pack.Write((byte)SessionStoreDataType.Bool);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is byte)
                {
                    //写入byte值对象。
                    pack.Write((byte)SessionStoreDataType.Byte);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is sbyte)
                {
                    //写入sbyte值对象。
                    pack.Write((byte)SessionStoreDataType.SByte);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is short)
                {
                    //写入short值对象。
                    pack.Write((byte)SessionStoreDataType.Short);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is ushort)
                {
                    //写入ushort值对象。
                    pack.Write((byte)SessionStoreDataType.UShort);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is int)
                {
                    //写入int值对象。
                    pack.Write((byte)SessionStoreDataType.Int);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is uint)
                {
                    //写入uint值对象。
                    pack.Write((byte)SessionStoreDataType.UInt);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is long)
                {
                    //写入long值对象。
                    pack.Write((byte)SessionStoreDataType.Long);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is ulong)
                {
                    //写入ulong值对象。
                    pack.Write((byte)SessionStoreDataType.ULong);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is float)
                {
                    //写入float值对象。
                    pack.Write((byte)SessionStoreDataType.Float);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is decimal)
                {
                    //写入decimal值对象。
                    pack.Write((byte)SessionStoreDataType.Decimal);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is double)
                {
                    //写入double值对象。
                    pack.Write((byte)SessionStoreDataType.Double);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is Guid)
                {
                    //写入Guid值对象。
                    pack.Write((byte)SessionStoreDataType.Guid);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is byte[])
                {
                    //写入byte[]值对象。
                    pack.Write((byte)SessionStoreDataType.Buffer);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is string)
                {
                    //写入string值对象。
                    pack.Write((byte)SessionStoreDataType.String);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is DateTime)
                {
                    //写入DateTime值对象。
                    pack.Write((byte)SessionStoreDataType.DateTime);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is TimeSpan)
                {
                    //写入TimeSpan值对象。
                    pack.Write((byte)SessionStoreDataType.TimeSpan);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is Icon)
                {
                    //写入Icon值对象。
                    pack.Write((byte)SessionStoreDataType.Icon);
                    pack.Write(key);
                    using (MemoryStream outms = new MemoryStream())
                    {
                        ((Icon)item).Save(outms);
                        pack.Write(outms.ToArray());
                    }
                }
                else if (item is Image)
                {
                    //写入Image值对象。
                    pack.Write((byte)SessionStoreDataType.Image);
                    pack.Write(key);
                    using (MemoryStream outms = new MemoryStream())
                    {
                        ((Image)item).Save(outms, ((Image)item).RawFormat);
                        pack.Write(outms.ToArray());
                    }
                }
                else if (item is char)
                {
                    //写入char值对象。
                    pack.Write((byte)SessionStoreDataType.Char);
                    pack.Write(key);
                    pack.Write(item);
                }
                else if (item is char[])
                {
                    //写入char[]值对象。
                    pack.Write((byte)SessionStoreDataType.Chars);
                    pack.Write(key);
                    pack.Write(item);
                }
                else
                {
                    //写入Object值对象。
                    pack.Write((byte)SessionStoreDataType.Object);
                    pack.Write(key);
                    pack.Write(item);
                }
            }
        }
        /// <summary>
        /// 使用当前请求中的值更新会话状态数据存储区中的会话项信息，并清除对数据的锁定。
        /// </summary>
        /// <remarks>使用当前请求中的值更新会话状态数据存储区中的会话项信息，并清除对数据的锁定。</remarks>
        /// <param name="context">当前请求的 HttpContext。</param>
        /// <param name="id">当前请求的会话标识符。</param>
        /// <param name="item">包含要存储的当前会话值的 SessionStateStoreData 对象。</param>
        /// <param name="lockId">当前请求的锁定标识符。</param>
        /// <param name="newItem">如果为 true，则将会话项标识为新项；如果为 false，则将会话项标识为现有的项。</param>
        public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
        {
            Debug.WriteLine("SetAndReleaseItemExclusive[会话Id:" + (id == null ? "null" : id) + ",锁定Id:" + lockId.ToString() + ",新建会话对象:" + newItem.ToString() + "]", "SessionStore");
            //创建写入流打包对象。
            WriteStreamPackaging pack = this.ChannelManager.CreateWriteStreamPackaging();
            //指令。
            pack.Write((byte)SessionStoreCommandHeader.SetAndReleaseItemExclusive);
            //会话Id。
            pack.Write(id);
            //是否新建。
            pack.Write(newItem);
            //验证是否新建。
            if (newItem)
            {
                //过期时间
                pack.Write(DateTime.Now);
                //创建时间
                pack.Write(DateTime.Now);
                //过期
                pack.Write(DateTime.Now.AddMinutes((Double)item.Timeout));
                //锁定时间
                pack.Write(DateTime.Now);
                //锁定Id
                pack.Write((int)0);
                //超时
                pack.Write(item.Timeout);
                //锁定
                pack.Write(false);
                //数据存储区中的会话项用于需要初始化的会话。
                pack.Write((int)0);
            }
            else
            {
                //过期
                pack.Write(DateTime.Now.AddMinutes((Double)item.Timeout));
                //锁定
                pack.Write(false);
                //锁定Id
                pack.Write((int)lockId);
            }
            //写入会话键集合。
            WriteItems(item, pack);
            //发送。
            this.ChannelManager.Write(pack);
        }
        /// <summary>
        /// 设置对 Global.asax 文件中定义的 Session_OnEnd 事件的 SessionStateItemExpireCallback 委托的引用。
        /// </summary>
        /// <remarks>设置对 Global.asax 文件中定义的 Session_OnEnd 事件的 SessionStateItemExpireCallback 委托的引用。</remarks>
        /// <param name="expireCallback">对 Global.asax 文件中定义的 Session_OnEnd 事件的 SessionStateItemExpireCallback 委托。</param>
        /// <returns>如果会话状态存储提供程序支持调用 Session_OnEnd 事件，则为 true；否则为 false。</returns>
        public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
        {
            Debug.WriteLine("SetItemExpireCallback");
            return false;
        }
    }
}