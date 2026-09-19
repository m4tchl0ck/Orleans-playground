public interface IPluginGrain : IGrainWithStringKey
{
    Task<PluginDescriptor> Describe();
    Task<string?> Execute(string command, string[] args);
}
