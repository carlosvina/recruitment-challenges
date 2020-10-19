namespace Refactoring.FraudDetection
{
    public interface INormalizer<T>
    {
        T Normalize(T value);
    }
}