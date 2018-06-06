using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace next.net
{
    /// <summary>
    /// 以json為基礎的封包處理類別
    ///
    ///  > 封包格式
    ///  jsonCode:
    ///  {
    ///      "type":[ <-- 存放封包名稱的列表
    ///          "packetName1", <-- 第一個封包的名稱
    ///          "packetName2", <-- 第二個封包的名稱
    ///          ...
    ///      ],
    ///      "data":[ <-- 存放封包內容的列表, 內容也是json格式
    ///          "\"data1\":\"12345\", \"data2\":99875", <-- 第一個封包的內容
    ///          "\"data1\":\"12345\", \"data2\":99875", <-- 第二個封包的內容
    ///          ...
    ///      ]
    ///  }
    ///
    ///  > 封包類別
    ///  封包類別只要是單純合法的java類別即可, 但是注意封包成員最好別用Map系列容器
    ///
    ///  > 注意!一個封包名稱(以及其封包物件)只能在核心封包中出現一次
    /// </summary>
    public class HandlerJson : BaseHandler
    {
        /// <summary>
        /// 接收者介面
        /// </summary>
        private interface ReceiverBase
        {
            /// <summary>
            /// 接收封包處理
            /// </summary>
            /// <param name="jsonInterface">json處理物件</param>
            /// <param name="json">json字串</param>
            void recv(JsonProcess jsonInterface, string json);
        }

        /// <summary>
        /// 核心封包類別
        /// </summary>
        public class CorePacket
        {
            public List<string> types = new List<string>(); // 封包名稱列表
            public List<string> datas = new List<string>(); // 封包內容列表

            /// <summary>
            /// 新增封包
            /// </summary>
            /// <param name="packetName">封包名稱</param>
            /// <param name="packetData">封包內容</param>
            public void add(string packetName, string packetData)
            {
                types.Add(packetName);
                datas.Add(packetData);
            }
        }

        /// <summary>
        /// 接收者類別
        /// </summary>
        private class Receiver<TPacket> : ReceiverBase
        {
            private Handler handler = null; // 封包處理委派

            /// <summary>
            /// 建構子需要封包處理委派
            /// </summary>
            /// <param name="handler">封包處理委派</param>
            public Receiver(Handler handler)
            {
                this.handler = handler;
            }

            /// <summary>
            /// 接收封包處理
            /// </summary>
            /// <param name="jsonInterface">json處理物件</param>
            /// <param name="json">json字串</param>
            public void recv(JsonProcess jsonInterface, string json)
            {
                if (handler == null)
                    throw new Exception("handler null");

                if (json.Length <= 0)
                    throw new Exception("json empty");

                handler(jsonInterface.toObject<TPacket>(json));
            }
        }

        /// <summary>
        /// 封包處理委派
        /// 當收到封包時, 會呼叫此委派類型的函式
        /// </summary>
        /// <param name="obj">封包物件</param>
        public delegate void Handler(System.Object obj);

        private System.Object syncLock = new System.Object(); // 同步鎖物件
        private JsonProcess jsonInterface = null; // json處理物件
        private DataBuffer buffer = new DataBuffer(); // 資料緩衝區
        private Dictionary<string, ReceiverBase> receivers = new Dictionary<string, ReceiverBase>(); // 封包處理列表

        /// <summary>
        /// 建構子需要json處理物件物件
        /// </summary>
        /// <param name="jsonInterface">json處理物件</param>
        public HandlerJson(JsonProcess jsonInterface)
        {
            this.jsonInterface = jsonInterface;
        }

        /// <summary>
        /// 新增封包處理
        /// </summary>
        /// <param name="handler">封包處理委派</param>
        public void add<TPacket>(Handler handler)
        {
            lock (syncLock)
            {
                if (jsonInterface == null)
                    throw new Exception("json interface null");

                Type type = typeof(TPacket);

                if (handler == null)
                    throw new Exception("handler null(" + type.Name + ")");

                if (receivers.ContainsKey(type.Name))
                    throw new Exception("duplicate handler(" + type.Name + ")");

                receivers.Add(type.Name, new Receiver<TPacket>(handler));
            }//lock
        }

        /// <summary>
        /// 接收封包處理
        /// </summary>
        /// <param name="data">資料陣列</param>
        /// <param name="length">資料長度</param>
        protected internal override void recv(byte[] data, int length)
        {
            lock (syncLock)
            {
                if (jsonInterface == null)
                    throw new Exception("json interface null");

                if (data == null)
                    throw new Exception("data null");

                if (data.Length <= 0)
                    throw new Exception("data empty");

                if (length <= 0)
                    throw new Exception("data empty");

                try
                {
                    buffer.push(data, length); // 將收到的數據加到緩衝區的最後面

                    while (true)
                    {
                        if (buffer.Length <= 0)
                            return;

                        CodedInputStream inputStream = new CodedInputStream(buffer.Data);

                        if (buffer.Length < inputStream.RecursionLimit)
                            return;

                        int jsonLength = inputStream.ReadInt32(); // 取得json長度
                        int headLength = (int)inputStream.Position;

                        // 如果緩衝區儲存的數據未超過json長度, 表示json還沒接收完畢
                        if (jsonLength > (buffer.Length - (int)inputStream.Position))
                            return;

                        byte[] temp = new byte[jsonLength];

                        Array.Copy(buffer.Data, (int)inputStream.Position, temp, 0, jsonLength);
                        buffer.pop(jsonLength + headLength); // 把讀取過的資料刪除

                        CorePacket corePacket = jsonInterface.toObject<CorePacket>(Encoding.UTF8.GetString(temp)); // 解析核心封包物件

                        if (corePacket == null)
                            throw new Exception("parse failed");

                        for (int i = 0, max = corePacket.types.Count; i < max; ++i)
                        {
                            string packetName = corePacket.types[i];
                            string packetData = corePacket.datas[i];

                            // 取得接收者物件
                            ReceiverBase receiver = null;

                            if (receivers.TryGetValue(packetName, out receiver) == false || receiver == null)
                                throw new Exception("receiver not found(" + packetName + ")");

                            receiver.recv(jsonInterface, packetData); // 接收處理
                        }//for
                    }//while
                }//try
                catch (Exception e)
                {
                    throw e;
                }//catch
            }//lock
        }

        /// <summary>
        /// 傳送封包處理
        /// </summary>
        /// <param name="objects">資料物件列表</param>
        /// <returns>資料陣列</returns>
        protected internal override byte[] send(params System.Object[] objects)
        {
            lock (syncLock)
            {
                if (jsonInterface == null)
                    throw new Exception("json interface null");

                if (objects == null)
                    throw new Exception("packet null");

                if (objects.Length <= 0)
                    throw new Exception("packet empty");

                CorePacket corePacket = new CorePacket();

                foreach (System.Object itor in objects)
                    corePacket.add(itor.GetType().Name, jsonInterface.toJson(itor));

                using (MemoryStream memory = new MemoryStream())
                {
                    CodedOutputStream outputStream = new CodedOutputStream(memory);

                    outputStream.WriteBytes(ByteString.CopyFrom(Encoding.UTF8.GetBytes(jsonInterface.toJson(corePacket))));
                    outputStream.Flush();

                    return memory.ToArray();
                }//using
            }//lock
        }
    }
}