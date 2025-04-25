using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ExtendUI
{
    public enum CustomAnimType
    {
        [LabelText("无")] None,
        [LabelText("缩放")] Scale,
    }

    [Serializable]
    public class CustomButton : Button
    {
        private enum EnumExButtonState
        {
            /// <summary>空</summary>
            None,

            /// <summary>单击</summary>
            Click,

            /// <summary>长按开始</summary>
            PressBegin,

            /// <summary>长按</summary>
            Press,

            /// <summary>长按结束</summary>
            PressEnd,
        }

        /// <summary>按钮状态</summary>
        private EnumExButtonState _buttonState = EnumExButtonState.None;


        #region CustomEvent

        [SerializeField] private ButtonClickedEvent onLongPress = new ButtonClickedEvent();

        public ButtonClickedEvent OnLongPress
        {
            get => onLongPress;
            set => onLongPress = value;
        }

        [SerializeField] private ButtonClickedEvent onPressBegin = new ButtonClickedEvent();

        public ButtonClickedEvent OnPressBegin
        {
            get => onPressBegin;
            set => onPressBegin = value;
        }

        [SerializeField] private ButtonClickedEvent onPress = new ButtonClickedEvent();

        public ButtonClickedEvent OnPress
        {
            get => onPress;
            set => onPress = value;
        }

        [SerializeField] private ButtonClickedEvent onPressEnd = new ButtonClickedEvent();

        public ButtonClickedEvent OnPressEnd
        {
            get => onPressEnd;
            set => onPressEnd = value;
        }


        private float _pressCacheTime = 0;
        private bool _isLongPressExecuted = false;


        [SerializeField] private float longPressTime = 0.5f;

        public float LongPressTime
        {
            get => longPressTime;
            set => longPressTime = value;
        }


        [SerializeField] private bool doClickAndLongPress = false;

        public bool DoClickAndLongPress
        {
            get => doClickAndLongPress;
            set => doClickAndLongPress = value;
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            _pressCacheTime = 0;
            if (HasPress())
            {
                _isLongPressExecuted = false;
                _buttonState = EnumExButtonState.PressBegin;
            }
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (HasPress())
            {
                _buttonState = EnumExButtonState.PressEnd;
            }
            else
            {
                _buttonState = EnumExButtonState.Click;
            }
        }

        private bool HasPress()
        {
            return OnLongPress.GetPersistentEventCount() > 0 ||
                   OnPressBegin.GetPersistentEventCount() > 0 ||
                   OnPress.GetPersistentEventCount() > 0 ||
                   onPressEnd.GetPersistentEventCount() > 0;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (_buttonState == EnumExButtonState.Click)
            {
                base.OnPointerClick(eventData);
                OnPlaySound();
                OnPlayAnim();
                _buttonState = EnumExButtonState.None;
            }
        }


        private void ResponseButtonState()
        {
            switch (_buttonState)
            {
                case EnumExButtonState.None:
                    break;
                case EnumExButtonState.Click:
                    // onClick.Invoke();
                    // _buttonState = EnumExButtonState.None;
                    break;

                case EnumExButtonState.PressBegin:
                    OnPressBegin?.Invoke();
                    _buttonState = EnumExButtonState.Press;
                    break;
                case EnumExButtonState.Press:
                    _pressCacheTime += Time.deltaTime;
                    if (_isLongPressExecuted == false && _pressCacheTime >= LongPressTime)
                    {
                        _isLongPressExecuted = true;
                        OnLongPress?.Invoke();
                    }

                    break;
                case EnumExButtonState.PressEnd:
                    OnPressEnd?.Invoke();
                    _buttonState = EnumExButtonState.None;
                    if (DoClickAndLongPress || !DoClickAndLongPress && onLongPress.GetPersistentEventCount() > 0 &&
                        !_isLongPressExecuted)
                    {
                        onClick.Invoke();
                        OnPlaySound();
                    }

                    break;

                default:
                    break;
            }
        }

        #endregion

        private void Update()
        {
            ResponseButtonState();
        }


        #region Sound

        [SerializeField] private int soundId = 1;

        public int SoundId
        {
            get => soundId;
            set => soundId = value;
        }

        public void OnPlaySound()
        {
            if (soundId != -1)
            {
                // SoundManager.PlaySound(soundId);
            }
        }

        #endregion

        #region Anim

        public CustomAnimType customAnimType = CustomAnimType.None;
        public GameObject animObject;
        public Ease ease = Ease.OutBack;

        private void OnPlayAnim()
        {
            if (customAnimType == CustomAnimType.None)
            {
                return;
            }

            if (animObject == null)
            {
                animObject = gameObject;
            }

            if (customAnimType == CustomAnimType.Scale)
            {
                OnScaleAnim();
            }
        }

        #region Scale

        public float scaleDuration = 0.1f;
        public float scaleValue = 1.1f;
        public float scaleValueNormal = 1f;

        private void OnScaleAnim()
        {
            transform.localScale = Vector3.one;
            animObject.transform.DOScale(Vector3.one * scaleValue, scaleDuration).SetEase(ease).OnComplete(() =>
            {
                animObject.transform.DOScale(Vector3.one * scaleValueNormal, scaleDuration).SetEase(ease);
            });
        }

        #endregion

        #endregion
    }
}