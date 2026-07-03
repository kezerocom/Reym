namespace Reym.Definitions;

public class ValueDefinition
{
    public static string ValueTagName = "value";
    public static string NameAttributeName = "name";
    public static string TypeAttributeName = "type";
    public static string DefaultAttributeName = "default";
    public string? Name { get; set; }
    public string? Type { get; set; }
    public string? Default { get; set; }
}