using UnityEngine;

// 繼承此類別後, 便擁有Singleton設計模式的能力
// 這是給Unity物件的特化版
public class SingletonMono<T> : MonoBehaviour where T : Object, new()
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<T>();

            return instance;
        }
    }
}