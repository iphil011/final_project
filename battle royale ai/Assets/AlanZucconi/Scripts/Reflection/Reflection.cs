using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using System.Reflection;

public static class ReflectionExtension
{
    // https://stackoverflow.com/questions/2362580/discovering-derived-types-using-reflection
    // var listOfDerived = typeof(MyClass).GetAllDerivedTypes();
    public static IEnumerable<Type> GetAllDerivedTypes(this Type type)
    {
        return Assembly.GetAssembly(type).GetAllDerivedTypes(type);
    }

    public static IEnumerable<Type> GetAllDerivedTypes(this Assembly assembly, Type type)
    {
        return assembly
            .GetTypes()
            .Where(t => t != type && type.IsAssignableFrom(t));
    }
}
