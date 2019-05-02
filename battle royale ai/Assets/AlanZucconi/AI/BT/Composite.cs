using System;
using System.Linq;

namespace AlanZucconi.AI.BT
{
    [Serializable]
    public abstract class Composite : Node
    {
        public bool Random;
        public Node[] _Nodes;

        //public Composite (params Node [] nodes)
        public Composite(bool random, params Node[] nodes)
        {
            Random = random;
            _Nodes = nodes;
        }
        public Composite(params Node[] nodes) : this(false, nodes) { }

        // Gets the nodes either in order or shuffled
        public Node[] Nodes
        {
            get
            {
                if (Random)
                    // https://stackoverflow.com/questions/9361470/random-order-of-an-ienumerable
                    return _Nodes.OrderBy(order => UnityEngine.Random.Range(0f, 1f)).ToArray();
                else
                    return _Nodes;
            }
        }
    }

    [Serializable]
    public class Sequence : Composite
    {
        public Sequence(bool random, params Node[] nodes) : base(random, nodes) { }
        public Sequence(params Node[] nodes) : base(nodes) { }

        // If one node fails,
        // the entire Sequence fails and terminates early
        public override Status Evaluate()
        {
            foreach (Node node in Nodes)
            {
                Status status = node.Evaluate();
                if (status == Status.Failure)
                    return Status.Failure;
                if (status == Status.Running)
                    return Status.Running;
            }

            // All nodes have succeeded
            return Status.Success;
        }
    }

    [Serializable]
    public class Selector : Composite
    {
        public Selector(bool random, params Node[] nodes) : base(random, nodes) { }
        public Selector(params Node[] nodes) : base(nodes) { }

        // If one node succeeds,
        // the entire Selector succeeds and terminates early
        public override Status Evaluate()
        {
            foreach (Node node in Nodes)
            {
                Status status = node.Evaluate();
                if (status == Status.Success)
                    return Status.Success;
                if (status == Status.Running)
                    return Status.Running;
            }

            // All nodes have failed
            return Status.Failure;
        }
    }
}