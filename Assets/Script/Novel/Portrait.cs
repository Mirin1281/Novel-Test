﻿using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Novel
{
    public enum PortraitPositionType
    {
        Left,
        Center,
        Right,
        Custom,
    }

    public class Portrait : MonoBehaviour
    {
        [SerializeField] Image portraitImage;
        [SerializeField] Vector2 leftPosition = new(-400, 0);
        [SerializeField] Vector2 centerPosition = new(0, 0);
        [SerializeField] Vector2 rightPosition = new(400, 0);
        public Image PortraitImage => portraitImage;

        readonly Color hideColor = new Color(0.5f, 0.5f, 0.5f, 0.8f);

        public async UniTask TurnAsync(float time)
        {
            var startScaleX = portraitImage.transform.localScale.x;
            var startScaleY = portraitImage.transform.localScale.y;
            var endScaleX = -startScaleX;

            if (time == 0f)
            {
                SetScaleX(endScaleX);
                return;
            }
            var outQuad = new OutQuad(endScaleX, time, startScaleX);
            var t = 0f;
            while (t < time)
            {
                SetScaleX(outQuad.Ease(t));
                t += Time.deltaTime;
                await MyStatic.Yield();
            }
            SetScaleX(endScaleX);

            void SetScaleX(float x)
            {
                portraitImage.transform.localScale = new Vector3(x, startScaleY);
            }
        }

        public void SetPos(PortraitPositionType posType)
        {
            var pos = posType switch
            {
                PortraitPositionType.Left => leftPosition,
                PortraitPositionType.Center => centerPosition,
                PortraitPositionType.Right => rightPosition,
                _ => throw new System.Exception()
            };
            portraitImage.transform.localPosition = pos;
        }
        public void SetPos(Vector2 pos)
        {
            portraitImage.transform.localPosition = pos;
        }

        public void SetSprite(Sprite sprite)
        {
            portraitImage.sprite = sprite;
        }

        public void HideOn()
        {
            portraitImage.color = hideColor;
        }

        public void HideOff()
        {
            portraitImage.color = Color.white;
        }

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

        void SetAlpha(float a)
        {
            var currentColor = portraitImage.color;
            portraitImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, a);
        }

        async UniTask FadeAlphaAsync(float toAlpha, float time)
        {
            if (time == 0f)
            {
                SetAlpha(toAlpha);
                return;
            }
            var outQuad = new OutQuad(toAlpha, time, portraitImage.color.a);
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