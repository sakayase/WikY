namespace WikYModels.Exceptions
{
    public class LoginException : Exception
    {
        public LoginException(string? message = null) : base(message)
        {
        }
    }
}
