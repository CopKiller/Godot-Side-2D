
using System.Text.RegularExpressions;
using static System.String;

namespace Side2D.Models.Validation
{
    public static partial class InputValidator
    {
        public const int MaxNameCaracteres = 20;
        public const int MinNameCaracteres = 3;

        public const int MaxPasswordCaracteres = 20;
        public const int MinPasswordCaracteres = 6;

        public const int MaxEmailCaracteres = 50;
        public const int MinEmailCaracteres = 6;
        
        
        [GeneratedRegex("^[a-zA-Z0-9_]+$")]
        private static partial Regex MyRegex();
        

        public static bool IsValidName(this string name)
        {
            if (IsNullOrEmpty(name) || 
                (name.Length > MaxNameCaracteres) || 
                    (name.Length < MinNameCaracteres))
            {
                return false;
            }

            return MyRegex().IsMatch(name);
        }

        public static bool IsValidPassword(this string pass)
        {
            return !IsNullOrEmpty(pass) && 
                   pass.Length is <= MaxPasswordCaracteres and >= MinPasswordCaracteres;
        }

        public static bool IsValidRetypePassword(this string retypePass, string originalPass)
        {
            return !IsNullOrEmpty(originalPass) && !IsNullOrEmpty(retypePass) && originalPass == retypePass &&
                   originalPass.Length is <= MaxPasswordCaracteres and >= MinPasswordCaracteres;
        }

        public static bool IsValidEmail(this string email)
        {
            if (IsNullOrEmpty(email) || 
                (email.Length > MaxEmailCaracteres) || 
                (email.Length < MinEmailCaracteres))
            {
                return false;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);

                return addr.Address == email && email.Contains('@');
            }
            catch
            {
                return false;
            }
        }

        /*public static bool IsValidBirthDate(this string date)
        {
            if (String.IsNullOrEmpty(date) || date.Length != MaxBirthDateCaracteres)
            {
                return false;
            }

            if (!Regex.IsMatch(date, @"^(\d{2})\/(\d{2})\/(\d{4})$"))
            {
                return false;
            }

            if (!DateTime.TryParse(date, out _))
            {
                return false;
            }

            return true;
        }*/
    }
}
