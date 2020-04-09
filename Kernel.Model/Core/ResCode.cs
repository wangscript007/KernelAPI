using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kernel.Model.Core
{
    public static class ResCode
    {
        /* 成功状态码 */
        public const int SUCCESS = 1;//成功

        /* 参数错误：10001-19999 */
        public const int PARAM_IS_INVALID = 10001;//参数无效
        public const int PARAM_IS_BLANK = 10002;//参数为空
        public const int PARAM_TYPE_BIND_ERROR = 10003;//参数类型错误
        public const int PARAM_NOT_COMPLETE = 10004;//参数缺失

        /* 用户错误：20001-29999 */
        public const int USER_NOT_LOGGED_IN = 20001;//用户未登录
        public const int USER_LOGIN_ERROR = 20002;//账号不存在或密码错误
        public const int USER_ACCOUNT_FORBIDDEN = 20003;//账号已被禁用
        public const int USER_NOT_EXIST = 20004;//用户不存在
        public const int USER_HAS_EXISTED = 20005;//用户已存在
        public const int USER_INFO_EXCEPTION = 20006;//获取用户信息异常

        /* 业务错误：30001-39999 */
        public const int SPECIFIED_QUESTIONED_USER_NOT_EXIST = 30001;//某业务出现问题

        /* 系统错误：40001-49999 */
        public const int SYSTEM_INNER_ERROR = 40001;//系统繁忙，请稍后重试

        /* 数据错误：50001-59999 */
        public const int RESULE_DATA_NONE = 50001;//数据未找到
        public const int DATA_IS_WRONG = 50002;//数据有误
        public const int DATA_ALREADY_EXISTED = 50003;//数据已存在

        /* 接口错误：60001-69999 */
        public const int INTERFACE_INNER_INVOKE_ERROR = 60001;//内部系统接口调用异常
        public const int INTERFACE_OUTTER_INVOKE_ERROR = 60002;//外部系统接口调用异常
        public const int INTERFACE_FORBID_VISIT = 60003;//该接口禁止访问
        public const int INTERFACE_ADDRESS_INVALID = 60004;//接口地址无效
        public const int INTERFACE_REQUEST_TIMEOUT = 60005;//接口请求超时
        public const int INTERFACE_EXCEED_LOAD = 60006;//接口负载过高

        /* 权限错误：70001-79999 */
        public const int PERMISSION_NO_ACCESS = 70001;//无访问权限

    }
}
