using System;
namespace FriendLoc.Common.Services
{
    public interface IResourceService
    {
        int LoginNameMinLength { get; }
        int LoginNameMaxLength { get; }
        int PasswordMinLength { get; }
        int PasswordMaxLength { get; }
        string UserToken { get; set; }
    }
}
