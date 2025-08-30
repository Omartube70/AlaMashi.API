using DnsClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class ValidationHelper
    {
        public static bool IsPasswordValid(string password)
        {
            // 1. الطول لا يقل عن 8 أحرف
            if (password.Length < 8)
            {
                return false;
            }

            // 2. يحتوي على رقم واحد على الأقل
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            // 3. يحتوي على حرف كبير واحد على الأقل
            if (!password.Any(char.IsUpper))
            {
                return false;
            }

            // 4. يحتوي على حرف صغير واحد على الأقل
            if (!password.Any(char.IsLower))
            {
                return false;
            }

            // 5. يحتوي على رمز خاص واحد على الأقل
            string specialCharacters = @"!@#$%^&*()_+=-`~?><,.";

            if (!password.Any(c => specialCharacters.Contains(c)))
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> IsEmailValidAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;


            var parts = email.Split('@');
            if (parts.Length != 2)
            {
                return false;
            }

            var domain = parts[1];

            try
            {
                var lookup = new LookupClient();
                var result = await lookup.QueryAsync(domain, QueryType.MX);

                if (result.HasError)
                {
                    return false;
                }

                return result.Answers.MxRecords().Any();
            }
            catch (DnsResponseException)
            {
                return false;
            }
        }


        public static bool IsPhoneValid(string phone)
        {
            // The pattern for an 11-digit Egyptian phone number starting with 01
            string pattern = @"^01[0-9]{9}$";

            // Check if the input phone number matches the pattern
            return Regex.IsMatch(phone, pattern);
        }
    }
}
