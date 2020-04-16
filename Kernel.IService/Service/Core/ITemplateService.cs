using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.IService.Service.Core
{
    public interface ITemplateService
    {
        void ForeachFields(List<Model.Core.FieldSchema> fieldSchemas);
        string TableHandle(Model.Core.TableSchema tableSchema);
    }
}
