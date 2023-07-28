using System.Collections.Generic;

using Godot;

public partial class InkPath : GodotObject
{
    private readonly Ink.Runtime.Path inner;

    public InkPath(Ink.Runtime.Path inner)
    {
        this.inner = inner;
    }

    public InkPath(InkPath otherPath)
    {
        var components = new List<Ink.Runtime.Path.Component>();
        for(var i = 0; i < otherPath.inner.length; i++)
        {
            components.Add(otherPath.inner.GetComponent(i));
        }
        inner = new Ink.Runtime.Path(components);
    }

    public override bool Equals(object obj) =>
        obj is InkPath inkPath &&
        inner.Equals(inkPath.inner);

    public override int GetHashCode() => inner.GetHashCode();

    public override string ToString() => inner.ToString();
}
