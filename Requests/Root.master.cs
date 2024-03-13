using System;
using System.Web.UI;
using DevExpress.Web;
using DSTM.Code;
using DSTM.Models;

namespace DSTM {
    public partial class Root : MasterPage {
        protected void Page_Init()
        {
            if (!IsPostBack) _func.CheckData(Request,Response);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var isAuthenticated = AppUser.IsAuthenticated();
            var myAccountItem = RightAreaMenu.Items.FindByName("MyAccountItem");
            myAccountItem.Visible = isAuthenticated;
            RightAreaMenu.Items.FindByName("SignOutItem").Visible = isAuthenticated;
            if (isAuthenticated)
            {
                var title = Page.Header.Title;
                Page.Header.Title = $"Gestion de demandes : {(title.IsNull() ? "" : $"{title} | ")}{_app.Company}";
                Page.Header.DataBind();

                var user = AppUser.Current;
                var userName = (ASPxLabel)myAccountItem.FindControl("UsernameLabel");
                userName.Text = $"{user.Firstname} {user.Lastname}";
                var email = (ASPxLabel)myAccountItem.FindControl("EmailLabel");
                email.Text = user.Email;

                FirstNameTextBox.Text = user.Firstname;
                LastNameTextBox.Text = user.Lastname;
                EmailTextBox.Text = user.Email;
            }
        }

        protected void RightAreaMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e) 
        {
            if (e.Item.Name != "SignOutItem") return;
            _session.Destroy();
            Response.Redirect("~/");
        }

        protected void RegisterButton_OnClick(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(ProfileForm)) return;
            _db.Exec($"UPDATE [F_COLLABORATEUR_PASS] SET [Password] = {PasswordButtonEdit.Text.Base64Encode().ToSqlString()} WHERE F_COLLABORATEUR_No = {AppUser.Current.Id}");
            Response.Redirect("/", false);
        }
    }
}