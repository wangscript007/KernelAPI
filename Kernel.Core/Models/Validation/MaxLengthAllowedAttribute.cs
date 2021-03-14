using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kernel.Core.Models.Validation
{
    /// <summary>
    /// 数值类型长度校验
    /// </summary>
    public class MaxLengthAllowedAttribute : ValidationAttribute
    {
        private int _length;

        /// <summary>
        /// 长度
        /// </summary>
        /// <param name="length"></param>
        public MaxLengthAllowedAttribute(int length)
        {
            _length = length;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value">当前属性值</param>
        /// <param name="validationContext">上下文</param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value.ToString().Length <= _length)
                return ValidationResult.Success;
            else
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage(string element) =>
            $"“{element}”参数长度不能大于{_length}！";

    }
}
