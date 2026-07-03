using HtmlAgilityPack;
using Reym.Definitions;

namespace Reym.Core;

public class Parser
{
    private readonly List<(string content, string metadata)> _contents = [];
    private readonly object _locker = new();

    public void AddRange(IEnumerable<KeyValuePair<string?, string?>>? contents)
    {
        if (contents == null) return;

        lock (_locker)
        {
            foreach (var content in contents)
                Add(content.Key, content.Value);
        }
    }

    public void Add(string? content, string? metadata)
    {
        if (string.IsNullOrWhiteSpace(content)) return;

        lock (_locker)
        {
            _contents.Add((content.Trim(), (metadata ?? string.Empty).Trim()));
        }
    }

    public void Clear()
    {
        lock (_locker)
        {
            _contents.Clear();
        }
    }

    public (List<RouteDefinition> routes, List<ComponentDefinition> components) Compute()
    {
        lock (_locker)
        {
            List<RouteDefinition> routeDefinitions = [];
            List<ComponentDefinition> componentDefinitions = [];

            foreach (var part in _contents)
            {
                var document = new HtmlDocument();
                document.LoadHtml(part.content);

                var rootElements = document.DocumentNode.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element).ToList();

                foreach (var route in rootElements.Where(x => x.Name == RouteDefinition.RouteTagName).ToList())
                {
                    var definition = new RouteDefinition
                    {
                        Path = route.GetAttributeValue(RouteDefinition.PathAttributeName, string.Empty)
                    };

                    routeDefinitions.Add(definition);
                    WriteBody(definition.Data, definition.View, definition.Logic, route);
                }

                foreach (var component in rootElements.Where(x => x.Name == ComponentDefinition.ComponentTagName).ToList())
                {
                    var definition = new ComponentDefinition
                    {
                        Name = component.GetAttributeValue(ComponentDefinition.NameAttributeName, string.Empty)
                    };

                    componentDefinitions.Add(definition);
                    WriteBody(definition.Data, definition.View, definition.Logic, component);
                }
            }

            return (routeDefinitions, componentDefinitions);
        }
    }

    private static void WriteBody(DataDefinition data, ViewDefinition view, LogicDefinition logic, HtmlNode node)
    {
        var childElements = node.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element).ToList();

        var dataHtmlNode = childElements.FirstOrDefault(x => x.Name == DataDefinition.DataTagName);
        var viewHtmlNode = childElements.FirstOrDefault(x => x.Name == ViewDefinition.ViewTagName);
        var logicHtmlNode = childElements.FirstOrDefault(x => x.Name == LogicDefinition.LogicTagName);

        if (dataHtmlNode != null)
            WriteData(data, dataHtmlNode);

        if (viewHtmlNode != null)
            WriteView(view, viewHtmlNode);

        if (logicHtmlNode != null)
            WriteLogic(logic, logicHtmlNode);
    }

    private static void WriteData(DataDefinition data, HtmlNode node)
    {
        var childElements = node.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element).ToList();

        foreach (var includeNode in childElements.Where(x => x.Name == DataDefinition.IncludeTagName))
        {
            data.Includes.Add(new IncludeDefinition
            {
                Source = includeNode.GetAttributeValue(DataDefinition.SourceAttributeName, string.Empty).Trim(),
                As = includeNode.GetAttributeValue(DataDefinition.AsAttributeName, string.Empty).Trim()
            });
        }

        foreach (var valueNode in childElements.Where(x => x.Name == ValueDefinition.ValueTagName))
        {
            data.Values.Add(new ValueDefinition
            {
                Name = valueNode.GetAttributeValue(ValueDefinition.NameAttributeName, string.Empty).Trim(),
                Type = valueNode.GetAttributeValue(ValueDefinition.TypeAttributeName, string.Empty).Trim(),
                Default = valueNode.GetAttributeValue(ValueDefinition.DefaultAttributeName, string.Empty).Trim()
            });
        }
    }

    private static void WriteLogic(LogicDefinition logic, HtmlNode node)
    {
        var childElements = node.ChildNodes.Where(x => x.NodeType == HtmlNodeType.Element).ToList();

        foreach (var actionNode in childElements.Where(x => x.Name == ActionDefinition.ActionTagName))
        {
            logic.Actions.Add(new ActionDefinition
            {
                Name = actionNode.GetAttributeValue(ActionDefinition.NameAttributeName, string.Empty).Trim(),
                Content = actionNode.InnerText.Trim(),
                Parameters = actionNode.GetAttributeValue(ActionDefinition.ParametersAttributeName, string.Empty).Trim()
            });
        }

        foreach (var eventNode in childElements.Where(x => x.Name == EventDefinition.EventTagName))
        {
            logic.Events.Add(new EventDefinition
            {
                On = eventNode.GetAttributeValue(EventDefinition.OnAttributeName, string.Empty).Trim(),
                Content = eventNode.InnerText.Trim()
            });
        }
    }

    private static void WriteView(ViewDefinition view, HtmlNode node)
    {
        foreach (var childNode in node.ChildNodes)
        {
            var element = MapElement(childNode, null);
            if (element != null)
                view.Elements.Add(element);
        }

        return;

        ElementDefinition? MapElement(HtmlNode currentNode, ElementDefinition? parentElement)
        {
            if (currentNode.NodeType == HtmlNodeType.Text)
            {
                if (string.IsNullOrWhiteSpace(currentNode.InnerText)) return null;

                return new ElementDefinition
                {
                    TagName = ElementDefinition.TextTagName,
                    Content = currentNode.InnerText.Trim(),
                    Parent = parentElement
                };
            }

            if (currentNode.NodeType == HtmlNodeType.Element)
            {
                var element = new ElementDefinition
                {
                    TagName = currentNode.Name,
                    Parent = parentElement
                };

                foreach (var attr in currentNode.Attributes)
                    element.Attributes[attr.Name] = attr.Value;

                foreach (var childChildNode in currentNode.ChildNodes)
                {
                    var childElement = MapElement(childChildNode, element);
                    if (childElement != null)
                        element.Children.Add(childElement);
                }

                return element;
            }

            return null;
        }
    }
}