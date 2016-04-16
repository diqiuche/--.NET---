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
using System.Threading;
namespace SessionsStore
{
    /// <summary>
    /// 会话传输通道管理类。
    /// </summary>
    /// <remarks>会话传输通道管理类。</remarks>
    [ToolboxItem(false)]
    public partial class SessionTransferChannelManager : Component
    {
        #region 初始化
        /// <summary>
        /// 初始化会话传输通道列表类。
        /// </summary>
        /// <remarks>初始化会话传输通道列表类。</remarks>
        /// <param name="owner">设置拥有者。</param>
        internal SessionTransferChannelManager(SessionStore owner)
        {
            this.Owner = owner;
            //初始化通道事件。
            //this.Event = new AutoResetEvent(false);
            InitializeComponent();
        }
        /*
        public SessionTransferChannelManager(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
         */
        #endregion
        /// <summary>
        /// 创建写入流打包对象。
        /// </summary>
        /// <remarks>创建写入流打包对象。</remarks>
        /// <returns>SessionsStore.WriteStreamPackaging</returns>
        internal WriteStreamPackaging CreateWriteStreamPackaging()
        {
            return new WriteStreamPackaging(this.Owner.Encoding);
        }
        /// <summary>
        /// 声明同步锁对象。
        /// </summary>
        /// <remarks>声明同步锁对象。</remarks>
        internal readonly object SyncRoot = new object();
        /*
        /// <summary>
        /// 获取或设置通道事件。
        /// </summary>
        /// <remarks>获取或设置通道事件。</remarks>
        /// <value>System.Threading.AutoResetEvent</value>
        internal AutoResetEvent Event
        {
            set;
            private get;
        }
        */
        /// <summary>
        /// 会话传输通道列表。
        /// </summary>
        /// <remarks>会话传输通道列表。</remarks>
        Dictionary<Guid, SessionTransferChannel> list = new Dictionary<Guid, SessionTransferChannel>();
        /// <summary>
        /// 获取被锁定的节点连接全局资源中心服务通道数量。
        /// </summary>
        /// <remarks>获取被锁定的节点连接全局资源中心服务通道数量。</remarks>
        /// <returns>int</returns>
        internal int LockCount()
        {
            //锁定通道同步。
            lock (this.SyncRoot)
            {
                //声明累计。
                int i = 0;
                //历遍通道列表。
                foreach (KeyValuePair<Guid, SessionTransferChannel> item in this.list)
                {
                    //所动通道。
                    lock (item.Value.SyncRoot)
                    {
                        //验证累计锁定数量。
                        if (item.Value.IsLock) i++;
                    }
                }
                //返回锁定通道数量。
                return i;
            }
        }
        /// <summary>
        /// 获取节点连接全局资源中心的通道数量。
        /// </summary>
        /// <remarks>获取节点连接全局资源中心的通道数量。</remarks>
        /// <value>int</value>
        [Browsable(false)]
        internal int Count
        {
            get
            {
                //锁定通道同步。
                lock (this.SyncRoot)
                {
                    return this.list.Count;
                }
            }
        }

        /// <summary>
        /// 通道锁动作。
        /// </summary>
        /// <remarks>通道锁动作。</remarks>
        /// <param name="action">设置锁动作。</param>
        internal void OnLockActionChange(LockAction action)
        {
            /*
            //验证锁动作类型。
            if (action == LockAction.Destruction || action == LockAction.Timeout || action == LockAction.UnLock)
            {
                //通知等待继续。
                this.Event.Set();
            }
            */
        }
        /// <summary>
        /// 获取拥有者。
        /// </summary>
        /// <remarks>获取拥有者。</remarks>
        /// <value>SessionsStore.SessionStore</value>
        [Browsable(false)]
        internal SessionStore Owner
        {
            get;
            private set;
        }
        /// <summary>
        /// 连接到全局会话中心。
        /// </summary>
        /// <remarks>连接到全局会话中心。</remarks>
        internal void Connection()
        {
            //锁定会话传输通道管理对象。
            lock (this.SyncRoot)
            {
                //声明会话传输通道。
                SessionTransferChannel channel = null;
                try
                {
                    //创建会话传输通道。
                    channel = this.CreateChannel();
                    //锁定通道。
                    lock (channel.SyncRoot)
                    {
                        //锁定。
                        channel.Lock();
                        //连接到全局会话中心。
                        channel.Connection();
                        //将会话传输通道添加入列表中。
                        this.list.Add(channel.Key, channel);
                        //发送初始化指令。
                        channel.Write(new object[] { (byte)SessionStoreCommandHeader.Initialize, (byte)SessionTransferNodeTypes.ASP_NET_FORM });
                        //初始化会话传输字符编码。
#if DEBUG
                        byte ed = channel.Reader.ReadByte();
                        Debug.WriteLine("Connection[CharacterEncoding:" + ed.ToString() + "]", "SessionTransferChannelManager");
                        switch ((CharacterEncoding)ed)
#else
                        switch ((CharacterEncoding)channel.Reader.ReadByte())
#endif
                        {
                            case CharacterEncoding.UTF8Encoding:
                                this.Owner.Encoding = System.Text.Encoding.UTF8;
                                break;
                            case CharacterEncoding.UTF7Encoding:
                                this.Owner.Encoding = System.Text.Encoding.UTF7;
                                break;
                            case CharacterEncoding.UTF32Encoding:
                                this.Owner.Encoding = System.Text.Encoding.UTF32;
                                break;
                            case CharacterEncoding.UnicodeEncoding:
                                this.Owner.Encoding = System.Text.Encoding.Unicode;
                                break;
                            case CharacterEncoding.ASCIIEncoding:
                                this.Owner.Encoding = System.Text.Encoding.ASCII;
                                break;
                            case CharacterEncoding.BigEndianUnicode:
                                this.Owner.Encoding = System.Text.Encoding.BigEndianUnicode;
                                break;
                        }
                        //初始化获取全局会话中心当前时间。
                        this.Owner.SessionCenterCurrentDateTime = channel.ReadDateTime();
                        Debug.WriteLine("Connection[SessionCenterCurrentDateTime:" + this.Owner.SessionCenterCurrentDateTime.ToString() + "]", "SessionTransferChannelManager");
                        //初始化获取全局会话中心通道最大限制。
                        int MaxSessionTransferChannel = channel.Reader.ReadInt32();
                        if (MaxSessionTransferChannel > 0)
                        {
                            this.Owner.MaxSessionTransferChannel = MaxSessionTransferChannel;
                        }
                        Debug.WriteLine("Connection[MaxSessionTransferChannel:" + this.Owner.MaxSessionTransferChannel.ToString() + "]", "SessionTransferChannelManager");
                        //发送安全连接内容。
                        if (string.IsNullOrEmpty(this.Owner.SessionCenterSecureConnectionContent))
                        {
                            channel.Writer.Write((int)0);
                            Debug.WriteLine("Connection[SessionCenterSecureConnectionContent:" + (this.Owner.SessionCenterSecureConnectionContent == null ? "null" : this.Owner.SessionCenterSecureConnectionContent) + "]", "SessionTransferChannelManager");
                        }
                        else
                        {
                            byte[] bytes = new byte[this.Owner.Encoding.GetByteCount(this.Owner.SessionCenterSecureConnectionContent)];
                            Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, bytes, 0, TypeLengthConstant.IntLength);
                            Buffer.BlockCopy(this.Owner.Encoding.GetBytes(this.Owner.SessionCenterSecureConnectionContent), 0, bytes, TypeLengthConstant.IntLength, bytes.Length);
                            channel.Writer.Write(bytes);
                            Debug.WriteLine("Connection[SessionCenterSecureConnectionContent:" + this.Owner.SessionCenterSecureConnectionContent + "]", "SessionTransferChannelManager");
                        }
                        //验证安全连接是否通过。
#if DEBUG
                        if (channel.Reader.ReadBoolean())
                        {
                            Debug.WriteLine("Connection[安全连接验证通过]", "SessionTransferChannelManager");
                        }
                        else
                        {
                            Debug.WriteLine("Connection[安全连接验证不通过]", "SessionTransferChannelManager");
                            throw new Exception("安全连接验证不通过！");
                        }
#else
                        if (!channel.Reader.ReadBoolean())
                        {
                            throw new Exception("安全连接验证不通过！");
                        }
#endif
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Connection[e:" + e.ToString() + "]", "SessionTransferChannelManager");
                    //验证通道对象是否为空。
                    if (channel != null)
                    {
                        //在通道列表中删除当前通道。
                        if (this.list.ContainsKey(channel.Key))
                        {
                            this.list.Remove(channel.Key);
                        }
                        //是否通道资源。
                        try
                        {
                            channel.Dispose();
                        }
                        finally
                        {
                            channel = null;
                        }
                    }
                    throw e;
                }
                finally
                {
                    //解锁。
                    if (channel != null)
                    {
                        channel.UnLock();
                    }
                }
            }
        }
        /// <summary>
        /// 获取会话传输通道。
        /// </summary>
        /// <remarks>获取会话传输通道。</remarks>
        /// <returns>SessionsStore.SessionTransferChannel</returns>
        internal SessionTransferChannel GetChannel()
        {
            //重新开始等待可用的会话传输通道。
            Restart:
            //锁定会话传输通道列表。
            lock (this.SyncRoot)
            {
                //验证释放有可用未被锁定的会话传输通道。
                if (this.list.Count > this.LockCount())
                {
                    //遍历全部会话传输通道对象。
                    foreach (SessionTransferChannel item in this.list.Values)
                    {
                        //锁定会话传输通道。
                        if (item.Lock())
                        {
                            Debug.WriteLine("找到可以用的通道获取它[Key:" + item.Key.ToString() + "]", "SessionTransferChannelManager.GetChannel");
                            //得到被有效锁定的会话传输通道。
                            return item;
                        }
                    }
                }
            }
            //验证是否有最大会话传输通道限制。
            if (this.Owner.MaxSessionTransferChannel > 0)
            {
                //声明会话传输通道数量。
                int n = this.Count;
                //验证会话传输通道是否已经在最大数量。
                if (n >= this.Owner.MaxSessionTransferChannel)
                {
                    //会话传输通道已经在最大数量不允许再创建新的会话传输通道。
                    goto Restart;
                }
            }
            //创建新的会话传输通道。
            SessionTransferChannel channel = null;
            try
            {
                //新建会话传输通道对象。
                channel = this.CreateChannel();
                //连接到会话中心。
                channel.Connection();
                //锁定会话传输通道对象。
                channel.Lock();
                //将新建的会话传输通道添加到会话传输通道列表中。
                this.list.Add(channel.Key, channel);
            }
            catch (Exception e)
            {
                //验证通道对象是否为空。
                if (channel != null)
                {
                    //在通道列表中删除当前通道。
                    if (this.list.ContainsKey(channel.Key))
                    {
                        this.list.Remove(channel.Key);
                    }
                    //是否通道资源。
                    try
                    {
                        channel.Dispose();
                    }
                    finally
                    {
                        channel = null;
                    }
                }
                throw e;
            }
            return channel;
        }
        /// <summary>
        /// 写入要打包的对象列表。
        /// </summary>
        /// <remarks>写入要打包的对象列表。</remarks>
        /// <param name="packItems">设置要打包的对象列表。</param>
        internal virtual void Write(List<object> packItems)
        {
            this.Write(packItems.ToArray());
        }
        /// <summary>
        /// 写入打包对象。
        /// </summary>
        /// <remarks>写入打包对象。</remarks>
        /// <param name="pack">设置打包对象。</param>
        internal virtual void Write(WriteStreamPackaging pack)
        {
            //刷新打包数据。
            pack.Flush();
            //声明会话传输通道。
            SessionTransferChannel channel = null;
            try
            {
                //获取会话传输通道对象，返回必须是被锁定的会话传输
                channel = this.GetChannel();
                //验证会话传输通道是否被锁定。
                if (!channel.IsLock)
#if DEBUG
                    throw new Exception("获取会话传输通道没有被锁定错误！[Key:" + channel.Key.ToString() + "]");
#else
                    throw new Exception("获取会话传输通道没有被锁定错误！");  
#endif
                //重置通道空闲超时计数器。
                channel.CurrentChannelTimeoutCounter = 0;
                //写入数据流。
                channel.Stream.Write(pack.GetBuffer, 0, pack.GetBuffer.Length);
            }
            finally
            {
                //解锁会话传输通道。
                channel.UnLock();
            }
        }
        /// <summary>
        /// 写入数组要打包的对象。
        /// </summary>
        /// <remarks>写入数组要打包的对象。</remarks>
        /// <param name="packItems">设置数组要打包的对象。</param>
        internal virtual void Write(object[] packItems)
        {
            //创建打包对象。
            WriteStreamPackaging pack = this.CreateWriteStreamPackaging();
            //将要打包的对象列表写入打包对象中。
            pack.Write(packItems);
            //写入打包对象。
            this.Write(pack);
        }
        /// <summary>
        /// 获取是否连接到全局会话中心。
        /// </summary>
        /// <remarks>获取是否连接到全局会话中心。</remarks>
        /// <value>bool</value>
        [Browsable(false)]
        internal bool IsConnected
        {
            get
            {
                //锁定会话传输通道列表。
                lock (this.SyncRoot)
                {
                    //遍历所有会话传输通道对象。
                    foreach (SessionTransferChannel item in this.list.Values)
                    {
                        //验证会话传输通道对象是否已经连接。
                        if (item.IsConnected) return true;
                    }
                    //没有找到会话传输通道连接对象。
                    return false;
                }
            }
        }

        /// <summary>
        /// 创建会话传输通道。
        /// </summary>
        /// <remarks>创建会话传输通道。</remarks>
        /// <returns>SessionsStore.SessionTransferChannel</returns>
        internal SessionsStore.SessionTransferChannel CreateChannel()
        {
            return new SessionTransferChannel(this);
        }

        /// <summary>
        /// 删除会话传输通道。
        /// </summary>
        /// <remarks>删除会话传输通道。</remarks>
        internal void Remove(Guid key)
        {
            //验证要删除的会话传输通道键是否有效。
            if (key == Guid.Empty) return;
            //创建线程删除会话传输通道。
            new Thread(new ParameterizedThreadStart(delegate (object obj)
            {
                //要删除的会话传输通道键。
                Guid k = (Guid)obj;
                //锁定同步。
                lock (this.SyncRoot)
                {
                    //验证会话传输通道是否存在。
                    if (this.list.ContainsKey(k))
                    {
                        //获取要删除的会话传输通道对象。
                        SessionTransferChannel channel = this.list[k];
                        try
                        {
                            //从会话传输通道列表中删除。
                            this.list.Remove(k);
                        }
                        finally
                        {
                            //释放会话传输通道资源。
                            channel.Dispose();
                        }
                    }
                }
                //启动线程。
            })).Start(key);
        }

        /// <summary>
        /// 关闭。
        /// </summary>
        /// <remarks>关闭。</remarks>
        private void Close()
        {
            //锁定会话传输通道列表。
            lock (this.SyncRoot)
            {
                //声明创建会话传输通道键列表。
                List<Guid> keys = new List<Guid>();
                //遍历会话传输通道键列表。
                foreach (Guid item in this.list.Keys)
                {
                    //将会话传输通道键添加入列表中。
                    keys.Add(item);
                }
                //遍历全部会话传输通道键。
                foreach (Guid item in keys)
                {
                    //验证会话传输通道是否存在。
                    if (this.list.ContainsKey(item))
                    {
                        //获取要删除的会话传输通道对象。
                        SessionTransferChannel channel = this.list[item];
                        try
                        {
                            //从会话传输通道列表中删除。
                            this.list.Remove(item);
                        }
                        finally
                        {
                            //释放会话传输通道资源。
                            channel.Dispose();
                        }
                    }
                }
            }
        }
    }
}
