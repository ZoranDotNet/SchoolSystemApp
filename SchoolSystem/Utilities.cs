using System.Globalization;
using System.Text.RegularExpressions;

namespace SchoolSystem
{
    internal static class Utilities
    {
        public static bool ValidateString(string input)
        {
            //only letters and space to prevent sql injection
            string pattern = @"^[a-zA-Z ]+$";
            return Regex.IsMatch(input, pattern);
        }
        public static bool ValidatePersonalNumber(string input)
        {
            //gives format 8digits-4digits(yyyymmdd-xxxx)
            string pattern = @"^\d{8}-\d{4}$";
            return Regex.IsMatch(input, pattern);
        }
        public static DateTime ValidateDateFormat(string input)
        {
            //parse string to DateTime without the time, we later cast it as Date to match datatype in db
            DateTime parsedHired;
            while (true)
            {
                if (DateTime.TryParseExact(input, "yyyy-mm-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedHired))
                {
                    parsedHired = parsedHired.Date;
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid Date Format. Try again. (YYYY-MM-DD");
                }
            }
            return parsedHired;
        }
        public static bool ValidateEmail(string input)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(input, pattern);
        }
    }
}
