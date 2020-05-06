using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Globalization;
using System.Drawing;

namespace Act_site
{
    public partial class siteWorkers : System.Web.UI.Page
    {
        int sumDataRow = 0;
        int workerCountforDD = 0;
        int workerDataRowCount = 0;
        int detailedDataRow = 0;

        string sumWorkerTable = "";
        string detailWorkerTable = "";

        private static System.Globalization.Calendar cal = CultureInfo.InvariantCulture.Calendar;

        string companyName = "NA";
        string globalSiteName = "Choose a site";
        string workerName = "";
        int siteRowCount = 0;
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

                // Session["userName"] = userName == null ? "NA" : userName;

                // Session["Company"] = rolesForUser.Count > 0 ? rolesForUser[0] : "NA";

                //Session["Site"]


                if (!Page.IsPostBack)
                {
                    loadSiteDropDown(companyName);
                
                    loadPayrollDateDropDown(companyName, globalSiteName);

                    loadWorkerNameDropDown(companyName, globalSiteName);

                    DateTime beginDate = Convert.ToDateTime(PayrollViewDropDownList.Items[0].ToString());
                    string[,] returnData = getPayrollData(ChooseSiteDropDownList.Items[0].ToString(), beginDate);
                    getPayrollTable(returnData, beginDate, finalPayrollRows);


                    workerName = WorkerDropDownList.SelectedItem.ToString();

                    List<workerDBsections> workerList = getWorkerTaskDataFromDB(companyName, globalSiteName, workerName);

                    workerList = workerList.OrderBy(w => w.sections).ThenBy(w => w.constructions).ThenBy(w => w.tasks).ThenBy(w => w.startDate).ToList();

                    getWorkerTable(workerList, workerName);

                    // getDetailedTable(globalSiteName, workerName);

                    //the payroll part
                    String[,] returnDataWorker = new String[100, 30];
                    DateTime beginDateWorker;

                    //  globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

                    beginDateWorker = Convert.ToDateTime(PayrollViewDropDownList.Items[0].ToString());

                    returnDataWorker = getPayrollData(globalSiteName, beginDateWorker);

                    getPayrollTable(returnDataWorker, beginDateWorker, finalPayrollRows);

                }
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

        protected void loadPayrollDateDropDown(String companyNameStr, String globalSiteNameStr)
        {
            //  updateSiteButton.Click += new EventHandler(updateSiteButtonClick);
            //  addNewSiteButton.Click += new EventHandler(addNewSiteButtonClick);
            //addNewSiteButtonClick"
            try { 
            DateTime[] siteData = new DateTime[50];

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
            }
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected void loadWorkerNameDropDown(String companyNameStr, String globalSiteNameStr)
        {
            try { 

            WorkerDropDownList.Items.Clear();

            string[,] workerData = new string[1, 100];

            workerData = getWorkerListFromDB(companyName, globalSiteName);

            if (workerCountforDD == 0)
            {
                WorkerDropDownList.Items.Add(new ListItem("No current sites"));
            }




            if (workerCountforDD > 0)
            {
                for (int x = 0; x < workerCountforDD; x++)
                {
                    WorkerDropDownList.Items.Add(new ListItem(workerData[0, x], workerData[0, x]));
                }
            }
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected void chooseWorkWeekButtonClick(object sender, EventArgs e)
        {
            try { 
            //   String noSiteError = "No site was choosen to update";
            //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + noSiteError + "');", true);

            // ClientScript.RegisterStartupScript(this.GetType(), "RefreshOpener", "f2();", true); expects a javascript function named f2();

            String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

            globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

            loadPayrollDateDropDown(companyName, globalSiteName);

                //  String[] tempTownArray = temptown.Split('-');

                //   globalSiteName = tempTownArray[0];
                // globalTownName = tempTownArray[1];

            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected DateTime[] getStartDatesPayroll(String companyNameStr, String globalSiteNameStr)
        {
            // CultureInfo cul = CultureInfo.CurrentCulture;
            DateTime[] returnValues = new DateTime[50];
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

        protected List<workerDBsections> getWorkerTaskDataFromDB(string companyNameStr, string globalSiteNameStr, string workerName)
        {
            
            List<List<String>> finalList = new List<List<String>>();

            List<workerDBsections> workerTasks = new List<workerDBsections>();

            // Add parts to the list.
            // workerTasks.Add(new Part() { PartName = "regular seat", PartId = 1434 });

            string[] sqlTaskArray = new string[4];

            sqlTaskArray[0] = "SELECT DISTINCT section1, construction1, task1 FROM construction.work_data WHERE task1 != 'Task 1' AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "'; ";
            sqlTaskArray[1] = "SELECT DISTINCT section2, construction2, task2 FROM construction.work_data WHERE task2 != 'Task 2' AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "'; ";
            sqlTaskArray[2] = "SELECT DISTINCT section3, construction3, task3 FROM construction.work_data WHERE task3 != 'Task 3' AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "'; ";
            sqlTaskArray[3] = "SELECT DISTINCT section4, construction4, task4 FROM construction.work_data WHERE task4 != 'Task 4' AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "'; ";

            int taskRowCount = 0;
            int taskRowAllCount = 0;

            int finalListCount = 0;

            List<List<String>> matrix = new List<List<String>>(); //Creates new nested List


            //   string xxxxxx = matrix[0][0];

            //   matrix.RemoveAt(1);

            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;
            workerDataRowCount = 0;
            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            conn = new MySqlConnection(cs);

            try
            {

                conn.Open();

                MySqlCommand cmd = new MySqlCommand(" ", conn);

                for (int x = 0; x < 4; x++)
                {

                    cmd.CommandText = sqlTaskArray[x];
                    rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        matrix.Add(new List<String>()); //Adds new sub List
                        matrix[taskRowCount].Add(rdr.IsDBNull(0) ? "NA" : rdr.GetString(0)); //Add values to the sub List at index 0
                        matrix[taskRowCount].Add(rdr.IsDBNull(1) ? "NA" : rdr.GetString(1));
                        matrix[taskRowCount].Add(rdr.IsDBNull(2) ? "NA" : rdr.GetString(2));
                        matrix[taskRowCount].Add(rdr.IsDBNull(2) ? "NA" : rdr.GetName(2));
                        taskRowCount++;
                    }
                    if (rdr.IsClosed == false) { rdr.Close(); }
                }

                taskRowAllCount = matrix.Count();

                for (int y = 0; y < taskRowAllCount; y++)
                {
                    for (int z = y + 1; z < taskRowAllCount; z++)
                    {
                        if (matrix[z][0].Contains(matrix[y][0]) && matrix[z][1].Contains(matrix[y][1]) && matrix[z][2].Contains(matrix[y][2]))
                        {
                            matrix.RemoveAt(z);
                            taskRowAllCount = matrix.Count();
                        }

                    }

                }

                taskRowAllCount = matrix.Count();
                //matrix[0].

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


            try
            {

                conn.Open();

                string[] workerWipSQL = new string[4];

                MySqlCommand cmd = new MySqlCommand(" ", conn);

                // cmd.CommandText = "";
                //   rdr = cmd.ExecuteReader();

                for (int x = 0; x < taskRowAllCount; x++)
                {

                    double tempInt = 0;
                    string hour = "";

                    List<double> hoursList = new List<double>();
                    List<DateTime> minWorkDate = new List<DateTime>();
                    List<DateTime> maxWorkDate = new List<DateTime>();

                    workerWipSQL[0] = " SELECT sum(hours1), min(workDate), max(workDate) FROM construction.work_data " +
                                    " WHERE section1 = '" + matrix[x][0] + "' AND construction1 = '" + matrix[x][1] + "' AND task1 = '" + matrix[x][2] + "' AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "';";

                    workerWipSQL[1] = " SELECT sum(hours2), min(workDate), max(workDate) FROM construction.work_data " +
                                    " WHERE section2 = '" + matrix[x][0] + "' AND construction2 = '" + matrix[x][1] + "' AND task2 = '" + matrix[x][2] + "' AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "';";

                    workerWipSQL[2] = " SELECT sum(hours3), min(workDate), max(workDate) FROM construction.work_data " +
                                    " WHERE section3 = '" + matrix[x][0] + "' AND construction3 = '" + matrix[x][1] + "' AND task3 = '" + matrix[x][2] + "'  AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "';";

                    workerWipSQL[3] = " SELECT sum(hours4), min(workDate), max(workDate) FROM construction.work_data " +
                                    " WHERE section4 = '" + matrix[x][0] + "' AND construction4 = '" + matrix[x][1] + "' AND task4 = '" + matrix[x][2] + "'  AND site = '" + globalSiteNameStr + "' AND workerName = '" + workerName + "';";

                    for (int z = 0; z < 4; z++)
                    {

                        cmd.CommandText = workerWipSQL[z];
                        rdr = cmd.ExecuteReader();

                        while (rdr.Read())
                        {
                            if (!rdr.IsDBNull(0)) { hoursList.Add(Convert.ToDouble(rdr.GetString(0))); }
                            if (rdr.IsDBNull(0)) { hoursList.Add(0); }
                            if (!rdr.IsDBNull(1)) { minWorkDate.Add(Convert.ToDateTime(rdr.GetString(1))); }
                            if (!rdr.IsDBNull(1)) { maxWorkDate.Add(Convert.ToDateTime(rdr.GetString(2))); }
                        }
                        if (rdr.IsClosed == false) { rdr.Close(); }

                        if (z == 3)
                        {
                           // finalList.Add(new List<String>()); //Adds new sub List
                           // finalList[x].Add(matrix[x][0]);
                            //finalList[x].Add(matrix[x][1]);
                           // finalList[x].Add(matrix[x][2]);
                            tempInt = hoursList[0] + hoursList[1] + hoursList[2] + hoursList[3];
                            hour = string.Format("{0:0.00}", tempInt);
                            // finalList[x].Add(hour);
                            //finalList[x].Add(Convert.ToString(minWorkDate.Min()));
                            //finalList[x].Add(Convert.ToString(maxWorkDate.Max()));
                            if (tempInt > 0)
                            {
                                workerTasks.Add(new workerDBsections() { sections = matrix[x][0], constructions = matrix[x][1], tasks = matrix[x][2], hours = hour, startDate = minWorkDate.Min(), endDate = maxWorkDate.Max() });
                            }
                        }

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
            //get the last few days of data or whatever from the the DB
            return workerTasks;
        }

        protected string[,] getWorkerListFromDB(String companyNameStr, String globalSiteNameStr)
        {
            //need to add company to db and other
            string[,] sumData = new string[1, 100];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;
            workerCountforDD = 0;
            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT workerName " +  //, osha10
                                    " FROM work_data WHERE site = '" + globalSiteNameStr + "' ;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);

                //  cmd.CommandText = "";
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    sumData[0, workerCountforDD] = rdr.GetString(0);
                    // sumData[1, workerCountforDD] = rdr.GetString(1);

                    workerCountforDD++;

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

        protected void getWorkerTable(List<workerDBsections> workerList, string workerName)
        {


            try
            {


                workerViewTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(1100);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Name";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;
                header1.Width = Unit.Percentage(10);
                header1.CssClass = "CssStyleBothBorder"; //"CssStyleBothBorder CssStyleBottomBorder"


                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Section";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                header2.Width = Unit.Percentage(10);
                header2.CssClass = "CssStyleBothBorder";

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Construction";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;
                header3.CssClass = "CssStyleBothBorder";

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Task";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;
                header4.CssClass = "CssStyleBothBorder";

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Hours";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;
                header5.CssClass = "CssStyleBothBorder";

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Start Date";
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.HorizontalAlign = HorizontalAlign.Center;
                header6.CssClass = "CssStyleBothBorder";

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = "End Date";
                header7.Font.Bold = true;
                header7.BackColor = Color.LightGray;
                header7.ColumnSpan = 4;
                header7.HorizontalAlign = HorizontalAlign.Center;
                header7.CssClass = "CssStyleBothBorder";


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
                workerViewTable.Rows.AddAt(0, headerRow);




                int numrows = workerList.Count;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type

                    a.Text = workerName;  
                    a.HorizontalAlign = HorizontalAlign.Center;
                    a.Width = Unit.Percentage(14.28);
                    a.Wrap = true;
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    b.Text = workerList[j].sections;
                    b.HorizontalAlign = HorizontalAlign.Center;
                    b.Width = Unit.Percentage(14.28);
                    b.Wrap = true;
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //task values
                    c.Text = workerList[j].constructions;
                    c.HorizontalAlign = HorizontalAlign.Center;
                    c.Width = Unit.Percentage(14.28);
                    c.Wrap = true;
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    d.Text = workerList[j].tasks;
                    d.HorizontalAlign = HorizontalAlign.Center;
                    d.Width = Unit.Percentage(14.28);
                    d.Wrap = true;
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    e.Text = workerList[j].hours;
                    e.HorizontalAlign = HorizontalAlign.Center;
                    e.Width = Unit.Percentage(14.28);
                    e.Wrap = true;
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    f.Text = workerList[j].startDate.ToString("MM/dd/yyyy");
                    f.HorizontalAlign = HorizontalAlign.Center;
                    f.Width = Unit.Percentage(14.28);
                    f.Wrap = true;
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    g.Text = workerList[j].endDate.ToString("MM/dd/yyyy");
                    g.HorizontalAlign = HorizontalAlign.Center;
                    g.Width = Unit.Percentage(14.28);
                    g.Wrap = true;
                    r.Cells.Add(g);

                    workerViewTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                String temp = ex.Message.ToString();
            }

        }

        protected string[,] getPayrollData(String siteStr, DateTime beginDate)
        {
            String[,] searchData = new String[100, 3];
            String[,] returnData = new String[100, 34];
            String[] tempData = new string[14];
            finalPayrollRows = 0;

            String[] days = new String[7];

            days[0] = beginDate.ToString("yyyy-MM-dd");
            days[1] = beginDate.AddDays(1).ToString("yyyy-MM-dd");
            days[2] = beginDate.AddDays(2).ToString("yyyy-MM-dd");
            days[3] = beginDate.AddDays(3).ToString("yyyy-MM-dd");
            days[4] = beginDate.AddDays(4).ToString("yyyy-MM-dd");
            days[5] = beginDate.AddDays(5).ToString("yyyy-MM-dd");
            days[6] = beginDate.AddDays(6).ToString("yyyy-MM-dd");

            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;
            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();


                string workerWipSQL = "SELECT DISTINCT workerName, osha10, site " +
                                        "FROM construction.work_data WHERE workDate between '" + days[0] + "' AND '" + days[6] + "' and site = '" + siteStr + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);

                //  cmd.CommandText = "";
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    searchData[finalPayrollRows, 0] = rdr.GetString(0);
                    searchData[finalPayrollRows, 1] = rdr.GetString(1);
                    searchData[finalPayrollRows, 2] = rdr.GetString(2);


                    finalPayrollRows++;

                }
                if (rdr.IsClosed != true) { rdr.Close(); }

                for (int x = 0; x < finalPayrollRows; x++)
                {
                    returnData[x, 0] = searchData[x, 0];
                    returnData[x, 1] = searchData[x, 1];

                    for (int y = 0; y < 7; y++)
                    {

                        double rainHours = 0;

                        double totalHours = 0.0;

                        cmd.CommandText = "SELECT workerName, osha10, workDate, site, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, approverName " +
                                                    "FROM construction.work_data WHERE workDate = '" + days[y] + "' AND  workerName = '" + searchData[x, 0] + "' AND osha10 = '" + searchData[x, 1] + "' and site = '" + siteStr + "'; ";
                        rdr = cmd.ExecuteReader();

                        if (rdr.HasRows == false)
                        {
                            returnData[x, ((y + 1) * 4) + 2] = "0";
                            returnData[x, ((y + 1) * 4) + 3] = "0";
                            returnData[x, ((y + 1) * 4) + 4] = "0";
                            returnData[x, ((y + 1) * 4) + 5] = "0";

                        }
                        while (rdr.Read())
                        {

                            tempData[0] = rdr.GetString(0); //name
                            tempData[1] = rdr.GetString(1); //osha10
                            tempData[2] = rdr.GetString(2); //workdate
                            tempData[3] = rdr.GetString(3); //site
                            tempData[4] = rdr.GetString(4); //totalhours
                            tempData[5] = rdr.GetString(5); //task1
                            tempData[6] = rdr.GetString(6); //hours1
                            tempData[7] = rdr.GetString(7); //task2
                            tempData[8] = rdr.GetString(8); //hours
                            tempData[9] = rdr.GetString(9); //task3
                            tempData[10] = rdr.GetString(10); //hours
                            tempData[11] = rdr.GetString(11); //task4
                            tempData[12] = rdr.GetString(12); //hours
                            tempData[13] = rdr.GetString(13); //approvername

                            totalHours = Convert.ToDouble(tempData[4]);

                            rainHours += tempData[5].Contains("Rain") || tempData[5].Contains("rain") ? Convert.ToDouble(tempData[6]) : 0.0;
                            rainHours += tempData[7].Contains("Rain") || tempData[7].Contains("rain") ? Convert.ToDouble(tempData[8]) : 0.0;
                            rainHours += tempData[9].Contains("Rain") || tempData[9].Contains("rain") ? Convert.ToDouble(tempData[10]) : 0.0;
                            rainHours += tempData[11].Contains("Rain") || tempData[11].Contains("rain") ? Convert.ToDouble(tempData[12]) : 0.0;

                            if (totalHours <= 8 && rainHours == 0)
                            {
                                returnData[x, ((y + 1) * 4) + 2] = Convert.ToString(totalHours);
                                returnData[x, ((y + 1) * 4) + 3] = "0";
                                returnData[x, ((y + 1) * 4) + 4] = "0";
                                returnData[x, ((y + 1) * 4) + 5] = "0";

                            }

                            if (totalHours <= 8 && rainHours > 0)
                            {
                                returnData[x, ((y + 1) * 4) + 2] = Convert.ToString(totalHours - rainHours);
                                returnData[x, ((y + 1) * 4) + 3] = "0";
                                returnData[x, ((y + 1) * 4) + 4] = "0";
                                returnData[x, ((y + 1) * 4) + 5] = Convert.ToString(rainHours);

                            }

                            if (totalHours > 8 && totalHours <= 11 && rainHours == 0)
                            {
                                returnData[x, ((y + 1) * 4) + 2] = Convert.ToString(8);
                                returnData[x, ((y + 1) * 4) + 3] = Convert.ToString(totalHours - 8);
                                returnData[x, ((y + 1) * 4) + 4] = "0";
                                returnData[x, ((y + 1) * 4) + 5] = "0";

                            }

                            if (totalHours > 8 && totalHours <= 11 && rainHours > 0)
                            {
                                returnData[x, ((y + 1) * 4) + 2] = Convert.ToString(8 - rainHours);
                                returnData[x, ((y + 1) * 4) + 3] = Convert.ToString(totalHours - 8 - rainHours);
                                returnData[x, ((y + 1) * 4) + 4] = "0";
                                returnData[x, ((y + 1) * 4) + 5] = Convert.ToString(rainHours);

                            }

                            if (totalHours > 11 && rainHours == 0)
                            {
                                returnData[x, ((y + 1) * 4) + 2] = Convert.ToString(8);
                                returnData[x, ((y + 1) * 4) + 3] = Convert.ToString(3);
                                returnData[x, ((y + 1) * 4) + 4] = Convert.ToString(totalHours - 11);
                                returnData[x, ((y + 1) * 4) + 5] = "0";

                            }

                            if (totalHours > 11 && rainHours > 0)
                            {
                                returnData[x, ((y + 1) * 4) + 2] = Convert.ToString(8 - rainHours);
                                returnData[x, ((y + 1) * 4) + 3] = Convert.ToString(3 - rainHours);
                                returnData[x, ((y + 1) * 4) + 4] = Convert.ToString(totalHours - 11 - rainHours);
                                returnData[x, ((y + 1) * 4) + 5] = Convert.ToString(rainHours);

                            }

                        }
                        if (rdr.IsClosed == false) { rdr.Close(); }

                    }

                    returnData[x, 2] = Convert.ToString(Convert.ToDouble(returnData[x, 6]) + Convert.ToDouble(returnData[x, 10]) + Convert.ToDouble(returnData[x, 14]) + Convert.ToDouble(returnData[x, 18]) +
                        Convert.ToDouble(returnData[x, 22]) + Convert.ToDouble(returnData[x, 26]) + Convert.ToDouble(returnData[x, 30]));

                    returnData[x, 3] = Convert.ToString(Convert.ToDouble(returnData[x, 7]) + Convert.ToDouble(returnData[x, 11]) + Convert.ToDouble(returnData[x, 15]) + Convert.ToDouble(returnData[x, 19]) +
                    Convert.ToDouble(returnData[x, 23]) + Convert.ToDouble(returnData[x, 27]) + Convert.ToDouble(returnData[x, 31]));

                    returnData[x, 4] = Convert.ToString(Convert.ToDouble(returnData[x, 8]) + Convert.ToDouble(returnData[x, 12]) + Convert.ToDouble(returnData[x, 16]) + Convert.ToDouble(returnData[x, 20]) +
                    Convert.ToDouble(returnData[x, 24]) + Convert.ToDouble(returnData[x, 28]) + Convert.ToDouble(returnData[x, 32]));

                    returnData[x, 5] = Convert.ToString(Convert.ToDouble(returnData[x, 9]) + Convert.ToDouble(returnData[x, 13]) + Convert.ToDouble(returnData[x, 17]) + Convert.ToDouble(returnData[x, 21]) +
                    Convert.ToDouble(returnData[x, 25]) + Convert.ToDouble(returnData[x, 29]) + Convert.ToDouble(returnData[x, 33]));

                    string tempxxx = returnData[x, 5];

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

        protected void getPayrollTable(String[,] data, DateTime beginDate, int payRows)
        {


            String[] days = new String[7];

            days[0] = beginDate.ToString("MM/dd/yyyy");
            days[1] = beginDate.AddDays(1).ToString("MM/dd/yyyy");
            days[2] = beginDate.AddDays(2).ToString("MM/dd/yyyy");
            days[3] = beginDate.AddDays(3).ToString("MM/dd/yyyy");
            days[4] = beginDate.AddDays(4).ToString("MM/dd/yyyy");
            days[5] = beginDate.AddDays(5).ToString("MM/dd/yyyy");
            days[6] = beginDate.AddDays(6).ToString("MM/dd/yyyy");

            try
            {


                payrollViewTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(1100);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Name";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.RowSpan = 3;
                header1.HorizontalAlign = HorizontalAlign.Center;
                header1.Width = Unit.Percentage(10);
                header1.CssClass = "CssStyleBothBorder CssStyleBottomBorder";


                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Osha 10";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.RowSpan = 3;
                header2.HorizontalAlign = HorizontalAlign.Center;
                header2.Width = Unit.Percentage(10);
                header2.CssClass = "CssStyleBothBorder CssStyleBottomBorder";

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Totals";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.RowSpan = 2;
                header3.ColumnSpan = 4;
                header3.HorizontalAlign = HorizontalAlign.Center;
                header3.CssClass = "CssStyleBothBorder";

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = days[0];
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.ColumnSpan = 4;
                header4.HorizontalAlign = HorizontalAlign.Center;
                header4.CssClass = "CssStyleBothBorder";

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = days[1];
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.ColumnSpan = 4;
                header5.HorizontalAlign = HorizontalAlign.Center;
                header5.CssClass = "CssStyleBothBorder";

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = days[2];
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.ColumnSpan = 4;
                header6.HorizontalAlign = HorizontalAlign.Center;
                header6.CssClass = "CssStyleBothBorder";

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = days[3];
                header7.Font.Bold = true;
                header7.BackColor = Color.LightGray;
                header7.ColumnSpan = 4;
                header7.HorizontalAlign = HorizontalAlign.Center;
                header7.CssClass = "CssStyleBothBorder";

                TableHeaderCell header8 = new TableHeaderCell();
                header8.Text = days[4];
                header8.Font.Bold = true;
                header8.BackColor = Color.LightGray;
                header8.ColumnSpan = 4;
                header8.HorizontalAlign = HorizontalAlign.Center;
                header8.CssClass = "CssStyleBothBorder";

                TableHeaderCell header9 = new TableHeaderCell();
                header9.Text = days[5];
                header9.Font.Bold = true;
                header9.BackColor = Color.LightGray;
                header9.ColumnSpan = 4;
                header9.HorizontalAlign = HorizontalAlign.Center;
                header9.CssClass = "CssStyleBothBorder";

                TableHeaderCell header10 = new TableHeaderCell();
                header10.Text = days[6];
                header10.Font.Bold = true;
                header10.BackColor = Color.LightGray;
                header10.ColumnSpan = 4;
                header10.HorizontalAlign = HorizontalAlign.Center;
                header10.CssClass = "CssStyleBothBorder";


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
                payrollViewTable.Rows.AddAt(0, headerRow);

                TableCell header41 = new TableCell();
                header41.Text = "Monday";
                header41.Font.Bold = true;
                header41.BackColor = Color.LightGray;
                header41.ColumnSpan = 4;
                header41.HorizontalAlign = HorizontalAlign.Center;
                header41.CssClass = "CssStyleBothBorder";

                TableCell header51 = new TableCell();
                header51.Text = "Tuesday";
                header51.Font.Bold = true;
                header51.BackColor = Color.LightGray;
                header51.ColumnSpan = 4;
                header51.HorizontalAlign = HorizontalAlign.Center;
                header51.CssClass = "CssStyleBothBorder";

                TableCell header61 = new TableCell();
                header61.Text = "Wednesday";
                header61.Font.Bold = true;
                header61.BackColor = Color.LightGray;
                header61.ColumnSpan = 4;
                header61.HorizontalAlign = HorizontalAlign.Center;
                header61.CssClass = "CssStyleBothBorder";

                TableCell header71 = new TableCell();
                header71.Text = "Thursday";
                header71.Font.Bold = true;
                header71.BackColor = Color.LightGray;
                header71.ColumnSpan = 4;
                header71.HorizontalAlign = HorizontalAlign.Center;
                header71.CssClass = "CssStyleBothBorder";

                TableCell header81 = new TableCell();
                header81.Text = "Friday";
                header81.Font.Bold = true;
                header81.BackColor = Color.LightGray;
                header81.ColumnSpan = 4;
                header81.HorizontalAlign = HorizontalAlign.Center;
                header81.CssClass = "CssStyleBothBorder";

                TableCell header91 = new TableCell();
                header91.Text = "Saturday";
                header91.Font.Bold = true;
                header91.BackColor = Color.LightGray;
                header91.ColumnSpan = 4;
                header91.HorizontalAlign = HorizontalAlign.Center;
                header91.CssClass = "CssStyleBothBorder";

                TableCell header101 = new TableCell();
                header101.Text = "Sunday";
                header101.Font.Bold = true;
                header101.BackColor = Color.LightGray;
                header101.ColumnSpan = 4;
                header101.HorizontalAlign = HorizontalAlign.Center;
                header101.CssClass = "CssStyleBothBorder";


                // Add the header to a new row.
                TableRow headerRow1 = new TableRow();

                headerRow1.Cells.Add(header41);
                headerRow1.Cells.Add(header51);
                headerRow1.Cells.Add(header61);
                headerRow1.Cells.Add(header71);
                headerRow1.Cells.Add(header81);
                headerRow1.Cells.Add(header91);
                headerRow1.Cells.Add(header101);

                // Add the header row to the table.
                //  payrollViewTable.Rows.AddAt(1, headerRow1);
                payrollViewTable.Rows.AddAt(1, headerRow1);
                TableRow hR = new TableRow();

                TableCell[] headers = new TableCell[32];

                for (int z = 0; z < 8; z++)
                {
                    headers[z * 4] = new TableCell(); //site type  
                    headers[z * 4].Text = "STR";
                    headers[z * 4].Width = Unit.Percentage(3);
                    headers[z * 4].CssClass = "CssStyleLeftBorder CssStyleBottomBorder";
                    headers[z * 4].BackColor = Color.LightGray;
                    hR.Cells.Add(headers[z * 4]);

                    headers[z * 4 + 1] = new TableCell(); //site type  
                    headers[z * 4 + 1].Text = "1.5";
                    headers[z * 4 + 1].Width = Unit.Percentage(3);
                    headers[z * 4 + 1].BackColor = Color.LightGray;
                    headers[z * 4 + 1].CssClass = "CssStyleBottomBorder";
                    hR.Cells.Add(headers[z * 4 + 1]);

                    headers[z * 4 + 2] = new TableCell(); //site type  
                    headers[z * 4 + 2].Text = "2.0";
                    headers[z * 4 + 2].Width = Unit.Percentage(3);
                    headers[z * 4 + 2].BackColor = Color.LightGray;
                    headers[z * 4 + 2].CssClass = "CssStyleBottomBorder";
                    hR.Cells.Add(headers[z * 4 + 2]);

                    headers[z * 4 + 3] = new TableCell(); //site type  
                    headers[z * 4 + 3].Text = "CIR";
                    headers[z * 4 + 3].Width = Unit.Percentage(3);
                    headers[z * 4 + 3].CssClass = "CssStyleRightBorder CssStyleBottomBorder";
                    headers[z * 4 + 3].BackColor = Color.LightGray;
                    hR.Cells.Add(headers[z * 4 + 3]);

                }


                payrollViewTable.Rows.Add(hR);


                int numrows = payRows;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell name = new TableCell();
                    name.Text = data[j, 0];
                    name.Width = Unit.Percentage(10);
                    r.Cells.Add(name);

                    TableCell osha10 = new TableCell();
                    osha10.Text = data[j, 1];
                    osha10.Width = Unit.Percentage(10);
                    r.Cells.Add(osha10);

                    TableCell[] payrollCells = new TableCell[32];

                    for (int payX = 0; payX < 32; payX++)
                    {

                        payrollCells[payX] = new TableCell(); //site type  
                        payrollCells[payX].Text = data[j, payX + 2];
                        payrollCells[payX].Width = Unit.Percentage(2.5);

                        if (payX == 0 || payX == 4 || payX == 8 || payX == 12 || payX == 16 || payX == 20 || payX == 24 || payX == 28 || payX == 32)
                        {
                            payrollCells[payX].CssClass = "CssStyleLeftBorder";
                        }

                        if (payX == 3 || payX == 7 || payX == 11 || payX == 15 || payX == 19 || payX == 23 || payX == 27 || payX == 31)
                        {
                            payrollCells[payX].CssClass = "CssStyleRightBorder";
                        }

                        if (payX == 1 || payX == 2 || payX == 5 || payX == 6 || payX == 9 || payX == 10 || payX == 13 || payX == 14
                            || payX == 17 || payX == 18 || payX == 21 || payX == 22 || payX == 25 || payX == 26 || payX == 29 || payX == 30)
                        {
                            payrollCells[payX].CssClass = "CssStyleNoBorderTA";
                        }


                        r.Cells.Add(payrollCells[payX]);
                    }

                    if (j % 2 == 1) { r.BackColor = Color.LightSteelBlue; }

                    payrollViewTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                String temp = ex.Message.ToString();
            }

        }
        /*
        protected void getLaborTaskTable(List<List<String>> laborData)
        {
            List<string> sectionList = new List<string>();
            List<string> constructionList = new List<string>();
            List<string> sectionList = new List<string>();
            List<string> sectionList = new List<string>();


            int listRowCount = laborData.Count();

            for(int x = 0; x < listRowCount; x++)
            {
                sectionList.Add(laborData[0][x]);

            }

            sectionList.Distinct(); //is this correct??



            String[] days = new String[7];

            days[0] = beginDate.ToString("MM/dd/yyyy");
            days[1] = beginDate.AddDays(1).ToString("MM/dd/yyyy");
            days[2] = beginDate.AddDays(2).ToString("MM/dd/yyyy");
            days[3] = beginDate.AddDays(3).ToString("MM/dd/yyyy");
            days[4] = beginDate.AddDays(4).ToString("MM/dd/yyyy");
            days[5] = beginDate.AddDays(5).ToString("MM/dd/yyyy");
            days[6] = beginDate.AddDays(6).ToString("MM/dd/yyyy");

            try
            {


                payrollViewTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(1100);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Name";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.RowSpan = 3;
                header1.HorizontalAlign = HorizontalAlign.Center;
                header1.Width = Unit.Percentage(10);
                header1.CssClass = "CssStyleBothBorder CssStyleBottomBorder";


                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Osha 10";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.RowSpan = 3;
                header2.HorizontalAlign = HorizontalAlign.Center;
                header2.Width = Unit.Percentage(10);
                header2.CssClass = "CssStyleBothBorder CssStyleBottomBorder";

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Totals";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.RowSpan = 2;
                header3.ColumnSpan = 4;
                header3.HorizontalAlign = HorizontalAlign.Center;
                header3.CssClass = "CssStyleBothBorder";

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = days[0];
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.ColumnSpan = 4;
                header4.HorizontalAlign = HorizontalAlign.Center;
                header4.CssClass = "CssStyleBothBorder";

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = days[1];
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.ColumnSpan = 4;
                header5.HorizontalAlign = HorizontalAlign.Center;
                header5.CssClass = "CssStyleBothBorder";

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = days[2];
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.ColumnSpan = 4;
                header6.HorizontalAlign = HorizontalAlign.Center;
                header6.CssClass = "CssStyleBothBorder";

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = days[3];
                header7.Font.Bold = true;
                header7.BackColor = Color.LightGray;
                header7.ColumnSpan = 4;
                header7.HorizontalAlign = HorizontalAlign.Center;
                header7.CssClass = "CssStyleBothBorder";

                TableHeaderCell header8 = new TableHeaderCell();
                header8.Text = days[4];
                header8.Font.Bold = true;
                header8.BackColor = Color.LightGray;
                header8.ColumnSpan = 4;
                header8.HorizontalAlign = HorizontalAlign.Center;
                header8.CssClass = "CssStyleBothBorder";

                TableHeaderCell header9 = new TableHeaderCell();
                header9.Text = days[5];
                header9.Font.Bold = true;
                header9.BackColor = Color.LightGray;
                header9.ColumnSpan = 4;
                header9.HorizontalAlign = HorizontalAlign.Center;
                header9.CssClass = "CssStyleBothBorder";

                TableHeaderCell header10 = new TableHeaderCell();
                header10.Text = days[6];
                header10.Font.Bold = true;
                header10.BackColor = Color.LightGray;
                header10.ColumnSpan = 4;
                header10.HorizontalAlign = HorizontalAlign.Center;
                header10.CssClass = "CssStyleBothBorder";


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
                payrollViewTable.Rows.AddAt(0, headerRow);

                TableCell header41 = new TableCell();
                header41.Text = "Monday";
                header41.Font.Bold = true;
                header41.BackColor = Color.LightGray;
                header41.ColumnSpan = 4;
                header41.HorizontalAlign = HorizontalAlign.Center;
                header41.CssClass = "CssStyleBothBorder";

                TableCell header51 = new TableCell();
                header51.Text = "Tuesday";
                header51.Font.Bold = true;
                header51.BackColor = Color.LightGray;
                header51.ColumnSpan = 4;
                header51.HorizontalAlign = HorizontalAlign.Center;
                header51.CssClass = "CssStyleBothBorder";

                TableCell header61 = new TableCell();
                header61.Text = "Wednesday";
                header61.Font.Bold = true;
                header61.BackColor = Color.LightGray;
                header61.ColumnSpan = 4;
                header61.HorizontalAlign = HorizontalAlign.Center;
                header61.CssClass = "CssStyleBothBorder";

                TableCell header71 = new TableCell();
                header71.Text = "Thursday";
                header71.Font.Bold = true;
                header71.BackColor = Color.LightGray;
                header71.ColumnSpan = 4;
                header71.HorizontalAlign = HorizontalAlign.Center;
                header71.CssClass = "CssStyleBothBorder";

                TableCell header81 = new TableCell();
                header81.Text = "Friday";
                header81.Font.Bold = true;
                header81.BackColor = Color.LightGray;
                header81.ColumnSpan = 4;
                header81.HorizontalAlign = HorizontalAlign.Center;
                header81.CssClass = "CssStyleBothBorder";

                TableCell header91 = new TableCell();
                header91.Text = "Saturday";
                header91.Font.Bold = true;
                header91.BackColor = Color.LightGray;
                header91.ColumnSpan = 4;
                header91.HorizontalAlign = HorizontalAlign.Center;
                header91.CssClass = "CssStyleBothBorder";

                TableCell header101 = new TableCell();
                header101.Text = "Sunday";
                header101.Font.Bold = true;
                header101.BackColor = Color.LightGray;
                header101.ColumnSpan = 4;
                header101.HorizontalAlign = HorizontalAlign.Center;
                header101.CssClass = "CssStyleBothBorder";


                // Add the header to a new row.
                TableRow headerRow1 = new TableRow();

                headerRow1.Cells.Add(header41);
                headerRow1.Cells.Add(header51);
                headerRow1.Cells.Add(header61);
                headerRow1.Cells.Add(header71);
                headerRow1.Cells.Add(header81);
                headerRow1.Cells.Add(header91);
                headerRow1.Cells.Add(header101);

                // Add the header row to the table.
                //  payrollViewTable.Rows.AddAt(1, headerRow1);
                payrollViewTable.Rows.AddAt(1, headerRow1);
                TableRow hR = new TableRow();

                TableCell[] headers = new TableCell[32];

                for (int z = 0; z < 8; z++)
                {
                    headers[z * 4] = new TableCell(); //site type  
                    headers[z * 4].Text = "STR";
                    headers[z * 4].Width = Unit.Percentage(3);
                    headers[z * 4].CssClass = "CssStyleLeftBorder CssStyleBottomBorder";
                    headers[z * 4].BackColor = Color.LightGray;
                    hR.Cells.Add(headers[z * 4]);

                    headers[z * 4 + 1] = new TableCell(); //site type  
                    headers[z * 4 + 1].Text = "1.5";
                    headers[z * 4 + 1].Width = Unit.Percentage(3);
                    headers[z * 4 + 1].BackColor = Color.LightGray;
                    headers[z * 4 + 1].CssClass = "CssStyleBottomBorder";
                    hR.Cells.Add(headers[z * 4 + 1]);

                    headers[z * 4 + 2] = new TableCell(); //site type  
                    headers[z * 4 + 2].Text = "2.0";
                    headers[z * 4 + 2].Width = Unit.Percentage(3);
                    headers[z * 4 + 2].BackColor = Color.LightGray;
                    headers[z * 4 + 2].CssClass = "CssStyleBottomBorder";
                    hR.Cells.Add(headers[z * 4 + 2]);

                    headers[z * 4 + 3] = new TableCell(); //site type  
                    headers[z * 4 + 3].Text = "CIR";
                    headers[z * 4 + 3].Width = Unit.Percentage(3);
                    headers[z * 4 + 3].CssClass = "CssStyleRightBorder CssStyleBottomBorder";
                    headers[z * 4 + 3].BackColor = Color.LightGray;
                    hR.Cells.Add(headers[z * 4 + 3]);

                }


                payrollViewTable.Rows.Add(hR);


                int numrows = payRows;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell name = new TableCell();
                    name.Text = data[j, 0];
                    name.Width = Unit.Percentage(10);
                    r.Cells.Add(name);

                    TableCell osha10 = new TableCell();
                    osha10.Text = data[j, 1];
                    osha10.Width = Unit.Percentage(10);
                    r.Cells.Add(osha10);

                    TableCell[] payrollCells = new TableCell[32];

                    for (int payX = 0; payX < 32; payX++)
                    {

                        payrollCells[payX] = new TableCell(); //site type  
                        payrollCells[payX].Text = data[j, payX + 2];
                        payrollCells[payX].Width = Unit.Percentage(2.5);

                        if (payX == 0 || payX == 4 || payX == 8 || payX == 12 || payX == 16 || payX == 20 || payX == 24 || payX == 28 || payX == 32)
                        {
                            payrollCells[payX].CssClass = "CssStyleLeftBorder";
                        }

                        if (payX == 3 || payX == 7 || payX == 11 || payX == 15 || payX == 19 || payX == 23 || payX == 27 || payX == 31)
                        {
                            payrollCells[payX].CssClass = "CssStyleRightBorder";
                        }

                        if (payX == 1 || payX == 2 || payX == 5 || payX == 6 || payX == 9 || payX == 10 || payX == 13 || payX == 14
                            || payX == 17 || payX == 18 || payX == 21 || payX == 22 || payX == 25 || payX == 26 || payX == 29 || payX == 30)
                        {
                            payrollCells[payX].CssClass = "CssStyleNoBorderTA";
                        }


                        r.Cells.Add(payrollCells[payX]);
                    }

                    if (j % 2 == 1) { r.BackColor = Color.LightSteelBlue; }

                    payrollViewTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                String temp = ex.Message.ToString();
            }

        }
        */
        protected string getBasicTable()
        {
            string[,] sumData = new string[12, 100];
            try
            {
                sumData = getSummaryWorkerFromDB();
                sumWorkerTable = "";

                sumWorkerTable = sumWorkerTable + "<p>Summary Hours</p>";

                sumWorkerTable = sumWorkerTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:8%\">Tablet Id</th>" +
                                            "<th style=\"width:16%\">Worker Name</th>" +
                                            "<th style=\"width:8%\">Total Hours</th>" +
                                            "<th style=\"width:8%\">Job 1</th>" +
                                            "<th style=\"width:8%\">Hours 1</th>" +
                                            "<th style=\"width:8%\">Job 2</th>" +
                                            "<th style=\"width:8%\">Hours 2</th>" +
                                            "<th style=\"width:8%\">Job 3</th>" +
                                            "<th style=\"width:8%\">Hours 3</th>" +
                                            "<th style=\"width:8%\">Job 4</th>" +
                                            "<th style=\"width:8%\">Hours 4</th>" +


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
                                                + sumData[6, x] + "</td><td>"
                                                + sumData[7, x] + "</td><td>"
                                                + sumData[8, x] + "</td><td>"
                                                + sumData[9, x] + "</td><td>"
                                                + sumData[10, x] + "</td></tr>";
                }
                sumWorkerTable = sumWorkerTable + "</table>";
            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            return sumWorkerTable;
        }

        protected string[,] getSummaryWorkerFromDB()
        {

            string[,] sumData = new string[12, 100];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                string workerWipSQL = "SELECT DISTINCT tabID, workerName, sum(totalHours), task1, sum(hours1), task2, sum(hours2), task3, sum(hours3), task4, sum(hours4)  " +
                                    " FROM work_data group by workerName, task1, task2, task3, task4 ;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);

                //  cmd.CommandText = "";
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
                    sumData[7, sumDataRow] = rdr.GetString(7);
                    sumData[8, sumDataRow] = rdr.GetString(8);
                    sumData[9, sumDataRow] = rdr.GetString(9);
                    sumData[10, sumDataRow] = rdr.GetString(10);

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

        protected void getDetailedTable(string site, string worker)
        {
            string[,] detData = new string[20, 5000];
            try
            {
                detData = getDetailedWorkerFromDB(site, worker);
                detailWorkerTable = "";

                detailWorkerTable = detailWorkerTable + "<p>Detailed Hours</p>";

                detailWorkerTable = detailWorkerTable + "<table class=\"tabStyle\"><tr>" +
                                            "<th style=\"width:5%\">Tablet Id</th>" +
                                            "<th style=\"width:5%\">Worker Name</th>" +
                                            "<th style=\"width:5%\">OSHA 10</th>" +
                                            "<th style=\"width:5%\">Work Date</th>" +
                                            "<th style=\"width:5%\">Job Start Time</th>" +
                                            "<th style=\"width:5%\">End Job Time</th>" +
                                            "<th style=\"width:5%\">Worker Start Time</th>" +
                                            "<th style=\"width:5%\">Worker End Time</th>" +
                                            "<th style=\"width:5%\">Total Hours</th>" +
                                            "<th style=\"width:5%\">Job 1</th>" +
                                            "<th style=\"width:5%\">Hours 1</th>" +
                                            "<th style=\"width:5%\">Job 2</th>" +
                                            "<th style=\"width:5%\">Hours 2</th>" +
                                            "<th style=\"width:5%\">Job 3</th>" +
                                            "<th style=\"width:5%\">Hours 3</th>" +
                                            "<th style=\"width:5%\">Job 4</th>" +
                                            "<th style=\"width:5%\">Hours 4</th>" +
                                            "<th style=\"width:5%\">Approver Name</th>" +
                                            "<th style=\"width:5%\">Approver Date Time</th>" +
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
                                                + detData[9, x] + "</td><td>"
                                                + detData[10, x] + "</td><td>"
                                                + detData[11, x] + "</td><td>"
                                                + detData[12, x] + "</td><td>"
                                                + detData[13, x] + "</td><td>"
                                                + detData[14, x] + "</td><td>"
                                                + detData[15, x] + "</td><td>"
                                                + detData[16, x] + "</td><td>"
                                                + detData[18, x] + "</td><td>"
                                                + detData[19, x] + "</td></tr>";
                }
                detailWorkerTable = detailWorkerTable + "</table>";
            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            //Literal1.Text = detailWorkerTable;
           // return detailWorkerTable;
        }

        protected string[,] getDetailedWorkerFromDB(string site, string worker)
        {
            string shortDate = "MM/dd/yy";
            string shortTime = "h:mm:ss tt";
            string tempS;
            DateTime tempDT;

            string[,] detailedData = new string[20, 5000];
            string cs = ConfigurationManager.ConnectionStrings["ConstructionConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                //tabletID, workerName, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, approverName, approvalDateTime, insertDate,
                string workerWipSQL = "SELECT DISTINCT tabID, workerName, osha10, workDate, workStartTime, workEndTime, jobStartTime, jobEndTime, " +
                                        "totalHours, task1, hours1, task2, hours2, task3, hours3, task4, hours4, dbEnterDateTime, " +
                                        "approverName, approvalDateTime  " +
                                        " FROM work_data WHERE site = '" + site + "' AND workerName = '" + worker + "' ;";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    detailedData[0, detailedDataRow] = rdr.GetString(0);
                    detailedData[1, detailedDataRow] = rdr.GetString(1);
                    detailedData[2, detailedDataRow] = rdr.GetString(2);
                    tempDT = Convert.ToDateTime(rdr.GetString(3));
                    detailedData[3, detailedDataRow] = tempDT.ToShortDateString();
                    tempDT = Convert.ToDateTime(rdr.GetString(4));
                    detailedData[4, detailedDataRow] = tempDT.ToShortTimeString();
                    tempDT = Convert.ToDateTime(rdr.GetString(5));
                    detailedData[5, detailedDataRow] = tempDT.ToShortTimeString();
                    tempDT = Convert.ToDateTime(rdr.GetString(6));
                    detailedData[6, detailedDataRow] = tempDT.ToShortTimeString();
                    tempDT = Convert.ToDateTime(rdr.GetString(7));
                    detailedData[7, detailedDataRow] = tempDT.ToShortTimeString();
                    detailedData[8, detailedDataRow] = rdr.GetString(8);
                    detailedData[9, detailedDataRow] = rdr.GetString(9);
                    detailedData[10, detailedDataRow] = rdr.GetString(10);
                    detailedData[11, detailedDataRow] = rdr.IsDBNull(11) == true ? "NA" : rdr.GetString(11);
                    detailedData[12, detailedDataRow] = rdr.GetString(12);
                    detailedData[13, detailedDataRow] = rdr.GetString(13);
                    detailedData[14, detailedDataRow] = rdr.GetString(14);
                    detailedData[15, detailedDataRow] = rdr.GetString(15);
                    detailedData[16, detailedDataRow] = rdr.GetString(16);
                    // detailedData[17, detailedDataRow] = rdr.GetString(17);
                    detailedData[18, detailedDataRow] = rdr.GetString(18);
                    tempDT = Convert.ToDateTime(rdr.GetString(19));
                    detailedData[19, detailedDataRow] = tempDT.ToShortDateString();


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
            try
            {

                System.Diagnostics.Debug.WriteLine("ChooseSiteDropDownList:" + ChooseSiteDropDownList.SelectedValue);
                System.Diagnostics.Debug.WriteLine("Page.IsPostBack:" + Page.IsPostBack);
                System.Diagnostics.Debug.WriteLine("ChooseSiteDropDownList.SelectedValue:" + ChooseSiteDropDownList.SelectedIndex);

                globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

                loadPayrollDateDropDown(companyName, globalSiteName);

                loadWorkerNameDropDown(companyName, globalSiteName);

                Session["Site"] = globalSiteName;

                siteNameLabel.Text = globalSiteName;

                DateTime beginDate = Convert.ToDateTime(PayrollViewDropDownList.Items[0].ToString());
                string[,] returnData = getPayrollData(ChooseSiteDropDownList.Items[0].ToString(), beginDate);
                getPayrollTable(returnData, beginDate, finalPayrollRows);


                workerName = WorkerDropDownList.SelectedItem.ToString();

                List<workerDBsections> workerList = getWorkerTaskDataFromDB(companyName, globalSiteName, workerName);

                workerList = workerList.OrderBy(w => w.sections).ThenBy(w => w.constructions).ThenBy(w => w.tasks).ThenBy(w => w.startDate).ToList();

                getWorkerTable(workerList, workerName);

                // getDetailedTable(globalSiteName, workerName);

                //the payroll part
                String[,] returnDataWorker = new String[100, 30];
                DateTime beginDateWorker;

                beginDateWorker = Convert.ToDateTime(PayrollViewDropDownList.Items[0].ToString());

                returnDataWorker = getPayrollData(globalSiteName, beginDateWorker);

                getPayrollTable(returnDataWorker, beginDateWorker, finalPayrollRows);

            }
            catch(Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected void PayrollViewDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            String[,] returnData = new String[100, 30];
            DateTime beginDate;

            String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

            globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

            beginDate = Convert.ToDateTime(PayrollViewDropDownList.SelectedValue.ToString());

            returnData = getPayrollData(globalSiteName, beginDate);

            getPayrollTable(returnData, beginDate, finalPayrollRows);

            //the worker part
            List<workerDBsections> workerList = new List<workerDBsections>();

            workerName = WorkerDropDownList.SelectedItem.ToString();

            workerList = getWorkerTaskDataFromDB(companyName, globalSiteName, workerName);

            workerList = workerList.OrderBy(w => w.sections).ThenBy(w => w.constructions).ThenBy(w => w.tasks).ThenBy(w => w.startDate).ToList();

            getWorkerTable(workerList, workerName);
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }

        }

        protected void WorkerDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            List<workerDBsections> workerList = new List<workerDBsections>();
            //   String noSiteError = "No site was choosen to update";
            //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + noSiteError + "');", true);
            // ClientScript.RegisterStartupScript(this.GetType(), "RefreshOpener", "f2();", true); expects a javascript function named f2();
            //  String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();
            //  globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();
            //  loadPayrollDateDropDown(companyName, globalSiteName);
            //  String[] tempTownArray = temptown.Split('-');
            //   globalSiteName = tempTownArray[0];
            // globalTownName = tempTownArray[1];
            //  ChooseSiteDropDownList.Items
            workerName = WorkerDropDownList.SelectedItem.ToString();

            workerList = getWorkerTaskDataFromDB(companyName, globalSiteName, workerName);

            workerList = workerList.OrderBy(w => w.sections).ThenBy(w => w.constructions).ThenBy(w => w.tasks).ThenBy(w => w.startDate).ToList();

            getWorkerTable(workerList, workerName);

            // getDetailedTable(globalSiteName, workerName);

            //the payroll part
            String[,] returnData = new String[100, 30];
            DateTime beginDate;

            String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

            globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

            beginDate = Convert.ToDateTime(PayrollViewDropDownList.SelectedValue.ToString());

            returnData = getPayrollData(globalSiteName, beginDate);

            getPayrollTable(returnData, beginDate, finalPayrollRows);
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }

        }
    }

    public sealed class HttpUnhandledException : System.Web.HttpException
    {

    }

    public class workerDBsections
    {
        public string sections { get; set; }
        public string constructions { get; set; }
        public string tasks { get; set; }
        public string hours { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

    }
}

