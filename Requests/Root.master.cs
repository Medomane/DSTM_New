using System;
using System.Web.UI;
using DevExpress.Web;
using System.Web.UI.HtmlControls;
using DSTM.Code;
using DSTM.Models;

namespace DSTM {
    public partial class Root : MasterPage {
        protected void Page_Init()
        {
            if (!IsPostBack) MyFunc.CheckData(Request,Response);
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
            }
        }


        protected void RightAreaMenu_ItemClick(object source, DevExpress.Web.MenuItemEventArgs e) 
        {
            if (e.Item.Name != "SignOutItem") return;
            _session.Destroy();
            Response.Redirect("~/");
        }
    }
}