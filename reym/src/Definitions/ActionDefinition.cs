namespace Reym.Definitions;

public class ActionDefinition
{
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, (string Type, string? Default)> Parameters { get; set; } = [];
    public string? Content { get; set; }
}