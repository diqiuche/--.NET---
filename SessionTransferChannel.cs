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
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
namespace SessionsStore
{
    /// <summary>
    /// 会话传输通道。
    /// </summary>
    /// <remarks>会话传输通道。</remarks>
    [ToolboxItem(false)]
    public partial class SessionTransferChannel : Component
    {
        #region 初始化
        /// <summary>
        /// 会话传输通道初始化。
        /// </summary>
        /// <remarks>会话传输通道初始化。</remarks>
        /// <param name="owner">设置拥有者。</param>
        internal SessionTransferChannel(SessionTransferChannelManager owner)
        {
            this.Key = Guid.NewGuid();
            this.Owner = owner;
            InitializeComponent();
            Debug.WriteLine("初始化会话传输通道[Key:" + this.Key.ToString() + "]", "SessionTransferChannel");
        }
        #endregion
        #region 声明属性
        /// <summary>
        /// 获取或设置流对象。
        /// </summary>
        /// <remarks>获取或设置流对象。</remarks>
        /// <value>System.Net.Sockets.NetworkStream</value>
        internal NetworkStream NetworkStream
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取或设置SSL流对象。
        /// </summary>
        /// <remarks>获取或设置SSL流对象。</remarks>
        /// <value>System.Net.Security.SslStream</value>
        internal SslStream SslStream
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取通道是否已经连接。
        /// </summary>
        /// <remarks>获取通道是否已经连接。</remarks>
        /// <value>bool</value>
        [Browsable(false)]
        internal bool IsConnected
        {
            get
            {
                lock (this.SyncRoot)
                {
                    if (this.SslStream == null)
                    {
                        return this.Client != null &&
                            this.Client.Connected &&
                            this.NetworkStream != null &&
                            this.NetworkStream.CanWrite;
                    }
                    else
                    {
                        return this.Client != null &&
                          this.Client.Connected &&
                          this.SslStream != null &&
                          this.SslStream.CanWrite;
                    }
                }
            }
        }
        /// <summary>
        /// 声明同步锁对象。
        /// </summary>
        /// <remarks>声明同步锁对象。</remarks>
        internal readonly object SyncRoot = new object();
        /// <summary>
        /// 获取或设置会话传输通道键值。
        /// </summary>
        /// <remarks>获取或设置会话传输通道键值。</remarks>
        /// <value>System.Guid</value>
        internal Guid Key
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取拥有者。
        /// </summary>
        /// <remarks>获取拥有者。</remarks>
        /// <value>SessionsStore.SessionTransferChannelManager</value>
        [Browsable(false)]
        internal SessionTransferChannelManager Owner
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取TCP客户端对象。
        /// </summary>
        /// <remarks>获取TCP客户端对象。</remarks>
        /// <value>System.Net.Sockets.TcpClient</value>
        [Browsable(false)]
        internal TcpClient Client
        {
            get;
            private set;
        }
        #endregion
        #region 锁定
        /// <summary>
        /// 获取或设置锁定时间。
        /// </summary>
        /// <remarks>获取或设置锁定时间。</remarks>
        /// <value>System.DateTime</value>
        [Browsable(false)]
        internal DateTime LockActionDateTime
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取是否锁定通道。
        /// </summary>
        /// <remarks>获取是否锁定通道。</remarks>
        /// <value>bool</value>
        [Browsable(false)]
        internal bool IsLock
        {
            get;
            private set;
        }
        /// <summary>
        /// 锁定通道。
        /// </summary>
        /// <remarks>锁定通道。</remarks>
        /// <returns>bool 返回是否通道锁定有效，如果已经被其它地方锁定侧返回否。</returns>
        internal bool Lock()
        {
            //锁定拥有者。
            lock (this.Owner.SyncRoot)
            {
                //锁定通道同步。
                lock (this.SyncRoot)
                {
                    //已经被锁定了，不可以在锁定通道。
                    if (this.IsLock) return false;
                    Debug.WriteLine("锁定[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.Lock");
                    //设置通道为锁定状态。
                    this.IsLock = true;
                    //设置锁动作时间。
                    this.LockActionDateTime = DateTime.Now;
                    //执行通道锁操作动作事件。
                    this.Owner.OnLockActionChange(LockAction.Lock);
                    Debug.WriteLine("锁定完成[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.Lock");
                    //返回有效锁定通道。
                    return true;
                }
            }
        }
        /// <summary>
        /// 设置解锁。
        /// </summary>
        /// <remarks>设置解锁。</remarks>
        /// <returns>bool 返回是否解锁有效，如果通道没被锁定侧解锁无效。</returns>
        internal bool UnLock()
        {
            //锁定拥有者。
            lock (this.Owner.SyncRoot)
            {
                //锁定同步。
                lock (this.SyncRoot)
                {
                    //验证是否通道被锁定。
                    if (this.IsLock)
                    {
                        Debug.WriteLine("解锁[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.UnLock");
                        //设置为解锁状态。
                        this.IsLock = false;
                        //设置锁动作时间。
                        this.LockActionDateTime = DateTime.Now;
                        //执行通道解锁事件动作。
                        this.Owner.OnLockActionChange(LockAction.UnLock);
                        Debug.WriteLine("解锁完成[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.UnLock");
                        //返回解锁操作有效。
                        return true;
                    }
                    //返回解锁操作无效。
                    return false;
                }
            }
        }
        #endregion
        #region SSL验证回调函数
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; //不要在意服务器的证书
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="targetHost"></param>
        /// <param name="localCertificates"></param>
        /// <param name="remoteCertificate"></param>
        /// <param name="acceptableIssuers"></param>
        /// <returns></returns>
        private X509Certificate SelectLocalCertificate(object sender, string targetHost, X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
        {
            return this.Owner.Owner.certificate;
        }
        #endregion
        /// <summary>
        /// 连接到全局会话中心。
        /// </summary>
        /// <remarks>连接到全局会话中心。</remarks>
        internal void Connection()
        {
            //锁定通道。
            lock (this.SyncRoot)
            {
                try
                {
                    if (this.IsConnected) throw new Exception("通道已经连接不可以再次连接！");
                    Debug.WriteLine("连接[Key:" + this.Key.ToString() + ",Host:" + this.Owner.Owner.SessionCenterServiceIPAddresse + ",Port:" + this.Owner.Owner.SessionCenterServicePort.ToString() + "]", "SessionTransferChannel.Connection");
                    this.Client = new TcpClient();
                    this.Client.NoDelay = true;
                    this.Client.Connect(this.Owner.Owner.SessionCenterServiceIPAddresse, this.Owner.Owner.SessionCenterServicePort);
                    Debug.WriteLine("连接完成[Key:" + this.Key.ToString() + ",Host:" + this.Owner.Owner.SessionCenterServiceIPAddresse + ",Port:" + this.Owner.Owner.SessionCenterServicePort.ToString() + "]", "SessionTransferChannel.Connection");
                }
                catch (Exception e)
                {
                    this.Close();
                    throw e;
                }
                //验证是否使用SSL连接。
                if (this.Owner.Owner.certificates != null && this.Owner.Owner.certificates.Count > 0)
                {
                    try
                    {
                        Debug.WriteLine("连接完成开始SSL握手[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.Connection");
                        this.SslStream = new SslStream(this.Client.GetStream(), false, ValidateServerCertificate, SelectLocalCertificate);
                        this.SslStream.AuthenticateAsClient(this.Owner.Owner.SessionCenterServiceIPAddresse, this.Owner.Owner.certificates, System.Security.Authentication.SslProtocols.Tls, false);
                        if (!this.SslStream.IsMutuallyAuthenticated)
                        {
                            Debug.WriteLine("连接完成SSL握手失败[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.Connection");
                            this.Close();
                            return;
                        }
                        if (!this.SslStream.CanWrite)
                        {
                            Debug.WriteLine("连接完成SSL握手创建SSL失败[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.Connection");
                            this.Close();
                            return;
                        }
                        Debug.WriteLine("连接完成开始SSL握手完成[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.Connection");
                    }
                    catch (Exception e)
                    {
                        this.Close();
                        throw e;
                    }
                }
                //初始化读写流。
                this.Reader = new BinaryReader(this.Stream, this.Owner.Owner.Encoding);
                this.Writer = new BinaryWriter(this.Stream, this.Owner.Owner.Encoding);
                Debug.WriteLine("连接SSL握手完成[Key:" + this.Key.ToString() + "]", "SessionTransferChannel.Connection");
                //启动通道空闲超时定时器。
                this.RunChannelIdleTimeout.Enabled = true;
                this.RunChannelIdleTimeout.Start();
            }
        }
        /// <summary>
        /// 写入要打包的对象列表。
        /// </summary>
        /// <remarks>写入要打包的对象列表。</remarks>
        /// <param name="packItems">设置要打包的对象列表。</param>
        internal virtual void Write(List<object> packItems)
        {
            //将要写入打包列表转换为数组。
            this.Write(packItems.ToArray());
        }
        /// <summary>
        /// 写入数组要打包的对象。
        /// </summary>
        /// <remarks>写入数组要打包的对象。</remarks>
        /// <param name="packItems">设置数组要打包的对象。</param>
        internal virtual void Write(object[] packItems)
        {
            //重置通道空闲超时计数器。
            this.CurrentChannelTimeoutCounter = 0;
            //创建写入流打包对象。
            WriteStreamPackaging pack = this.CreateWriteStreamPackaging(true);
            //写入要打包的数组。
            pack.Write(packItems);
            //刷新发送打包的数组对象。
            pack.Flush();
        }
        /// <summary>
        /// 创建写入流打包对象。
        /// </summary>
        /// <remarks>创建写入流打包对象。</remarks>
        /// <param name="isWriteStream">是否直接写入流。</param>
        /// <returns>SessionsStore.WriteStreamPackaging</returns>
        internal WriteStreamPackaging CreateWriteStreamPackaging(bool isWriteStream)
        {
            //是否使用写入流。
            if (isWriteStream)
            {
                //使用写入流。
                return new WriteStreamPackaging(this.Stream, this.Owner.Owner.Encoding);
            }
            //不使用写入流。
            return new WriteStreamPackaging(this.Owner.Owner.Encoding);
        }
        /// <summary>
        /// 获取写入对象。
        /// </summary>
        /// <remarks>获取写入对象。</remarks>
        /// <value>System.IO.BinaryWriter</value>
        internal BinaryWriter Writer
        {
            get;
            private set;
        }
        /// <summary>
        /// 获取读取对象。
        /// </summary>
        /// <remarks>获取读取对象。</remarks>
        /// <value>System.IO.BinaryReader</value>
        internal BinaryReader Reader
        {
            get;
            private set;
        }
        /// <summary>
        /// 读取string。
        /// </summary>
        /// <remarks>读取string。</remarks>
        /// <returns>string</returns>
        internal string ReadString()
        {
            int count = this.Reader.ReadInt32();
            if (count < 0) throw new ArgumentOutOfRangeException("ReadString");
            if (count == 0) return null;
            return this.Owner.Owner.Encoding.GetString(this.Reader.ReadBytes(count));
        }

        /// <summary>
        /// 读取byte[]。
        /// </summary>
        /// <remarks>读取byte[]。</remarks>
        /// <returns>byte[]</returns>
        internal byte[] ReadBytes()
        {
            int count = this.Reader.ReadInt32();
            if (count < 0) throw new ArgumentOutOfRangeException("ReadBytes");
            if (count == 0) return null;
            return this.Reader.ReadBytes(count);
        }
        /// <summary>
        /// 读取char[]。
        /// </summary>
        /// <remarks>读取char[]。</remarks>
        /// <returns>char[]</returns>
        internal char[] ReadChars()
        {
            int count = this.Reader.ReadInt32();
            if (count < 0) throw new ArgumentOutOfRangeException("ReadChars");
            if (count == 0) return null;
            return this.Reader.ReadChars(count);
        }
        /// <summary>
        /// 读取Guid。
        /// </summary>
        /// <remarks>读取Guid。</remarks>
        /// <returns>Guid</returns>
        internal Guid ReadGuid()
        {
            return new Guid(this.Reader.ReadBytes(TypeLengthConstant.GuidLength));
        }
        /// <summary>
        /// 读取DateTime。
        /// </summary>
        /// <remarks>读取DateTime。</remarks>
        /// <returns>DateTime</returns>
        internal DateTime ReadDateTime()
        {
            return new DateTime(this.Reader.ReadInt64());
        }
        /// <summary>
        /// 读取TimeSpan。
        /// </summary>
        /// <remarks>读取TimeSpan。</remarks>
        /// <returns>TimeSpan</returns>
        internal TimeSpan ReadTimeSpan()
        {
            return new TimeSpan(this.Reader.ReadInt64());
        }
        /// <summary>
        /// 读取object。
        /// </summary>
        /// <remarks>读取object。</remarks>
        /// <returns>object</returns>
        internal object ReadObject()
        {
            return Serialization.Deserialize(this.ReadBytes());
        }

        /// <summary>
        /// 获取通道的传输流对象。
        /// </summary>
        /// <remarks>获取通道的传输流对象。</remarks>
        /// <value>System.IO.Stream</value>
        internal Stream Stream
        {
            get
            {
                //判断是否使用SSL流传输。
                if (this.SslStream == null) 
                    return this.NetworkStream;
                //返回使用SSL基础流传输。
                return this.SslStream;
            }
        }

        /// <summary>
        /// 关闭。
        /// </summary>
        /// <remarks>关闭。</remarks>
        private void Close()
        {
            //锁定同步。
            lock (this.SyncRoot)
            {
                //停止通道空闲超时定时器。
                if (this.RunChannelIdleTimeout != null)
                {
                    this.RunChannelIdleTimeout.Enabled = false;
                    this.RunChannelIdleTimeout.Stop();
                }
                //释放读取对象。
                if (this.Reader != null)
                {
                    try
                    {
                        this.Reader.Close();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.Reader.Close");
                    }
#else
                    catch{}
#endif
                    try
                    {
                        this.Reader.Dispose();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.Reader.Dispose");
                    }
#else
                    catch{}
#endif
                    this.Reader = null;
                }
                //释放写对象。
                if (this.Writer != null)
                {
                    try
                    {
                        this.Writer.Close();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.Reader.Close");
                    }
#else
                    catch{}
#endif
                    try
                    {
                        this.Writer.Dispose();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.Writer.Dispose");
                    }
#else
                    catch{}
#endif
                    this.Writer = null;
                }
                //释放SSL读写流对象。
                if (this.SslStream != null)
                {
                    try
                    {
                        this.SslStream.Close();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.SslStream.Close");
                    }
#else
                    catch{}
#endif
                    try
                    {
                        this.SslStream.Dispose();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.SslStream.Dispose");
                    }
#else
                    catch{}
#endif
                    this.SslStream = null;
                }
                //释放网络读写流对象。
                if (this.NetworkStream != null)
                {
                    try
                    {
                        this.NetworkStream.Close();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.NetworkStream.Close");
                    }
#else
                    catch{}
#endif
                    try
                    {
                        this.NetworkStream.Dispose();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.NetworkStream.Dispose");
                    }
#else
                    catch{}
#endif
                    this.NetworkStream = null;
                }
                //释放TCP对象。
                if (this.Client != null)
                {
                    try
                    {
                        this.Client.Close();
                    }
#if DEBUG
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.ToString(), "SessionTransferChannel.Close.Client.Close");
                    }
#else
                    catch{}
#endif
                    this.Client = null;
                }
            }
        }
        /// <summary>
        /// 通道空闲超时定时器。
        /// </summary>
        /// <remarks>通道空闲超时定时器。</remarks>
        private void Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //验证是否需要计算会话传输通道空闲超时。
            if (this.Owner.Owner.SessionTransferChannelIdleTimeout > 0)
            {
                //累计会话传输通道空闲超时计数器，单位为毫秒。
                this.CurrentChannelTimeoutCounter++;
                //验证会话传输通道是否空闲超时。
                if (this.CurrentChannelTimeoutCounter >= this.Owner.Owner.SessionTransferChannelIdleTimeout)
                {
                    //锁定同步。
                    lock (this.SyncRoot)
                    {
                        //验证会话传输通道是否被锁定。
                        if (this.IsLock)
                        {
                            //被锁定侧重新计算会话传输通道空闲超时。
                            this.CurrentChannelTimeoutCounter = 0;
                            return;
                        }
                        else
                        {
                            //会话传输通道空闲超时没有被锁定删除它。
                            //删除本会话传输通道。
                            if (this.Lock())
                            {
                                //停止会话传输通道空闲超时计数器。
                                this.RunChannelIdleTimeout.Enabled = false;
                                this.RunChannelIdleTimeout.Stop();
                                //能锁定表示通道空闲删除它。
                                this.Owner.Remove(this.Key);
                            }
                            else
                            {
                                //被锁定侧重新计算会话传输通道空闲超时。
                                this.CurrentChannelTimeoutCounter = 0;
                                return;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取或设置当前通道空闲超时计数器。
        /// </summary>
        /// <remarks>获取或设置当前通道空闲超时计数器。</remarks>
        /// <value>int</value>
        internal int CurrentChannelTimeoutCounter
        {
            get;
            set;
        }
    }
}
