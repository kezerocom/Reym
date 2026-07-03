namespace Reym.Definitions;

public class DataDefinition
{
    public const string DataTagName = "data";
    public const string IncludeTagName = "include";
    public const string SourceAttributeName = "source";
    public const string AsAttributeName = "as";
    public List<ValueDefinition> Values { get; set; } = [];
    public List<IncludeDefinition> Includes { get; set; } = [];
}