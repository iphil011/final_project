using UnityEngine;

[System.Serializable]
public struct SmoothFloat {

    public float value;
    public float target;
    private float velocity;
    [Range(0,10)]
    public float smoothTime;

    // Automatic clamping?
    public bool clamp;
    public float min;
    public float max;
    
    public SmoothFloat (float value, float target, float smoothTime)
    {
        this.value = value;
        this.target = target;
        this.velocity = 0;
        this.smoothTime = smoothTime;

        clamp = false;
        min = 0;
        max = 1;
    }

    public float Update ()
    {
        value = Mathf.SmoothDamp(value, target, ref velocity, smoothTime);
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
    //public float GetNormalised ()
    public float value01
    {
        get
        {
            if (max == min)
                return 0.5f;

            return (value - min) / (max - min);
        }

        set
        {
            this.value = Mathf.Clamp01(value) * (max - min) + min;
        }
    }



    // Gets the target, between 0 and 1
    public float target01
    {
        get
        {
            if (max == min)
                return 0.5f;

            return (target - min) / (max - min);
        }

        set
        {
            target = Mathf.Clamp01(value) * (max - min) + min;
        }
    }



    // Convert from SmoothVector3 to Vector3
    public static implicit operator float (SmoothFloat smoothVector)
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
