using System.Collections;
using UnityEngine;

namespace AlanZucconi
{
    // Creates a global GameObject, that can be used to start Coroutines
    // Ispired by CoroutineRunner by Lotte
    // https://gist.github.com/LotteMakesStuff/d179d28f29bc9bb499dc5260e0146154
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _S;
        public static CoroutineRunner S
        {
            get
            {
                if (_S == null)
                {
                    // Instantiates a new (hidden) gameObject
                    GameObject gameObject = new GameObject("CoroutineRunner");
                    gameObject.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector;
                    // Adds this MonoBehaviour to it
                    _S = gameObject.AddComponent<CoroutineRunner>();
                }
                return _S;
            }
        }

        // Static version of StartCoroutine
        public static Coroutine Start (IEnumerator routine)
        {
            return S.StartCoroutine(routine);
        }
    }
}