using System;

namespace DSTM.Account {
    public partial class RegisterModule : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
        }

        protected void RegisterButton_Click(object sender, EventArgs e) {
            /*var err = FormLayout.FindItemOrGroupByName("GeneralError");
            err.Visible = false;
            if (ASPxEdit.ValidateEditorsInContainer(FormLayout))
            {
                try
                {
                    var firstname = FirstNameTextBox.Text.ToSqlString();
                    var lastname = LastNameTextBox.Text.ToSqlString();
                    var email = EmailTextBox.Text.ToSqlString();
                    var password = PasswordButtonEdit.Text.Base64Encode().ToSqlString();
                    _db.Exec($"EXEC KS_AjouterCollaborateur @prenom = {firstname}, @nom = {lastname}, @email = {email}, @motDePasse = {password}");
                    Response.Redirect("/Account/SignIn.aspx", false);
                }
                catch(Exception ex)
                {
                    err.Visible = true;
                    GeneralErrorDiv.InnerText = ex.Message;
                }
            }*/
        }
    }
}