using System;
using UnityEngine;
namespace Enjine
{
    [RequireComponent(typeof(Collider2D))]
    public class FocusOnObjectTrigger : MonoBehaviour
    {
        [SerializeField, StatusIcon] private Transform _objectToFocus;
        [SerializeField, StatusIcon(minValue: 0f)] private float _focusTime;

        private void Awake()
        {
            if (_objectToFocus == null) throw new NullReferenceException();
            if (_focusTime <= 0) Debug.LogError("Focus time value can't be less than or equal to zero");

            GetComponent<Collider2D>().isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                PlayerCameraController.Instance.FocusOnObject(_objectToFocus, _focusTime);
                Destroy(this.gameObject);
            }
        }
    }
}
