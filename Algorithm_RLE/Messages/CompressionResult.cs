namespace Algorithm_RLE.Messages
{
    // сообщение результата сжатия
    public class CompressionResult: IMessage
    {
        // имя алгоритма
        public string AlgorithmName { get; set; } = string.Empty;
        // сжатые полученные данные
        public string CompressedData { get; set; } = string.Empty;
        // длина исходных данных
        public int DataLength { get; set; } 
        // длина полученных данных
        public int CompressedDataLength { get => CompressedData.Length; } 
        // коэффициент сжатия
        public double CompressionFactor { get => (double)CompressedDataLength / DataLength * 100; }
        public CompressionResult(string algorithmName, string compressedData, int dataLength)
        {
            AlgorithmName = algorithmName;
            CompressedData = compressedData;    
            DataLength = dataLength;
        }
        public CompressionResult() {}
    }
}
