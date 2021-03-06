using MCS.Library.Core;
using MCS.Library.Net.SNTP;
using MCS.Library.Principal;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace MCS.Web.Passport.TestPages
{
    public partial class PrincipalTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SignInLogo.ReturnUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.SafeUnescaped);

            if (Request.IsAuthenticated)
            {
                ShowPrincipalInfo();
            }
        }

        private void ShowPrincipalInfo()
        {
            HtmlTable table = new HtmlTable();
            table.Attributes["class"] = "table";

            ShowSinglePrincipalInfo(table, "User Logon Name", DeluxeIdentity.CurrentUser.LogOnName);
            ShowSinglePrincipalInfo(table, "User Display Name", DeluxeIdentity.CurrentUser.DisplayName);

            if (DeluxeIdentity.CurrentUser.TopOU != null)
                ShowSinglePrincipalInfo(table, "Top OU",
                    string.Format("{0}({1})",
                    DeluxeIdentity.CurrentUser.TopOU.DisplayName,
                    DeluxeIdentity.CurrentUser.TopOU.FullPath));

            ShowSinglePrincipalInfo(table, "Simulated Time", SNTPClient.AdjustedTime.SimulateTime().ToString("yyyy-MM-dd HH:mm:ss.fff"));
            ShowSinglePrincipalInfo(table, "TimePoint Simulated Time", TimePointContext.Current.SimulatedTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            ShowSinglePrincipalInfo(table, "Use Current Time", TimePointContext.Current.UseCurrentTime.ToString());


            principalInfo.Controls.Add(table);
        }

        private void ShowSinglePrincipalInfo(Control parent, string name, string data)
        {
            HtmlTableRow row = new HtmlTableRow();

            HtmlTableCell cell = new HtmlTableCell();
            cell.InnerText = name + ":";
            cell.Attributes["class"] = "captionCell";
            row.Controls.Add(cell);

            cell = new HtmlTableCell();
            cell.InnerText = data;
            row.Controls.Add(cell);

            parent.Controls.Add(row);
        }

        protected void clearPrincipal_Click(object sender, EventArgs e)
        {
            DeluxePrincipal.Current.ClearCacheInWebApp();

            Response.Redirect(this.Request.Url.ToString());
        }
    }
}
