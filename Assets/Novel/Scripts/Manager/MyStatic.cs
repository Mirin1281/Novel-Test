﻿using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
#if UNITY_EDITOR
using UnityEditor; // AssetDatabaseを使うために必要
#endif

namespace Novel
{
    public static class MyStatic
    {
        public const float DefaultFadeTime = 0.3f;

        static CancellationTokenSource cts;
        public static CancellationToken TokenOnSceneChange;


        public static void Init()
        {
            SceneManager.activeSceneChanged += (_, _) => ResetToken();
        }

        public static void ResetToken()
        {
            cts?.Cancel();
            cts = new();
            TokenOnSceneChange = cts.Token;
        }


        public static UniTask WaitSeconds(float waitTime, CancellationToken token)
        {
            if (waitTime > 0)
            {
                return UniTask.Delay(TimeSpan.FromSeconds(waitTime),
                    cancellationToken: token == default ? TokenOnSceneChange : token);
            }
            else
            {
                return UniTask.CompletedTask;
            }
        }

        public static UniTask Yield(CancellationToken token)
            => UniTask.Yield(token == default ? TokenOnSceneChange : token);

        public static UniTask WaitFrame(int frame, CancellationToken token)
            => UniTask.DelayFrame(frame, cancellationToken: token == default ? TokenOnSceneChange : token);


        /// <summary>
        /// シーン内のコンポーネントを検索します
        /// </summary>
        /// <typeparam name="T">コンポーネント</typeparam>
        /// <param name="objName">コンポーネントのついたオブジェクトの名前(コンポーネント名と同じなら省略可)</param>
        /// <param name="findInactive">非アクティブも検索する</param>
        /// <param name="callLog">呼ばれた際にログを出す</param>
        /// <returns></returns>
        public static T FindComponent<T>(
            string objName = null, bool findInactive = true, bool callLog = true)
        {
#if UNITY_EDITOR
#else
        callLog = false;
#endif
            var componentName = typeof(T).Name;
            var findName = objName ?? componentName;
            var obj = GameObject.Find(findName);
            if (obj == null && findInactive)
            {
                obj = FindIncludInactive(findName);
            }
            if (obj == null)
            {
                Log(findName + " オブジェクトが見つかりませんでした", true);
                return default;
            }

            var component = obj.GetComponent<T>();
            if (component == null)
            {
                Log(componentName + " コンポーネントが見つかりませんでした", true);
                return default;
            }
            Log(componentName + "をFindしました", false);

            return component;


            void Log(string str, bool isWarning)
            {
                if (callLog == false) return;
                if (isWarning)
                {
                    Debug.LogWarning("<color=red>" + str + "</color>");
                }
                else
                {
                    Debug.Log("<color=lightblue>" + str + "</color>");
                }
            }
        }

        /// <summary>
        /// 非アクティブのも含めてFindできます
        /// </summary>
        static GameObject FindIncludInactive(string targetName)
        {
            var gameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (var gameObjectInHierarchy in gameObjects)
            {
#if UNITY_EDITOR
                //Hierarchy上のものでなければスルー
                if (!AssetDatabase.GetAssetOrScenePath(gameObjectInHierarchy).Contains(".unity"))
                {
                    continue;
                }
#endif
                if (gameObjectInHierarchy.name == targetName)
                {
                    return gameObjectInHierarchy;
                }
            }
            return null;
        }
    }
}