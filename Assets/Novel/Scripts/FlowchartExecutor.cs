﻿using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Novel
{
    public class FlowchartExecutor : MonoBehaviour, IFlowchartObject
    {
        [SerializeField] bool isStartExecute;
        [SerializeField] Flowchart flowchart;
        public Flowchart Flowchart => flowchart;

        public string Name => name;

        void Start()
        {
            if (isStartExecute) ExecuteAsync().Forget();
        }

        public async UniTask ExecuteAsync(int index = 0, FlowchartCallStatus callStatus = null)
        {
            await Flowchart.ExecuteAsync(index, callStatus);
        }
    }

    public interface IFlowchartObject
    {
        public string Name { get; }
        public Flowchart Flowchart { get; }
        public UniTask ExecuteAsync(int index = 0, FlowchartCallStatus callStatus = null);
    }
}