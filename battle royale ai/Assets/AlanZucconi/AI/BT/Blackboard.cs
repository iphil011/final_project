using System.Collections.Generic;
using System;

namespace AlanZucconi.AI.BT
{
    [Serializable]
    public class Blackboard
    {
        //public Dictionary<string, bool> Bools = new Dictionary<string, bool>();
        //public Dictionary<string, float> Floats = new Dictionary<string, float>();

        public Dictionary<string, object> Variables = new Dictionary<string, object>();

        public void Set<T>(string name, T value)
        {
            Variables[name] = value;
        }
        public T Get<T> (string name, T defaultValue = default(T))
        {
            object value;
            if (!Variables.TryGetValue(name, out value))
                return defaultValue;
            return (T) value;
        }

        // Tasks
        // Assumes "name" is a bool
        public WaitWhile WaitWhile(string name)
        {
            return new WaitWhile(() => Get<bool>(name));
        }
        public WaitUntil WaitUntil(string name)
        {
            return new WaitUntil(() => Get<bool>(name));
        }

        // Filters
        // Checks if the variable "name" has the value "value"
        public Filter Filter<T>(string name, T value, Node node)
        {
            return new Filter
            (
                // https://stackoverflow.com/questions/390900/cant-operator-be-applied-to-generic-types-in-c
                () => EqualityComparer<T>.Default.Equals(Get<T>(name), value),
                node
            );
        }
        public Condition Condition<T>(string name, T value)
        {
            return new Condition
            (
                // https://stackoverflow.com/questions/390900/cant-operator-be-applied-to-generic-types-in-c
                () => EqualityComparer<T>.Default.Equals(Get<T>(name), value)
            );
        }
        /*
        public Condition Condition(string name, bool value)
        {
            return new Condition(() => Get<bool>(name) == value);
        }
        */




        /*
        // Indexer
        public bool this[string name]
        {
            get { return Bools[name]; }
            set { Bools[name] = value; }
        }

        // Tasks
        public WaitWhile WaitWhile (string name)
        {
            return new WaitWhile(() => this[name]);
        }
        public WaitUntil WaitUntil(string name)
        {
            return new WaitUntil(() => this[name]);
        }

        // Filters
        // Checks if the variable "name" has the value "value"
        public Filter Filter (string name, bool value, Node node)
        {
            return new Filter
            (
                () => Bools[name] == value,
                node
            );
        }
        public Condition Condition (string name, bool value)
        {
            return new Condition(() => Bools[name] == value);
        }
        */

        //public Filter True(string name)
        //{
        //    return new Filter(() => this[name]);
        //}
    }
}