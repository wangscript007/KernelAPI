
using System.ComponentModel.DataAnnotations;

namespace Kernel.Model.Demo
{
    /// <summary>
    /// 入参模型
    /// </summary>
    public class SysUserInParams : SysUser
    {
        public override int? PwdFailures { get; set; }

        [StringLength(50, ErrorMessage = "userID的长度不能超过50")]
        public string userID { get => UserID; set => UserID = value; }

        public string userName { get; set; }

        [Range(0, 1, ErrorMessage = "isLocked必须在0-1之间")]
        public string isLocked { get => DictIsLocked; set => DictIsLocked = value; }

        public int pageIndex { get; set; }

        public int pageSize { get; set; }
    }

}

