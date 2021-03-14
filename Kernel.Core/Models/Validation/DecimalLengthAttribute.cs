using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kernel.Core.Models.Validation
{
    /// <summary>
    /// 小数位数长度校验
    /// </summary>
    public class DecimalLengthAttribute : ValidationAttribute
    {
        private int _length;

        /// <summary>
        /// 长度
        /// </summary>
        /// <param name="length"></param>
        public DecimalLengthAttribute(int length)
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
            if (value == null)
                return ValidationResult.Success;

            var arr = new string[] { };
            try
            {
                arr = Convert.ToDecimal(value).ToString().Split('.');
            }
            catch (Exception)
            {
                arr = value.ToString().Split('.');
            }

            if (arr.Length == 1)
                return ValidationResult.Success;
            if (arr.Length == 2 && arr[1].Length <= _length)
                return ValidationResult.Success;
            else
                return new ValidationResult(GetErrorMessage(validationContext.DisplayName));
        }

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <returns></returns>
        public string GetErrorMessage(string element) =>
            $"“{element}”参数小数位数不能大于{_length}！";

    }
}
