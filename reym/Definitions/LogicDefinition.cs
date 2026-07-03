namespace Reym.Definitions;

public class LogicDefinition
{
    public const string LogicTagName = "logic";
    public List<ActionDefinition> Actions { get; set; } = [];
    public List<EventDefinition> Events { get; set; } = [];
}