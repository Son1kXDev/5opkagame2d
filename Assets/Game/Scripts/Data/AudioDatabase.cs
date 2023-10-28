using UnityEngine;
using FMODUnity;

public class AudioDatabase : MonoBehaviour
{
    public static AudioDatabase Instance { get; private set; }

    [field: Header("Player")]
    [field: Header("SFX")]
    [field: SerializeField] public EventReference PlayerFootsteps { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        else Instance = this;
    }
}
