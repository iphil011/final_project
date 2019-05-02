using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace AlanZucconi
{
    // https://docs.unity3d.com/ScriptReference/Events.UnityEvent_1.html
    [Serializable]
    public class UnityEventVector2 : UnityEvent<Vector2> { }

    [Serializable]
    public class UnityEventFloat : UnityEvent<float> { }

    // Allows to have Invoke with a return type
    // To be used like this:
    /*
     * public class UnityEventFloatFloat : UnityEventFunc<float, float> {}
     * 
     * public UnityEventFloatFloat F;
     * 
     * float f = F.Invoke(10f);
     * 
     * The delegate must match this sintax:
     * public void Method (float t0, Wrapper<float>)
     * and store the results in r.
     */
    public class Wrapper<T> where T : struct
    {
        public T Value;
        public Wrapper ()
        {
            Value = default(T);
        }
        public Wrapper (T t)
        {
            Value = t;
        }
        // Conversion to T
        public static implicit operator T (Wrapper<T> t)
        {
            return t.Value;
        }
        /*
        public static implicit operator Wrapper<T>(T t)
        {
            return new Wrapper<T>(t);
        }
        */
    }

    [Serializable]
    public abstract class UnityEventFunc<T0, R> : UnityEvent<T0, Wrapper<R>>
    where R : struct
    {
        public R InvokeReturn (T0 t0)
        {
            Wrapper<R> r = new Wrapper<R>();
            Invoke(t0, r);
            return r;
        }
    }

    [Serializable]
    public class UnityEventFloatFloat : UnityEventFunc<float, float> { }
}