using System;

namespace AlanZucconi.AI.BT
{
    [Serializable]
    public abstract class Decorator : Node
    {
        public Node Node;

        public Decorator (Node node)
        {
            Node = node;
        }
    }

    [Serializable]
    public class Inverter : Decorator
    {
        public Inverter (Node node) : base(node) { }

        // Inverts the status of the node (on completion)
        public override Status Evaluate()
        {
            Status status = Node.Evaluate();
            
            if (status == Status.Failure)
                return Status.Success;
            if (status == Status.Success)
                return Status.Failure;

            //if (status == Status.Running)
            return Status.Running;
        }
    }

    // Executes its node only if the condition evaluates to true
    // This is equivalent to:
    // Sequence node
    //      Condition node
    //      Child node
    // but allows to write expressions in a much simpler way
    [Serializable]
    public class Filter : Decorator
    {
        public Func<bool> Condition;

        public Filter(Func<bool> condition, Node node) : base(node)
        {
            Condition = condition;
        }

        // Inverts the status of the node (on completion)
        public override Status Evaluate()
        {
            // Condition failed
            if (!Condition())
                return Status.Failure;

            return Node.Evaluate();
        }
    }

    // Succeeder always return true
    // These are useful in cases where you want to process a branch of a tree
    // where a failure is expected or anticipated,
    // but you don’t want to abandon processing of a sequence that branch sits on.
    [Serializable]
    public class Succeeder : Decorator
    {
        public Succeeder (Node node) : base(node) { }

        public override Status Evaluate()
        {
            Status status = Node.Evaluate();
            if (status == Status.Running)
                return Status.Running;

            return Status.Success;
        }
    }


    // Repeats a node until it fails
    // It returns "running" while the node return "success".
    [Serializable]
    public class RepeatUntilFail : Decorator
    {
        public RepeatUntilFail(Node node) : base(node) { }

        public override Status Evaluate()
        {
            Status status = Node.Evaluate();
            if (status == Status.Failure)
                return Status.Failure;

            return Status.Running;
        }
    }
}