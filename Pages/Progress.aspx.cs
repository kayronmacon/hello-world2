using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Text;

namespace Act_site
{


    public partial class siteProgress : System.Web.UI.Page
    {

        int sumHoursRow = 0;
        int sumDataRow = 0;
        int detailedDataRow = 0;

        string sumHoursTable = "";
        string taskHoursTable = "";
        string sumWorkerTable = "";
        string detailWorkerTable = "";

        String companyName = "NA";
        String globalSiteName = "Choose a site";
        int siteRowCount = 0;
        int constructCount = 0;
        int taskCount = 0;

        int rowCountStartDatePayroll = 0;
        int finalPayrollRows = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
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
                   // siteNameLabel.Text = ChooseSiteDropDownList.Items[0].ToString();
                    loadPayrollDateDropDown(companyName, Convert.ToString(ChooseSiteDropDownList.Items[0]));
                }
            }
            catch(Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        private void ShowPopUpMsg(string msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("alert('");
            sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
            sb.Append("');");
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "showalert", sb.ToString(), true);
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
          //  ShowPopUpMsg(Session["roleID"].ToString());
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
                Session["Site"] = ChooseSiteDropDownList.SelectedItem.Text;
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
                for (int x = 0; x < siteRowCount; x++)
                {
                    ChooseSiteDropDownList.Items.Add(new ListItem(siteData[0, x], siteData[0, x]));
                }

                
               // loadPayrollDateDropDown(companyName, String globalSiteNameStr);

                getBasicHoursTable(Convert.ToString(ChooseSiteDropDownList.Items[0]));
               // loadPayrollDateDropDown(companyName, Convert.ToString(ChooseSiteDropDownList.Items[0]));
            }
        }

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
        protected void chooseSiteButtonClick(object sender, EventArgs e)
        {
            //   String noSiteError = "No site was choosen to update";
            //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + noSiteError + "');", true);

            // ClientScript.RegisterStartupScript(this.GetType(), "RefreshOpener", "f2();", true); expects a javascript function named f2();

           // String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

           // globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

           // loadPayrollDateDropDown(companyName, globalSiteName);

          //  getBasicHoursTable(globalSiteName);

            //  String[] tempTownArray = temptown.Split('-');

            //   globalSiteName = tempTownArray[0];
            // globalTownName = tempTownArray[1];


        }

        protected void loadPayrollDateDropDown(String companyNameStr, String globalSiteNameStr)
        {
            DateTime[] siteData = new DateTime[500];

            siteData = getStartDatesPayroll(companyName, globalSiteNameStr);

            if (ChooseSiteDropDownList.Items.Count == 0)
            {
                ChooseSiteDropDownList.Items.Add(new ListItem("No current sites"));
            }


            PayrollViewDropDownList.Items.Clear();

            if (rowCountStartDatePayroll > 0)
            {
                for (int x = 0; x < rowCountStartDatePayroll; x++)
                {
                    PayrollViewDropDownList.Items.Add(new ListItem(siteData[x].ToString("yyyy-MM-dd"), siteData[x].ToString("yyyy-MM-dd")));
                }
                string tempxxx = PayrollViewDropDownList.Items[0].ToString();
                PayrollViewDropDownList.Items.Add(new ListItem("Entire time"));
                getBasicTaskHoursTable(globalSiteNameStr, Convert.ToDateTime(tempxxx), false);
                //getPayrollTable(returnData, beginDate, finalPayrollRows);
            } 

        }

        protected DateTime[] getStartDatesPayroll(String companyNameStr, String globalSiteNameStr)
        {
            // CultureInfo cul = CultureInfo.CurrentCulture;
            DateTime[] returnValues = new DateTime[500];
            DateTime tempDT;
            rowCountStartDatePayroll = 0;
            int subtractValue = 0;

            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT workDate " +
                                    " FROM work_data WHERE site = '" + globalSiteNameStr + "' ORDER BY workDate LIMIT 500;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    tempDT = Convert.ToDateTime(rdr.GetString(0));

                    if (rowCountStartDatePayroll > 0)
                    {
                        for (int x = 0; x < rowCountStartDatePayroll; x++)
                        {
                            if (tempDT > returnValues[x].AddDays(6) && x == rowCountStartDatePayroll - 1)
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

                                returnValues[rowCountStartDatePayroll] = tempDT.AddDays(subtractValue);
                                rowCountStartDatePayroll++;
                            }
                        }
                    }


                    if (rowCountStartDatePayroll == 0)
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

                        returnValues[rowCountStartDatePayroll] = tempDT.AddDays(subtractValue);
                        rowCountStartDatePayroll++;
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

        protected void chooseWeekButtonClick(object sender, EventArgs e)
        {
            String[,] returnData = new String[10,100];
            DateTime beginDate;
            Boolean allTime = false;

            String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

            globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

            if(PayrollViewDropDownList.SelectedValue.ToString() == "Entire time")
            {
                beginDate = new DateTime(2012, 1, 1, 12, 12, 12);
                allTime = true;
            }
            else { beginDate = Convert.ToDateTime(PayrollViewDropDownList.SelectedValue.ToString()); }

          
        //   returnData = getTaskProgressHoursFromDB(globalSiteName, beginDate, allTime);

            getBasicTaskHoursTable(globalSiteName, beginDate, allTime);
         //   getPayrollTable(returnData, beginDate, finalPayrollRows);


        }

        protected string[,] getTaskProgressHoursFromDB(String site, DateTime beginDT, Boolean all)
        {

            string[,] sumData = new string[8, 100];
            taskCount = 0;

            DateTime endDT = beginDT.AddDays(7);


            if (all == true) { endDT = beginDT.AddYears(30); }
            if (all == false) { endDT = beginDT.AddDays(7); }

            String beginStr = beginDT.ToString("yyyy-MM-dd");
            String endStr = endDT.ToString("yyyy-MM-dd");

            String[] hours = new String[4];

            hours[0] = "hours1";
            hours[1] = "hours2";
            hours[2] = "hours3";
            hours[3] = "hours4";

            String[] tasks = new String[4];

            tasks[0] = "task1";
            tasks[1] = "task2";
            tasks[2] = "task3";
            tasks[3] = "task4";

            double hoursDouble = 0;


            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString; //WCFConnection
            string csWCF = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString; //WCFConnection

            MySqlConnection connWCF = null;
            MySqlDataReader rdrWCF = null;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            MySqlConnection connProg = null;
            MySqlDataReader rdrProg = null;

            try
            {

                connWCF = new MySqlConnection(csWCF);
                connWCF.Open();

                String progressHoursSQL = "";

                MySqlCommand cmdWCF = new MySqlCommand(progressHoursSQL, connWCF);


                progressHoursSQL = "SELECT DISTINCT siteName, sectionName, construction, task, measure, fieldWork " +
                                         " FROM wcf_data.tasks where siteName = '" + site + "'";

                cmdWCF.CommandText = progressHoursSQL;
                rdrWCF = cmdWCF.ExecuteReader();
                while (rdrWCF.Read())
                {
                    sumData[0, taskCount] = rdrWCF.GetString(0);
                    sumData[1, taskCount] = rdrWCF.GetString(1);
                    sumData[2, taskCount] = rdrWCF.GetString(2);
                    sumData[3, taskCount] = rdrWCF.GetString(3);
                    sumData[4, taskCount] = rdrWCF.GetString(4);
                    sumData[7, taskCount] = rdrWCF.GetString(5) == "1" ? "true" : "false";
                    taskCount++;
                }


                rdrWCF.Close();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }
            finally
            {

                if (rdrWCF != null)
                {
                    rdrWCF.Close();
                }
                if (connWCF != null)
                {
                    connWCF.Close();
                }

            }

            try
            {
                connProg = new MySqlConnection(cs);
                connProg.Open();

                string workerWipSQL = "SELECT DISTINCT SELECT sum(hours1) FROM construction.work_data WHERE task1 LIKE '%Bridging%' AND siteName = '" + site + "'";


                MySqlCommand cmd = new MySqlCommand(workerWipSQL, connProg);

                for (int x = 0; x < taskCount; x++)
                {

                        cmd.CommandText = "SELECT DISTINCT sum(taskData) FROM construction.progress_data where section = '" + sumData[1, x] + "' AND construction = '" + sumData[2, x] +
                        "' AND taskName = '" + sumData[3, x] + "' AND workDate BETWEEN '" + beginStr + "' AND '" + endStr + "';" ;
                        rdrProg = cmd.ExecuteReader();

                        while (rdrProg.Read())
                        {
                            if (rdrProg.IsDBNull(0) == false && rdrProg.HasRows)
                            {
                                Boolean temp = Convert.IsDBNull(rdrProg.GetString(0));
                                 sumData[5, x] = Convert.IsDBNull(rdrProg.GetString(0)) ? "0" : rdrProg.GetString(0);
                            }
                            if (rdrProg.IsDBNull(0) == true && rdrProg.HasRows)
                            {
                                sumData[5, x] = "0";
                            }
                    }
                        if (rdrProg.IsClosed == false) { rdrProg.Close(); }
                    
                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            catch (Exception ex)
            {
                string temp = ex.ToString();
            }

            finally
            {
                if (rdrProg != null)
                {
                    rdrProg.Close();
                }
                if (connProg != null)
                {
                    connProg.Close();
                }

            }

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = " ";


                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);

                for (int x = 0; x < taskCount; x++)
                {

                    for (int y = 0; y < 4; y++)
                    {
                        cmd.CommandText = "SELECT DISTINCT sum(" + hours[y] + ") FROM construction.work_data WHERE " + tasks[y] + " LIKE '%" + sumData[3, x] + "%' AND site = '" + site + "' AND " 
                            + tasks[y] + " LIKE '%" + sumData[2, x] + "%' AND '" +  hours[y] + "' > '0' " + "AND workDate BETWEEN '" + beginStr + "' AND '" + endStr + "'; " ;
                        rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            if (rdr.IsDBNull(0) == false && rdr.HasRows)
                            {
                                Boolean temp = Convert.IsDBNull(rdr.GetString(0));
                                hoursDouble += Convert.IsDBNull(rdr.GetString(0)) ? 0 : Convert.ToDouble(rdr.GetString(0));
                            }
                        }
                        if (rdr.IsClosed == false) { rdr.Close(); }
                    }

                    sumData[6, x] = Convert.ToString(hoursDouble);
                   // sumData[5, x] = Convert.ToString(hoursDouble - Convert.ToDouble(sumData[3, x]));
                    hoursDouble = 0;
                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            catch (Exception ex)
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

        protected void getBasicTaskHoursTable(String site, DateTime beginDT, Boolean all)
        {
            string[,] sumData = new string[9, 100];

            DateTime endDT = beginDT.AddDays(7);

            double worked = 0;

            if (all == true) { endDT = beginDT.AddYears(30); }
            if (all == false) { endDT = beginDT.AddDays(6); }

            String beginStr = beginDT.ToString("yyyy-MM-dd");
            String endStr = endDT.ToString("yyyy-MM-dd");

            try
            {
                sumData = getTaskProgressHoursFromDB(site, beginDT, all);
                taskHoursTable = "";

                if (all == false) { taskHoursTable = taskHoursTable + "<h2>Progress vs. hours for " + beginStr + " to " + endStr + "</h2>"; }
                if (all == true) { taskHoursTable = taskHoursTable + "<h2>Progress vs. hours for the entire job</h2>"; }

                taskHoursTable = taskHoursTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:24%\">Site</th>" +
                                            "<th style=\"width:14%\">Section</th>" +
                                            "<th style=\"width:14%\">Construction</th>" +
                                            "<th style=\"width:12%\">Task</th>" +
                                            "<th style=\"width:12%\">Measure</th>" +
                                            "<th style=\"width:8%\">Data</th>" +
                                            "<th style=\"width:8%\">Hours</th>" +
                                            "<th style=\"width:8%\">Field Work</th>" +

                                            "</tr>";

                for (int x = 0; x < taskCount; x++)
                {
                    //     string temp = sumData[0, x];
                    //      temp = sumData[1, x];
                    //     temp = sumData[2, x];
                    //    forecast += Convert.ToDouble(sumData[3, x]);
                         worked += Convert.ToDouble(sumData[6, x]);
                    //     variance += Convert.ToDouble(sumData[5, x]);
                    //  temp = sumData[6, x];


                    taskHoursTable = taskHoursTable + "<tr><td>"



                                                + sumData[0, x] + "</td><td>"
                                                + sumData[1, x] + "</td><td>"
                                                + sumData[2, x] + "</td><td>"
                                                + sumData[3, x] + "</td><td>"
                                                + sumData[4, x] + "</td><td>"
                                                + sumData[5, x] + "</td><td>"
                                                + sumData[6, x] + "</td><td>"
                                                + sumData[7, x] + "</td></tr>"; //Convert.ToInt32(sumData[6, x]) -

                }

                taskHoursTable = taskHoursTable + "<tr><td colspan = \"6\"><b>" + "Total hours</b></td><td><b>"
                               //                + Convert.ToString(forecast) + "</b></td><td><b>"
                               //               + Convert.ToString(worked) + "</b></td><td><b>"
                               + Convert.ToString(worked) + "</b></td><td>";
                //                + " " + "</td></tr>"; //Convert.ToInt32(sumData[6, x]) -


                // colspan = "2"

                taskHoursTable = taskHoursTable + "</table>";
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }
       //     Literal1.Text = taskHoursTable;
            Literal2.Text = taskHoursTable;

        }

        protected string[,] getTaskProgressFromDB()
        {

            string[,] sumData = new string[7, 100];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                string workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, taskName, taskType, sum(taskData) " +
                                    " FROM progress_data group by tabID, site, section, construction, taskName, taskType;";

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

        protected string getBasicHoursTable(String site)
        {
            string[,] sumData = new string[9, 100];

            double forecast = 0;
            double worked = 0;
            double variance = 0;

            try
            {
                sumData = getSummaryProgressHoursFromDB(site);
                sumHoursTable = "";

                sumHoursTable = sumHoursTable + "<h2>To date hours for the site</h2>";

                sumHoursTable = sumHoursTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:24%\">Site</th>" +
                                            "<th style=\"width:16%\">Section</th>" +
                                            "<th style=\"width:16%\">Construction</th>" +
                                            "<th style=\"width:12%\">Budget</th>" +
                                            "<th style=\"width:12%\">Worked</th>" +                                        
                                            "<th style=\"width:12%\">Variance</th>" +
                                            "<th style=\"width:8%\">Field Work</th>" +

                                            "</tr>";

                for (int x = 0; x < constructCount; x++)
                {
                    //     string temp = sumData[0, x];
                    //      temp = sumData[1, x];
                    //     temp = sumData[2, x];
                    forecast += Convert.ToDouble(sumData[3, x]);
                    worked += Convert.ToDouble(sumData[4, x]);
                    variance += Convert.ToDouble(sumData[5, x]);
                    //  temp = sumData[6, x];


                    sumHoursTable = sumHoursTable + "<tr><td>"

                        

                                                + sumData[0, x] + "</td><td>"
                                                + sumData[1, x] + "</td><td>"
                                                + sumData[2, x] + "</td><td>"
                                                + sumData[3, x] + "</td><td>"
                                                + sumData[4, x] + "</td><td>"
                                                + sumData[5, x] + "</td><td>"
                                                + sumData[6, x] + "</td></tr>"; //Convert.ToInt32(sumData[6, x]) -
                    
                }

                sumHoursTable = sumHoursTable + "<tr><td colspan = \"3\"><b>" + "Totals</b></td><td><b>"
                            + Convert.ToString(forecast) + "</b></td><td><b>"
                            + Convert.ToString(worked) + "</b></td><td><b>"
                            + Convert.ToString(variance) + "</b></td><td>"
                            + " " + "</td></tr>"; //Convert.ToInt32(sumData[6, x]) -


                // colspan = "2"

                sumHoursTable = sumHoursTable + "</table>";
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }
            Literal1.Text = sumHoursTable;
            return sumHoursTable;
        }

        protected string getBasicTable()
        {
            string[,] sumData = new string[7, 100];
            try
            {
                sumData = getSummaryProgressFromDB();
                sumWorkerTable = "";

                sumWorkerTable = sumWorkerTable + "<p>Summary progress</p>";

                sumWorkerTable = sumWorkerTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:8%\">Tab Id</th>" +
                                            "<th style=\"width:19%\">Site</th>" +
                                            "<th style=\"width:18%\">Section</th>" +
                                            "<th style=\"width:19%\">Construction</th>" +
                                            "<th style=\"width:10%\">Task</th>" +
                                            "<th style=\"width:10%\">Task Measure</th>" +
                                            "<th style=\"width:8%\">Data</th>" +

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

        protected string[,] getSummaryProgressHoursFromDB(String site)
        {

            string[,] sumData = new string[7, 100];
            constructCount = 0;

            String[] hours = new String[4];

            hours[0] = "hours1";
            hours[1] = "hours2";
            hours[2] = "hours3";
            hours[3] = "hours4";

            String[] sections = new String[4];
            String[] constructions = new String[4];

            sections[0] = "section1";
            sections[1] = "section2";
            sections[2] = "section3";
            sections[3] = "section4";

            constructions[0] = "construction1";
            constructions[1] = "construction2";
            constructions[2] = "construction3";
            constructions[3] = "construction4";

            double hoursDouble = 0;


            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString; //WCFConnection
            string csWCF = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString; //WCFConnection

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            MySqlConnection connWCF = null;
            MySqlDataReader rdrWCF = null;

            try
            {

                connWCF = new MySqlConnection(csWCF);
                connWCF.Open();

                String progressHoursSQL = "";

                MySqlCommand cmdWCF = new MySqlCommand(progressHoursSQL, connWCF);


                    progressHoursSQL = "SELECT DISTINCT siteName, sectionName, construction, hours, fieldWork " +
                                             " FROM wcf_data.construction where siteName = '" + site + "'";

                    cmdWCF.CommandText = progressHoursSQL;
                    rdrWCF = cmdWCF.ExecuteReader();
                    while (rdrWCF.Read())
                    {
                        sumData[0, constructCount] = rdrWCF.GetString(0); //site
                        sumData[1, constructCount] = rdrWCF.GetString(1); //section
                        sumData[2, constructCount] = rdrWCF.GetString(2); //construction
                        sumData[3, constructCount] = rdrWCF.GetString(3); //hours
                        sumData[6, constructCount] = rdrWCF.GetString(4) == "1" ? "true" : "false";
                        constructCount++;
                    }

                    
                    rdrWCF.Close();

                
            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {

                if (rdrWCF != null)
                {
                    rdrWCF.Close();
                }
                if (connWCF != null)
                {
                    connWCF.Close();
                }

            }

            try { 
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "";


                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);

                for (int x = 0; x < constructCount; x++)
                {

                    for (int y = 0; y < 4; y++)
                    {
                        cmd.CommandText = "SELECT sum(" + hours[y] + ") FROM construction.work_data WHERE " + sections[y] + " = '" + sumData[1, x] + "' AND " + constructions[y] + " = '" + sumData[2, x] + "' AND site = '" + site + "' AND " + hours[y] + " > 0 ";
                        rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            if (rdr.IsDBNull(0) == false && rdr.HasRows)
                            {
                               // Boolean temp = Convert.IsDBNull(rdr.GetString(0));
                                hoursDouble += Convert.IsDBNull(rdr.GetString(0)) ? 0 : Convert.ToDouble(rdr.GetString(0));
                            }
                        }
                        if (rdr.IsClosed == false) { rdr.Close(); }
                    }

                    sumData[4, x] = Convert.ToString(hoursDouble);
                    sumData[5, x] = Convert.ToString(hoursDouble - Convert.ToDouble(sumData[3, x]) );
                    hoursDouble = 0;
                }

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            catch (Exception ex)
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

        protected string[,] getSummaryProgressFromDB()
        {

            string[,] sumData = new string[7, 100];
             string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                string workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, taskName, taskType, sum(taskData) " +
                                    " FROM progress_data group by tabID, site, section, construction, taskName, taskType;";

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
                detData = getDetailedProgressFromDB();
                detailWorkerTable = "";

                detailWorkerTable = detailWorkerTable + "<p>Detailed Progress</p>";

                detailWorkerTable = detailWorkerTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:8%\">Tab Id</th>" +
                                            "<th style=\"width:12%\">Site</th>" +
                                            "<th style=\"width:12%\">Section</th>" +
                                            "<th style=\"width:12%\">Construction</th>" +
                                            "<th style=\"width:14%\">Task Name</th>" +
                                            "<th style=\"width:10%\">Task Measure</th>" +
                                            "<th style=\"width:8%\">Data</th>" +
                                            "<th style=\"width:8%\">Work Date</th>" +
                                            "<th style=\"width:16%\">Approver Name</th>" +
                                            

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
                                                + detData[8, x] + "</td></tr>";
                }
                detailWorkerTable = detailWorkerTable + "</table>";
            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            return detailWorkerTable;
        }

        protected string[,] getDetailedProgressFromDB()
        {
       //     string shortDate = "MM/dd/yy";
        //    string shortTime = "h:mm:ss tt";
          //  string tempS;
            DateTime tempDT;

            string[,] detailedData = new string[9, 5000];
                        string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                string workerWipSQL = "SELECT DISTINCT tabID, site, section, construction, taskName, taskType, taskData, workDate, approver " +
       
                                     " FROM progress_data ;";

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
                    detailedData[8, detailedDataRow] = rdr.GetString(8);
                
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
            globalSiteName = ChooseSiteDropDownList.Text;
           // globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

            loadPayrollDateDropDown(companyName, globalSiteName);

            getBasicHoursTable(globalSiteName);

            Session["Site"] = globalSiteName;

            siteNameLabel.Text = globalSiteName;

            string temp = siteNameLabel.Text;
        }

        protected void PayrollViewDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[,] returnData = new String[10, 100];
            DateTime beginDate;
            Boolean allTime = false;

          //  String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

            globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

            if (PayrollViewDropDownList.SelectedValue.ToString() == "Entire time")
            {
                beginDate = new DateTime(2012, 1, 1, 12, 12, 12);
                allTime = true;
            }
            else { beginDate = Convert.ToDateTime(PayrollViewDropDownList.SelectedValue.ToString()); }


            //   returnData = getTaskProgressHoursFromDB(globalSiteName, beginDate, allTime);

            getBasicTaskHoursTable(globalSiteName, beginDate, allTime);
            //   getPayrollTable(returnData, beginDate, finalPayrollRows);
        }
    }
}