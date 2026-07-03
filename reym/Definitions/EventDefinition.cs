namespace Reym.Definitions;

public class EventDefinition
{
    public const string EventTagName = "event";
    public const string OnAttributeName = "on";
    public string On { get; set; } = string.Empty;
    public string? Content { get; set; }
}