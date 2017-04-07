using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AtlasPrefabMaker : EditorWindow
{
    #region 菜單

    // 製作圖集prefab
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

    // 副檔名
    private string extension = "*.png";

    // 來源路徑
    private string sourcePath = "";

    // 目標路徑
    private string targetPath = "";

    // 取得以Assets為開頭的路徑
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

    // 建立預製物件
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