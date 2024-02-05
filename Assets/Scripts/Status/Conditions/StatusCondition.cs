using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusCondition : MonoBehaviour
{
    public virtual void Remove ()
    {
        Status status = GetComponentInParent<Status>();
        if (status)
            status.Remove(this);
    }
}