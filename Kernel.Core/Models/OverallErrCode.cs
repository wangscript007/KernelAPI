using System;
using System.Collections.Generic;
using System.Text;

namespace Kernel.Core.Models
{
    public static class OverallErrCode
    {
        /// <summary>
        /// 无异常
        /// </summary>
        public static int ERR_NO = 0;

        /// <summary>
        /// 参数验证不通过
        /// </summary>
        public static int ERR_VER_PAR = 10;

        /// <summary>
        /// 对象异常
        /// </summary>
        public static int ERR_OBJ = 20;

        /// <summary>
        /// //数据库交互异常,
        /// </summary>
        public static int ERR_DB = 30;

        /// <summary>
        /// 三方接口调用异常
        /// </summary>
        public static int ERR_I = 40;

        /// <summary>
        /// 身份认证
        /// </summary>
        public static int ERR_VER_ID = 50;

        /// <summary>
        /// Token无效
        /// </summary>
        public static int ERR_VER_TOKEN_INVALID = 51;

        /// <summary>
        /// Token已过期
        /// </summary>
        public static int ERR_VER_TOKEN_EXPIRED = 52;

        /// <summary>
        /// Token未找到
        /// </summary>
        public static int ERR_VER_TOKEN_NOTFOUND = 51;

        /// <summary>
        /// 执行过程捕获异常
        /// </summary>
        public static int ERR_EXCEPTION = 99;
    }
}
