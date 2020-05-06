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
    public partial class siteInUseData : System.Web.UI.Page
    {

        String companyName = "NA";
        String globalSiteName = "Choose a site";
        int siteRowCount = 0;

        List<string> workerDateList = new List<string>();
        List<string> progressDateList = new List<string>();
        List<string> deliveryDateList = new List<string>();

        List<string> siteList = new List<string>();

        int workerRowCount = 0;
        int progressRowCount = 0;
        int deliveryRowCount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try { 
            if (Session["Company"] != null)
            {
                companyName = Session["Company"].ToString();
            }

            if (Session["Site"] != null)
            {
                globalSiteName = Session["Site"].ToString();
            }

               loadSiteDropDown(companyName);
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
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
                    //siteNameLabel.Text = globalSiteName;
                }

                if (Session["Site"] == null && returnData.Count > 0)
                {
                    ChooseSiteDropDownList.SelectedValue = returnData[0][0];
                    globalSiteName = ChooseSiteDropDownList.SelectedItem.Text;
                    Session["Site"] = ChooseSiteDropDownList.SelectedItem.Text;
                   // siteNameLabel.Text = globalSiteName;
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
            try
            {
                siteList.Clear();
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
                        siteList.Add(siteData[0, x]);
                    }

                    ChooseSiteDropDownList.SelectedValue = globalSiteName;
                }
            }
            catch(Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }
        */
        protected List<string> getDateWorkerList(string site)
        {
            List<string> DateList = new List<string>();
            ChooseWorkerDate.Items.Clear();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            DateTime tempDate;
            string tempx = "";
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT DATE(dbInsertDateTime) FROM construction.worker_measure where site = '" + site + "' ORDER BY DATE(dbInsertDateTime);";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DateList.Add(rdr.GetString(0));
                    tempDate = Convert.ToDateTime(rdr.GetString(0));
                    tempx = tempDate.ToString("yyyy-MM-dd");
                    ChooseWorkerDate.Items.Add(tempx);
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


            return DateList;
        }

        protected List<string> getDateProgressList(string site)
        {
            List<string> DateList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            DateTime tempDate;
            string tempx = "";

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT DATE(dbEnterDateTime) FROM construction.progress_measure where site = '" + site + "' ORDER BY DATE(dbEnterDateTime) ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DateList.Add(rdr.GetString(0));
                    tempDate = Convert.ToDateTime(rdr.GetString(0));
                    tempx = tempDate.ToString("yyyy-MM-dd");
                    ChooseProgressDate.Items.Add(tempx);
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


            return DateList;
        }

        protected List<string> getDateDeliveryList(string site)
        {
            List<string> DateList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            DateTime tempDate;
            string tempx = "";

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT DATE(dbInsertDateTime) FROM construction.delivery_measure where site = '" + site + "' ORDER BY DATE(dbInsertDateTime) ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DateList.Add(rdr.GetString(0));
                    tempDate = Convert.ToDateTime(rdr.GetString(0));
                    tempx = tempDate.ToString("yyyy-MM-dd");
                    ChooseDeliveryDate.Items.Add(tempx);
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


            return DateList;
        }
        public String[,] getSiteDatax(String comp)
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
         protected string[,] getWorkerFromDB(string site, string date)
        {

            string[,] sumData = new string[5, 200];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            workerRowCount = 0;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT tabID, site, workerName, oshaNumber, workerLevel " +
                                    " FROM construction.worker_measure WHERE site = '" + site + "' AND DATE(dbInsertDateTime) = '" + date + "' limit 200;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    sumData[0, workerRowCount] = rdr.IsDBNull(0) ? "false" : rdr.GetString(0);
                    sumData[1, workerRowCount] = rdr.IsDBNull(1) ? "false" : rdr.GetString(1);
                    sumData[2, workerRowCount] = rdr.IsDBNull(2) ? "false" : rdr.GetString(2);
                    sumData[3, workerRowCount] = rdr.IsDBNull(3) ? "false" : rdr.GetString(3);
                    sumData[4, workerRowCount] = rdr.IsDBNull(4) ? "false" : rdr.GetString(4);

                    workerRowCount++;

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
        protected void getWorkerTable(string siteN, string dateN)
        {
            String[,] sumData = new String[5, 200];


            try
            {
                sumData = getWorkerFromDB(siteN, dateN);



                workerTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(400);

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
                //  header.VerticalAlign = VerticalAlign.Middle;  tabID, site, workerName, oshaNumber, workerLevel

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Worker";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "OSHA";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Level";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;


                // Add the header to a new row.
                TableRow headerRow = new TableRow();
                headerRow.Cells.Add(header1);
                headerRow.Cells.Add(header2);
                headerRow.Cells.Add(header3);
                headerRow.Cells.Add(header4);
                headerRow.Cells.Add(header5);

                // Add the header row to the table.
                workerTable.Rows.AddAt(0, headerRow);



                int numrows = workerRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type

                    a.Text = sumData[0, j];
                    a.HorizontalAlign = HorizontalAlign.Center;
                    a.Width = Unit.Percentage(20);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    b.Text = sumData[1, j];
                    b.HorizontalAlign = HorizontalAlign.Center;
                    b.Width = Unit.Percentage(20);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //task values
                    c.Text = sumData[2, j];
                    c.HorizontalAlign = HorizontalAlign.Center;
                    c.Width = Unit.Percentage(20);
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    d.Text = sumData[3, j];
                    d.HorizontalAlign = HorizontalAlign.Center;
                    d.Width = Unit.Percentage(20);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    e.Text = sumData[4, j];
                    e.HorizontalAlign = HorizontalAlign.Center;
                    e.Width = Unit.Percentage(20);
                    r.Cells.Add(e);

                    workerTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
            }

        }
        protected string[,] getProgressFromDB(string site, string date)
        {

            string[,] sumData = new string[7, 100];
                        string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            progressRowCount = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, taskName, measureType, fieldWork " +
                                    " FROM construction.progress_measure WHERE site = '" + site + "' AND DATE(dbEnterDateTime) = '" + date + "' limit 200;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    sumData[0, progressRowCount] = rdr.IsDBNull(0) ? "false" : rdr.GetString(0);
                    sumData[1, progressRowCount] = rdr.IsDBNull(1) ? "false" : rdr.GetString(1);
                    sumData[2, progressRowCount] = rdr.IsDBNull(2) ? "false" : rdr.GetString(2);
                    sumData[3, progressRowCount] = rdr.IsDBNull(3) ? "false" : rdr.GetString(3);
                    sumData[4, progressRowCount] = rdr.IsDBNull(4) ? "false" : rdr.GetString(4);
                    sumData[5, progressRowCount] = rdr.IsDBNull(5) ? "false" : rdr.GetString(5);
                    sumData[6, progressRowCount] = rdr.IsDBNull(6) ? "false" : rdr.GetString(6);

                    progressRowCount++;

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
        protected void getProgressTable(string siteN, string dateN)
        {
            String[,] sumData = new String[7, 200];


            try
            {
                sumData = getProgressFromDB(siteN, dateN);



                progressTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(400);

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
                //  header.VerticalAlign = VerticalAlign.Middle;  tabID, site, section, construction, taskName, measureType, fieldWork

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
                header5.Text = "Task";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Measure";
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = "FieldWork";
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
                progressTable.Rows.AddAt(0, headerRow);



                int numrows = progressRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type

                    a.Text = sumData[0, j];
                    a.HorizontalAlign = HorizontalAlign.Center;
                    a.Width = Unit.Percentage(14.28);
                    a.Wrap = true;
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    b.Text = sumData[1, j];
                    b.HorizontalAlign = HorizontalAlign.Center;
                    b.Width = Unit.Percentage(14.28);
                    b.Wrap = true;
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //task values
                    c.Text = sumData[2, j];
                    c.HorizontalAlign = HorizontalAlign.Center;
                    c.Width = Unit.Percentage(14.28);
                    c.Wrap = true;
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    d.Text = sumData[3, j];
                    d.HorizontalAlign = HorizontalAlign.Center;
                    d.Width = Unit.Percentage(14.28);
                    d.Wrap = true;
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    e.Text = sumData[4, j];
                    e.HorizontalAlign = HorizontalAlign.Center;
                    e.Width = Unit.Percentage(14.28);
                    e.Wrap = true;
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    f.Text = sumData[5, j];
                    f.HorizontalAlign = HorizontalAlign.Center;
                    f.Width = Unit.Percentage(14.28);
                    f.Wrap = true;
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    g.Text = sumData[6, j];
                    g.HorizontalAlign = HorizontalAlign.Center;
                    g.Width = Unit.Percentage(14.28);
                    g.Wrap = true;
                    r.Cells.Add(g);

                    progressTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
            }

        }
        protected string[,] getDeliveryFromDB(string site, string date)
        {

            string[,] sumData = new string[6, 100];
                        string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            deliveryRowCount = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, deliveryName, deliveryMeasure " +
                    " FROM construction.delivery_measure WHERE site = '" + site + "' AND DATE(dbInsertDateTime) = '" + date + "' limit 200;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    sumData[0, deliveryRowCount] = rdr.IsDBNull(0) ? "false" : rdr.GetString(0);
                    sumData[1, deliveryRowCount] = rdr.IsDBNull(1) ? "false" : rdr.GetString(1);
                    sumData[2, deliveryRowCount] = rdr.IsDBNull(2) ? "false" : rdr.GetString(2);
                    sumData[3, deliveryRowCount] = rdr.IsDBNull(3) ? "false" : rdr.GetString(3);
                    sumData[4, deliveryRowCount] = rdr.IsDBNull(4) ? "false" : rdr.GetString(4);
                    sumData[5, deliveryRowCount] = rdr.IsDBNull(5) ? "false" : rdr.GetString(5);

                    deliveryRowCount++;

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
        protected void getDeliveryTable(string siteN, string dateN)
        {
            String[,] sumData = new String[6, 200];


            try
            {
                sumData = getDeliveryFromDB(siteN, dateN);

                deliveryTable.Rows.Clear();

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
                //  header.VerticalAlign = VerticalAlign.Middle;  tabID, site, workerName, oshaNumber, workerLevel

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Worker";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "OSHA";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Level";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Level";
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.HorizontalAlign = HorizontalAlign.Center;


                // Add the header to a new row.
                TableRow headerRow = new TableRow();
                headerRow.Cells.Add(header1);
                headerRow.Cells.Add(header2);
                headerRow.Cells.Add(header3);
                headerRow.Cells.Add(header4);
                headerRow.Cells.Add(header5);
                headerRow.Cells.Add(header6);

                // Add the header row to the table.
                deliveryTable.Rows.AddAt(0, headerRow);



                int numrows = deliveryRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type

                    a.Text = sumData[0, j];
                    a.HorizontalAlign = HorizontalAlign.Center;
                    a.Width = Unit.Percentage(20);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    b.Text = sumData[1, j];
                    b.HorizontalAlign = HorizontalAlign.Center;
                    b.Width = Unit.Percentage(20);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //task values
                    c.Text = sumData[2, j];
                    c.HorizontalAlign = HorizontalAlign.Center;
                    c.Width = Unit.Percentage(20);
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    d.Text = sumData[3, j];
                    d.HorizontalAlign = HorizontalAlign.Center;
                    d.Width = Unit.Percentage(20);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    e.Text = sumData[4, j];
                    e.HorizontalAlign = HorizontalAlign.Center;
                    e.Width = Unit.Percentage(20);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    f.Text = sumData[5, j];
                    f.HorizontalAlign = HorizontalAlign.Center;
                    f.Width = Unit.Percentage(20);
                    r.Cells.Add(f);

                    deliveryTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
            }

        }
        protected void ChooseSiteDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                workerDateList.Clear();
                globalSiteName = ChooseSiteDropDownList.SelectedValue;

                workerDateList = getDateWorkerList(globalSiteName);
                if(workerDateList.Count > 0)
                {
                    DateTime tempDate = Convert.ToDateTime(workerDateList[workerDateList.Count - 1]);
                    string tempx = tempDate.ToString("yyyy-MM-dd");
                    getWorkerTable(globalSiteName, tempx);
                }

                progressDateList = getDateProgressList(globalSiteName);
                if (progressDateList.Count > 0)
                {
                    DateTime tempDate = Convert.ToDateTime(progressDateList[progressDateList.Count - 1]);
                    string tempx = tempDate.ToString("yyyy-MM-dd");
                    getProgressTable(globalSiteName, tempx);
                }

                deliveryDateList = getDateDeliveryList(globalSiteName);
                if (deliveryDateList.Count > 0)
                {
                    DateTime tempDate = Convert.ToDateTime(deliveryDateList[deliveryDateList.Count - 1]);
                    string tempx = tempDate.ToString("yyyy-MM-dd");
                    getDeliveryTable(globalSiteName, tempx);
                }
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }

        }

        protected void ChooseWorkerDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
                getWorkerTable(globalSiteName, ChooseWorkerDate.SelectedValue);
                getProgressTable(globalSiteName, ChooseProgressDate.SelectedValue);
                getDeliveryTable(globalSiteName, ChooseDeliveryDate.SelectedValue);
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected void ChooseProgressDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getWorkerTable(globalSiteName, ChooseWorkerDate.SelectedValue);
                getProgressTable(globalSiteName, ChooseProgressDate.SelectedValue);
                getDeliveryTable(globalSiteName, ChooseDeliveryDate.SelectedValue);
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected void ChooseDeliveryDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getWorkerTable(globalSiteName, ChooseWorkerDate.SelectedValue);
                getProgressTable(globalSiteName, ChooseProgressDate.SelectedValue);
                getDeliveryTable(globalSiteName, ChooseDeliveryDate.SelectedValue);
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }
    }
}