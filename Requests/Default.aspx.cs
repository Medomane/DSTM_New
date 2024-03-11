using System;
using System.Data;
using DevExpress.Web.Data;

namespace DSTM {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dt = new DataTable("data");
            dt.Columns.Add(new DataColumn("Subject", typeof(string)) { ReadOnly = false });
            dt.Columns.Add(new DataColumn("DeliveryDate", typeof(DateTime)) { ReadOnly = false });
            GridView.DataSource = dt;
            GridView.DataBind();
        }

        protected void GridView_OnRowInserting(object sender, ASPxDataInsertingEventArgs e)
        {

            var data = GridView.DataSource as DataTable;

        }
    }
}