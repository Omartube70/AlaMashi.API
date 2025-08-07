using System.Text.RegularExpressions;
using System.Linq;


namespace AlaMashi.BLL
{
    public static class ValidationHelper
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

        public static bool IsEmailValid(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"; 
            return Regex.IsMatch(email, pattern);
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