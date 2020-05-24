using Kernel.Model.System;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface IApiResourceRepository
    {
        Task<bool> HasApiPerm_V1_0(string userID, string navUrl, string resPath);
    }
}