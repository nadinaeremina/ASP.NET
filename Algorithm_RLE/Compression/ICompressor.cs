namespace JsonAPI.Compression
{
    // интерфейс - контракт по реализации методов,
    // позволяет использовать функциональность без деталей реализации контракта
    public interface ICompressor
    {
        // получение имени алгоритма сжатия
        string AlgorithmName { get; }
        // само сжатие данных
        string Compress(string data);
        // у интерфейса есть свойства и эти свойства надо переопределять
    }
}
