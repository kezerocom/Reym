namespace Reym.Definitions;

public class RouteDefinition
{
    public string? Path { get; set; }
    public DataDefinition? Data { get; set; }
    public ViewDefinition? Layout { get; set; }
    public LogicDefinition? Logic { get; set; }

}