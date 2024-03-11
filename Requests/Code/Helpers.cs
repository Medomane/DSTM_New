using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Linq;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using DSTM.Models;

namespace DSTM.Code
{
    public class MyFunc
    {
        public static void CheckData(HttpRequest request, HttpResponse response)
        {
            var acc = request.Url.AbsolutePath.Has("Account");
            if (!AppUser.IsAuthenticated() && !acc) response.Redirect("/Account/SignIn.aspx", false);
            else if(AppUser.IsAuthenticated() && acc) response.Redirect("/Default.aspx", false);
        }
    }


    public static class _session
    {
        public static void Set(string key, object value) => HttpContext.Current.Session[key] = value;
        public static object Get(string key) => HttpContext.Current.Session[key];
        public static void Assert(string key, object value)
        {
            if (Get(key) == null) Set(key, value);
        }
        public static bool Exists(string key) => Get(key) != null;
        public static void Avoid(string key) => Set(key, null);
        public static void Destroy()
        {
            HttpContext.Current.Session.RemoveAll();
            var myCookies = HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var cookie in myCookies)
            {
                if (HttpContext.Current.Response.Cookies[cookie] != null) HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }
        public static void SetCookie(string key, string value, double interval = 10)
        {
            var c = new HttpCookie(key)
            {
                Value = value,
                Expires = DateTime.Now.AddDays(interval)
            };
            HttpContext.Current.Response.Cookies.Add(c);
        }
        public static string GetCookie(string key) => HttpContext.Current.Request.Cookies[key] != null && HttpContext.Current.Request.Cookies[key].Value != null ? HttpContext.Current.Request.Cookies.Get(key)?.Value : null;
    }

    public static class _app
    {
        public static void Init(Assembly assembly)
        {
            _session.Set("CurrentAssembly", FileVersionInfo.GetVersionInfo(assembly.Location));
            _session.Set("currentDirectoryPath", Path.GetDirectoryName(assembly.Location));
        }

        public static FileVersionInfo Data => _session.Get("CurrentAssembly") as FileVersionInfo;
        public static string Version => Data.ProductVersion;
        public static string Name => Data.ProductName;
        public static string Company => Data.CompanyName;
        public static string CurrentDirectoryPath => _session.Get("currentDirectoryPath")?.ToString();
        public static string FullName => $"{Company}#{Name}#{Version.Replace(".", "_")}";
    }

    public static class _file
    {
        #region Files

        public static string ToFile(this byte[] byteArray, string path)
        {
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
            return path;
        }
        public static byte[] ToBytes(this Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0) ms.Write(buffer, 0, read);
                return ms.ToArray();
            }
        }
        public static byte[] ToBytes(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)) return fs.ToBytes();
        }

        public static byte[] ClassFile(string path) => !File.Exists(path) ? null : ToBytes(path);
        public static string ClassFile(this byte[] bytes, string extension = "xml") => bytes.IsNull() || bytes.Length <= 0 ? null : bytes.ToFile(Temp(extension));

        public static bool IsValidImage(this Stream stream)
        {
            try { using (Image.FromStream(stream)) { return true; } }
            catch (Exception) { return false; }
        }

        #endregion

        #region Folders

        public static string CreateFolder(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path.TrimEnd('\\').TrimEnd('/');
        }
        public static void DeleteFolder(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true);
        }

        #endregion

        #region Others

        public static string GetAbsolutePathIfExitsOf(string path)
        {
            try
            {
                var tmp = GetAbsolutePathOf(path);
                return File.Exists(tmp) || Directory.Exists(tmp) ? tmp : null;
            }
            catch (Exception) { return null; }
        }
        public static string GetAbsolutePathOf(string path) => new System.Web.UI.Page().Server.MapPath(path);
        public static Stream GetStream(string handle) => new MemoryStream(Encoding.GetEncoding(1252).GetBytes(handle));

        public static string RootFolder() => AppFolder();
        public static string AppFolder() => GetAbsolutePathOf("~\\Content");
        public static string TempFolder() => CreateFolder(AppFolder() + "\\Temp");
        public static string Temp(string extension = "pdf") => $"{TempFolder()}\\{_str.UniqueId()}.{extension}";
        public static string Log() => AppFolder() + $"\\{_app.Name}Log.txt";
        public static void EmptyTempFolder()
        {
            var di = new DirectoryInfo(TempFolder());
            foreach (var file in di.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        #endregion
    }

    public static class _str
    {
        public static int DefaultNumberValue { get; set; }

        #region Convertion to number

        public static string ToDecimalFormat(this object val) => val.ToDecimal().ToString("n");
        public static decimal ToDecimal(this object val) => decimal.TryParse(val.GetNumericFormat().Item1, out var res) ? res : DefaultNumberValue;
        public static int ToInt(this object val) => int.TryParse(val.GetNumericFormat().Item1, out var res) ? res : DefaultNumberValue;
        public static long ToLong(this object val) => long.TryParse(val.GetNumericFormat().Item1, out var res) ? res : DefaultNumberValue;
        public static double ToDouble(this object val) => double.TryParse(val.GetNumericFormat().Item1, out var res) ? res : DefaultNumberValue;

        #endregion

        #region Checks

        public static bool IsNull(this string str)
        {
            if (str == null) return true;
            str = str.Trim();
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) return true;
            return str == "" || str.Equals("NULL", StringComparison.OrdinalIgnoreCase);
        }
        public static bool IsNotNull(this string str) => !str.IsNull();
        public static bool IsNull(this object obj)
        {
            switch (obj)
            {
                case null:
                    return true;
                case DateTime _ when !obj.IsDate():
                    return true;
                default:
                    return obj.ToString().IsNull();
            }
        }

        public static bool IsNotNull(this object obj) => !obj.IsNull();

        public static bool IsNumeric(this object obj) => GetNumericFormat(obj).Item2;

        public static (string, bool) GetNumericFormat(this object obj)
        {
            try
            {
                if (obj != null)
                {
                    var str = obj.ToString().Trim();
                    if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
                    {
                        if (decimal.TryParse(str, out _) || int.TryParse(str, out _) || double.TryParse(str, out _)) return (str, true);
                        str = str.LastIndexOf(',') > str.LastIndexOf('.') ? str.Replace(".", "").Replace(",", ".") : str.Replace(",", "");
                        str = str.Trim('.').Trim(',').ReplaceWhitespace("");
                        if (decimal.TryParse(str, out _) || int.TryParse(str, out _) || double.TryParse(str, out _)) return (str, true);
                        str = str.Replace(".", ",");
                        if (decimal.TryParse(str, out _) || int.TryParse(str, out _) || double.TryParse(str, out _)) return (str, true);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return (DefaultNumberValue.ToString(), false);
        }

        public static bool Has(this object str, string handle) => str != null && handle != null && str.ToString().IndexOf(handle, StringComparison.OrdinalIgnoreCase) != -1 && str.ToString().ToLower().Contains(handle.ToLower());
        public static bool Is(this object str, string handle) => str != null && handle != null && str.ToString().Equals(handle, StringComparison.OrdinalIgnoreCase);

        #endregion

        #region Others

        public static string UniqueId() => Guid.NewGuid().ToString("N");
        public static string RemoveDiacritics(this string text, bool andSpaces = false)
        {
            if (andSpaces) text = text.ToLower().Replace(' ', '_');
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in from c in normalizedString let unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c) where unicodeCategory != UnicodeCategory.NonSpacingMark select c) stringBuilder.Append(c);
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public static string FirstCharToUpper(this string text) => text[0].ToString().ToUpper() + text.Substring(1);
        public static string FirstCharToLower(this string text) => text[0].ToString().ToLower() + text.Substring(1);

        public static string ReplaceWhitespace(this string input, string replacement) => new Regex(@"\s+").Replace(input, replacement);

        public static string Resolve(this string val) => val.IsNull() ? null : "/Content" + val.Split(new[] { "Content" }, StringSplitOptions.None)[1].Replace("\\", "/");

        #endregion
    }

    public static class _date
    {
        public static DateTime DefaultDateValue { get; set; } = new DateTime(1900, 1, 1);

        public static bool IsDate(this object date)
        {
            if (date == null) return false;
            return DateTime.TryParse(date.ToString(), CultureInfo.CurrentCulture, DateTimeStyles.None, out var dt) &&
                   DateTime.Compare(Convert.ToDateTime(dt, CultureInfo.CurrentCulture), DefaultDateValue) > 0;
        }

        public static DateTime ToDate(this object handle) => handle.IsDate() ? Convert.ToDateTime(handle, CultureInfo.CurrentCulture) : DefaultDateValue;
        public static int UnixTimestamp() => (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        public static string Now(string type = "dd/MM/yyyy HH:mm:ss") => DateTime.Now.Format(type);
        public static string Format(this DateTime dt, string format = "dd/MM/yyyy") => dt.ToString(format);
        public static List<string> WeekDays()
        {
            var culture = CultureInfo.CurrentCulture;
            return new List<string>
            {
                culture.DateTimeFormat.GetDayName(DayOfWeek.Monday),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Tuesday),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Wednesday),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Thursday),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Friday),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Saturday),
                culture.DateTimeFormat.GetDayName(DayOfWeek.Sunday)
            };
        }
        public static int WeekNumber(this DateTime date)
        {
            var day = CultureInfo.CurrentCulture.Calendar.GetDayOfWeek(date);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) date = date.AddDays(3);
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            var jan1 = new DateTime(year, 1, 1);
            var daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;
            var firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            var firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var weekNum = weekOfYear;
            if (firstWeek == 1) weekNum -= 1;
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
        public static DateTime FirstDateOfWeek(int weekOfYear) => FirstDateOfWeek(DateTime.Now.Year, weekOfYear);

        public static List<string> YearMonths()
        {
            var culture = CultureInfo.CurrentCulture;
            var list = new List<string>();
            for (var i = 1; i <= 12; i++) list.Add(culture.DateTimeFormat.GetMonthName(i));
            return list;
        }
        public static DateTime FirstDayOfWeek(this DateTime date)
        {
            DateTime dateTime;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    {
                        dateTime = date.AddDays(-6);
                        break;
                    }
                case DayOfWeek.Monday:
                    {
                        dateTime = date;
                        break;
                    }
                case DayOfWeek.Tuesday:
                    {
                        dateTime = date.AddDays(-1);
                        break;
                    }
                case DayOfWeek.Wednesday:
                    {
                        dateTime = date.AddDays(-2);
                        break;
                    }
                case DayOfWeek.Thursday:
                    {
                        dateTime = date.AddDays(-3);
                        break;
                    }
                case DayOfWeek.Friday:
                    {
                        dateTime = date.AddDays(-4);
                        break;
                    }
                case DayOfWeek.Saturday:
                    {
                        dateTime = date.AddDays(-5);
                        break;
                    }
                default:
                    {
                        dateTime = date;
                        break;
                    }
            }
            return dateTime.Date;
        }
        public static DateTime FirstDayOfWeek(string week)
        {
            DateTime dateTime;
            var minValue = DateTime.MinValue;
            if (int.TryParse(week, out var num) && num > 200000 & num < 203053 && int.TryParse(num.ToString().Substring(0, 4), out var num1) && int.TryParse(num.ToString().Substring(4, 2), out var num2) && num2 <= 53 & num2 > 0)
            {
                var dateTime1 = new DateTime(num1, 1, 4);
                int num3 = checked((checked(num2 - 1)) * 7), dayOfWeek;
                if (dateTime1.DayOfWeek == DayOfWeek.Sunday) dayOfWeek = 6;
                else dayOfWeek = checked((int)dateTime1.DayOfWeek - (int)DayOfWeek.Monday);
                dateTime = dateTime1.AddDays(checked(num3 - dayOfWeek));
                return dateTime;
            }
            dateTime = minValue;
            return dateTime;
        }
        public static string Hour(long second)
        {
            string empty;
            if (second != 0)
            {
                var num = checked((long)Math.Floor((double)second / 3600));
                var num1 = checked((long)Math.Floor((double)(checked(second - checked(num * 3600))) / 60));
                var num2 = Convert.ToInt64(Math.Floor(new decimal(checked(checked(second - checked(num * 3600)) - checked(num1 * 60)))));
                empty = string.Concat(Convert.ToString(num), "h ", Convert.ToString(num1), "min ", Convert.ToString(num2), "s");
            }
            else empty = "0h 0min 0s";
            return empty;
        }
        public static string TimeAgo(this DateTime dateTime)
        {
            string result;
            var timeSpan = DateTime.Now.Subtract(dateTime);
            if (timeSpan <= TimeSpan.FromSeconds(60)) result = $"Il y a {timeSpan.Seconds} seconde" + (timeSpan.Seconds > 1 ? "s" : "");
            else if (timeSpan <= TimeSpan.FromMinutes(60)) result = timeSpan.Minutes > 1 ? $"Il y a  {timeSpan.Minutes} minutes" : "Il y a une minute";
            else if (timeSpan <= TimeSpan.FromHours(24)) result = timeSpan.Hours > 1 ? $"Il y a {timeSpan.Hours} heures" : "Il y une heure";
            else if (timeSpan <= TimeSpan.FromDays(30)) result = timeSpan.Days > 1 ? $"Il y a {timeSpan.Days} jours" : "Hier";
            else if (timeSpan <= TimeSpan.FromDays(365)) result = timeSpan.Days > 30 ? $"Il y a {timeSpan.Days / 30} mois" : "Il y a un mois";
            else result = timeSpan.Days > 365 ? $"Il y a  {timeSpan.Days / 365} années" : "Il y a une année";
            return result;
        }
    }

    public static class _db
    {
        public static int QueryTimeout { get; set; } = 1800;
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"]?.ConnectionString;

        #region SqlManipulation

        public static DataTable Query(string query)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(query, con)
                {
                    CommandTimeout = QueryTimeout
                };
                var tbl = new DataTable("DataTable");
                tbl.Load(cmd.ExecuteReader());
                con.Close();
                return tbl;
            }
        }

        public static DataTable From(string table, string fields) => Query("SELECT " + fields + " FROM " + table);
        public static DataTable Where(string conditions, string table, string fields = "*") => Query("SELECT " + fields + " FROM " + table + " WHERE " + conditions);
        public static object Exec(string query)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var isInsert = query.ToLower().Trim().IndexOf("insert", StringComparison.Ordinal) == 0;
                if (isInsert) query += ";SELECT SCOPE_IDENTITY();";
                var cmd = new SqlCommand(query, con)
                {
                    CommandTimeout = QueryTimeout
                };
                if (!isInsert) return cmd.ExecuteNonQuery();
                var id = cmd.ExecuteScalar();
                con.Close();
                return id;
            }
        }
        public static object TransExec(string query)
        {
            query = $@"
                SET XACT_ABORT ON
                BEGIN TRAN;
	                {query}
                COMMIT TRAN;
            ";
            return Exec(query);
        }
        public static object FirstResOfQuery(string query)
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                con.Open();
                var cmd = new SqlCommand(query, con) { CommandTimeout = QueryTimeout };
                var res = cmd.ExecuteScalar();
                con.Close();
                return res;
            }
        }
        public static int Count(string table, string condition = null)
        {
            var req = string.Concat("SELECT COUNT(*) FROM ", table);
            if (condition.IsNotNull()) req = string.Concat(req, " WHERE ", condition);
            return FirstResOfQuery(req).ToInt();
        }
        public static bool Exists(string table, string condition = null) => Count(table, condition) > 0;

        #endregion

        #region SqlConversion

        public static string ToSqlDate(this object date, bool hasTimestamp = false)
        {
            if (!date.IsDate()) return "NULL";
            var dt = date.ToDate();
            return $"'{dt.Format($"yyyyMMdd{(hasTimestamp ? " HH:mm:ss.fff" : "")}")}'";
        }
        public static string ToSqlBool(this object value)
        {
            if (value.IsNull()) return "0";
            if (value.Is("true") || value.Is("oui")) return "1";
            if (value.Is("false") || value.Is("non")) return "0";
            return "CONVERT(Bit, '" + value + "')";
        }
        public static string ToSqlNumber(this object numeric) => numeric.GetNumericFormat().Item1.Replace(',', '.');
        public static string ToSqlString(this object str) => str.IsNotNull() ? string.Concat("'", str.ToString().Trim().Replace("'", "''"), "'") : "NULL";
        public static string ToSqlValue(this object value)
        {
            if (value.IsNumeric()) return value.ToSqlNumber();
            if (value.Is("true") || value.Is("false")) return value.ToSqlBool();
            return value.IsDate() ? value.ToSqlDate() : value.ToSqlString();
        }

        #endregion
    }

    public static class _encode
    {
        public static string Base64Encode(this string text) => Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
        public static string Base64Decode(this string text) => Encoding.UTF8.GetString(Convert.FromBase64String(text));
    }
}