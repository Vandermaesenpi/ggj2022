using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public bool IsLocked = false;
    public float lerpSpeed;
    Transform target;
    public Vector4 cameraBounds;

    public void SetTarget(Transform t)
    {
        target = t;
        IsLocked = target != GM.I.player.transform;
    }

    public void FocusPlayer()
    {
        SetTarget(GM.I.player.transform);
    }

    void Update()
    {
        if (target == null) { return; }
        Vector3 targetPos = new Vector3(target.position.x, 0, 0);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * lerpSpeed);
    }

    public Bounds CamBounds()
    {
        return new Bounds(transform.position, new Vector3(cameraBounds.x + cameraBounds.y, cameraBounds.z + cameraBounds.w, 0));
    }
}
