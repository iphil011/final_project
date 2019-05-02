using UnityEngine;
using System;

// Enum field must be System.Flags
// Also, if more attributes are present, this must be the last one
[AttributeUsage
    (
        AttributeTargets.Field,
        AllowMultiple = false,
        Inherited = true
    )
]
public class ShowIfAttribute : PropertyAttribute
{
    public string EnumField;
    public object EnumValue;

    public ShowIfAttribute (string enumField, object enumValue)
    {
        EnumField = enumField;
        EnumValue = enumValue;
    }
}