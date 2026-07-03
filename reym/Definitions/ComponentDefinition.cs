namespace Reym.Definitions;

public class ComponentDefinition
{
    public const string ComponentTagName = "component";
    public const string NameAttributeName = "name";
    public string Name { get; set; } = string.Empty;
    public DataDefinition Data { get; set; } = new();
    public ViewDefinition View { get; set; } = new();
    public LogicDefinition Logic { get; set; } = new();

}