using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kernel.Core.Models.Validation
{
    /// <summary>
    /// 必须为数字
    /// </summary>
    public class MustDecimalAttribute : RequiredAttribute
    {

        public MustDecimalAttribute()
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
                Convert.ToDecimal(value);
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
            $"“{element}”必须为数字！";

    }
}
