using System;
using System.Collections;
using NTC.Global.Cache;
using UnityEngine;

public class PlayerCameraController : MonoCache
{
    public static PlayerCameraController Instance => _instance;
    private static PlayerCameraController _instance;

    [SerializeField, StatusIcon] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 0, -10f);
    [SerializeField] float _smoothTime = 0.25f;

    private Vector3 _velocity = Vector3.zero;
    private Vector3? _cashedOffset = null;


    private void Awake()
    {
        if (_instance == null) _instance = this;
        else DestroyImmediate(this.gameObject);

        if (_target == null)
            throw new NullReferenceException("Target reference not set to an instance of an object");
    }

    protected override void Run()
    {
        Vector3 targetPosition = _target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    }

    /// <summary>
    /// Focuses the camera on the selected object for the selected amount of time
    /// </summary>
    /// <param name="focusedObject">The object on which the camera focuses</param>
    /// <param name="focusTime">Time until camera returns to target position</param>
    public void FocusOnObject(Transform focusedObject, float focusTime)
    {
        Vector3 offset = focusedObject.position - _target.position;
        offset.z = -10;
        StartCoroutine(FocusOnObjectProcess(offset, focusTime));
    }

    private IEnumerator FocusOnObjectProcess(Vector3 offset, float focusTime)
    {
        _cashedOffset = _offset;
        _offset = offset;
        Player.Instance.SetInput(false);
        yield return new WaitForSeconds(focusTime);
        Player.Instance.SetInput(true);
        _offset = (Vector3)_cashedOffset;
        _cashedOffset = null;
    }
}
