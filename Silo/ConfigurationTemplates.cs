using Microsoft.Extensions.Configuration;

public static class ConfigurationTemplates
{
    public static IConfigurationBuilder ApplyTemplates(this IConfigurationBuilder configurationBuilder)
    {
        var configuration = configurationBuilder.Build();
        const string start = "{{";
        const string end = "}}";

        var newValues = configuration.AsEnumerable(true)
            .Where(section => section.Value != null
                && section.Value.Contains(start)
                && section.Value.Contains(end))
            .Select(section =>
            {
                string pattern = $@"\{start}(.*?)\{end}";
                var newValue = section.Value ?? string.Empty;
                var matches = System.Text.RegularExpressions.Regex.Matches(newValue, pattern);
                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    string valuePath = match.Groups[1].Value;
                    var value = configuration[valuePath];
                    newValue = newValue.Replace(match.Value, value ?? string.Empty);
                }
                return new KeyValuePair<string, string?>(
                    section.Key,
                    newValue
                );
            })
            .ToArray();

        _ = configurationBuilder.AddInMemoryCollection(newValues);

        return configurationBuilder;
    }
}
