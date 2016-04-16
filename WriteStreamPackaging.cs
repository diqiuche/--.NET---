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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
namespace SessionsStore
{
    /// <summary>
    /// 写入流打包类。
    /// </summary>
    /// <remarks>写入流打包类。</remarks>
    public class WriteStreamPackaging
    {
        #region 声明
        /// <summary>
        /// 打包字符编码。
        /// </summary>
        /// <remarks>打包字符编码。</remarks>
        private Encoding packEncoding;
        /// <summary>
        /// 打包后要写入的流对象。
        /// </summary>
        /// <remarks>打包后要写入的流对象。</remarks>
        private Stream stream;
        /// <summary>
        /// 写入流打包对象列表。
        /// </summary>
        /// <remarks>写入流打包对象列表。</remarks>
        private List<object> packList = new List<object>();
        /// <summary>
        /// 写入流打包对象需序列化列表。
        /// </summary>
        /// <remarks>写入流打包对象需序列化列表。</remarks>
        private List<byte[]> packSerializeList;
        #endregion
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="stream">设置要写入的流对象。</param>
        public WriteStreamPackaging(Stream stream)
        {
            this.stream = stream;
            this.packEncoding = Encoding.UTF8;
        }
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="stream">设置要写入的流对象。</param>
        /// <param name="packEncoding">设置要打包的字符编码。</param>
        public WriteStreamPackaging(Stream stream, Encoding packEncoding)
        {
            this.stream = stream;
            this.packEncoding = packEncoding;
        }
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="packList">设置要打包的对象列表。</param>
        public WriteStreamPackaging(List<object> packList)
        {
            this.packEncoding = Encoding.UTF8;
            this.packList.AddRange(packList);
        }
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="packList">设置要打包的数组对象。</param>
        public WriteStreamPackaging(object[] packList)
        {
            this.packEncoding = Encoding.UTF8;
            this.packList.AddRange(packList);
        }
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="packList">设置要打包的对象集合。</param>
        public WriteStreamPackaging(ICollection packList)
        {
            this.packEncoding = Encoding.UTF8;
            foreach (object item in packList)
            {
                this.packList.Add(item);
            }
        }
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="packList">设置要打包的对象集合。</param>
        public WriteStreamPackaging(ICollection<object> packList)
        {
            this.packEncoding = Encoding.UTF8;
            this.packList.AddRange(packList);
        }
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="packEncoding">设置要打包的字符编码。</param>
        public WriteStreamPackaging(Encoding packEncoding)
        {
            this.packEncoding = packEncoding;
        }
        /// <summary>
        /// 初始化写入流打包类。
        /// </summary>
        /// <remarks>初始化写入流打包类。</remarks>
        /// <param name="packEncoding">设置要打包的字符编码。</param>
        /// <param name="packList">设置要打包的对象列表。</param>
        public WriteStreamPackaging(Encoding packEncoding, List<object> packList)
        {
            this.packEncoding = packEncoding;
            this.packList.AddRange(packList);
        }

        /// <summary>
        /// 获取打包的字符编码对象。
        /// </summary>
        /// <remarks>获取打包的字符编码对象。</remarks>
        /// <value>System.Text.Encoding</value>
        public Encoding PackEncoding
        {
            get { return this.packEncoding; }
        }
        /// <summary>
        /// 写入要打包的对象列表。
        /// </summary>
        /// <remarks>写入要打包的对象列表。</remarks>
        /// <param name="packItems">设置要打包的对象列表。</param>
        public virtual void Write(List<object> packItems)
        {
            this.packList.AddRange(packItems);
        }
        /// <summary>
        /// 写入数组要打包的对象。
        /// </summary>
        /// <remarks>写入数组要打包的对象。</remarks>
        /// <param name="packItems">设置数组要打包的对象。</param>
        public virtual void Write(object[] packItems)
        {
            this.packList.AddRange(packItems);
        }
        /// <summary>
        /// 写入要打包的对象。
        /// </summary>
        /// <remarks>写入要打包的对象。</remarks>
        /// <param name="packItem">设置要打包的对象。</param>
        public virtual void Write(object packItem)
        {
            this.packList.Add(packItem);
        }
        /// <summary>
        /// 单个对象计算长度。
        /// </summary>
        /// <remarks>单个对象计算长度。</remarks>
        /// <param name="length">返回计算对象长度累计的长度。</param>
        /// <param name="item">设置要计算长度的对象。</param>
        protected virtual void OnComputeLength(ref int length, object item)
        {
            if (item is sbyte)
            {
                //累加sbyte长度。
                length = length + TypeLengthConstant.SByteLength;
            }
            else if (item is ushort)
            {
                //累加ushort长度。
                length = length + TypeLengthConstant.UShortLength;
            }
            else if (item is uint)
            {
                //累加uint长度。
                length = length + TypeLengthConstant.UIntLength;
            }
            else if (item is ulong)
            {
                //累加ulong长度。
                length = length + TypeLengthConstant.ULongLength;
            }
            else if (item is byte)
            {
                //累加byte长度。
                length = length + TypeLengthConstant.ByteLength;
            }
            else if (item is bool)
            {
                //累加bool长度。
                length = length + TypeLengthConstant.BoolLength;
            }
            else if (item is short)
            {
                //累加short长度。
                length = length + TypeLengthConstant.ShortLength;
            }
            else if (item is int)
            {
                //累加int长度。
                length = length + TypeLengthConstant.IntLength;
            }
            else if (item is long)
            {
                //累加long长度。
                length = length + TypeLengthConstant.LongLength;
            }
            else if (item is float)
            {
                //累加float长度。
                length = length + TypeLengthConstant.FloatLength;
            }
            else if (item is double)
            {
                //累加double长度。
                length = length + TypeLengthConstant.DoubleLength;
            }
            else if (item is decimal)
            {
                //累加decimal长度。
                length = length + TypeLengthConstant.DecimalLength;
            }
            else if (item is Guid)
            {
                //累加Guid长度。
                length = length + TypeLengthConstant.GuidLength;
            }
            else if (item is char)
            {
                //累加char长度。
                length = length + TypeLengthConstant.CharLength;
            }
            else if (item is char[])
            {
                //累加char[]长度。
                length = length + (TypeLengthConstant.CharLength * ((char[])item).Length);
            }
            else if (item is DateTime)
            {
                //累加DateTime长度。
                length = length + TypeLengthConstant.DateTimeLength;
            }
            else if (item is TimeSpan)
            {
                //累加TimeSpan长度。
                length = length + TypeLengthConstant.TimeSpanLength;
            }
            else if (item is string)
            {
                //累加string长度。
                length = length + TypeLengthConstant.IntLength;
                length = length + this.packEncoding.GetByteCount((string)item);
            }
            else if (item is Stream)
            {
                //累加Stream长度。
                length = length + TypeLengthConstant.IntLength;
                length = length + Convert.ToInt32(((Stream)item).Length);
            }
            else if (item is byte[])
            {
                //累加byte[]长度。
                length = length + TypeLengthConstant.IntLength;
                length = length + ((byte[])item).Length;
            }
            else if (item is object)
            {
                //累加object长度。
                length = length + TypeLengthConstant.IntLength;
                //验证对象是否可以序列化。
                if (!item.GetType().IsSerializable) throw new ArgumentException("对象打包不支持序列化错误[Item:" + item.ToString() + "]");
                //系列化对象。
                byte[] bytes = Serialization.Serialize(item);
                //验证打包序列化列表对象是否未初始化。
                if (this.packSerializeList == null)
                {
                    //初始化打包序列化对象。
                    this.packSerializeList = new List<byte[]>();
                }
                //将对象添加入打包序列化列表中。
                this.packSerializeList.Add(bytes);
                length = length + bytes.Length;
            }
            else if (item == null)
            {
                //累加null长度。
                length = length + TypeLengthConstant.IntLength;
            }
        }
        /// <summary>
        /// 计算长度。
        /// </summary>
        /// <remarks>计算长度。</remarks>
        /// <returns>int 返回计算全部要打包的数据长度。</returns>
        protected virtual int ComputeLength()
        {
            //声明长度。
            int length = 0;
            //遍历所有打包对象。
            foreach (object item in this.packList)
            {
                //逐个计算长度。
                this.OnComputeLength(ref length, item);
            }
            //返回计算的长度。
            return length;
        }
        /// <summary>
        /// 刷新逐个对象的数据复制到缓冲区中。
        /// </summary>
        /// <remarks>刷新逐个对象的数据复制到缓冲区中。</remarks>
        /// <param name="position">复制对象数据到缓冲区的位置。</param>
        /// <param name="buffer">复制对象数据到缓冲区。</param>
        /// <param name="serializeIndex">复制对象数据到缓冲区对象序列化的位置。</param>
        /// <param name="item">复制对象数据到缓冲区的对象。</param>
        protected virtual void OnFlush(ref int position, ref byte[] buffer, ref int serializeIndex, object item)
        {
            if (item is sbyte)
            {
                //将sbyte类型数据写入缓冲区。
                this.buffer[position] = (byte)item;
                position = position + TypeLengthConstant.SByteLength;
            }
            else if (item is ushort)
            {
                //将ushort类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((ulong)item), 0, this.buffer, position, TypeLengthConstant.UShortLength);
                position = position + TypeLengthConstant.UShortLength;
            }
            else if (item is uint)
            {
                //将uint类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((ulong)item), 0, this.buffer, position, TypeLengthConstant.UIntLength);
                position = position + TypeLengthConstant.UIntLength;
            }
            else if (item is ulong)
            {
                //将ulong类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((ulong)item), 0, this.buffer, position, TypeLengthConstant.ULongLength);
                position = position + TypeLengthConstant.ULongLength;
            }
            else if (item is byte)
            {
                //将byte类型数据写入缓冲区。
                this.buffer[position] = (byte)item;
                position = position + TypeLengthConstant.ByteLength;
            }
            else if (item is bool)
            {
                //将bool类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((bool)item), 0, this.buffer, position, TypeLengthConstant.BoolLength);
                position = position + TypeLengthConstant.BoolLength;
            }
            else if (item is short)
            {
                //将short类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((short)item), 0, this.buffer, position, TypeLengthConstant.ShortLength);
                position = position + TypeLengthConstant.ShortLength;
            }
            else if (item is int)
            {
                //将int类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((int)item), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
            }
            else if (item is long)
            {
                //将long类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((long)item), 0, this.buffer, position, TypeLengthConstant.LongLength);
                position = position + TypeLengthConstant.LongLength;
            }
            else if (item is float)
            {
                //将float类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((float)item), 0, this.buffer, position, TypeLengthConstant.FloatLength);
                position = position + TypeLengthConstant.FloatLength;
            }
            else if (item is double)
            {
                //将double类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((double)item), 0, this.buffer, position, TypeLengthConstant.DoubleLength);
                position = position + TypeLengthConstant.DoubleLength;
            }
            else if (item is decimal)
            {
                //将decimal类型数据写入缓冲区。
                int[] ints = Decimal.GetBits(Convert.ToDecimal(item));
                Buffer.BlockCopy(BitConverter.GetBytes(ints[0]), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
                Buffer.BlockCopy(BitConverter.GetBytes(ints[1]), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
                Buffer.BlockCopy(BitConverter.GetBytes(ints[2]), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
                Buffer.BlockCopy(BitConverter.GetBytes(ints[3]), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
            }
            else if (item is Guid)
            {
                //将Guid类型数据写入缓冲区。
                Buffer.BlockCopy(((Guid)item).ToByteArray(), 0, this.buffer, position, TypeLengthConstant.GuidLength);
                position = position + TypeLengthConstant.GuidLength;
            }
            else if (item is char)
            {
                //将char类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((char)item), 0, this.buffer, position, TypeLengthConstant.CharLength);
                position = position + TypeLengthConstant.CharLength;
            }
            else if (item is DateTime)
            {
                //将DateTime类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes(((DateTime)item).Ticks), 0, this.buffer, position, TypeLengthConstant.DateTimeLength);
                position = position + TypeLengthConstant.DateTimeLength;
            }
            else if (item is TimeSpan)
            {
                //将TimeSpan类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes(((TimeSpan)item).Ticks), 0, this.buffer, position, TypeLengthConstant.TimeSpanLength);
                position = position + TypeLengthConstant.TimeSpanLength;
            }
            else if (item is char[])
            {
                //将char[]类型数据写入缓冲区。
                foreach (char citem in (char[])item)
                {
                    Buffer.BlockCopy(BitConverter.GetBytes((char)citem), 0, this.buffer, position, TypeLengthConstant.CharLength);
                    position = position + TypeLengthConstant.CharLength;
                }
            }
            else if (item is string)
            {
                //将string类型数据写入缓冲区。
                byte[] sBytes = this.packEncoding.GetBytes((string)item);
                Buffer.BlockCopy(BitConverter.GetBytes(sBytes.Length), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
                Buffer.BlockCopy(sBytes, 0, this.buffer, position, sBytes.Length);
                position = position + sBytes.Length;
            }
            else if (item is Stream)
            {
                //将Stream类型数据写入缓冲区。
                Stream stream = (Stream)item;
                Buffer.BlockCopy(BitConverter.GetBytes((int)stream.Length), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
                byte[] streamBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(streamBytes, 0, streamBytes.Length);
                Buffer.BlockCopy(streamBytes, 0, this.buffer, position, streamBytes.Length);
                position = position + streamBytes.Length;
            }
            else if (item is byte[])
            {
                //将byte[]类型数据写入缓冲区。
                byte[] stream = (byte[])item;
                Buffer.BlockCopy(BitConverter.GetBytes(stream.Length), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
                Buffer.BlockCopy(stream, 0, this.buffer, position, stream.Length);
                position = position + stream.Length;
            }
            else if (item is object)
            {
                //将object类型数据写入缓冲区。
                byte[] serializeBytes = this.packSerializeList[serializeIndex];
                Buffer.BlockCopy(BitConverter.GetBytes(serializeBytes.Length), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
                Buffer.BlockCopy(serializeBytes, 0, this.buffer, position, serializeBytes.Length);
                position = position + serializeBytes.Length;
                //累加序列化位置。
                serializeIndex++;
            }
            else if (item == null)
            {
                //将null类型数据写入缓冲区。
                Buffer.BlockCopy(BitConverter.GetBytes((int)0), 0, this.buffer, position, TypeLengthConstant.IntLength);
                position = position + TypeLengthConstant.IntLength;
            }
        }
        /// <summary>
        /// 刷新打包内容到缓冲区，当有初始化流对象时直接写入。
        /// </summary>
        /// <remarks>刷新打包内容到缓冲区，当有初始化流对象时直接写入。</remarks>
        public virtual void Flush()
        {
            //声明缓冲区位置。
            int position = 0;
            //初始化缓冲区。
            this.buffer = new byte[this.ComputeLength()];
            //声明序列化位置。
            int serializeIndex = 0;
            //遍历所有打包对象。
            foreach (object item in this.packList)
            {
                //刷新打包对象到缓冲区。
                this.OnFlush(ref position, ref buffer, ref serializeIndex, item);
            }
            //清理打包列表。
            if (this.packList != null) this.packList.Clear();
            //清理打包序列化列表。
            if (this.packSerializeList != null) this.packSerializeList.Clear();
            //验证是否有写入流对象。
            if (this.stream != null)
            {
                //将数据直接写入到流中。
                this.stream.Write(this.buffer, 0, this.buffer.Length);
            }
        }
        /// <summary>
        /// 刷新打包内容写入流。
        /// </summary>
        /// <remarks>刷新打包内容到缓冲区，当有初始化流对象时直接写入。</remarks>
        /// <param name="writeStream">设置要写入的流对象。</param>
        public virtual void Flush(Stream writeStream)
        {
            //设置写入流对象。
            this.stream = writeStream;
            //刷新打包数据并直接写入流对象。
            this.Flush();
        }
        /// <summary>
        /// 获取打包后的缓冲区。
        /// </summary>
        /// <remarks>获取打包后的缓冲区。</remarks>
        /// <value>byte[]</value>
        public byte[] GetBuffer
        {
            get
            {
                return this.buffer;
            }
        }

        /// <summary>
        /// 写入流缓冲区。
        /// </summary>
        /// <remarks>写入流缓冲区。</remarks>
        private byte[] buffer;
    }
}
