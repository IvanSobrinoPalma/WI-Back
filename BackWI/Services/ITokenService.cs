namespace BackWI.Services
{
    public interface ITokenService
    {
       Guid GetContentByToken(HttpContext httpContext, string variableId);
    }
}