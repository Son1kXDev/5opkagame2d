using UnityEngine;
using UnityEngine.InputSystem;

public class ResetAllBindings : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;

    public void ResetBindings()
    {
        foreach (InputActionMap map in _inputActions.actionMaps)
            map.RemoveAllBindingOverrides();

        PlayerPrefs.DeleteKey("rebinds");
    }
}
