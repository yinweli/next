using UnityEngine;

// 用來檢查鼠標是否在NGUI的物件上
// 使用時把此腳本掛在NGUI的物件上, 並且該物件必須要有Box Collider元件才能運作
public class NGUIClickChecker : MonoBehaviour
{
    #region 介面事件

    private void OnHover(bool isOver)
    {
        onNGUI = isOver;
    }

    #endregion 介面事件

    private static bool onNGUI = false;

    public static bool OnNGUI
    {
        get
        {
            return onNGUI;
        }
    }
}