namespace Reym.Definitions;

public class DataDefinition
{
    public List<ValueDefinition> Values { get; set; } = [];
    public List<IncludeDefinition> Includes { get; set; } = [];
}