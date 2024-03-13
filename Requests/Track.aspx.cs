using System;
using System.Data;
using DevExpress.Web;
using DSTM.Code;

namespace DSTM {
    public partial class Track : System.Web.UI.Page {
        private static DataTable Data { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Data == null || !IsPostBack) Data = _db.Query("KS_SuivieDemande");
            GridView.DataSource = Data;
            GridView.DataBind();
        }

        protected void GridView_OnHtmlRowPrepared(object sender, ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;
            var status = e.GetValue("Statut")?.ToString();
            switch (status)
            {
                case "Emise":
                    e.Row.BackColor = System.Drawing.Color.LightGray;
                    break;
                case "En cours de traitement":
                    e.Row.BackColor = System.Drawing.Color.White;
                    break;
                case "Traitée":
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                    break;
            }
        }
    }
}