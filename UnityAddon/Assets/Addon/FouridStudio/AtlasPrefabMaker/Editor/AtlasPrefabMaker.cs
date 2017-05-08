using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FouridStudio
{
    /// <summary>
    /// 圖集Prefab製作類別
    /// 用於UGUI的Sprite上, 預先製作好Sprite的圖集Prefab, 就可以在遊戲啟動後用Unity方式載入
    /// </summary>
    public class AtlasPrefabMaker : EditorWindow
    {
        #region 菜單

        /// <summary>
        /// 製作圖集prefab
        /// </summary>
        [MenuItem("Atlas/PrefabMaker")]
        private static void prefabMaker()
        {
            EditorWindow.GetWindow<AtlasPrefabMaker>().Show();
        }

        #endregion 菜單

        #region 介面

        private void OnGUI()
        {
            extension = EditorGUILayout.TextField("extension:", extension);

            if (GUILayout.Button("choose source directory"))
                sourcePath = EditorUtility.OpenFolderPanel("choose source directory", Application.dataPath, "");

            sourcePath = assetsPath(EditorGUILayout.TextField("sourcePath:", sourcePath));

            if (GUILayout.Button("choose target directory"))
                targetPath = EditorUtility.OpenFolderPanel("choose source directory", Application.dataPath, "");

            targetPath = assetsPath(EditorGUILayout.TextField("targetPath:", targetPath));

            bool isFailed = false;

            if (sourcePath.Length <= 0)
            {
                EditorGUILayout.LabelField("source directory not unity assets directory");
                isFailed = true;
            }//if

            if (targetPath.Length <= 0)
            {
                EditorGUILayout.LabelField("target directory not unity assets directory");
                isFailed = true;
            }//if

            if (isFailed)
                return;

            if (GUILayout.Button("start"))
            {
                foreach (FileInfo itor in new DirectoryInfo(sourcePath).GetFiles(extension, SearchOption.AllDirectories))
                    createPrefab(itor);
            }//if
        }

        #endregion 介面

        /// <summary>
        /// 副檔名
        /// </summary>
        private string extension = "*.png";

        /// <summary>
        /// 來源路徑
        /// </summary>
        private string sourcePath = "";

        /// <summary>
        /// 目標路徑
        /// </summary>
        private string targetPath = "";

        /// <summary>
        /// 取得以Assets為開頭的路徑
        /// </summary>
        /// <param name="path">完整路徑</param>
        /// <returns>包含Assets之後的路徑</returns>
        private string assetsPath(string path)
        {
            try
            {
                return path.Substring(path.IndexOf("Assets")).Replace('\\', '/');
            }//try
            catch (Exception)
            {
                return "";
            }//catch
        }

        /// <summary>
        /// 建立預製物件
        /// </summary>
        /// <param name="file">檔案物件</param>
        private void createPrefab(FileInfo file)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(assetsPath(file.FullName));
            GameObject gameObject = new GameObject(sprite.name);

            gameObject.AddComponent<SpriteRenderer>().sprite = sprite;

            string prefabPath = targetPath;
            List<string> directorys = assetsPath(file.FullName)
                .Replace(sourcePath, "")
                .Replace(file.Name, "")
                .Split(new char[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            directorys.ForEach(itor => prefabPath = Path.Combine(prefabPath, itor));

            if (Directory.Exists(prefabPath) == false)
                Directory.CreateDirectory(prefabPath);

            PrefabUtility.CreatePrefab(Path.Combine(prefabPath, string.Format("{0}.prefab", sprite.name)).Replace('\\', '/'), gameObject);
            GameObject.DestroyImmediate(gameObject);
        }
    }
}