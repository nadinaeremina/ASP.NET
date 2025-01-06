namespace Algorithm_RLE.Compression
{
    public class InvalidCompressionDataException: ApplicationException
    {
        public InvalidCompressionDataException(string details) : base($"data for compression is invalid: {details}") {}
    }
}
