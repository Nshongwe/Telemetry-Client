using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Service;

namespace Snmp_Web_Client
{
    public partial class _Default : Page
    {
        private ILogService _logService;
        protected void Page_Load(object sender, EventArgs e)
        {
            _logService = new LogService();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDataGrid();
        }
        private void BindDataGrid()
        {
            var searchResults = _logService.Search(txtSearch.Text);
            dgLog.DataSource = searchResults;
            dgLog.DataBind();

        }
    }
}