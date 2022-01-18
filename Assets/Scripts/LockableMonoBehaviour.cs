using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LockableMonoBehaviour : MonoBehaviour
{
    [SerializeField]
    [HideInInspector]
    private protected bool _locked;

    [SerializeField]
    [HideInInspector]
    private protected UnityEvent _lockedEvent = new UnityEvent();

    [SerializeField]
    [HideInInspector]
    private protected UnityEvent _unlockedEvent = new UnityEvent();

    [HideInInspector]
    public UnityEvent LockedEvent => _lockedEvent;

    [HideInInspector]
    public UnityEvent UnlockedEvent => _unlockedEvent;

    public void Lock()
    {
        if (!_locked)
        {
            _locked = true;

            _lockedEvent.Invoke();
        }
    }

    public void Unlock()
    {
        if (_locked)
        {
            _locked = false;

            _unlockedEvent.Invoke();
        }
    }
}
