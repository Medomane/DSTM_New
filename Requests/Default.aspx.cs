using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using DevExpress.Web;
using DevExpress.Web.Data;
using DSTM.Code;

namespace DSTM {
    public partial class Default : System.Web.UI.Page {
        private static DataTable Data { get; set; }
        private static List<string> Files { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if (Data == null)
                {
                    Data = new DataTable("data");
                    Data.Columns.Add(new DataColumn("Id", typeof(int)) { ReadOnly = false, AllowDBNull = false });
                    Data.Columns.Add(new DataColumn("Article", typeof(string)) { ReadOnly = false, AllowDBNull = false });
                    Data.Columns.Add(new DataColumn("Subject", typeof(string)) { ReadOnly = false, AllowDBNull = false, MaxLength = 69 });
                    Data.Columns.Add(new DataColumn("DeliveryDate", typeof(DateTime)) { ReadOnly = false, AllowDBNull = false });
                }
                GridView.DataSource = Data;
                GridView.DataBind();

                var articles = ConfigurationManager.AppSettings.AllKeys.Where(key => key.Has("article")).Select(_func.GetAppSetting);
                foreach (var article in articles) ((GridViewDataComboBoxColumn)GridView.Columns["Article"]).PropertiesComboBox.Items.Add(article);
            }
        }

        protected void GridView_OnRowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            var valSubject = e.NewValues["Subject"];
            if (valSubject.IsNull())
            {
                e.Cancel = true;
                return;
            }
            var valDeliveryDate = e.NewValues["DeliveryDate"];
            if (!valDeliveryDate.IsDate())
            {
                e.Cancel = true;
                return;
            }
            var valArticle = e.NewValues["Article"];
            if (valArticle.IsNull())
            {
                e.Cancel = true;
                return;
            }
            GridView.CancelEdit();
            e.Cancel = true;
            var max = Data.Rows.Count > 0 ? Data.Rows.Cast<DataRow>().Max(row => (int)row["Id"]) : 0;
            Data.Rows.Add(max+1, valArticle, valSubject, valDeliveryDate);
            GridView.DataSource = Data;
        }

        protected void GridView_OnRowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            var valSubject = e.NewValues["Subject"];
            if (valSubject.IsNull())
            {
                e.Cancel = true;
                return;
            }
            var valDeliveryDate = e.NewValues["DeliveryDate"];
            if (!valDeliveryDate.IsDate())
            {
                e.Cancel = true;
                return;
            }
            var valArticle = e.NewValues["Article"];
            if (valArticle.IsNull())
            {
                e.Cancel = true;
                return;
            }
            GridView.CancelEdit();
            e.Cancel = true;
            var uRow = Data.Rows.Cast<DataRow>().First(row => (int)row["Id"] == (int)e.Keys[0]);
            uRow["Article"] = valArticle;
            uRow["Subject"] = valSubject;
            uRow["DeliveryDate"] = valDeliveryDate;
            GridView.DataSource = Data;
        }

        protected void GridView_OnRowDeleting(object sender, ASPxDataDeletingEventArgs e)
        {
            e.Cancel = true;
            Data.Rows.Remove(Data.Rows.Cast<DataRow>().First(row => (int)row["Id"] == (int)e.Keys[0]));
            GridView.DataSource = Data;
        }

        protected void UploadControl_OnFilesUploadComplete(object sender, FilesUploadCompleteEventArgs e)
        {
            Files = new List<string>();
            var files = UploadControl.UploadedFiles;
            foreach (var file in files)
            {
                var resultFileUrl = _file.TempFolder() + "/" + Path.ChangeExtension(Path.GetRandomFileName(), Path.GetExtension(file.FileName));
                file.SaveAs(resultFileUrl);
                Files.Add(resultFileUrl);
            }
        }

        protected void ValidateButton_OnClick(object sender, EventArgs e)
        {
            if (!ASPxEdit.ValidateEditorsInContainer(FormLayout)) return;
            var client = ClientField.Text;
            var num = NumField.Text;
            var dateDemand = DateDemandField.Date;
            var comment = CommentField.Text;
            var docType = _func.GetAppSetting("DocType");

            foreach (DataRow dr in Data.Rows)
            {
                var objet = dr["Subject"]?.ToString();
                var acticle = dr["Article"]?.ToString();
                var dateLiv = dr["DeliveryDate"].ToDate();


            }
            /*_db.Exec($@"insert into F_DOCENTETE  ([DO_Domaine]  ,[DO_Type]  ,[DO_Piece]   ,[DO_Date] ,do_period , [DO_Tiers] ,[CO_No]  ,[CT_NumPayeur])
                  values(0,0,'CDE0258','12/03/2024',1,'CDSTI',2,'CDSTI')


                SELECT 0 [DO_Domaine],0 [DO_Type],'CDSTI' [CT_Num],'CDE0258' [DO_Piece]  ,[DO_Date],1000 [DL_Ligne]--incremente par devis
                ,[AR_Ref] 'ZREF'
                ,[DL_Design] ,1 [DL_Qte]
                      ,1 [DL_QteBC]  ,[CO_No] ,0[DL_MvtStock],0[DE_No] ,max ([DL_No])[DL_No] ,[DO_DateLivr] 
                  FROM [DSTM].[dbo].[F_DOCLIGNE]");*/
        }

        protected void ClientField_OnLoad(object sender, EventArgs e)
        {
            var clients = ConfigurationManager.AppSettings.AllKeys.Where(key => key.Has("client")).Select(_func.GetAppSetting);
            foreach (var client in clients) ClientField.Items.Add(client);
        }
    }
}