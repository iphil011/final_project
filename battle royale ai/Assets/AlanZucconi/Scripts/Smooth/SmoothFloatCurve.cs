using UnityEngine;

[System.Serializable]
public struct SmoothFloatCurve {

    public float value;
    public float target;
    private float velocity;
    [Range(0,10)]
    public float smoothTime;
    public AnimationCurve smoothCurve;

    // Automatic clamping?
    public bool clamp;
    public float min;
    public float max;

    public SmoothFloatCurve (float value, float target, float smoothTime)
    //public SmoothFloatCurve(float value, float target)
    {
        this.value = value;
        this.target = target;
        this.velocity = 0;
        this.smoothTime = smoothTime;
        this.smoothCurve = new AnimationCurve();
        
        clamp = false;
        min = 0;
        max = 1;
    }

    public float Update ()
    {
        value = Mathf.SmoothDamp(value, target, ref velocity, smoothTime * smoothCurve.Evaluate(GetNormalised()));
        if (clamp)
        {
            value = Mathf.Clamp(value, min, max);
            target = Mathf.Clamp(target, min, max); // SHOULD BE MOVED IN SET of target
        }
        return value;
    }

    public void ResetTo (float position)
    {
        value = position;
        target = position;
        velocity = 0;
    }

    // Gets the value between 0 and 1
    public float GetNormalised ()
    {
        if (max == min)
            return 0.5f;

        return (value - min) / (max - min);
    }

    // Convert from SmoothFloat to float
    public static implicit operator float (SmoothFloatCurve smoothFloat)
    {
        return smoothFloat.value;
    }
}
