using Newtonsoft.Json;

namespace next.template
{
    public class SettingGlobal
    {
        /// <summary>
        /// 來源資料路徑
        /// </summary>
        public string sourcePath = "";

        /// <summary>
        /// 目標輸出路徑
        /// </summary>
        public string targetPath = "";

        /// <summary>
        /// 由json字串反序列化為SettingGlobal物件
        /// </summary>
        /// <param name="json">json字串</param>
        /// <returns>SettingGlobal物件</returns>
        public static SettingGlobal deserialize(string json)
        {
            SettingGlobal settingGlobal = JsonConvert.DeserializeObject<SettingGlobal>(json);

            if (settingGlobal == null)
            {
                Output.outputError("setting global read failed");
                Output.outputError("> " + (json != null ? json : ""));

                return null;
            }//if

            return settingGlobal;
        }
    }

    public class SettingDetail
    {
        /// <summary>
        /// 來源Excel檔名
        /// </summary>
        public string sourceXls = "";

        /// <summary>
        /// 來源Excel表單名稱
        /// </summary>
        public string sourceSheet = "";

        /// <summary>
        /// 目標資料庫名稱
        /// </summary>
        public string targetDatabase = "";

        /// <summary>
        /// 目標資料表名稱
        /// </summary>
        public string targetTable = "";

        /// <summary>
        /// 由json字串反序列化為SettingDetail物件
        /// </summary>
        /// <param name="json">json字串</param>
        /// <returns>SettingDetail物件</returns>
        public static SettingDetail deserialize(string json)
        {
            SettingDetail settingDetail = JsonConvert.DeserializeObject<SettingDetail>(json);

            if (settingDetail == null)
            {
                Output.outputError("setting detail read failed");
                Output.outputError("> " + (json != null ? json : ""));

                return null;
            }//if

            if (settingDetail.sourceXls.Length <= 0 ||
                settingDetail.sourceSheet.Length <= 0 ||
                settingDetail.targetDatabase.Length <= 0 ||
                settingDetail.targetTable.Length <= 0)
            {
                Output.outputError("setting detail check failed");
                Output.outputError("> " + (json != null ? json : ""));

                return null;
            }//if

            return settingDetail;
        }

        public override string ToString()
        {
            return string.Format("{0}#{1}", sourceXls, sourceSheet);
        }
    }
}