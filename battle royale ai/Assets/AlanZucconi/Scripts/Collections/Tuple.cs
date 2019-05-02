// https://stackoverflow.com/questions/955982/tuples-or-arrays-as-dictionary-keys-in-c-sharp
using System;

[Serializable]
public struct Tuple<T, U> : IEquatable<Tuple<T, U>>
//public class Tuple<T, U> : IEquatable<Tuple<T, U>>
{
    //[SerializeField]
    public T first;
    //[SerializeField]
    public U second;
    //readonly W third;

    public Tuple(T first, U second)
    {
        this.first = first;
        this.second = second;
        //this.third = third;
    }

    public T First { get { return first; } }
    public U Second { get { return second; } }
    //public W Third { get { return third; } }

    public override int GetHashCode()
    {
        return first.GetHashCode() ^ second.GetHashCode();// ^ third.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        //return Equals((Tuple<T, U, W>)obj);
        return Equals((Tuple<T, U>)obj);
    }

    public bool Equals(Tuple<T, U> other)
    {
        //return other.first.Equals(first) && other.second.Equals(second) && other.third.Equals(third);
        return other.first.Equals(first) && other.second.Equals(second);
    }
}