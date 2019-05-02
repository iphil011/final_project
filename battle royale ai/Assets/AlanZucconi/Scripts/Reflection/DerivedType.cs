using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;

[Serializable]
public class DerivedType
{
    public Type BaseType;
    public Type [] Derived;
    public int Index = 0;

    public DerivedType(Type baseType)
    {
        BaseType = baseType;
        Derived = baseType
            .GetAllDerivedTypes()
            .OrderBy(type => type.FullName)
            .ToArray();
    }

    public T Instantiate <T> (params object [] parameters) where T : class
    {
        if (Derived.Length == 0)
            return null;

        return Activator.CreateInstance(Derived[Index], parameters) as T;
    }
}
