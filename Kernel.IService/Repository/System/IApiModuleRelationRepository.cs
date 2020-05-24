using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.IService.Repository.System
{
    public interface IApiModuleRelationRepository
    {
        Task AddApiModuleRelation_V1_0(ApiModuleRelation apiResource);
        Task<ApiModuleRelation> GetApiModuleRelation_V1_0(string modID, string resID);
    }
}
