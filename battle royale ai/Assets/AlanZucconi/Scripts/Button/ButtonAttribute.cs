using System;

// Restrict to methods only
[AttributeUsage(AttributeTargets.Method)]
public class ButtonAttribute : Attribute
{
    public bool Editor = false;
}