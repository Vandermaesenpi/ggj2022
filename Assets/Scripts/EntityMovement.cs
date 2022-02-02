using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }
    public void Move(Vector2 direction, float speedMultiplier = 1f)
    {
        _transform.localPosition += new Vector3(direction.x * 1.5f, direction.y, 0) * Time.deltaTime * speedMultiplier;
    }
}
