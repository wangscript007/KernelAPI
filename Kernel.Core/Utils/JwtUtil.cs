using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Kernel.Core.Utils
{
    //生成token示例
    //var claims = new[]
    //{
    //    new Claim("UserName", "123"),//用户信息
    //    new Claim("ValidTime", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"))//过期时间
    //};
    //string token = JwtUtil.EncodeToken(claims);

    /// <summary>
    /// jwt在线调试
    /// https://jwt.io/
    /// </summary>
    public static class JwtUtil
    {
        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="securityKey"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static string EncodeToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppsettingsConfig.GetConfigValue("Token:Secret")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }

        /// <summary>
        /// 解析token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static IEnumerable<Claim> DecodeToken(string token)
        {
            var securityToken = new JwtSecurityToken(token);
            return securityToken.Claims;
        }
    }
}
