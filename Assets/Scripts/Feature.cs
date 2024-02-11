using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Feature : MonoBehaviour
{
    protected GameObject _target { get; private set; }

    public void Activate (GameObject target){
        if (_target == null){
            _target = target;
            OnApply();
        }
    }
    public void Deactivate (){
        if (_target != null){
            OnRemove();
            _target = null;
        }
    }
    public void Apply (GameObject target){
        _target = target;
        OnApply();
        _target = null;
    }
    protected abstract void OnApply ();
    protected virtual void OnRemove () {}
}
