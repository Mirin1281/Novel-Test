﻿using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Novel
{
    public class MessageBox : MonoBehaviour, IFadable
    {
        [SerializeField] BoxType type;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Image boxImage;
        [SerializeField] Writer writer;
        [SerializeField] MsgBoxInput input;
        [SerializeField] AudioClip nextSE;

        public BoxType Type => type;
        public Writer Writer => writer; 
        public MsgBoxInput Input => input;

        public async UniTask ShowFadeAsync(float time = MyStatic.DefaultFadeTime)
        {
            gameObject.SetActive(true);
            SetAlpha(0f);
            await FadeAlphaAsync(1f, time);
        }

        public async UniTask ClearFadeAsync(float time = MyStatic.DefaultFadeTime)
        {
            await FadeAlphaAsync(0f, time);
            gameObject.SetActive(false);
        }

        void SetAlpha(float a) => canvasGroup.alpha = a;

        async UniTask FadeAlphaAsync(float toAlpha, float time)
        {
            if (time == 0f)
            {
                SetAlpha(toAlpha);
                return;
            }
            var outQuad = new OutQuad(toAlpha, time, canvasGroup.alpha);
            var t = 0f;
            while (t < time)
            {
                SetAlpha(outQuad.Ease(t));
                t += Time.deltaTime;
                await MyStatic.Yield();
            }
            SetAlpha(toAlpha);
        }
    }
}