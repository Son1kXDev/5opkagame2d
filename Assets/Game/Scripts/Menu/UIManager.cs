using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enjine
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        #region BUTTONS
        public void ButtonStart()
        {
            //TODO: Get scene or sceneID from game data file
            SceneLoadManager.Instance.LoadScene(3);
        }

        public void ButtonContinue() { }
        public void ButtonCustomization() { }
        public void ButtonSettings() { }
        public void ButtonExit()
        {
            //TODO: Dialog window for quit confirmation
            Application.Quit();
        }
        #endregion
    }
}
