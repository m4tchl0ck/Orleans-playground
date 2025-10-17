using Microsoft.Extensions.Configuration;

public static class ConfigurationTemplates
{
    public static IConfigurationBuilder ApplyTemplates(this IConfigurationBuilder configurationBuilder)
    {
        var configuration = configurationBuilder.Build();
        const string start = "{{";
        const string end = "}}";

        var newValues = configuration.AsEnumerable(true)
            .Where(section => !string.IsNullOrEmpty(section.Value))
            .Where(section => section.Value.Contains(start) && section.Value.Contains(end))
            .Select(section =>
            {
                string pattern = $@"\{start}(.*?)\{end}";
                var matches = System.Text.RegularExpressions.Regex.Matches(section.Value, pattern);
                var newValue = section.Value;
                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    string valuePath = match.Groups[1].Value;
                    var value = configuration[valuePath];
                    newValue = newValue.Replace(match.Value, value ?? string.Empty);
                }
                return new KeyValuePair<string, string>(
                    section.Key,
                    newValue
                );
            });

        configurationBuilder.AddInMemoryCollection(newValues);

        return configurationBuilder;
    }
}
