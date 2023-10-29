using UnityEngine;

namespace Enjine
{
    public class StartSoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioZone _audioZone;

        private void Start()
        {
            Player.Instance.SetAudioZone(_audioZone);
            Destroy(this);
        }
    }
}
