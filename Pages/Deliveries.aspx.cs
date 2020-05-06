using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Drawing;

namespace Act_site
{
    public partial class siteDeliveries : System.Web.UI.Page
    {
        int sumDataRow = 0;
        int detailedDataRow = 0;
        int siteRowCount = 0;

        string sumWorkerTable = "";
        string detailWorkerTable = "";

        String companyName = "NA";
        String globalSiteName = "Choose a site";

        int rowCountStartDateDelivery = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Company"] != null)
            {
                companyName = Session["Company"].ToString();
            }

            if (Session["Site"] != null && Convert.ToString(Session["Site"]) != "No current sites" && !Page.IsPostBack)
            {
                globalSiteName = Session["Site"].ToString();
                ChooseSiteDropDownList.ClearSelection();
               // ChooseSiteDropDownList.Items.FindByValue(globalSiteName).Selected = true;
                siteNameLabel.Text = globalSiteName;
            }

            if (!Page.IsPostBack)
            {
                loadSiteDropDown(companyName);
            }
        }

        public List<String[]> getSiteData(String comp)
        {
            List<string[]> returnData = new List<string[]>();

            siteRowCount = 0;
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT siteName, siteTown, startDate, endDate, autoInt FROM wcf_data.sites where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    String[] returnDataArray = new String[5];
                    returnDataArray[0] = rdr.IsDBNull(0) ? "NA" : rdr.GetString(0);
                    returnDataArray[1] = rdr.IsDBNull(1) ? "NA" : rdr.GetString(1);
                    returnDataArray[2] = rdr.IsDBNull(2) ? "NA" : Convert.ToString(rdr.GetDateTime(2));
                    returnDataArray[3] = rdr.IsDBNull(3) ? "NA" : Convert.ToString(rdr.GetDateTime(3));
                    returnDataArray[4] = rdr.IsDBNull(4) ? "NA" : rdr.GetString(4);

                    returnData.Add(returnDataArray);
                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }


            return returnData;
        }

        protected void loadSiteDropDown(string comp)
        {
            List<string[]> returnData = new List<string[]>();

            try
            {

                returnData = getSiteData(comp);

                ChooseSiteDropDownList.Items.Clear();

                if (returnData.Count == 0)
                {
                    ChooseSiteDropDownList.Items.Add(new ListItem("No current sites"));
                }

                if (returnData.Count > 0)
                {
                    for (int x = 0; x < returnData.Count; x++)
                    {
                        string xcv = returnData[x][0];
                        ChooseSiteDropDownList.Items.Add(new ListItem(returnData[x][0]));
                    }

                }

                if (Session["Site"] != null && returnData.Count > 0)
                {
                    ChooseSiteDropDownList.SelectedValue = Session["Site"].ToString();
                    globalSiteName = ChooseSiteDropDownList.SelectedItem.Text;
                    siteNameLabel.Text = globalSiteName;
                }

                if (Session["Site"] == null && returnData.Count > 0)
                {
                    ChooseSiteDropDownList.SelectedValue = returnData[0][0];
                    globalSiteName = ChooseSiteDropDownList.SelectedItem.Text;
                    Session["Site"] = ChooseSiteDropDownList.SelectedItem.Text;
                    siteNameLabel.Text = globalSiteName;
                }


            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }

        }

        /*
        protected void loadSiteDropDown()
        {
            //  updateSiteButton.Click += new EventHandler(updateSiteButtonClick);
            //  addNewSiteButton.Click += new EventHandler(addNewSiteButtonClick);
            //addNewSiteButtonClick"

            if (ChooseSiteDropDownList.Items.Count == 0)
            {
                ChooseSiteDropDownList.Items.Add(new ListItem("No current sites"));
            }


            if (ChooseSiteDropDownList.SelectedItem.Text != null)
            {
                globalSiteName = ChooseSiteDropDownList.SelectedItem.Text;
            }

            if (globalSiteName.Length > 0 && globalSiteName != "No current sites" && globalSiteName != "0")
            {

                //   String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();
                //   String[] tempTownArray = temptown.Split('-');
                //  globalTownName = tempTownArray[1];
            }

            if (globalSiteName.Length == 0)
            {
                globalSiteName = "Choose a site";
            }
            String[,] siteData = new String[5, 500];
            ChooseSiteDropDownList.Items.Clear();

            siteData = getSiteData(companyName);
            if (siteRowCount == 0)
            {
                ChooseSiteDropDownList.Items.Add(new ListItem("No current sites", "0"));
                ChooseSiteDropDownList.SelectedValue = "0";
            }

            if (siteRowCount > 0)
            {
                ChooseSiteDropDownList.Items.Add(new ListItem("Choose a site", "0"));
                for (int x = 0; x < siteRowCount; x++)
                {
                    ChooseSiteDropDownList.Items.Add(new ListItem(siteData[0, x], siteData[0, x]));
                }

                ChooseSiteDropDownList.SelectedValue = globalSiteName;
            }
        }
        */
        protected void loadDeliveryDateDropDown(String companyNameStr, String globalSiteNameStr)
        {

            DateTime[] siteData = new DateTime[50];

            siteData = getStartDatesDelivery(companyName, globalSiteName);

            if (ChooseSiteDropDownList.Items.Count == 0)
            {
                ChooseSiteDropDownList.Items.Add(new ListItem("No current sites"));
            }


            DeliveryViewDropDownList.Items.Clear();

            if (rowCountStartDateDelivery > 0)
            {
                for (int x = 0; x < rowCountStartDateDelivery; x++)
                {
                    DeliveryViewDropDownList.Items.Add(new ListItem(siteData[x].ToString("yyyy-MM-dd"), siteData[x].ToString("yyyy-MM-dd")));
                }
                DeliveryViewDropDownList.Items.Add(new ListItem("Entire time"));
            }
        }
        /*
        public String[,] getSiteData(String comp)
        {
            String[,] returnData = new String[5, 100];
            siteRowCount = 0;
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT siteName, siteTown, startDate, endDate, autoInt FROM wcf_data.sites where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, siteRowCount] = rdr.GetString(0);
                    returnData[1, siteRowCount] = rdr.GetString(1);
                    returnData[2, siteRowCount] = Convert.ToString(rdr.GetDateTime(2));
                    returnData[3, siteRowCount] = Convert.ToString(rdr.GetDateTime(3));
                    returnData[4, siteRowCount] = rdr.GetString(4);

                    siteRowCount++;
                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }


            return returnData;
        }
        */
        protected string getBasicTable()
        {
            string[,] sumData = new string[4, 100];
            try
            {
              //  sumData = getSummaryDeliveryFromDB();
                sumWorkerTable = "";

                sumWorkerTable = sumWorkerTable + "<p>Summary Deliveries</p>";

                sumWorkerTable = sumWorkerTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:10%\">Tab Id</th>" +
                                            "<th style=\"width:17%\">Site</th>" +
                                            "<th style=\"width:17%\">Section</th>" +
                                            "<th style=\"width:17%\">Construction</th>" +
                                            "<th style=\"width:14%\">Delivery</th>" +
                                            "<th style=\"width:10%\">Delivery Measure</th>" +
                                            "<th style=\"width:10%\">Data</th>" +

                                            "</tr>";

                for (int x = 0; x < sumDataRow; x++)
                {
                    sumWorkerTable = sumWorkerTable + "<tr><td>"
                                                + sumData[0, x] + "</td><td>"
                                                + sumData[1, x] + "</td><td>"
                                                + sumData[2, x] + "</td><td>"
                                                + sumData[3, x] + "</td><td>"
                                                + sumData[4, x] + "</td><td>"
                                                + sumData[5, x] + "</td><td>"
                                                + sumData[6, x] + "</td></tr>";
                }
                sumWorkerTable = sumWorkerTable + "</table>";
            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            return sumWorkerTable;
        }

        protected string[,] getSummaryDeliveryFromDB(string comp, string site)
        {

            string[,] sumData = new string[7, 500];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, item, measureType, sum(measureData) " +
                                    " FROM delivery_data WHERE site = '" + site + "' " +
                                    " group by tabID, site, section, construction, item, measureType;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    sumData[0, sumDataRow] = rdr.GetString(0);
                    sumData[1, sumDataRow] = rdr.GetString(1);
                    sumData[2, sumDataRow] = rdr.GetString(2);
                    sumData[3, sumDataRow] = rdr.GetString(3);
                    sumData[4, sumDataRow] = rdr.GetString(4);
                    sumData[5, sumDataRow] = rdr.GetString(5);
                    sumData[6, sumDataRow] = rdr.GetString(6);

                    sumDataRow++;

                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            //get the last few days of data or whatever from the the DB
            return sumData;
        }

        protected string getDetailedTable()
        {
            string[,] detData = new string[20, 5000];
            try
            {
              //  detData = getDetailedDeliveryFromDB();
                detailWorkerTable = "";

                detailWorkerTable = detailWorkerTable + "<p>Detailed Deliveries</p>";

                detailWorkerTable = detailWorkerTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:8%\">Tab Id</th>" +
                                            "<th style=\"width:12%\">Site</th>" +
                                            "<th style=\"width:12%\">Section</th>" +
                                            "<th style=\"width:12%\">Construction</th>" +
                                            "<th style=\"width:10%\">Delivery Name</th>" +
                                            "<th style=\"width:8%\">Delivery Measure</th>" +
                                            "<th style=\"width:8%\">Data</th>" +
                                            "<th style=\"width:10%\">Work Date</th>" +
                                            "<th style=\"width:10%\">Delivery Time</th>" +
                                            "<th style=\"width:10%\">Approver Name</th>" +
                                            

                                            "</tr>";

                for (int x = 0; x < detailedDataRow; x++)
                {
                    detailWorkerTable = detailWorkerTable + "<tr><td>"
                                                + detData[0, x] + "</td><td>"
                                                + detData[1, x] + "</td><td>"
                                                + detData[2, x] + "</td><td>"
                                                + detData[3, x] + "</td><td>"
                                                + detData[4, x] + "</td><td>"
                                                + detData[5, x] + "</td><td>"
                                                + detData[6, x] + "</td><td>"
                                                + detData[7, x] + "</td><td>"
                                                + detData[8, x] + "</td><td>"
                                                + detData[9, x] + "</td></tr>";
                }
                detailWorkerTable = detailWorkerTable + "</table>";
            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            return detailWorkerTable;
        }

        protected string[,] getDetailedDeliveryFromDB(string comp, string site, DateTime beginDate, bool allTime)
        {
            string shortDate = "MM/dd/yy";
            string shortTime = "h:mm:ss tt";
            string tempS;
            DateTime tempDT;

            string workerWipSQL = "";
            DateTime endDate;

            string[,] detailedData = new string[10, 5000];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            detailedDataRow = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                if (allTime == true)
                {
                    workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, item, measureType, measureData, workDate, deliveryTime, approver " +

                                         " FROM delivery_data WHERE site ='" + site + "' ORDER BY workDate;";
                }

                if (allTime == false)
                {
                    endDate = beginDate.AddDays(7);
                    workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, item, measureType, measureData, workDate, deliveryTime, approver " +

                                         " FROM delivery_data WHERE site ='" + site + "' AND workDate BETWEEN '" + beginDate.ToString("yyyy-MM-dd") + "' AND '" + endDate.ToString("yyyy-MM-dd") + "' ;";
                }

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    detailedData[0, detailedDataRow] = rdr.GetString(0);
                    detailedData[1, detailedDataRow] = rdr.GetString(1);
                    detailedData[2, detailedDataRow] = rdr.GetString(2);
                    detailedData[3, detailedDataRow] = rdr.GetString(3);
                    detailedData[4, detailedDataRow] = rdr.GetString(4);
                    detailedData[5, detailedDataRow] = rdr.GetString(5);
                    detailedData[6, detailedDataRow] = rdr.GetString(6);
                    tempDT = Convert.ToDateTime(rdr.GetString(7));
                    detailedData[7, detailedDataRow] = tempDT.ToShortDateString();
                    tempDT = Convert.ToDateTime(rdr.GetString(8));
                    detailedData[8, detailedDataRow] = tempDT.ToShortTimeString();
                    detailedData[9, detailedDataRow] = rdr.GetString(9);

                    detailedDataRow++;

                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            //get the last few days of data or whatever from the the DB
            return detailedData;
        }

        protected void ChooseSiteDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime tempDate = new DateTime();


            if(ChooseSiteDropDownList.SelectedIndex > -1)
            {
                globalSiteName = ChooseSiteDropDownList.SelectedValue;
                getSummaryTable(companyName, globalSiteName);
                getDetailedTable(companyName, globalSiteName, tempDate, true);
                loadDeliveryDateDropDown(companyName, globalSiteName);
                siteNameLabel.Text = globalSiteName;
            }         
        }

        protected void getSummaryTable(string compN, string siteN)
        {
            String[,] sumData = new String[7, 500];


            try
            {
                sumData = getSummaryDeliveryFromDB(compN, siteN);



                summaryTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Tablet";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Site";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;  tabID, site, section, construction, item, measureType, sum(measureData)

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Section";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Construction";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Item";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Measure";
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = "Data";
                header7.Font.Bold = true;
                header7.BackColor = Color.LightGray;
                header7.HorizontalAlign = HorizontalAlign.Center;

                // Add the header to a new row.
                TableRow headerRow = new TableRow();
                headerRow.Cells.Add(header1);
                headerRow.Cells.Add(header2);
                headerRow.Cells.Add(header3);
                headerRow.Cells.Add(header4);
                headerRow.Cells.Add(header5);
                headerRow.Cells.Add(header6);
                headerRow.Cells.Add(header7);

                // Add the header row to the table.
                summaryTable.Rows.AddAt(0, headerRow);



                int numrows = sumDataRow;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type

                    a.Text = sumData[0, j];
                    a.HorizontalAlign = HorizontalAlign.Center;
                    a.Width = Unit.Percentage(14.2);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    b.Text = sumData[1, j];
                    b.HorizontalAlign = HorizontalAlign.Center;
                    b.Width = Unit.Percentage(14.2);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //task values
                    c.Text = sumData[2, j];
                    c.HorizontalAlign = HorizontalAlign.Center;
                    c.Width = Unit.Percentage(14.2);
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    d.Text = sumData[3, j];
                    d.HorizontalAlign = HorizontalAlign.Center;
                    d.Width = Unit.Percentage(14.2);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    e.Text = sumData[4, j];
                    e.HorizontalAlign = HorizontalAlign.Center;
                    e.Width = Unit.Percentage(14.2);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    f.Text = sumData[5, j];
                    f.HorizontalAlign = HorizontalAlign.Center;
                    f.Width = Unit.Percentage(14.2);
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    g.Text = sumData[6, j];
                    g.HorizontalAlign = HorizontalAlign.Center;
                    g.Width = Unit.Percentage(14.2);
                    r.Cells.Add(g);

                    summaryTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getDetailedTable(string compN, string siteN, DateTime beginDate, bool allTime)
        {
            String[,] sumData = new String[10, 5000];

            try
            {
                sumData = getDetailedDeliveryFromDB(compN, siteN, beginDate, allTime);

                detailTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Tablet";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Site";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;  tabID, site, section, construction, item, measureType, measureData, workDate, deliveryTime, approver

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Section";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Construction";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Item";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Measure";
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = "Data";
                header7.Font.Bold = true;
                header7.BackColor = Color.LightGray;
                header7.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header8 = new TableHeaderCell();
                header8.Text = "Work Date";
                header8.Font.Bold = true;
                header8.BackColor = Color.LightGray;
                header8.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header9 = new TableHeaderCell();
                header9.Text = "Delivery Time";
                header9.Font.Bold = true;
                header9.BackColor = Color.LightGray;
                header9.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header10 = new TableHeaderCell();
                header10.Text = "Approver";
                header10.Font.Bold = true;
                header10.BackColor = Color.LightGray;
                header10.HorizontalAlign = HorizontalAlign.Center;

                // Add the header to a new row.
                TableRow headerRow = new TableRow();
                headerRow.Cells.Add(header1);
                headerRow.Cells.Add(header2);
                headerRow.Cells.Add(header3);
                headerRow.Cells.Add(header4);
                headerRow.Cells.Add(header5);
                headerRow.Cells.Add(header6);
                headerRow.Cells.Add(header7);
                headerRow.Cells.Add(header8);
                headerRow.Cells.Add(header9);
                headerRow.Cells.Add(header10);

                // Add the header row to the table.
                detailTable.Rows.AddAt(0, headerRow);



                int numrows = detailedDataRow;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type

                    a.Text = sumData[0, j];
                    a.HorizontalAlign = HorizontalAlign.Center;
                    a.Width = Unit.Percentage(10);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    b.Text = sumData[1, j];
                    b.HorizontalAlign = HorizontalAlign.Center;
                    b.Width = Unit.Percentage(10);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //task values
                    c.Text = sumData[2, j];
                    c.HorizontalAlign = HorizontalAlign.Center;
                    c.Width = Unit.Percentage(10);
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    d.Text = sumData[3, j];
                    d.HorizontalAlign = HorizontalAlign.Center;
                    d.Width = Unit.Percentage(10);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    e.Text = sumData[4, j];
                    e.HorizontalAlign = HorizontalAlign.Center;
                    e.Width = Unit.Percentage(10);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    f.Text = sumData[5, j];
                    f.HorizontalAlign = HorizontalAlign.Center;
                    f.Width = Unit.Percentage(10);
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    g.Text = sumData[6, j];
                    g.HorizontalAlign = HorizontalAlign.Center;
                    g.Width = Unit.Percentage(10);
                    r.Cells.Add(g);

                    TableCell h = new TableCell();
                    h.Text = sumData[7, j];
                    h.HorizontalAlign = HorizontalAlign.Center;
                    h.Width = Unit.Percentage(10);
                    r.Cells.Add(h);

                    TableCell i = new TableCell();
                    i.Text = sumData[8, j];
                    i.HorizontalAlign = HorizontalAlign.Center;
                    i.Width = Unit.Percentage(10);
                    r.Cells.Add(i);

                    TableCell jj = new TableCell();
                    jj.Text = sumData[9, j];
                    jj.HorizontalAlign = HorizontalAlign.Center;
                    jj.Width = Unit.Percentage(10);
                    r.Cells.Add(jj);

                    detailTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected DateTime[] getStartDatesDelivery(String companyNameStr, String globalSiteNameStr)
        {
            // CultureInfo cul = CultureInfo.CurrentCulture;
            DateTime[] returnValues = new DateTime[50];
            DateTime tempDT;
            rowCountStartDateDelivery = 0;
            int subtractValue = 0;

            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT workDate " +
                                    " FROM work_data WHERE site = '" + globalSiteNameStr + "' ORDER BY workDate LIMIT 50;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tempDT = Convert.ToDateTime(rdr.GetString(0));

                    if (rowCountStartDateDelivery > 0)
                    {
                        for (int x = 0; x < rowCountStartDateDelivery; x++)
                        {
                            if (tempDT > returnValues[x].AddDays(6) && x == rowCountStartDateDelivery - 1)
                            {
                                DayOfWeek tempDTdow = tempDT.DayOfWeek;
                                switch (tempDTdow)
                                {
                                    case DayOfWeek.Sunday:
                                        subtractValue = -6;
                                        break;
                                    case DayOfWeek.Saturday:
                                        subtractValue = -5;
                                        break;
                                    case DayOfWeek.Friday:
                                        subtractValue = -4;
                                        break;
                                    case DayOfWeek.Thursday:
                                        subtractValue = -3;
                                        break;
                                    case DayOfWeek.Wednesday:
                                        subtractValue = -2;
                                        break;
                                    case DayOfWeek.Tuesday:
                                        subtractValue = -1;
                                        break;
                                    case DayOfWeek.Monday:
                                        subtractValue = 0;
                                        break;
                                    default:
                                        subtractValue = 0;
                                        break;
                                }

                                returnValues[rowCountStartDateDelivery] = tempDT.AddDays(subtractValue);
                                rowCountStartDateDelivery++;
                            }
                        }
                    }


                    if (rowCountStartDateDelivery == 0)
                    {

                        DayOfWeek tempDTdow = tempDT.DayOfWeek;
                        switch (tempDTdow)
                        {
                            case DayOfWeek.Sunday:
                                subtractValue = -6;
                                break;
                            case DayOfWeek.Saturday:
                                subtractValue = -5;
                                break;
                            case DayOfWeek.Friday:
                                subtractValue = -4;
                                break;
                            case DayOfWeek.Thursday:
                                subtractValue = -3;
                                break;
                            case DayOfWeek.Wednesday:
                                subtractValue = -2;
                                break;
                            case DayOfWeek.Tuesday:
                                subtractValue = -1;
                                break;
                            case DayOfWeek.Monday:
                                subtractValue = 0;
                                break;
                            default:
                                subtractValue = 0;
                                break;
                        }

                        returnValues[rowCountStartDateDelivery] = tempDT.AddDays(subtractValue);
                        rowCountStartDateDelivery++;
                    }


                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }



            return returnValues;
        }

        protected void DeliveryViewDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[,] returnData = new String[10, 100];
            DateTime beginDate;
            Boolean allTime = false;

            globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();
            siteNameLabel.Text = globalSiteName;

            if (DeliveryViewDropDownList.SelectedValue.ToString() == "Entire time")
            {
                beginDate = new DateTime(2012, 1, 1, 12, 12, 12);
                allTime = true;
            }
            else { beginDate = Convert.ToDateTime(DeliveryViewDropDownList.SelectedValue.ToString()); }

            getDetailedTable(companyName, globalSiteName, beginDate, allTime);
            getSummaryTable(companyName, globalSiteName);
        }
    }
}