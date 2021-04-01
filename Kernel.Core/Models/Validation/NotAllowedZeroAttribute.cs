using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kernel.Core.Models.Validation
{
    /// <summary>
    /// 不允许为0
    /// </summary>
    public class NotAllowedZeroAttribute : RequiredAttribute
    {

        public NotAllowedZeroAttribute()
        {

        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value">当前属性值</param>
        /// <param name="validationContext">上下文</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                var result = Convert.ToDouble(value);
                if (result == 0)
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
            }
            catch
            {
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage(string element) =>
            $"“{element}”不允许为0！";

    }
}
