namespace JsonAPIEncryption.Encryption
{
    public class InvalidEncodingDataException: ApplicationException
    {
        public InvalidEncodingDataException(string details) : base($"no coding data available: {details}") { }
    }
}
