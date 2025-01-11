namespace UserController.Model.Exceptions
{
    // UserNotFoundException - исключение не найденного пользователя
    public class UserNotFoundException : ApplicationException
    {
        public UserNotFoundException() : base("user was not found") { }
    }
}
