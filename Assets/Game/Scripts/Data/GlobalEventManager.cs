using UnityEngine.Events;
using UnityEngine;

namespace Enjine.Data
{
    public class GlobalEventManager : MonoBehaviour
    {
        public static GlobalEventManager Instance { get; private set; }

        #region Events
        public event BoolEvent OnPauseButtonPressed;
        public event BoolEvent OnSettingsButtonPressed;
        public event ActionEvent OnConfirmationPopupCalled;
        public event EmptyEvent OnLanguageChanged;
        #endregion
        #region Delegates
        public delegate void BoolEvent(bool value);
        public delegate void IntStringEvent(int value, string label);
        public delegate void StringEvent(string value);
        public delegate void IntEvent(int value);
        public delegate void ActionEvent(string title, UnityAction confirmAction, UnityAction cancelAction);
        public delegate void EmptyEvent();
        #endregion

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(this);
        }

        public void PauseButton(bool value) => OnPauseButtonPressed?.Invoke(value);
        public void SettingsButton(bool value) => OnSettingsButtonPressed?.Invoke(value);
        public void LanguageChanged() => OnLanguageChanged?.Invoke();
        public void ActivateConfirmationPopup(string displayLabel, UnityAction confirmAction, UnityAction cancelAction)
        => OnConfirmationPopupCalled?.Invoke(displayLabel, confirmAction, cancelAction);

    }
}