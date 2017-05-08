using UnityEngine;

namespace FouridStudio
{
    /// <summary>
    /// 資料路徑類別
    /// </summary>
    public class DataRoute
    {
        /// <summary>
        /// StreamAssets路徑
        /// </summary>
        public static string STREAM_ASSETS_PATH
        {
            get
            {
#if UNITY_EDITOR
                return Application.streamingAssetsPath;
#elif UNITY_ANDROID
                return Application.persistentDataPath;
#elif UNITY_IOS
                return Application.persistentDataPath;
#else
                return Application.streamingAssetsPath;
#endif
            }
        }
    }
}