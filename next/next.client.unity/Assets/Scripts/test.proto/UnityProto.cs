using next.client;
using UnityEngine;

public class UnityProto : MonoBehaviour
{
    public string ip = "192.168.31.227";
    public int port = 9527;

    private ProtoClient client = new ProtoClient();

    public void Start()
    {
    }

    public void Link()
    {
        client.start(ip, port);
    }

    private void Update()
    {
        client.update();
    }
}