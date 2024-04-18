namespace WikYModels.Exceptions
{
    public class SignInException : Exception
    {
        public SignInException(string? message = null) : base(message)
        {
        }
    }
}
