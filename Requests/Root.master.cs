using System;
using System.Web.UI;

namespace DSTM {
    public partial class Root : MasterPage {
        public bool EnableBackButton { get; set; }
        protected void Page_Load(object sender, EventArgs e) {
            if(!string.IsNullOrEmpty(Page.Header.Title))
                Page.Header.Title += " - ";
            Page.Header.Title += "DSTM - Gestion de demandes";

            Page.Header.DataBind();
            UpdateUserMenuItemsVisible();
            UpdateUserInfo();
        }

        // SignIn/Register

        protected void UpdateUserMenuItemsVisible()
        {
            var isAuthenticated = false;//AuthHelper.IsAuthenticated();
            RightAreaMenu.Items.FindByName("MyAccountItem").Visible = isAuthenticated;
            RightAreaMenu.Items.FindByName("SignOutItem").Visible = isAuthenticated;
        }

        protected void UpdateUserInfo() {
            /*if(AuthHelper.IsAuthenticated()) {
                var user = AuthHelper.GetLoggedInUserInfo();
                var myAccountItem = RightAreaMenu.Items.FindByName("MyAccountItem");
                var userName = (ASPxLabel)myAccountItem.FindControl("UserNameLabel");
                var email = (ASPxLabel)myAccountItem.FindControl("EmailLabel");
                var accountImage = (HtmlGenericControl)RightAreaMenu.Items[0].FindControl("AccountImage");
                userName.Text = string.Format("{0} ({1} {2})", user.UserName, user.FirstName, user.LastName);
                email.Text = user.Email;
                accountImage.Attributes["class"] = "account-image";

                if(string.IsNullOrEmpty(user.AvatarUrl)) {
                    accountImage.InnerHtml = string.Format("{0}{1}", user.FirstName[0], user.LastName[0]).ToUpper();
                } else {
                    var avatarUrl = (HtmlImage)myAccountItem.FindControl("AvatarUrl");
                    avatarUrl.Attributes["src"] = ResolveUrl(user.AvatarUrl);
                    accountImage.Style["background-image"] = ResolveUrl(user.AvatarUrl);                    
                }
            }*/
        }

        protected void RightAreaMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e) {
            if(e.Item.Name == "SignOutItem") {
                //AuthHelper.SignOut(); // DXCOMMENT: Your Signing out logic
                Response.Redirect("~/");
            }
        }
    }
}