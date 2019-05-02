using System;

namespace AlanZucconi.AI.BT
{
    public enum Status
    {
        Failure,
        Success,
        Running
    }

    [Serializable]
    public abstract class Node
    {
        public abstract Status Evaluate ();
    }

    [Serializable]
    public class BehaviourTree
    {
        public Node Root;

        public BehaviourTree (Node root)
        {
            Root = root;
        }

        public void Update()
        {
            Root.Evaluate();
        }
    }
}