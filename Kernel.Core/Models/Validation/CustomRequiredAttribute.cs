using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kernel.Core.Models.Validation
{
    /// <summary>
    /// 必填项校验
    /// </summary>
    public class CustomRequiredAttribute : RequiredAttribute
    {

        public CustomRequiredAttribute()
        {

        }

        public override string FormatErrorMessage(string name)
        {
            return $"“{name}”参数不能为空！";
        }

    }
}
