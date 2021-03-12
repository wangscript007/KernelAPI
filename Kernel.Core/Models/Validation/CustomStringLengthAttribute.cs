using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kernel.Core.Models.Validation
{
    /// <summary>
    /// 必填项校验
    /// </summary>
    public class CustomStringLengthAttribute : StringLengthAttribute
    {
        public CustomStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return $"“{name}”长度不能超出{MaximumLength}！";
        }

    }
}
