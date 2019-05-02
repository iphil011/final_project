using System;

namespace AlanZucconi.AI.BT
{
    [Serializable]
    public abstract class Leaf : Node { }

    // A condition checked in a single frame
    // (cannot return Running)
    [Serializable]
    public class Condition : Leaf
    {
        public Func<bool> Function;

        public Condition (Func<bool> function)
        {
            Function = function;
        }

        public override Status Evaluate()
        {
            return Function()
                ? Status.Success
                : Status.Failure
                ;
        }

        public static implicit operator Condition(Func<bool> function)
        {
            return new Condition(function);
        }

        // Static
        public static Condition True = new Condition(() => true);
        public static Condition False = new Condition(() => false);
    }

    // An action that lasts one frame
    // (cannot return Running)
    [Serializable]
    public class Action : Leaf
    {
        public System.Action Function;

        public Action(System.Action function)
        {
            Function = function;
        }

        public override Status Evaluate()
        {
            Function();
            return Status.Success;
        }

        public static implicit operator Action (System.Action function)
        {
            return new Action(function);
        }

        // Static
        public static Action Nothing = new Action(()=> { });
    }


    // A task that can lasts for multiple frames
    // (musth return a Status to indicate its status)
    [Serializable]
    public class Task : Leaf
    {
        public Func<Status> Function;

        public Task(Func<Status> function)
        {
            Function = function;
        }

        public override Status Evaluate()
        {
            return Function();
        }

        public static implicit operator Task(Func<Status> function)
        {
            return new Task(function);
        }

        // Static
        public static Task Forever = new Task(() => Status.Running);
    }

    // While the predicate is true, we wait
    [Serializable]
    public class WaitWhile : Task
    {
        public WaitWhile(Func<bool> condition)
            : base
            (
                () => condition()
                    ? Status.Running
                    : Status.Success
            )
        {
        }
    }
    // While the predicate is false, we wait
    [Serializable]
    public class WaitUntil : Task
    {
        public WaitUntil (Func<bool> condition)
            : base
            (
                () => condition()
                    ? Status.Success
                    : Status.Running
            )
        {
        }
    }
}