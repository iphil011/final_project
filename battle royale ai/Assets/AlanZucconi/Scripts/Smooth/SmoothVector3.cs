using UnityEngine;

[System.Serializable]
public struct SmoothVector3 {

    public Vector3 value;
    public Vector3 target;
    private Vector3 velocity;
    [Range(0, 10)]
    public float smoothTime;
    
    public SmoothVector3 (Vector3 value, Vector3 target, float smoothTime)
    {
        this.value = value;
        this.target = target;
        this.velocity = Vector3.zero;
        this.smoothTime = smoothTime;
    }

    public Vector3 Update ()
    {
        value = Vector3.SmoothDamp(value, target, ref velocity, smoothTime);
        return value;
    }

    public void ResetTo (Vector3 position)
    {
        value = position;
        target = position;
        velocity = Vector3.zero;
    }

    // Convert from SmoothVector3 to Vector3
    public static implicit operator Vector3 (SmoothVector3 smoothVector)
    {
        return smoothVector.value;
    }

    public static implicit operator Vector4(SmoothVector3 smoothVector)
    {
        return smoothVector.value;
    }
    /*
    public static implicit operator SmoothVector3(Vector3 vector)
    {
        return new SmoothVector3(vector, vector);
    }
    */
}
