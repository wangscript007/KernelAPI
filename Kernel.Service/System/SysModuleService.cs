using Kernel.Core.Utils;
using Kernel.IService.Repository.System;
using Kernel.IService.Service.System;
using Kernel.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Service.System
{
    public class SysModuleService : ISysModuleService
    {
        public ISysModuleRepository SysModuleRepository { get; set; }

        public async Task<SysModuleInit> GetSysModuleInit()
        {
            var result = new SysModuleInit();
            var homeInfo = await SysModuleRepository.GetSysModuleList_V1_0<SysModuleHomeInfo>("home");
            result.HomeInfo = homeInfo.FirstOrDefault();

            var logoInfo = await SysModuleRepository.GetSysModuleList_V1_0<SysModuleLogoInfo>("logo");
            result.LogoInfo = logoInfo.FirstOrDefault();

            var menu = await SysModuleRepository.GetSysModuleList_V1_0<SysModuleMenuInfo>("menu");
            result.MenuInfo = GetMenuList(menu, "-1");

            return result;
        }

        private IEnumerable<SysModuleMenuInfo> GetMenuList(IEnumerable<SysModuleMenuInfo> allMenu, string pid)
        {
            var result = allMenu.Where(o => o.ModPID == pid);
            foreach (var menu in result)
            {
                menu.Child = GetMenuList(allMenu, menu.ModID);
            }

            return result;
        }

        public async Task<IEnumerable<SysPermTree>> GetPermTree(string roleID)
        {
            ISysFuncPermRepository permRep = ServiceHost.GetScopeService<ISysFuncPermRepository>();
            var funcPerms = await permRep.GetSysFuncPermList_V1_0(roleID);

            var result = await SysModuleRepository.GetMenuPermList_V1_0<SysPermTree>(roleID, "menu", "home");
            foreach (var item in result)
            {
                item.FuncPerms = funcPerms.Where(o => o.ModID == item.ModID);
            }
            return result;
        }

    }
}
