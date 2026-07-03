namespace Reym.Definitions;

public class RouteDefinition
{
    public const string RouteTagName = "route";
    public const string PathAttributeName = "path";

    public string Path { get; set; } = string.Empty;
    public DataDefinition Data { get; set; } = new();
    public ViewDefinition View { get; set; } = new();
    public LogicDefinition Logic { get; set; } = new();

}