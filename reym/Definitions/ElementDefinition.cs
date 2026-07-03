namespace Reym.Definitions;

public class ElementDefinition
{
    public const string TextTagName = "#text";
    public string TagName { get; set; } = string.Empty;
    public string? Content { get; set; }
    public Dictionary<string, string> Attributes { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    public ElementDefinition? Parent { get; set; }
    public List<ElementDefinition> Children { get; set; } = [];
}