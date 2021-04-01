using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kernel.Core.Models.Validation
{
    /// <summary>
    /// 必须为正数
    /// </summary>
    public class MustPlusDecimalAttribute : RequiredAttribute
    {

        public MustPlusDecimalAttribute()
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
                var num = Convert.ToDecimal(value);
                if (num < 0)
                    return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
            }
            catch (Exception ex)
            {
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage(string element) =>
            $"“{element}”必须为正数！";

    }
}
