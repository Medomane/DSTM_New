using System;
using DevExpress.Web;
using DSTM.Models;

namespace DSTM.Account {
    public partial class SignInModule : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
        }

        protected void SignInButton_Click(object sender, EventArgs e)
        {
            var err = FormLayout.FindItemOrGroupByName("GeneralError");
            err.Visible = false;
            if(ASPxEdit.ValidateEditorsInContainer(FormLayout))
            {
                try
                {
                    AppUser.Login(EmailTextBox.Text, PasswordButtonEdit.Text);
                    Response.Redirect("~/");
                }
                catch (Exception ex)
                {
                    err.Visible = true;
                    GeneralErrorDiv.InnerText = ex.Message;
                }
            }
        }
    }
}