using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SchemeEditor.Identity.Abstractions;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace SchemeEditor.Identity.Infrastructure
{
	public static class IdentityHelper
	{
		public static string UserName(this System.Security.Principal.IIdentity identity)
		{
			return (identity as ClaimsIdentity)?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
		}

		public static string GetJwtToken(this IApplicationUser user, IConfiguration config)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.NormalizedName)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtKey"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(config["JwtExpireDays"]));

			var token = new JwtSecurityToken(
				config["JwtIssuer"],
				config["JwtIssuer"],
				claims,
				expires: expires,
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public static bool ConfirmTokenIsValid(this IApplicationUser user, IConfiguration config, string receivedToken)
		{
			var token = string.IsNullOrEmpty(receivedToken)
				? null
				: new JwtSecurityToken(receivedToken);

			var templateToken = new JwtSecurityToken(user.GetJwtToken(config));
			return token != null
			       && token.ValidTo >= DateTime.UtcNow
			       && token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value == user.Email
			       && token.EncodedHeader == templateToken.EncodedHeader;
		}

		public static void RefreshToken(this IHeaderDictionary responseHeaders, IApplicationUser user, IConfiguration config, string receivedToken)
		{
			var token = string.IsNullOrEmpty(receivedToken)
				? null
 				: new JwtSecurityToken(receivedToken);

			if (token == null || token.ValidTo >= DateTime.UtcNow.AddHours(2))
				return;

			var newToken = user.GetJwtToken(config);
			responseHeaders.Add("JwtToken", newToken);
		}

		public static string GeneratePassword(this string email, int length = 8)
		{
			const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()?/.,:;-+{}[]";
			var res = new StringBuilder();
			var rnd = new Random(email.GetHashCode());
			while (0 < length--)
			{
				res.Append(valid[rnd.Next(valid.Length)]);
			}
			return res.ToString();
		}
	}
}