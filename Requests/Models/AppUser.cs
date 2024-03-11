using System;
using DSTM.Code;

namespace DSTM.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public static bool IsAuthenticated() => _session.Exists($"current{_app.Name}User");
        public static AppUser Current
        {
            get => _session.Get($"current{_app.Name}User") as AppUser;
            set => _session.Set($"current{_app.Name}User", value);
        }
        public static void Login(string email, string password)
        {
            var data = _db.Query($@"SELECT [CO_No] Id
                    ,[CO_Nom] Firstname
                    ,[CO_Prenom] Lastname
                    ,[CO_EMail] Email
                FROM [F_COLLABORATEUR] co INNER JOIN [F_COLLABORATEUR_PASS] cop ON cop.F_COLLABORATEUR_No = co.CO_No
                WHERE [CO_EMail] = {email.ToSqlString()} AND cop.[Password] = {password.Base64Encode().ToSqlString()}");
            if (data.Rows.Count > 0)
            {
                var user = data.Rows[0];
                Current = new AppUser
                {
                    Id = (int)user["Id"],
                    Firstname = user["Firstname"]?.ToString(),
                    Lastname = user["Lastname"]?.ToString(),
                    Email = user["Email"]?.ToString(),
                    Password = password
                };
            }
            else throw new Exception("L'email ou le mot de passe est incorrect !!!");
        }
    }
}