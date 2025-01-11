namespace UserController.Model.Service
{
    // IEncoder - интерфейс шифрования строки
    public interface IEncoder
    {
        string Encode(string data);
    }
}
