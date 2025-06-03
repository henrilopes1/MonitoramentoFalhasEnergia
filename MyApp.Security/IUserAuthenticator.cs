namespace MyApp.Security
{
    public interface IUserAuthenticator
    {
        bool Autenticar(string usuario, string senha);
    }
}
