using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MCS.Web.WebControls.Test.WordPrint
{
    public partial class WordPrint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void WordPrint1_OnPrint(WordPrintDataSourceCollection DataSourceList)
        {
            DataTable Dt = new DataTable();
            Dt.Columns.Add("ID", typeof(string));
            Dt.Columns.Add("Text", typeof(string));

            DataRow newRow;

            
            newRow = Dt.NewRow();
            newRow["ID"] = "�Ҿ���AA�е�ID";
            newRow["Text"] = "�Ҿ���AA�е�Text";
            Dt.Rows.Add(newRow);

            WordPrintDataSource aa = new WordPrintDataSource("aa", (IEnumerable)Dt.DefaultView);


            DataTable Dt2 = new DataTable();
            Dt2.Columns.Add("ID2", typeof(string));
            Dt2.Columns.Add("Text2", typeof(string));

            //DataRow newRow;

            newRow = Dt2.NewRow();
            newRow["ID2"] = "�Ҿ���BB�е�ID";
            newRow["Text2"] = "�Ҿ���BB�е�Text";
            Dt2.Rows.Add(newRow);
            

            WordPrintDataSource bb = new WordPrintDataSource("bb", (IEnumerable)Dt2.DefaultView);

            DataTable Dt3 = new DataTable();
            Dt3.Columns.Add("ID3", typeof(string));
            Dt3.Columns.Add("Text3", typeof(string));


            newRow = Dt3.NewRow();
            newRow["ID3"] = "1";
            newRow["Text3"] = "http://localhost:2032/WordPrint/doc2.doc";
            Dt3.Rows.Add(newRow);

            WordPrintDataSource cc = new WordPrintDataSource("cc", (IEnumerable)Dt3.DefaultView);

            DataSourceList.Add(aa);
            DataSourceList.Add(bb);
            DataSourceList.Add(cc);
        }
    }
}
