using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Act_site
{
    public partial class siteTablets : System.Web.UI.Page
    {

        String companyName = "NA";
        String globalSiteName = "NA";
        string tabId = "NA";
        string tabletTable = "";
        int rowCount = 0;
        string[,] returnData = new string[31, 500];
        string[] summaryData = new string[11];

        int distinctTabCount = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Company"] != null)
            {
                companyName = Session["Company"].ToString();
            }

            if (Session["Site"] != null )
            {
                globalSiteName = Session["Site"].ToString();
            }

            if(Session["TabID"] != null)
            {
                tabId = Session["TabID"].ToString();
            }
            /*
            //need if not post back
            if (!IsPostBack)
            {
                calStart.Visible = false;
                calEnd.Visible = false;
                DropDownListTab.Visible = false;
                ButtonUpdateChoice.Visible = false;
            }

            string v = Request.QueryString["tabletId"];
            if (v != null)
            {
                tabId = v;
               // getTabFromDB(tabId);
            }

            if (v == null)
            {
              //  prepPageNoQuery();
               // getTabFromDB("MDC-001");
                //getBasicTable();
            }
            */
        }

        public List<string> getTabList(string comp)
        {
            List<string> returnData = new List<string>();
            string returnDataStr = "";
            //  string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            //  string image_path = ConfigurationManager.AppSettings["Test1"];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT tabletID FROM construction.tablet_summary WHERE company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {                 
                    returnDataStr = rdr.IsDBNull(0) ? "NA" : rdr.GetString(0);
                    returnData.Add(returnDataStr);
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

        public List<string[]> getSiteData(String comp)
        {
            List<string[]> returnData = new List<string[]>();

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

        protected string getBasicTable()
        {
            try
            {
                string[,] tableData = new string[12, 500];
                tableData = getTabFromDB("MDC-001");
                string[] sumTableData = new string[5];

                tabletTable = tabletTable + "<p>Tablet Summary</p>";

                tabletTable = tabletTable + "<table class=\"tabStyle\"><tr><th style=\"width:8%\">Tablet Id</th><th style=\"width:6%\">Location</th><th style=\"width:8%\">Project</th><th style=\"width:8%\">Contractor</th><th style=\"width:8%\">Worker</th>" +
                                             "<th style=\"width:8%\">Town</th><th style=\"width:18%\">Date & Time</th></tr>";

                if (summaryData[1].Length > 4 && summaryData[2].Length > 4)
                {
                    sumTableData[0] = "<div style=\"width: 85px; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;><a href=\"#\" class=\"tempxx\"\">Mouse Over," + summaryData[1] + "," + summaryData[2] + "</a></div>";
                }
                else { sumTableData[0] = "NA"; }

                if (summaryData[3].Length > 2)
                {
                    sumTableData[1] = "<a href=\"/Projects.aspx/?projectid=" + summaryData[4] + "\">" + summaryData[3] + "</a>";
                }
                else { sumTableData[1] = "NA"; }

                if (summaryData[5].Length > 2)
                {
                    sumTableData[2] = "<a href=\"/contractors.aspx/?contractorid=" + summaryData[6] + "\">" + summaryData[5] + "</a>";
                }
                else { sumTableData[2] = "NA"; }

                if (summaryData[7].Length > 2)
                {
                    sumTableData[3] = "<a href=\"/Workers.aspx/?workerid=" + summaryData[8] + "\">" + summaryData[7] + "</a>";
                }
                else { sumTableData[3] = "NA"; }

                if (summaryData[9].Length > 2)
                {
                    sumTableData[4] = "<a href=\"/Towns.aspx/?townid=" + summaryData[9] + "\">" + summaryData[9] + "</a>"; //+ returnData[5, y] + "\">" + returnData[5, y] + "</a>";
                }
                else { sumTableData[4] = "NA"; }


                tabletTable = tabletTable + "<tr><td>" + summaryData[0] + "</td><td>" + sumTableData[0] + "</td><td>" + sumTableData[1] + "</td><td>" + sumTableData[2] + "</td><td>" +
                                sumTableData[3] + "</td><td>" + sumTableData[4] + "</td><td>" + summaryData[10] + "</td></tr>";

                tabletTable = tabletTable + "</table>";


                tabletTable = tabletTable + "<p>Tablet Movements</p>";
                // have to do better with the sytling -->table>tbody>td:nth-child(2){font-weight: bolder;}
                tabletTable = tabletTable + "<table class=\"tabStyle\"><tr><th style=\"width:8%\">Tablet Id</th><th style=\"width:10%\">Date Time</th><th style=\"width:5%\">Altitude</th><th style=\"width:5%\">Sats Count</th><th style=\"width:5%\">Temp</th>" +
                    "<th style=\"width:5%\">Bat Volts</th><th style=\"width:6%\">Location</th><th style=\"width:8%\">Needs Charge</th><th style=\"width:5%\">Screen On</th><th style=\"width:5%\">Speed</th><th style=\"width:5%\">Distance</th><th style=\"width:5%\">Minutes</th></tr>";

                for (int y = 0; y < rowCount; y++)
                {


                    if (returnData[4, y].Length > 4 && returnData[5, y].Length > 4)
                    {
                        tableData[4, y] = "<div style=\"width: 85px; text-overflow: ellipsis; white-space: nowrap; overflow: hidden;><a href=\"#\" class=\"tempxx\"\">Mouse Over," + returnData[4, y] + "," + returnData[5, y] + "</a>";
                    }
                    else { tableData[4, y] = "NA"; }

                    tableData[5, y] = returnData[13, y] + returnData[14, y];

                }


                for (int x = 0; x < rowCount; x++)
                {
                    tabletTable = tabletTable + "<tr><td>" + returnData[0, x] + "</td><td>" + returnData[1, x] + "</td><td>" + returnData[6, x] + "</td><td>" + returnData[8, x] + "</td><td>" + returnData[11, x] + "</td><td>" + returnData[13, x] +
                        "</td><td>" + tableData[4, x] + "</td><td>" + returnData[23, x] + "</td><td>" + returnData[15, x] + "</td><td>" + returnData[9, x] +
                        "</td><td>" + returnData[28, x] + "</td><td>" + returnData[29, x] + "</td></tr>";
                }
                tabletTable = tabletTable + "</table>";
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
            }

            return tabletTable;
        }

        protected string[,] getTabFromDB(string tabID)
        {
            rowCount = 0;

            string cs = @"server=192.168.1.53;user=root; password=United@5; database=jvbs; SslMode=none;";

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string stm = "SELECT tabletID, viewDateTime, postDate, postTime, gpsLat, gpsLon, gpsAlt, gpsValidity, gpsSats, gpsSpeed, gpsCourse, boxTemp, " +
                             " chargeVoltage, batteryVoltage, cellBatteryVoltage, isPiOn, " +
                             " gpsDateTime, insertTimeStamp, project, project_id, contractor, contractor_id, hoursSinceCharge, chargeAlert, " +
                             " hoursSince_lastValid, alt_travel, speed_travel, course_travel, distance_travel, minutes_travel, did_travel FROM tablet_movement WHERE tabletID = '" + "MDC-001" +
                             "' ORDER BY viewDateTime";
                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, rowCount] = rdr.GetString(0); //tab id
                    returnData[1, rowCount] = rdr.GetString(1); //contractor
                    returnData[2, rowCount] = rdr.GetString(2); //contractor ID
                    returnData[3, rowCount] = rdr.GetString(3);
                    returnData[4, rowCount] = rdr.GetString(4);
                    returnData[5, rowCount] = rdr.GetString(5);
                    returnData[6, rowCount] = rdr.GetString(6);
                    returnData[7, rowCount] = rdr.GetString(7);
                    returnData[8, rowCount] = rdr.GetString(8);
                    returnData[9, rowCount] = rdr.GetString(9);
                    returnData[10, rowCount] = rdr.GetString(10);
                    returnData[11, rowCount] = rdr.GetString(11);
                    returnData[12, rowCount] = rdr.GetString(12);
                    returnData[13, rowCount] = rdr.GetString(13);
                    returnData[14, rowCount] = rdr.GetString(14);
                    returnData[15, rowCount] = rdr.GetString(15);
                    returnData[16, rowCount] = rdr.GetString(16);
                    returnData[17, rowCount] = rdr.GetString(17);
                    returnData[18, rowCount] = rdr.GetString(18);
                    returnData[19, rowCount] = rdr.GetString(19);
                    returnData[20, rowCount] = rdr.GetString(20);
                    returnData[21, rowCount] = rdr.GetString(21);
                    returnData[22, rowCount] = rdr.GetString(22);
                    returnData[23, rowCount] = rdr.GetString(23);
                    returnData[24, rowCount] = rdr.GetString(24);
                    returnData[25, rowCount] = rdr.GetString(25);
                    returnData[26, rowCount] = rdr.GetString(26);
                    returnData[27, rowCount] = rdr.GetString(27);
                    returnData[28, rowCount] = rdr.GetString(28);
                    returnData[29, rowCount] = rdr.GetString(29);
                    returnData[30, rowCount] = rdr.GetString(30);

                    rowCount++;
                }

                rdr.Close();
                /*
                string mainTabSQL = "SELECT DISTINCT tabletID, GPSlat, GPSlon, project, project_id, contractor, contractor_id, worker, " +
                                    " badgeNumber, town, last_update FROM main_status WHERE tabletID = '" + "MDC-001" + "' ;";

                MySqlCommand cmd1 = new MySqlCommand(mainTabSQL, conn);
                rdr1 = cmd1.ExecuteReader();

                while (rdr1.Read())
                {
                    summaryData[0] = rdr1.GetString(0);
                    summaryData[1] = rdr1.GetString(1);
                    summaryData[2] = rdr1.GetString(2);
                    summaryData[3] = rdr1.GetString(3);
                    summaryData[4] = rdr1.GetString(4);
                    summaryData[5] = rdr1.GetString(5);
                    summaryData[6] = rdr1.GetString(6);
                    summaryData[7] = rdr1.GetString(7);
                    summaryData[8] = rdr1.GetString(8);
                    summaryData[9] = rdr1.GetString(9);
                    summaryData[10] = rdr1.GetString(10);
                }
                */
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
            return returnData;
        }

        protected void CheckBoxShowChoice_CheckedChanged(object sender, System.EventArgs e)
        {
            string[] tabsFromDB = new string[50];

            tabsFromDB = getDistinctTabletsDB();

            if (CheckBoxShowChoice.Checked == true)
            {
                calStart.Visible = true;
                calEnd.Visible = true;
                DropDownListTab.Visible = true;
                ButtonUpdateChoice.Visible = true;

                for (int x = 0; x < distinctTabCount; x++)
                {
                    DropDownListTab.Items.Add(new ListItem(tabsFromDB[x]));
                }
                // DropDownListTab.Items.Insert(0, new ListItem("Add New", ""));

                DateTime thirtyDaysBack = DateTime.Today.AddDays(-30);
                calStart.TodaysDate = thirtyDaysBack;
                calStart.SelectedDate = calStart.TodaysDate;

                DateTime endDateToday = DateTime.Now;
                calEnd.TodaysDate = endDateToday;
                calEnd.SelectedDate = calEnd.TodaysDate;

            }
            else
            {
                calStart.Visible = false;
                calEnd.Visible = false;
                DropDownListTab.Visible = false;
                ButtonUpdateChoice.Visible = false;
                DropDownListTab.Items.Clear();

                //  s.Items.Remove("nameofitemtoremovehere") ' or s.Items.RemoveAt(3) 

            }

        }

        private void prepPageNoQuery()
        {
            string[] tabsFromDB = new string[50];

            tabsFromDB = getDistinctTabletsDB();

            calStart.Visible = true;
            calEnd.Visible = true;
            DropDownListTab.Visible = true;
            ButtonUpdateChoice.Visible = true;
            CheckBoxShowChoice.Checked = true;
            DropDownListTab.Items.Add(new ListItem());
            for (int x = 0; x < distinctTabCount; x++)
            {
                DropDownListTab.Items.Add(new ListItem(tabsFromDB[x]));
            }
            // DropDownListTab.Items.Insert(0, new ListItem("Add New", ""));

            DateTime thirtyDaysBack = DateTime.Today.AddDays(-3000);
            calStart.TodaysDate = thirtyDaysBack;
            calStart.SelectedDate = calStart.TodaysDate;

            DateTime endDateToday = DateTime.Now;
            calEnd.TodaysDate = endDateToday;
            calEnd.SelectedDate = calEnd.TodaysDate;

        }

        protected void ButtonUpdateChoice_Click(Object sender, EventArgs e)
        {
            DateTime startDate;
            DateTime endDate;
            string chosenTablet = "";

            chosenTablet = DropDownListTab.SelectedItem.Value;
            startDate = calStart.SelectedDate;
            endDate = calEnd.SelectedDate;

            CheckBoxShowChoice.Checked = false;
            calStart.Visible = false;
            calEnd.Visible = false;
            DropDownListTab.Visible = false;
            ButtonUpdateChoice.Visible = false;
            DropDownListTab.Items.Clear();
            if (chosenTablet != "" && startDate < endDate)
            {


            }

        }

        private string[] getDistinctTabletsDB()
        {
            string[] returnTabs = new string[50];
            distinctTabCount = 0;

            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string stm = "SELECT DISTINCT tabletID FROM ardlocation;";

                MySqlCommand cmd = new MySqlCommand(stm, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnTabs[distinctTabCount] = rdr.GetString(0); //tab id
                    distinctTabCount++;
                }

                returnTabs[0] = "MDC-001"; //tab id
                distinctTabCount = 1;

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

            return returnTabs;
        }

        protected void ChooseSiteDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                System.Diagnostics.Debug.WriteLine("ChooseSiteDropDownList:" + ChooseSiteDropDownList.SelectedValue);
                System.Diagnostics.Debug.WriteLine("Page.IsPostBack:" + Page.IsPostBack);
                System.Diagnostics.Debug.WriteLine("ChooseSiteDropDownList.SelectedValue:" + ChooseSiteDropDownList.SelectedIndex);

                globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

              //  loadPayrollDateDropDown(companyName, globalSiteName);

              //  loadWorkerNameDropDown(companyName, globalSiteName);

                Session["Site"] = globalSiteName;

                siteNameLabel.Text = globalSiteName;

              //  DateTime beginDate = Convert.ToDateTime(PayrollViewDropDownList.Items[0].ToString());
              //  string[,] returnData = getPayrollData(ChooseSiteDropDownList.Items[0].ToString(), beginDate);
             //   getPayrollTable(returnData, beginDate, finalPayrollRows);


              //  workerName = WorkerDropDownList.SelectedItem.ToString();

             //   List<workerDBsections> workerList = getWorkerTaskDataFromDB(companyName, globalSiteName, workerName);

                //workerList = workerList.OrderBy(w => w.sections).ThenBy(w => w.constructions).ThenBy(w => w.tasks).ThenBy(w => w.startDate).ToList();

              //  getWorkerTable(workerList, workerName);

                // getDetailedTable(globalSiteName, workerName);

                //the payroll part
                String[,] returnDataWorker = new String[100, 30];
                DateTime beginDateWorker;

              //  beginDateWorker = Convert.ToDateTime(PayrollViewDropDownList.Items[0].ToString());

               // returnDataWorker = getPayrollData(globalSiteName, beginDateWorker);

              //  getPayrollTable(returnDataWorker, beginDateWorker, finalPayrollRows);

            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected void WeekViewDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String[,] returnData = new String[100, 30];
                DateTime beginDate;

                String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

                globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

              //  beginDate = Convert.ToDateTime(PayrollViewDropDownList.SelectedValue.ToString());

             //   returnData = getPayrollData(globalSiteName, beginDate);

             //   getPayrollTable(returnData, beginDate, finalPayrollRows);

                //the worker part
                List<workerDBsections> workerList = new List<workerDBsections>();

             //   workerName = WorkerDropDownList.SelectedItem.ToString();

             //   workerList = getWorkerTaskDataFromDB(companyName, globalSiteName, workerName);

                workerList = workerList.OrderBy(w => w.sections).ThenBy(w => w.constructions).ThenBy(w => w.tasks).ThenBy(w => w.startDate).ToList();

             //   getWorkerTable(workerList, workerName);
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }

        }

    }
}