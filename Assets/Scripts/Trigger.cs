using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trigger : MonoBehaviour
{
    public UnityEvent onTriggerEnter;

    private void Update()
    {
        if (GM.I.player.transform.position.x > transform.position.x)
        {
            onTriggerEnter?.Invoke();
            this.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position - Vector3.up * 1f, transform.position + Vector3.up * 1f);
    }
}
