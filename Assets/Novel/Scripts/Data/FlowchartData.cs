﻿using UnityEngine;
using Cysharp.Threading.Tasks;
using Novel.Command;

namespace Novel
{
    [CreateAssetMenu(
        fileName = "Flowchart",
        menuName = ConstContainer.DATA_CREATE_PATH + "Flowchart",
        order = 0)
    ]
    public class FlowchartData : ScriptableObject, IFlowchartObject
    {
        [SerializeField] Flowchart flowchart;
        public Flowchart Flowchart => flowchart;
        public string Name => name;

        public void Execute(int index = 0)
        {
            Flowchart.ExecuteAsync(index).Forget();
        }
        public UniTask ExecuteAsync(int index = 0)
        {
            return Flowchart.ExecuteAsync(index);
        }

#if UNITY_EDITOR
        /// <summary>
        /// リストの中に特定のCommandDataがあるか調べます
        /// </summary>
        public bool IsUsed(CommandData targetData)
        {
            foreach(var commandData in Flowchart.GetReadOnlyCommandDataList())
            {
                if (commandData == targetData) return true;
            }
            return false;
        }
#endif
    }
}