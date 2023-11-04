using UnityEngine;
using FMODUnity;

public class AudioDatabase : MonoBehaviour
{
    public static AudioDatabase Instance { get; private set; }

    [field: Header("Player")]
    [field: Header("SFX")]
    [field: SerializeField] public EventReference PlayerFootstepsWalk { get; private set; }
    [field: SerializeField] public EventReference PlayerFootstepsRun { get; private set; }
    [field: SerializeField] public EventReference PlayerFootstepsCrouch { get; private set; }
    [field: SerializeField] public EventReference PlayerJumpGrass { get; private set; }
    [field: SerializeField] public EventReference PlayerJumpStone { get; private set; }
    [field: SerializeField] public EventReference PlayerJumpWood { get; private set; }



    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        else Instance = this;
    }
}
