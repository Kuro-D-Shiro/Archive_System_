namespace Archive_System.Model.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ClassNameInRuAttribute(string name) : Attribute
{
    public string Name { get; } = name;
    public override string ToString() => Name;
}
