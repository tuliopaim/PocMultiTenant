namespace PocMultiTenant.Api.Core;

public interface ICurrentUser
{
    int Tenant();
    int Id();
}
