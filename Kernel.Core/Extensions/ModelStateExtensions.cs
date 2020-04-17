using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kernel.Core.Extensions
{
    public static class ModelStateExtensions
    {
        /// <summary>
        /// 获取验证消息提示并格式化提示
        /// </summary>
        public static List<string> GetValidationSummary(this ModelStateDictionary modelState)
        {
            if (modelState.IsValid) return null;

            List<string> errors = new List<string>();

            foreach (var item in modelState)
            {
                var state = item.Value;
                var message = state.Errors.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p.ErrorMessage))?.ErrorMessage;
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = state.Errors.FirstOrDefault(o => o.Exception != null)?.Exception.Message;
                }
                if (string.IsNullOrWhiteSpace(message)) continue;

                errors.Add(message);
            }

            return errors;
        }
    }
}
