using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.ExceptionLog
{
    // Enum for Random Type
    public enum RandomType
    {
        Alphabet,
        Numeric,
        AlphaNumeric
    }

    public static class ExtensionHelper
    {
        private static readonly string _logFilePath = "exception_log.txt";

        public static string GenerateRandom(this string input, RandomType randomType, int length)
        {
            try
            {
                const string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                const string numbers = "0123456789";
                const string alphaNumeric = alphabets + numbers;

                string characters;
                switch (randomType)
                {
                    case RandomType.Alphabet:
                        characters = alphabets;
                        break;
                    case RandomType.Numeric:
                        characters = numbers;
                        break;
                    case RandomType.AlphaNumeric:
                        characters = alphaNumeric;
                        break;
                    default:
                        throw new ArgumentException("Invalid RandomType");
                }

                Random random = new Random();
                return new string(Enumerable.Repeat(characters, length)
                                            .Select(s => s[random.Next(s.Length)])
                                            .ToArray());
            }
            catch (Exception ex)
            {
                new ExceptionLogFactory(LogType.File, _logFilePath).LogException(ex);
                throw;
            }
        }

        public static string Digitize(this string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input))
                    return string.Empty;

                return new string(input.Where(char.IsDigit).ToArray());
            }
            catch (Exception ex)
            {
                new ExceptionLogFactory(LogType.File, _logFilePath).LogException(ex);
                throw;
            }
        }

        // Convert to Date
        public static DateTime ToDate(this string dateString)
        {
            try
            {
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
                throw new FormatException("Invalid date string format. Expected format: dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                new ExceptionLogFactory(LogType.File, _logFilePath).LogException(ex);
                throw;
            }
        }

        // Convert to Long Date
        public static DateTime ToLongDate(this string dateString)
        {
            try
            {
                if (DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    return date;
                }
                throw new FormatException("Invalid date string format. Expected format: dd/MM/yyyy HH:mm");
            }
            catch (Exception ex)
            {
                new ExceptionLogFactory(LogType.File, _logFilePath).LogException(ex);
                throw;
            }
        }

    }
}
