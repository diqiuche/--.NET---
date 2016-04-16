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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace SessionsStore
{
    /// <summary>
    /// 序列化类。
    /// </summary>
    /// <remarks>序列化类。</remarks>
    static class Serialization
    {
        /// <summary>
        /// 系列化。
        /// </summary>
        /// <remarks>系列化。</remarks>
        /// <param name="item">设置要序列化的对象。</param>
        /// <returns>byte[]</returns>
        public static byte[] Serialize(object item)
        {
            byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, item);
                ms.Close();
                bytes = ms.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// 反系列化。
        /// </summary>
        /// <remarks>反系列化。</remarks>
        /// <param name="item">设置要反序列化的数据。</param>
        /// <returns>object</returns>
        public static object Deserialize(byte[] item)
        {
            object o = null;
            using (MemoryStream ms = new MemoryStream(item))
            {
                BinaryFormatter bf = new BinaryFormatter();
                o = bf.Deserialize(ms);
            }
            return o;
        }
    }
}