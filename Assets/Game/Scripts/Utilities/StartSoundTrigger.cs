using UnityEngine;

namespace Enjine
{
    public class StartSoundTrigger : MonoBehaviour
    {
        [SerializeField] private AudioZone _audioZone;

        private void Awake()
        {
            foreach (Transform child in transform)
            {
                if (child.name.ToLower().Contains(_audioZone.ToString().ToLower()))
                    continue;

                child.gameObject.SetActive(false);
            }

            transform.parent = null;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Player"))
                Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                Destroy(this.gameObject);
        }
    }
}
