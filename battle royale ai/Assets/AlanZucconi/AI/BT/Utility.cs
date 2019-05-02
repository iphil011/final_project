using System;

namespace AlanZucconi.AI.BT
{
    // A wrapper that adds a utility value to a node
    // To be used with UtilitySelector
    [Serializable]
    public class UtilityNode : Node
    {
        public Func<float> Utility;
        public Node Node;

        public UtilityNode(Func<float> utility, Node node)
        {
            Utility = utility;
            Node = node;
        }

        public override Status Evaluate()
        {
            return Node.Evaluate();
        }
    }

    // Evaluates the node with the maximum utilty
    [Serializable]
    public class UtilitySelector : Node
    {
        public UtilityNode[] Nodes;

        public UtilitySelector (params UtilityNode [] nodes)
        {
            Nodes = nodes;
        }

        // Selects the node with the maximum utility
        public override Status Evaluate()
        {
            UtilityNode node = Nodes.MaxBy(n => n.Utility());
            return node.Evaluate();
        }
    }
}