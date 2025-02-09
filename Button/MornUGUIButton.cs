using System;
using System.Collections.Generic;
using MornFlag;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace MornUGUI
{
    [RequireComponent(typeof(Button))]
    public sealed class MornUGUIButton : MonoBehaviour,
        ISelectHandler,
        IDeselectHandler,
        ISubmitHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler
    {
        [SerializeField] private Button _button;
        [SerializeField] private bool _isNegative;
        [SerializeField] private MornUGUIButtonActiveModule _activeModule;
        [SerializeField] private MornUGUIButtonColorModule _colorModule;
        [SerializeField] private MornUGUIButtonConvertPointerToSelectModule _convertPointerToSelectModule;
        [SerializeField] private MornUGUIButtonSoundModule _soundModule;
        [Inject] private IMornFlagGetter _flagGetter;
        public bool IsInteractable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }
        public bool IsNegative => _isNegative;
        public IMornFlagGetter FlagGetter => _flagGetter;
        public IObservable<Unit>　OnButtonSelected => _button.OnSelectAsObservable().Select(_ => Unit.Default);
        public IObservable<Unit> OnButtonSubmit => _button.OnSubmitAsObservable().Select(_ => Unit.Default);

        private IEnumerable<MornUGUIButtonModuleBase> GetModules()
        {
            yield return _activeModule;
            yield return _colorModule;
            yield return _convertPointerToSelectModule;
            yield return _soundModule;
        }

        private void Execute(Action<MornUGUIButtonModuleBase, MornUGUIButton> action)
        {
            foreach (var module in GetModules())
            {
                action(module, this);
            }
        }

        private void Awake()
        {
            Execute((module, parent) => module.Awake(parent));
        }

        public void OnSelect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSelect(parent));
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnDeselect(parent));
        }

        public void OnSubmit(BaseEventData eventData)
        {
            Execute((module, parent) => module.OnSubmit(parent));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerEnter(parent));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerExit(parent));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Execute((module, parent) => module.OnPointerDown(parent));
        }
    }
}