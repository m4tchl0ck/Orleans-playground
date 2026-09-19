public interface IDirtyStateSimulation : IGrainWithStringKey
{
    Task SetValue(int value);
    Task<int> GetValue();
}