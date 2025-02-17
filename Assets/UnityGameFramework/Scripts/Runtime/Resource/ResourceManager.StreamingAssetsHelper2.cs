﻿// using System.IO;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityGameFramework.Runtime;
//
// namespace GameFramework.Resource
// {
//     internal partial class ResourceManager
//     {
//         public class StreamingAssetsDefine
//         {
//             public const string RootFolderName = "tao";
//         }
//         public sealed class StreamingAssetsHelper
//         {
//             private static bool _isInit = false;
//             private static readonly Dictionary<string, bool> _cacheData = new Dictionary<string, bool>(1000);
//
// #if UNITY_ANDROID && !UNITY_EDITOR
//             private static AndroidJavaClass _unityPlayerClass;
//
//             public static AndroidJavaClass UnityPlayerClass
//             {
//                 get
//                 {
//                     if (_unityPlayerClass == null)
//                         _unityPlayerClass = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
//                     return _unityPlayerClass;
//                 }
//             }
//
//             private static AndroidJavaObject _currentActivity;
//
//             public static AndroidJavaObject CurrentActivity
//             {
//                 get
//                 {
//                     if (_currentActivity == null)
//                         _currentActivity = UnityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
//                     return _currentActivity;
//                 }
//             }
//             
//             private static AndroidJavaObject _assetManager;
//
//             public static AndroidJavaObject AssetManager
//             {
//                 get
//                 {
//                     if (_assetManager == null)
//                         _assetManager = CurrentActivity.Call<AndroidJavaObject>("GetAssets");;
//                     return _assetManager;
//                 }
//             }
//             
//             /// <summary>
//             /// 利用安卓原生接口查询内置文件是否存在
//             /// </summary>
//             public static bool FileExists(string filePath)
//             {
//                 if (_cacheData.TryGetValue(filePath, out bool result) == false)
//                 {
//                     result = CurrentActivity.Call<bool>("CheckAssetExist", filePath);
//                     _cacheData.Add(filePath, result);
//                 }
//
//                 Log.Warning($"FileExists ? :{filePath} result:{result}");
//
//                 return result;
//             }
//             
//             /// <summary>
//             /// 内置文件查询方法
//             /// </summary>
//             public static bool FileExists(string packageName, string fileName)
//             {
//                 return _cacheData.Contains(fileName);
//             }
// #else
//             public static bool FileExists(string filePath)
//             {
//                 string path = string.Empty;
//
//                 if (_cacheData.TryGetValue(filePath, out bool result) == false)
//                 {
//                     path = System.IO.Path.Combine(Application.streamingAssetsPath, filePath);
//                     result = System.IO.File.Exists(path);
//                     _cacheData.Add(filePath, result);
//                 }
//                 
//                 Log.Warning($"FileExists ? :{path} result:{result}");
//
//                 return result;
//             }
//             
//             /// <summary>
//             /// 内置文件查询方法
//             /// </summary>
//             public static bool FileExists(string packageName, string fileName)
//             {
//                 string filePath = Path.Combine(Application.streamingAssetsPath, StreamingAssetsDefine.RootFolderName, packageName, fileName);
//                 return File.Exists(filePath);
//             }
//             
// #endif
//         }
//     }
// }
//
//
// #if UNITY_ANDROID && UNITY_EDITOR
// /// <summary>
// /// 为Github对开发者的友好，采用自动补充UnityPlayerActivity.java文件的通用姿势满足各个开发者
// /// </summary>
// internal class AndroidPost : UnityEditor.Android.IPostGenerateGradleAndroidProject
// {
//     public int callbackOrder => 99;
//     public void OnPostGenerateGradleAndroidProject(string path)
//     {
//         path = path.Replace("\\", "/");
//         string untityActivityFilePath = $"{path}/src/main/java/com/unity3d/player/UnityPlayerActivity.java";
//         var readContent = System.IO.File.ReadAllLines(untityActivityFilePath);
//         string postContent =
//             "    //auto-gen-function \n" +
//             "    public boolean CheckAssetExist(String filePath) \n" +
//             "    { \n" +
//             "        android.content.res.AssetManager assetManager = getAssets(); \n" +
//             "        try \n" +
//             "        { \n" +
//             "            java.io.InputStream inputStream = assetManager.open(filePath); \n" +
//             "            if (null != inputStream) \n" +
//             "            { \n" +
//             "                 inputStream.close(); \n" +
//             "                 return true; \n" +
//             "            } \n" +
//             "        } \n" +
//             "        catch(java.io.IOException e) \n" +
//             "        { \n" +
//             "            e.printStackTrace(); \n" +
//             "        } \n" +
//             "        return false; \n" +
//             "    } \n" +
//             "}";
//
//         if (CheckFunctionExist(readContent) == false)
//             readContent[readContent.Length - 1] = postContent;
//         System.IO.File.WriteAllLines(untityActivityFilePath, readContent);
//     }
//     private bool CheckFunctionExist(string[] contents)
//     {
//         for (int i = 0; i < contents.Length; i++)
//         {
//             if (contents[i].Contains("CheckAssetExist"))
//             {
//                 return true;
//             }
//         }
//         return false;
//     }
// }
// #endif
//
// /*
// //auto-gen-function
// public boolean CheckAssetExist(String filePath)
// {
// 	android.content.res.AssetManager assetManager = getAssets();
// 	try
// 	{
// 		java.io.InputStream inputStream = assetManager.open(filePath);
// 		if(null != inputStream)
// 		{
// 			inputStream.close();
// 			return true;
// 		}
// 	}
// 	catch(java.io.IOException e)
// 	{
// 		e.printStackTrace();
// 	}
// 	return false;
// }
// */