using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using DSTM.Models;

namespace DSTM.Code
{
    public class _func
    {
        public static void CheckData(HttpRequest request, HttpResponse response)
        {
            var acc = request.Url.AbsolutePath.Has("Account");
            if (!AppUser.IsAuthenticated() && !acc) response.Redirect("/Account/SignIn.aspx", false);
            else if(AppUser.IsAuthenticated() && acc) response.Redirect("/Default.aspx", false);
        }
        public static string GetAppSetting(string key) => System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").AppSettings.Settings[key]?.Value;
    }


    public static class _session
    {
        public static void Set(string key, object value) => HttpContext.Current.Session[key] = value;
        public static object Get(string key) => HttpContext.Current.Session[key];
        public static bool Exists(string key) => Get(key) != null;
        public static void Destroy()
        {
            HttpContext.Current.Session.RemoveAll();
            var myCookies = HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var cookie in myCookies)
            {
                if (HttpContext.Current.Response.Cookies[cookie] != null) HttpContext.Current.Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }

    public static class _app
    {
        public static void Init(Assembly assembly)
        {
            _session.Set("CurrentAssembly", FileVersionInfo.GetVersionInfo(assembly.Location));
            _session.Set("currentDirectoryPath", Path.GetDirectoryName(assembly.Location));
        }

        public static FileVersionInfo Data => _session.Get("CurrentAssembly") as FileVersionInfo;
        public static string Name => Data.ProductName;
        public static string Company => Data.CompanyName;
    }

    public static class _file
    {
        public static string CreateFolder(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            return path.TrimEnd('\\').TrimEnd('/');
        }
        public static string GetAbsolutePathOf(string path) => new System.Web.UI.Page().Server.MapPath(path);

        public static string AppFolder() => GetAbsolutePathOf("~\\Content");
        public static string TempFolder() => CreateFolder(AppFolder() + "\\Temp");
    }

    public static class _str
    {
        public static bool IsNull(this string str)
        {
            if (str == null) return true;
            str = str.Trim();
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str)) return true;
            return str == "" || str.Equals("NULL", StringComparison.OrdinalIgnoreCase);
        }
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

        public static bool Has(this object str, string handle) => str != null && handle != null && str.ToString().IndexOf(handle, StringComparison.OrdinalIgnoreCase) != -1 && str.ToString().ToLower().Contains(handle.ToLower());
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
    }

    public static class _db
    {
        public static int QueryTimeout { get; set; } = 1800;
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"]?.ConnectionString;

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

        public static string ToSqlString(this object str) => str.IsNotNull() ? string.Concat("'", str.ToString().Trim().Replace("'", "''"), "'") : "NULL";
    }

    public static class _encode
    {
        public static string Base64Encode(this string text) => Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }
}