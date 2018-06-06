using Google.Protobuf;
using Google.Protobuf.Collections;
using Google.Protobuf.Reflection;
using System;
using System.Collections.Generic;
using System.IO;

namespace FouridStudio
{
    /// <summary>
    ///  以google protobuf為基礎的封包處理類別
    ///
    ///  > 封包格式
    ///  以一個核心proto, 其他的proto將以子proto方式被包含在此核心proto內, 並且欄位名稱必須與這些proto的名稱一致
    ///  另外以一個int32的列表, 名稱為type的成員來指示封包使用的proto的編號, 此type成員的封包編號必須為1
    ///  核心proto內各個子proto的封包編號不可重複
    ///  messageCode(google protobuf 3.0.0):
    ///  message Proto
    ///  {
    ///      repeated int32 type = 1; // 封包編號, 編號內容為的子項目proto的編號列表
    ///
    ///      Data1 Data1 = 101; // proto類別, 欄位名稱與類別名稱必須一致
    ///      Data2 Data2 = 102; // proto類別
    ///      Data3 Data3 = 103; // proto類別
    ///      Data4 Data4 = 104; // proto類別
    ///  }
    ///
    ///  > 封包類別
    ///  以google protobuf 3.0.0的編譯工具編譯出來即可
    ///
    ///  > 注意!一個封包編號(以及其封包物件)只能在核心封包中出現一次
    /// </summary>
    public class HandlerProto<TProtoCore> : BaseHandler where TProtoCore : class, IMessage<TProtoCore>, new()
    {
        /// <summary>
        /// 封包處理委派
        /// 當收到封包時, 會呼叫此委派類型的函式
        /// </summary>
        /// <param name="obj">封包物件</param>
        public delegate void Handler(System.Object obj);

        private const int FIELD_TYPE = 1; // 型態欄位編號

        private System.Object syncLock = new System.Object(); // 同步鎖物件
        private DataBuffer buffer = new DataBuffer(); // 資料緩衝區
        private Dictionary<int, Handler> receivers = new Dictionary<int, Handler>(); // 封包處理列表

        /// <summary>
        /// 新增封包處理
        /// </summary>
        /// <param name="handler">封包處理委派</param>
        public void add<T>(Handler handler) where T : class, IMessage, new()
        {
            lock (syncLock)
            {
                Type type = typeof(T);

                if (handler == null)
                    throw new Exception("handler null(" + type.Name + ")");

                FieldDescriptor field = new TProtoCore().Descriptor.FindFieldByName(type.Name);

                if (field == null)
                    throw new Exception("field not found(" + type.Name + ")");

                if (receivers.ContainsKey(field.FieldNumber))
                    throw new Exception("duplicate handler(" + type.Name + ")");

                receivers.Add(field.FieldNumber, handler);
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

                        CodedInputStream inputStream = new CodedInputStream(buffer.Data, 0, buffer.Length);
                        int protoLength = inputStream.ReadInt32(); // 取得proto長度
                        int headLength = (int)inputStream.Position;

                        // 如果緩衝區儲存的數據未超過proto長度, 表示proto還沒接收完畢
                        if (protoLength > (buffer.Length - (int)inputStream.Position))
                            return;

                        byte[] temp = new byte[protoLength];

                        Array.Copy(buffer.Data, (int)inputStream.Position, temp, 0, protoLength);
                        buffer.pop(protoLength + headLength); // 把讀取過的資料刪除

                        TProtoCore protoCore = new MessageParser<TProtoCore>(() => new TProtoCore()).ParseFrom(temp); // 解析核心proto物件

                        // 取得型態欄位物件
                        FieldDescriptor fieldType = protoCore.Descriptor.FindFieldByNumber(FIELD_TYPE);

                        if (fieldType == null)
                            throw new Exception("type not found");

                        // 取得型態列表
                        RepeatedField<int> types = (RepeatedField<int>)fieldType.Accessor.GetValue(protoCore);

                        if (types == null)
                            throw new Exception("get type failed");

                        foreach (int itor in types)
                        {
                            // 取得資料欄位物件
                            FieldDescriptor fieldData = protoCore.Descriptor.FindFieldByNumber(itor);

                            if (fieldData == null)
                                throw new Exception("data not found(" + itor + ")");

                            // 取得封包物件
                            System.Object packet = fieldData.Accessor.GetValue(protoCore);

                            if (packet == null)
                                throw new Exception("get data failed(" + itor + ")");

                            // 取得封包處理委派
                            Handler handler = null;

                            if (receivers.TryGetValue(itor, out handler) == false || handler == null)
                                throw new Exception("receiver not found(" + itor + ")");

                            handler(packet);
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
        /// <param name="objects">封包物件</param>
        /// <returns>資料陣列</returns>
        protected internal override byte[] send(params System.Object[] objects)
        {
            lock (syncLock)
            {
                if (objects == null)
                    throw new Exception("packet null");

                if (objects.Length <= 0)
                    throw new Exception("packet empty");

                TProtoCore protoCore = new TProtoCore();
                FieldDescriptor fieldType = protoCore.Descriptor.FindFieldByNumber(FIELD_TYPE);

                if (fieldType == null)
                    throw new Exception("type not found");

                RepeatedField<int> types = (RepeatedField<int>)fieldType.Accessor.GetValue(protoCore);

                foreach (System.Object itor in objects)
                {
                    if (itor is IMessage == false)
                        throw new Exception("packet not proto");

                    string packetName = itor.GetType().Name;
                    FieldDescriptor fieldData = protoCore.Descriptor.FindFieldByName(packetName);

                    if (fieldData == null)
                        throw new Exception("data not found(" + packetName + ")");

                    types.Add(fieldData.FieldNumber);
                    fieldData.Accessor.SetValue(protoCore, itor);
                }//for

                using (MemoryStream memory = new MemoryStream())
                {
                    CodedOutputStream outputStream = new CodedOutputStream(memory);

                    outputStream.WriteMessage(protoCore);
                    outputStream.Flush();

                    return memory.ToArray();
                }//using
            }//lock
        }
    }
}