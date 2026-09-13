namespace RedirectURL.Services;

public interface IRedirectURLService
{
    Task<string> Redirect(string code);
}