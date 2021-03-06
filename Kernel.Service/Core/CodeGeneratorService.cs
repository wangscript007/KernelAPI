using Kernel.Core.Utils;
using Kernel.IService.Repository.Core;
using Kernel.IService.Service.Core;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Service.Core
{
    public class CodeGeneratorService : ICodeGeneratorService
    {
        public ICodeGeneratorRepository GenerationRepository { get; set; }

        public string Generation(string tableName)
        {
            IEnumerable<ITemplateService> templateServices = ServiceHost.GetScopeServices<ITemplateService>();

            StringBuilder sb = new StringBuilder();
            foreach (var templateService in templateServices)
            {
                var tableSchema = GenerationRepository.GetTableSchema(tableName);
                var fieldSchemas = GenerationRepository.GetFieldSchema(tableName);

                templateService.ForeachFields(fieldSchemas);

                var code = templateService.TableHandle(tableSchema);
                sb.AppendLine(code);

                sb.AppendLine("/**************************分隔线**************************/");
            }
            return sb.ToString();
        }
    }
}
