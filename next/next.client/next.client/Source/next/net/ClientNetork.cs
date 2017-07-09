using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FouridStudio
{
    /// <summary>
    /// 客戶端網路類別
    /// 使用時須要建立繼承了 ClientNetork.BaseHandler 的封包處理類別
    /// 並以此類別建立實體, 設定到 ClientNetork.Handler
    /// 然後呼叫 ClientNetork.Connect() 函式來連線到伺服器
    /// 最後要記得必須要定時呼叫 ClientNetork.Update() 函式讓它持續運作
    ///
    /// 目前有預設的 ClientNetork.BaseHandler 類別可以使用:
    ///     HandlerJson 以json為基礎的封包處理類別
    ///     HandlerProto 以google protobuf為基礎的封包處理類別
    /// </summary>
    public class ClientNetork
    {
        public string ip = ""; // 要連線的位址
        public int port = 0; // 要連線的埠號
        public int readLength = 8192; // 每次從Socket讀取的長度
        public bool tcpNoDelay = true; // 是否關閉Nagle演算法
        public bool dns = false; // 是否要使用Dns
        public BaseHandler handler = null; // 封包處理物件

        private TcpClient client = null;

        /// <summary>
        /// 是否連線中
        /// </summary>
        /// <returns>true表示連線中, false則否</returns>
        public bool isConnect
        {
            get
            {
                try
                {
                    if (client == null)
                        return false;

                    if (client.Connected == false)
                        return false;

                    if (client.Client.Poll(0, SelectMode.SelectRead) == false)
                        return true;

                    return client.Client.Receive(new byte[1], SocketFlags.Peek) != 0;
                }//try
                catch (Exception)
                {
                    return false;
                }//catch
            }
        }

        /// <summary>
        /// 取得SocketID
        /// </summary>
        /// <returns>SocketID</returns>
        public int socketID
        {
            get
            {
                return client != null ? client.Client.Handle.ToInt32() : 0;
            }
        }

        /// <summary>
        /// 連線到伺服器
        /// </summary>
        public void connect()
        {
            disconnect();

            if (handler == null)
                throw new Exception("handler null");

            IPAddress address = null;

            if (dns)
            {
                IPAddress[] addressList = Dns.GetHostAddresses(ip); // 經由dns來取得網路位址

                if (addressList.Length <= 0)
                    throw new Exception("dns failed");

                address = addressList[0];
            }
            else
                address = IPAddress.Parse(ip);

            if (address == null)
                throw new Exception("ip address null");

            client = new TcpClient();

            client.NoDelay = tcpNoDelay; // 設定客戶端
            client.Connect(address, port); // 連線到伺服器
        }

        // 封包處理物件
        /// <summary>
        /// 中止連線
        /// </summary>
        public void disconnect()
        {
            if (client != null)
            {
                client.GetStream().Close();
                client.Close();
                client = null;
            }//if
        }

        /// <summary>
        /// 定時處理
        /// 要持續定期的呼叫這個函式, 這個元件不會自動運作的
        /// </summary>
        public void update()
        {
            if (handler == null)
                throw new Exception("handler null");

            if (isConnect == false)
                return;

            if (client.GetStream().DataAvailable == false)
                return;

            byte[] data = new byte[readLength];
            int length = client.GetStream().Read(data, 0, data.Length); // 取得從網路收到的資料

            if (data == null || data.Length <= 0 || length <= 0)
                return;

            handler.recv(data, length); // 給下一層去處理
        }

        /// <summary>
        /// 傳送資料
        /// </summary>
        /// <param name="objects">資料物件列表</param>
        public void send(params System.Object[] objects)
        {
            if (handler == null)
                throw new Exception("handler null");

            if (isConnect == false)
                throw new Exception("not connect");

            if (objects == null)
                throw new Exception("data null");

            byte[] data = handler.send(objects);
            StreamWriter writer = new StreamWriter(client.GetStream());

            client.GetStream().Write(data, 0, data.Length);
            writer.Flush();
        }
    }
}