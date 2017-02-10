using UnityEngine;
using UnityEngine.UI;

public class ConnectSwitchButton : MonoBehaviour {

    public Text text;
    public UnityProto proto;
    public UnityJson json;
    private bool isProto;

    // Use this for initialization
    void Start () {
        setProto(false);
    }

    public void OnClick()
    {
        isProto = !isProto;
        setProto(isProto);
    }

    private void setProto(bool isProto)
    {
        this.isProto = isProto;
        json.enabled = !isProto;
        proto.enabled = isProto;

        if (isProto)
            proto.Link();
        else
            json.Link();

        text.text = isProto ? "Proto" : "Json";
    }
}
