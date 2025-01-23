

using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DemoApi.Common.System
{
    public static class SystemFunctions
    {
        public static double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds)
        {
            return degrees + (minutes / 60) + (seconds / 3600);
        }
        public static string GetFullDiaPhuong(string sonha, string diachi, string xa, string huyen, string tinh = "")
        {
            string str = "";
            if (!String.IsNullOrEmpty(sonha))
            {
                str += sonha + " - ";
            }
            if (!String.IsNullOrEmpty(diachi))
            {
                str += diachi + " - ";
            }
            if (!String.IsNullOrEmpty(xa))
            {
                str += xa + " - ";
            }
            if (!String.IsNullOrEmpty(huyen))
            {
                str += huyen;
            }
            if (!String.IsNullOrEmpty(tinh))
            {
                str += " - " + tinh;
            }
            return str;
        }
        public static DateTime ConvertDateToSql(string date)
        {
            try
            {
                string str = "";
                if (date.IndexOf("/") > 0)
                {
                    string[] str_split = date.Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                DateTime date_orc = Convert.ToDateTime(str + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                return date_orc;
            }
            catch
            {
                return DateTime.MinValue;
            }

        }
        public static DateTime ConvertDateToSql(DateTime date)
        {
            try
            {
                string str = "";
                if (date.ToString("dd/MM/yyyy").IndexOf("/") > 0)
                {
                    string[] str_split = date.ToString("dd/MM/yyyy").Split('/');
                    str += str_split[2] + "-" + str_split[1] + "-" + str_split[0];
                }
                DateTime date_orc = Convert.ToDateTime(str + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second);
                return date_orc;
            }
            catch
            {
                return DateTime.MinValue;
            }

        }
        public static string GetDatetimeToVn(DateTime? date)
        {
            if (date != null)
            {
                if (Convert.ToDateTime(date).Year > 0001)
                    return Convert.ToDateTime(date).ToString("dd/MM/yyyy");
                else return String.Empty;
            }
            else return String.Empty;
        }
        public static string GetTimeToVn(DateTime date)
        {
            if (date.Year > 0001)
                return date.ToString("hh:mm dd/MM/yyyy");
            else return String.Empty;
        }
        public static List<T> ToListData<T>(this DataTable dataTable)
        {
            var dataList = new List<T>();
            dataList = JsonConvert.DeserializeObject<List<T>>(ToJsonData(dataTable));
            return dataList;
        }
        public static T ToData<T>(this DataTable dataTable) where T : new()
        {
            var dataList = new T();
            dataList = JsonConvert.DeserializeObject<T>(ToJsonData(dataTable));
            return dataList;
        }
        static string ToJsonData(DataTable dataTable)
        {
            return JsonConvert.SerializeObject(dataTable);
        }
        public static string ConvertDecimalToVnd(decimal value)
        {
            var cul = CultureInfo.GetCultureInfo("vi-VN");

            return Convert.ToDouble(value).ToString("#,### vnđ", cul.NumberFormat);

        }
        public static string ToRoman(int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRoman(number - 1000);
            if (number >= 900) return "CM" + ToRoman(number - 900);
            if (number >= 500) return "D" + ToRoman(number - 500);
            if (number >= 400) return "CD" + ToRoman(number - 400);
            if (number >= 100) return "C" + ToRoman(number - 100);
            if (number >= 90) return "XC" + ToRoman(number - 90);
            if (number >= 50) return "L" + ToRoman(number - 50);
            if (number >= 40) return "XL" + ToRoman(number - 40);
            if (number >= 10) return "X" + ToRoman(number - 10);
            if (number >= 9) return "IX" + ToRoman(number - 9);
            if (number >= 5) return "V" + ToRoman(number - 5);
            if (number >= 4) return "IV" + ToRoman(number - 4);
            if (number >= 1) return "I" + ToRoman(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }
        public static string GetColumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }

        public static string RemoveIllegalCharacters(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }
            text = text.Replace("[", string.Empty);
            text = text.Replace("]", string.Empty);
            text = text.Replace("@", string.Empty);
            text = text.Replace("*", string.Empty);
            text = text.Replace("'", string.Empty);
            text = text.Replace("(", string.Empty);
            text = text.Replace(")", string.Empty);
            text = text.Replace("<", string.Empty);
            text = text.Replace(">", string.Empty);

            return text;
        }
        private static string RemoveDiacritics(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var c in
                normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
        private static string RemoveExtraHyphen(string text)
        {
            if (text.Contains("--"))
            {
                text = text.Replace("--", "-");
                return RemoveExtraHyphen(text);
            }
            return text;
        }
        private static string RemoveUnicodePunctuation(string text)
        {
            var normalized = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in
                normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.InitialQuotePunctuation &&
                                              CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.FinalQuotePunctuation))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
        static string GetNewPrams(string url)
        {
            return !String.IsNullOrEmpty(url) ? Regex.Replace(url, @"~[^a-zA-Z0-9\-\\_\/\.]+~", string.Empty) : "";
        }
        public static bool CheckUrl(string url)
        {
            string news_url = RemoveIllegalCharacters(url);

            if (url != news_url)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static string TrimSpace(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text.Trim();
        }
        public static string EncodeId(this int? id, string key = SystemConstants.AppSettings.Key)
        {
            if (id == null || id == 0) return String.Empty;
            return SystemHashUtil.EncodeID(id.ToString(), key);
        }
        public static int DecodeId(this string id, string key = SystemConstants.AppSettings.Key)
        {
            return Convert.ToInt32(SystemHashUtil.DecodeID(id, key));
        }

        public static bool IsContains(this string text, string key)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (string.IsNullOrEmpty(key)) return false;

            return text.Trim().ToUpper().Contains(key.Trim().ToUpper());
        }
        public static bool IsAny(this string text, string key)
        {
            if (string.IsNullOrEmpty(text)) return false;
            if (string.IsNullOrEmpty(key)) return false;

            return text.Trim().ToUpper() == key.Trim().ToUpper();
        }

        public static bool IsVietnamesePhoneNumber(this string input)
        {
            var regex = new Regex(@"\b(84|0[3|5|7|8|9])([0-9]{8})\b", RegexOptions.Compiled);
            return regex.IsMatch(input);
        }

        public static bool IsValidEmail(this string email)
        {
            var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", RegexOptions.Compiled);
            return regex.IsMatch(email);
        }

        public static string ConvertToTrieuDong(decimal money)
        {
            //decimal newMoney = money / 1000000;

            //return newMoney.ToString("#,##0").Replace(",", ".");

            decimal a = money / 1000000;

            decimal b = money % 1000000;

            decimal c = b / 1000000;

            string s = (a - c).ToString("#,##0");

            if (c > 0)
            {
                s += $".{c.ToString().TrimEnd('0').Substring(2)}";
            }

            return s;

        }

        public static string ConvertToTrieuDongExcel(decimal money)
        {
            //decimal newMoney = money / 1000000;

            //return newMoney.ToString("#,##0");

            decimal a = money / 1000000;

            decimal b = money % 1000000;

            decimal c = b / 1000000;

            string s = (a - c).ToString("#,##0");

            if (c > 0)
            {
                s += $".{c.ToString().TrimEnd('0').Substring(2)}";
            }

            return s;
        }
    }
}
