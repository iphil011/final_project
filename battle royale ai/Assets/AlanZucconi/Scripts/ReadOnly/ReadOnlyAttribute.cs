using UnityEngine;
// http://answers.unity3d.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
// Has to be the last attriute
public class ReadOnlyAttribute : PropertyAttribute
{
    // Unlocked in editor
    public bool Editor = false;
}