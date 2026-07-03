namespace Reym.Definitions;

public class ActionDefinition
{
    public const string ActionTagName = "action";
    public const string NameAttributeName = "name";
    public const string ParametersAttributeName = "parameters";
    public string Name { get; set; } = string.Empty;
    public string? Parameters { get; set; }
    public string? Content { get; set; }
}