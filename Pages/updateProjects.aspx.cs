using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Drawing;
using System.IO;

namespace Act_site.action
{
    public partial class siteUpdateProjects : System.Web.UI.Page
    {
        String companyName = "Action";

        String globalSiteName = "Choose a site";
        String globalTownName = "";

        int siteRowCount = 0;
        int sectionRowCount = 0;
        int constructionRowCount = 0;
        int WorkerRowCount = 0;
        int taskRowCount = 0;
        int deliveryRowCount = 0;
        int commentRowCount = 0;
        int documentRowCount = 0;

        int sectionSelectedConstructionRowCount = 0;

        // section area

        Button[] sectionUpdateButtons;
        Button[] sectionDeleteButtons;

        Label[] sectionSiteLabels;
        TextBox[] sectionTextBoxes;
        CheckBox[] sectionCheckFW;

        Label sectionSiteLabel;
        TextBox sectionTB;
        CheckBox sectionCB;


        Button addNewSectionButton = new Button();

        //*******************************************
        //construction type

        Button[] constructionUpdateButtons;
        Button[] constructionDeleteButtons;

        Label[] constructionSiteNameLabels;
        DropDownList[] constructionSectionNameDropDownList;
        DropDownList[] constructionDropDown;
        TextBox[] constructionHoursTextBoxes;
        Label[] constructionLabelFW;

        

        ListItemCollection ConstructionSectionItems = new ListItemCollection();
        

        Label constructionSiteNameLB;
        DropDownList constructionSectionDD;
        DropDownList constructionDD;
        TextBox constructionHoursTB;
        Label constructionFWLB;


        Button[] workerUpdateButtons;
        Button[] workerDeleteButtons;

        TextBox[] workerNameTextBoxes;
        TextBox[] workerOsha10TextBoxes;

        DropDownList[] workerTradeNameDropDownList;
        DropDownList[] workerLevelDropDownList;

        DropDownList workerTradeDD;
        DropDownList workerLevelDD;
        TextBox workerNameTB;
        TextBox workerOsha10TB;

        // the array of buttons etc for the tasks
        Button[] taskUpdateButtons;
        Button[] taskDeleteButtons;

        DropDownList[] taskDropDown;
        
        DropDownList[] taskSectionNameDropDownList;
        DropDownList[] taskConstructionDropDownList;
        DropDownList[] taskMeasureDropDownList;
        Label[] taskFWLabel;

        DropDownList taskSectionDD;
        DropDownList taskConstructionDD;
        DropDownList taskMeasureDD;
        Label taskFWLab;

        DropDownList taskDD;

        ListItemCollection taskSectionItems = new ListItemCollection();

        ListItemCollection tempConstrutListItems = new ListItemCollection();

        // the array of buttons etc for the delivery section
        private Button[] deliveryUpdateButtons;
        private Button[] deliveryDeleteButtons;

        private TextBox[] deliveryTextBoxes;

        private DropDownList[] deliverySectionNameDropDownList;
        private DropDownList[] deliveryConstructionDropDownList;
        private DropDownList[] deliveryMeasureDropDownList;

        DropDownList deliverySectionDD;
        DropDownList deliveryConstructionDD;
        TextBox deliveryTB;
        DropDownList deliveryMeasureDD;

        // the array of buttons etc for the comments
        private Button[] commentUpdateButtons;
        private Button[] commentDeleteButtons;

        private TextBox[] commentTextBoxes;

        private DropDownList[] commentTradeNameDropDownList;
        private DropDownList[] commentTypeDropDownList;

        DropDownList commentTradeDD;
        DropDownList commentConstructionTypeDD;

        TextBox commentTB;

        TextBox[] docNameTextBoxes;
        TextBox[] docDateTextBoxes;
        Button[] documentDeleteButtons;

        

        ListItemCollection deliverySectionItems = new ListItemCollection();

        ListItemCollection tempDeliveryConstrutListItems = new ListItemCollection();

        //site info creation
        private Button siteUpdateButton = new Button();
        private Button siteAddButton = new Button();

        Button rowSiteUpdateButton = new Button();
        Button rowsiteDeleteButton = new Button();

        TextBox siteNameTextBox = new TextBox();
        TextBox siteTownTextBox = new TextBox();
        TextBox siteStartDateTextBox = new TextBox();
        TextBox siteEndDateTextBox = new TextBox();

        DateTime siteStartDate = new DateTime();
        DateTime siteEndDate = new DateTime();

        String siteNameStr = "";
        String siteTownStr = "";
        String siteStartDateStr = "";
        String siteEndDateStr = "";



        //site info creation
        private Button[] siteUpdateButtons;
        private Button[] siteDeleteButtons;
        private Button[] siteAddButtons;

        private TextBox[] siteNameTextBoxes;
        private TextBox[] siteTownTextBoxes;
        private TextBox[] siteStartDateTextBoxes;
        private TextBox[] siteEndDateTextBoxes;

        private Calendar[] siteStartDates;
        private Calendar[] siteEndDates;


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                loadSiteDropDown();
                getSectionTable(globalSiteName);
                getConstructionTable(globalSiteName);
                //   getWorkerTable(); //<!--  <% =getWorkerTable()%>-->
                getTaskTable();
                getDeliveryTable();
                //   getCommentTable();
                //  getDocumentTable();
                //  }
            }

            if (IsPostBack)
            {
                loadSiteDropDown();
                getSectionTable(globalSiteName);
                getConstructionTable(globalSiteName);
                //   getWorkerTable(); //<!--  <% =getWorkerTable()%>-->
                getTaskTable();
                getDeliveryTable();
                //   getCommentTable();
                //  getDocumentTable();
                //  }
            }
        }

        public List<string> getDistinctConstructionData(String comp)
        {
            List<string> constructionDistinctList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT constructionType FROM wcf_data.constructiontypes where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

                // if (rdr.FieldCount == 0) { returnData[0] = "-1"; distinctConstructionRowCount = 0; }


                while (rdr.Read())
                {
                    constructionDistinctList.Add(rdr.GetString(0));
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


            return constructionDistinctList;
        }

        public List<string> getDistinctTaskData(String comp)
        {
            List<string> taskDistinctList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT task FROM wcf_data.tasktypes where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

                  while (rdr.Read())
                {
                    taskDistinctList.Add(rdr.GetString(0));
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


            return taskDistinctList;
        }

        public List<string> getDistinctMeasureData(String comp)
        {
            List<string> measureDistinctList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT measure FROM wcf_data.measuretypes where company = '" + comp + "' ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    measureDistinctList.Add(rdr.GetString(0));
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


            return measureDistinctList;
        }

        public List<string> getDistinctDeliveryData(String comp)
        {
            List<string> deliveryDistinctList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            // deliveryRowCount = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT material FROM wcf_data.deliverytypes where company = '" + comp + "' ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    deliveryDistinctList.Add(rdr.GetString(0));
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


            return deliveryDistinctList;
        }

        public String[,] getSiteData(String comp)
        {
            String[,] returnData = new String[6, 100];
            siteRowCount = 0;
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT siteName, siteTown, startDate, endDate, isActive, mySqlID FROM wcf_data.sites where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, siteRowCount] = rdr.IsDBNull(0) == true ? "NA" : rdr.GetString(0);
                    returnData[1, siteRowCount] = rdr.IsDBNull(1) == true ? "NA" : rdr.GetString(1);
                    returnData[2, siteRowCount] = rdr.IsDBNull(2) == true ? "NA" : Convert.ToString(rdr.GetDateTime(2));
                    returnData[3, siteRowCount] = rdr.IsDBNull(3) == true ? "NA" : Convert.ToString(rdr.GetDateTime(3));
                    returnData[4, siteRowCount] = rdr.IsDBNull(4) == true ? "NA" : rdr.GetString(4);
                    returnData[5, siteRowCount] = rdr.IsDBNull(5) == true ? "NA" : rdr.GetString(5);

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

        public String[] getOneSiteData(String comp, String site)
        {
            String[] returnData = new String[6];
            siteRowCount = 0;
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT siteName, siteTown, startDate, endDate, isActive, mySqlID FROM wcf_data.sites where company = '" + comp + "' and siteName = '" + site + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    if (siteRowCount >= 1) { rdr = null; }
                    returnData[0] = rdr.IsDBNull(0) == true ? "NA" : rdr.GetString(0);
                    returnData[1] = rdr.IsDBNull(1) == true ? "NA" : rdr.GetString(1);
                    returnData[2] = rdr.IsDBNull(2) == true ? "NA" : Convert.ToString(rdr.GetDateTime(2));
                    returnData[3] = rdr.IsDBNull(3) == true ? "NA" : Convert.ToString(rdr.GetDateTime(3));
                    returnData[4] = rdr.IsDBNull(4) == true ? "NA" : rdr.GetString(4);
                    returnData[5] = rdr.IsDBNull(5) == true ? "NA" : rdr.GetString(5);

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

        public String[,] getSiteSectionsData(String comp, String siteName)
        {

            String[,] returnData = new String[5, 500];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            sectionRowCount = 0;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT siteName, siteTown, sectionName, fieldWork, mySqlID FROM wcf_data.site_sections where company = '" + comp + "' AND siteName = '" + siteName + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

              // if( rdr.FieldCount == 0) { returnData[0, 0] = "-1"; sectionRowCount = 0; }


                    while (rdr.Read())
                    {
                        returnData[0, sectionRowCount] = rdr.GetString(0);
                        returnData[1, sectionRowCount] = rdr.GetString(1);
                        returnData[2, sectionRowCount] = rdr.GetString(2);
                        returnData[3, sectionRowCount] = rdr.GetString(3);
                        returnData[4, sectionRowCount] = rdr.GetString(4);

                    sectionRowCount++;
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

        public String getSectionsFWstatus(String comp, String sectionName)
        {

            String returnData = "0";

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            sectionRowCount = 0;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT fieldWork FROM wcf_data.site_sections where company = '" + comp + "' AND sectionName = '" + sectionName + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

                // if( rdr.FieldCount == 0) { returnData[0, 0] = "-1"; sectionRowCount = 0; }

                if(rdr.HasRows == false)
                {
                    return returnData;
                }


                while (rdr.Read())
                {
                    returnData = rdr.GetString(0);
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

        public String[,] getConstructionData(String comp, String siteName)
        {
            String[,] returnData = new String[6, 500];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            constructionRowCount = 0;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT siteName, sectionName, construction, hours, fieldWork, mySqlID FROM wcf_data.construction where company = '" + comp + "' AND siteName = '" + siteName + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

                if (rdr.FieldCount == 0) { returnData[0, 0] = "-1"; constructionRowCount = 0; }


                while (rdr.Read())
                {
                    returnData[0, constructionRowCount] = rdr.IsDBNull(0) ? "0" : rdr.GetString(0);
                    returnData[1, constructionRowCount] = rdr.IsDBNull(1) ? "0" : rdr.GetString(1);
                    returnData[2, constructionRowCount] = rdr.IsDBNull(2) ? "0" : rdr.GetString(2);
                    returnData[3, constructionRowCount] = rdr.IsDBNull(3) ? "0" : rdr.GetString(3);
                    returnData[4, constructionRowCount] = rdr.IsDBNull(4) ? "0" : rdr.GetString(4);
                    returnData[5, constructionRowCount] = rdr.IsDBNull(5) ? "0" : rdr.GetString(5);
                    constructionRowCount++;
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

        public String[,] getConstructionDataBySection(String comp, String siteName, String sect)
        {
            if (siteName.Contains("-"))
            {
                String temptown = siteName;
                String[] tempTownArray = temptown.Split('-');
                siteName = tempTownArray[0];

            }

            String[,] returnData = new String[6, 500];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            sectionSelectedConstructionRowCount = 0;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT siteName, sectionName, construction, hours, fieldWork, mySqlID FROM wcf_data.construction where company = '" + comp + "' AND siteName = '" + siteName + "' AND sectionName = '" + sect + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

               // if (rdr.FieldCount == 0) { returnData[0, 0] = "-1"; sectionSelectedConstructionRowCount = 0; }

                if(rdr.HasRows)
                {
                   // string texmpXX = rdr.GetString(0);
                }

                while (rdr.Read())
                {
                    returnData[0, sectionSelectedConstructionRowCount] = rdr.IsDBNull(0) ? "0" : rdr.GetString(0);
                    returnData[1, sectionSelectedConstructionRowCount] = rdr.IsDBNull(1) ? "0" : rdr.GetString(1);
                    returnData[2, sectionSelectedConstructionRowCount] = rdr.IsDBNull(2) ? "0" : rdr.GetString(2);
                    returnData[3, sectionSelectedConstructionRowCount] = rdr.IsDBNull(3) ? "0" : rdr.GetString(3);
                    returnData[4, sectionSelectedConstructionRowCount] = rdr.IsDBNull(4) ? "0" : rdr.GetString(4);
                    returnData[5, sectionSelectedConstructionRowCount] = rdr.IsDBNull(5) ? "0" : rdr.GetString(5);
                    sectionSelectedConstructionRowCount++;
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

        public String[,] getWorkerData(String comp)
        {
            String[,] returnData = new String[6,100];
            
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT trade, name, osha10, level, updateDate, mySqlID FROM wcf_data.worker where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, WorkerRowCount] = rdr.GetString(0);
                    returnData[1, WorkerRowCount] = rdr.GetString(1);
                    returnData[2, WorkerRowCount] = rdr.GetString(2);
                    returnData[3, WorkerRowCount] = rdr.GetString(3);
                    returnData[4, WorkerRowCount] = Convert.ToString(rdr.GetDateTime(4));
                    returnData[5, WorkerRowCount] = rdr.GetString(5);
                    WorkerRowCount++;
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

        public String[,] getTaskData(String comp, String site)
        {
            String[,] returnData = new String[7, 100];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            taskRowCount = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT siteName, sectionName, construction, task, measure, fieldWork, mySqlID FROM wcf_data.tasks where company = '" + comp + "' AND siteName ='" + site + "';";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, taskRowCount] = rdr.IsDBNull(0) ? "0" : rdr.GetString(0);
                    returnData[1, taskRowCount] = rdr.IsDBNull(1) ? "0" : rdr.GetString(1);
                    returnData[2, taskRowCount] = rdr.IsDBNull(2) ? "0" : rdr.GetString(2);
                    returnData[3, taskRowCount] = rdr.IsDBNull(3) ? "0" : rdr.GetString(3);
                    returnData[4, taskRowCount] = rdr.IsDBNull(4) ? "0" : rdr.GetString(4);
                    returnData[5, taskRowCount] = rdr.IsDBNull(5) ? "0" : rdr.GetString(5);
                    returnData[6, taskRowCount] = rdr.IsDBNull(6) ? "0" : rdr.GetString(6);

                    taskRowCount++;
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

        public String[,] getDeliveryData(String comp, String site)
        {
            String[,] returnData = new String[6, 100];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            deliveryRowCount = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT siteName, sectionName, construction, material, measure, mySqlID FROM wcf_data.delivery where company = '" + comp + "' AND siteName = '" + site + "';";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, deliveryRowCount] = rdr.IsDBNull(0) ? "0" : rdr.GetString(0);
                    returnData[1, deliveryRowCount] = rdr.IsDBNull(1) ? "0" : rdr.GetString(1);
                    returnData[2, deliveryRowCount] = rdr.IsDBNull(2) ? "0" : rdr.GetString(2);
                    returnData[3, deliveryRowCount] = rdr.IsDBNull(3) ? "0" : rdr.GetString(3);
                    returnData[4, deliveryRowCount] = rdr.IsDBNull(4) ? "0" : rdr.GetString(4);
                    returnData[5, deliveryRowCount] = rdr.IsDBNull(5) ? "0" : rdr.GetString(5);
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


            return returnData;
        }

        public String[,] getCommentData(String comp)
        {
            String[,] returnData = new String[5, 100];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT trade, commentType, comment, updateDate, mySqlID FROM wcf_data.comment where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, commentRowCount] = rdr.GetString(0);
                    returnData[1, commentRowCount] = rdr.GetString(1);
                    returnData[2, commentRowCount] = rdr.GetString(2);
                    returnData[3, commentRowCount] = Convert.ToString(rdr.GetDateTime(3));
                    returnData[4, commentRowCount] = rdr.GetString(4);
                    commentRowCount++;
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

        public String[,] getDocumentData(String comp)
        {
            String[,] returnData = new String[4, 100];
            documentRowCount = 0;
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string workerWipSQL = "SELECT DISTINCT docName, docPath, uploadDate, mySqlID FROM wcf_data.documents where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(workerWipSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, documentRowCount] = rdr.GetString(0);
                    returnData[1, documentRowCount] = rdr.GetString(1);
                    returnData[2, documentRowCount] = Convert.ToString(rdr.GetDateTime(2));
                    returnData[3, documentRowCount] = rdr.GetString(3);

                    documentRowCount++;
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


            return returnData;
        }

        protected void loadSiteDropDown()
        {

            if(ChooseSiteDropDownList.Items.Count == 0)
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

            if(siteRowCount == 0)
            { ChooseSiteDropDownList.Items.Add(new ListItem("No current sites", "0"));
                ChooseSiteDropDownList.SelectedValue = "0";
            }

            if (siteRowCount > 0)
            {
                for (int x = 0; x < siteRowCount; x++)
                {
                    ChooseSiteDropDownList.Items.Add(new ListItem(siteData[0, x], siteData[0, x]));
                }

                ChooseSiteDropDownList.SelectedValue = globalSiteName;
            }
        }

        protected void getSiteTable()
        {
            string[,] siteData = new string[6, 100];
            string[,] sectionData = new string[7, 500];


            try
            {
                siteData = getSiteData(companyName);
                // this section is for creatin the worker update buttons

                workerNameTextBoxes = new TextBox[WorkerRowCount + 1];
                workerOsha10TextBoxes = new TextBox[WorkerRowCount + 1];

                workerTradeNameDropDownList = new DropDownList[WorkerRowCount + 1];
                workerLevelDropDownList = new DropDownList[WorkerRowCount + 1];

                workerUpdateButtons = new Button[WorkerRowCount + 1];
                workerDeleteButtons = new Button[WorkerRowCount + 1];


                for (int i = 0; i < WorkerRowCount + 1; i++)
                {
                    var a = new DropDownList();
                    workerTradeNameDropDownList[i] = a;

                    var b = new TextBox();
                    workerNameTextBoxes[i] = b;

                    var c = new TextBox();
                    workerOsha10TextBoxes[i] = c;

                    var d = new DropDownList();
                    workerLevelDropDownList[i] = d;

                    var e = new Button();
                    workerUpdateButtons[i] = e;

                    var f = new Button();
                    workerDeleteButtons[i] = f;
                }


                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Trade";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Name";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "OSHA 10";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Level";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Delete";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Update";
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
                //workerTable.Rows.AddAt(0, headerRow);

                int numrows = WorkerRowCount;
                for (int j = 0; j < numrows; j++)
                {

                    // for (int i = 0; i < numcells; i++)
                    //  { //  c.Controls.Add(new LiteralControl("row " + j.ToString() + ", cell " ));
                    //  dropdownlist.ClearSelection(); //making sure the previous selection has been cleared
                    //   dropdownlist.Items.FindByValue(value).Selected = true;
                    //DropDownList
                    //   dd1.ClearSelection(); //making sure the previous selection has been cleared
                    //dd1.Items.FindByValue(sumData[0, j]).Selected = true;

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type
                    ListItem[] tradeItems = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                    workerTradeNameDropDownList[j].Items.AddRange(tradeItems);
                    workerTradeNameDropDownList[j].ID = "1|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    a.Controls.Add(workerTradeNameDropDownList[j]);
                    workerTradeNameDropDownList[j].SelectedValue = siteData[0, j];
                    a.Width = Unit.Percentage(16.6);
                    r.Cells.Add(a);

                    TableCell b = new TableCell(); //worker name
                    workerNameTextBoxes[j].Text = siteData[1, j];
                    workerNameTextBoxes[j].ID = "2|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    b.Controls.Add(workerNameTextBoxes[j]);
                    b.Width = Unit.Percentage(16.6);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //osha 10
                    workerOsha10TextBoxes[j].Text = siteData[2, j];
                    workerOsha10TextBoxes[j].ID = "3|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    c.Controls.Add(workerOsha10TextBoxes[j]);
                    c.Width = Unit.Percentage(16.6);
                    r.Cells.Add(c);

                    TableCell d = new TableCell();
                    ListItem[] levelItems = { new ListItem("General Foreman"), new ListItem("Foreman"), new ListItem("Journeyman"), new ListItem("Apprentice") };
                    workerLevelDropDownList[j].Items.AddRange(levelItems);
                    workerLevelDropDownList[j].ID = "4|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    workerLevelDropDownList[j].SelectedValue = siteData[3, j];
                    d.Controls.Add(workerLevelDropDownList[j]);
                    d.Width = Unit.Percentage(16.6);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    workerUpdateButtons[j].Text = "Update this row";
                    workerUpdateButtons[j].Click += new EventHandler(OnWorkerUpdateButtonClick);
                    workerUpdateButtons[j].ID = "5|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    workerUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    e.Controls.Add(workerUpdateButtons[j]);
                    e.Width = Unit.Percentage(16.6);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    workerDeleteButtons[j].Text = "Delete this row";
                    workerDeleteButtons[j].Click += new EventHandler(OnWorkerDeleteButtonClick);
                    workerDeleteButtons[j].ID = "6|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    workerDeleteButtons[j].ToolTip = "Click delete to remove this worker";
                    f.Controls.Add(workerDeleteButtons[j]);
                    f.Width = Unit.Percentage(16.6);
                    r.Cells.Add(f);


                    //   }
                    //workerTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //trade type
                workerTradeDD = new DropDownList();
                ListItem[] tradeItemsx = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                workerTradeDD.Items.AddRange(tradeItemsx);
                workerTradeDD.ID = "workerTradeDD";
                aa.Controls.Add(workerTradeDD);
                workerTradeDD.SelectedValue = "Ironworker";
                aa.Width = Unit.Percentage(20);
                rr.Cells.Add(aa);

                TableCell bb = new TableCell(); //worker name
                workerNameTB = new TextBox();
                workerNameTB.Text = "";
                workerNameTB.ID = "workerNameTB";
                bb.Controls.Add(workerNameTB);
                bb.Width = Unit.Percentage(20);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                workerOsha10TB = new TextBox();
                workerOsha10TB.Text = "";
                workerOsha10TB.ID = "oshaTB";
                cc.Controls.Add(workerOsha10TB);
                cc.Width = Unit.Percentage(20);
                rr.Cells.Add(cc);

                TableCell dd = new TableCell();
                workerLevelDD = new DropDownList();
                ListItem[] levelItemsx = { new ListItem("General Foreman"), new ListItem("Foreman"), new ListItem("Journeyman"), new ListItem("Apprentice") };
                workerLevelDD.Items.AddRange(levelItemsx);
                workerLevelDD.ID = "workerlevelDD";
                dd.Controls.Add(workerLevelDD);
                // levelDropDownList[WorkerRowCount + 1].SelectedValue = sumData[3, j];
                dd.Width = Unit.Percentage(20);
                rr.Cells.Add(dd);

                TableCell ee = new TableCell();
                var addNewWorkerButton = new Button();
                addNewWorkerButton.Text = "Add this row";
                addNewWorkerButton.Click += new EventHandler(addNewWorkerButtonClick);
                addNewWorkerButton.ID = "5";
                addNewWorkerButton.ToolTip = "Add the needed data and then click add this row";
                ee.Controls.Add(addNewWorkerButton);
                ee.Width = Unit.Percentage(20);
                rr.Cells.Add(ee);

                //workerTable.Rows.Add(rr);


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void addSiteTable()
        {
            string[,] siteData = new string[6, 100];
            string[,] sectionData = new string[7, 500];


            try
            {
                siteData = getSiteData(companyName);
                // this section is for creatin the worker update buttons

                workerNameTextBoxes = new TextBox[WorkerRowCount + 1];
                workerOsha10TextBoxes = new TextBox[WorkerRowCount + 1];

                workerTradeNameDropDownList = new DropDownList[WorkerRowCount + 1];
                workerLevelDropDownList = new DropDownList[WorkerRowCount + 1];

                workerUpdateButtons = new Button[WorkerRowCount + 1];
                workerDeleteButtons = new Button[WorkerRowCount + 1];


                for (int i = 0; i < WorkerRowCount + 1; i++)
                {
                    var a = new DropDownList();
                    workerTradeNameDropDownList[i] = a;

                    var b = new TextBox();
                    workerNameTextBoxes[i] = b;

                    var c = new TextBox();
                    workerOsha10TextBoxes[i] = c;

                    var d = new DropDownList();
                    workerLevelDropDownList[i] = d;

                    var e = new Button();
                    workerUpdateButtons[i] = e;

                    var f = new Button();
                    workerDeleteButtons[i] = f;
                }


                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Trade";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Name";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "OSHA 10";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Level";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Delete";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Update";
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
                //workerTable.Rows.AddAt(0, headerRow);

                int numrows = WorkerRowCount;
                for (int j = 0; j < numrows; j++)
                {

                    // for (int i = 0; i < numcells; i++)
                    //  { //  c.Controls.Add(new LiteralControl("row " + j.ToString() + ", cell " ));
                    //  dropdownlist.ClearSelection(); //making sure the previous selection has been cleared
                    //   dropdownlist.Items.FindByValue(value).Selected = true;
                    //DropDownList
                    //   dd1.ClearSelection(); //making sure the previous selection has been cleared
                    //dd1.Items.FindByValue(sumData[0, j]).Selected = true;

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type
                    ListItem[] tradeItems = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                    workerTradeNameDropDownList[j].Items.AddRange(tradeItems);
                    workerTradeNameDropDownList[j].ID = "1|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    a.Controls.Add(workerTradeNameDropDownList[j]);
                    workerTradeNameDropDownList[j].SelectedValue = siteData[0, j];
                    a.Width = Unit.Percentage(16.6);
                    r.Cells.Add(a);

                    TableCell b = new TableCell(); //worker name
                    workerNameTextBoxes[j].Text = siteData[1, j];
                    workerNameTextBoxes[j].ID = "2|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    b.Controls.Add(workerNameTextBoxes[j]);
                    b.Width = Unit.Percentage(16.6);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //osha 10
                    workerOsha10TextBoxes[j].Text = siteData[2, j];
                    workerOsha10TextBoxes[j].ID = "3|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    c.Controls.Add(workerOsha10TextBoxes[j]);
                    c.Width = Unit.Percentage(16.6);
                    r.Cells.Add(c);

                    TableCell d = new TableCell();
                    ListItem[] levelItems = { new ListItem("General Foreman"), new ListItem("Foreman"), new ListItem("Journeyman"), new ListItem("Apprentice") };
                    workerLevelDropDownList[j].Items.AddRange(levelItems);
                    workerLevelDropDownList[j].ID = "4|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    workerLevelDropDownList[j].SelectedValue = siteData[3, j];
                    d.Controls.Add(workerLevelDropDownList[j]);
                    d.Width = Unit.Percentage(16.6);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    workerUpdateButtons[j].Text = "Update this row";
                    workerUpdateButtons[j].Click += new EventHandler(OnWorkerUpdateButtonClick);
                    workerUpdateButtons[j].ID = "5|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    workerUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    e.Controls.Add(workerUpdateButtons[j]);
                    e.Width = Unit.Percentage(16.6);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    workerDeleteButtons[j].Text = "Delete this row";
                    workerDeleteButtons[j].Click += new EventHandler(OnWorkerDeleteButtonClick);
                    workerDeleteButtons[j].ID = "6|" + j.ToString() + "|" + siteData[5, j] + "|W";
                    workerDeleteButtons[j].ToolTip = "Click delete to remove this worker";
                    f.Controls.Add(workerDeleteButtons[j]);
                    f.Width = Unit.Percentage(16.6);
                    r.Cells.Add(f);


                    //   }
                    //workerTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //trade type
                workerTradeDD = new DropDownList();
                ListItem[] tradeItemsx = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                workerTradeDD.Items.AddRange(tradeItemsx);
                workerTradeDD.ID = "workerTradeDD";
                aa.Controls.Add(workerTradeDD);
                workerTradeDD.SelectedValue = "Ironworker";
                aa.Width = Unit.Percentage(20);
                rr.Cells.Add(aa);

                TableCell bb = new TableCell(); //worker name
                workerNameTB = new TextBox();
                workerNameTB.Text = "";
                workerNameTB.ID = "workerNameTB";
                bb.Controls.Add(workerNameTB);
                bb.Width = Unit.Percentage(20);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                workerOsha10TB = new TextBox();
                workerOsha10TB.Text = "";
                workerOsha10TB.ID = "oshaTB";
                cc.Controls.Add(workerOsha10TB);
                cc.Width = Unit.Percentage(20);
                rr.Cells.Add(cc);

                TableCell dd = new TableCell();
                workerLevelDD = new DropDownList();
                ListItem[] levelItemsx = { new ListItem("General Foreman"), new ListItem("Foreman"), new ListItem("Journeyman"), new ListItem("Apprentice") };
                workerLevelDD.Items.AddRange(levelItemsx);
                workerLevelDD.ID = "workerlevelDD";
                dd.Controls.Add(workerLevelDD);
                // levelDropDownList[WorkerRowCount + 1].SelectedValue = sumData[3, j];
                dd.Width = Unit.Percentage(20);
                rr.Cells.Add(dd);

                TableCell ee = new TableCell();
                var addNewWorkerButton = new Button();
                addNewWorkerButton.Text = "Add this row";
                addNewWorkerButton.Click += new EventHandler(addNewWorkerButtonClick);
                addNewWorkerButton.ID = "5";
                addNewWorkerButton.ToolTip = "Add the needed data and then click add this row";
                ee.Controls.Add(addNewWorkerButton);
                ee.Width = Unit.Percentage(20);
                rr.Cells.Add(ee);

                //workerTable.Rows.Add(rr);


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getWorkerTable()
        {
            string[,] sumData = new string[6, 100];
            

            //ListItem[] tradeItems = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };

            //ListItem[] levelItems = { new ListItem("General Foreman"), new ListItem("Foreman"), new ListItem("Journeyman"), new ListItem("Apprentice") };


            try
            {
                sumData = getWorkerData(companyName);
                // this section is for creatin the worker update buttons

                workerNameTextBoxes = new TextBox[WorkerRowCount + 1];
                workerOsha10TextBoxes = new TextBox[WorkerRowCount + 1];

                workerTradeNameDropDownList = new DropDownList[WorkerRowCount + 1];
                workerLevelDropDownList = new DropDownList[WorkerRowCount + 1];

                workerUpdateButtons = new Button[WorkerRowCount + 1];
                workerDeleteButtons = new Button[WorkerRowCount + 1];


                for (int i = 0; i < WorkerRowCount + 1; i++)
                {
                    var a = new DropDownList();
                    workerTradeNameDropDownList[i] = a;

                    var b = new TextBox();
                    workerNameTextBoxes[i] = b;

                    var c = new TextBox();
                    workerOsha10TextBoxes[i] = c;

                    var d = new DropDownList();
                    workerLevelDropDownList[i] = d;

                    var e = new Button();
                    workerUpdateButtons[i] = e;

                    var f = new Button();
                    workerDeleteButtons[i] = f;
                }


                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Trade";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Name";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "OSHA 10";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Level";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Delete";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Update";
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
                //workerTable.Rows.AddAt(0, headerRow);

                int numrows = WorkerRowCount;
                for (int j = 0; j < numrows; j++)
                {

                    // for (int i = 0; i < numcells; i++)
                    //  { //  c.Controls.Add(new LiteralControl("row " + j.ToString() + ", cell " ));
                    //  dropdownlist.ClearSelection(); //making sure the previous selection has been cleared
                    //   dropdownlist.Items.FindByValue(value).Selected = true;
                    //DropDownList
                    //   dd1.ClearSelection(); //making sure the previous selection has been cleared
                    //dd1.Items.FindByValue(sumData[0, j]).Selected = true;

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type
                    ListItem[] tradeItems = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                    workerTradeNameDropDownList[j].Items.AddRange(tradeItems);
                    workerTradeNameDropDownList[j].ID = "1|" + j.ToString() + "|"  + sumData[5, j] + "|W";
                    a.Controls.Add(workerTradeNameDropDownList[j]);
                    workerTradeNameDropDownList[j].SelectedValue = sumData[0, j];
                    a.Width = Unit.Percentage(16.6);
                    r.Cells.Add(a);

                    TableCell b = new TableCell(); //worker name
                    workerNameTextBoxes[j].Text = sumData[1, j];
                    workerNameTextBoxes[j].ID = "2|" + j.ToString() + "|" + sumData[5, j] + "|W";
                    b.Controls.Add(workerNameTextBoxes[j]);
                    b.Width = Unit.Percentage(16.6);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //osha 10
                    workerOsha10TextBoxes[j].Text = sumData[2, j];
                    workerOsha10TextBoxes[j].ID = "3|" + j.ToString() + "|" + sumData[5, j] + "|W";
                    c.Controls.Add(workerOsha10TextBoxes[j]);
                    c.Width = Unit.Percentage(16.6);
                    r.Cells.Add(c);

                    TableCell d = new TableCell();
                    ListItem[] levelItems = { new ListItem("General Foreman"), new ListItem("Foreman"), new ListItem("Journeyman"), new ListItem("Apprentice") };
                    workerLevelDropDownList[j].Items.AddRange(levelItems);
                    workerLevelDropDownList[j].ID = "4|" + j.ToString() + "|" + sumData[5, j] + "|W";
                    workerLevelDropDownList[j].SelectedValue = sumData[3, j];
                    d.Controls.Add(workerLevelDropDownList[j]);              
                    d.Width = Unit.Percentage(16.6);
                    r.Cells.Add(d);
                    
                    TableCell e = new TableCell();
                    workerUpdateButtons[j].Text = "Update this row";
                    workerUpdateButtons[j].Click += new EventHandler(OnWorkerUpdateButtonClick);
                    workerUpdateButtons[j].ID = "5|" + j.ToString() + "|" + sumData[5, j] + "|W";
                    workerUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    e.Controls.Add(workerUpdateButtons[j]);
                    e.Width = Unit.Percentage(16.6);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    workerDeleteButtons[j].Text = "Delete this row";
                    workerDeleteButtons[j].Click += new EventHandler(OnWorkerDeleteButtonClick);
                    workerDeleteButtons[j].ID = "6|" + j.ToString() + "|" + sumData[5, j] + "|W";
                    workerDeleteButtons[j].ToolTip = "Click delete to remove this worker";
                    f.Controls.Add(workerDeleteButtons[j]);
                    f.Width = Unit.Percentage(16.6);
                    r.Cells.Add(f);


                    //   }
                    //workerTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //trade type
                workerTradeDD = new DropDownList();
                ListItem[] tradeItemsx = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                workerTradeDD.Items.AddRange(tradeItemsx);
                workerTradeDD.ID = "workerTradeDD";
                aa.Controls.Add(workerTradeDD);
                workerTradeDD.SelectedValue = "Ironworker";
                aa.Width = Unit.Percentage(20);
                rr.Cells.Add(aa);

                TableCell bb = new TableCell(); //worker name
                workerNameTB = new TextBox();
                workerNameTB.Text = "";
                workerNameTB.ID = "workerNameTB";
                bb.Controls.Add(workerNameTB);
                bb.Width = Unit.Percentage(20);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                workerOsha10TB = new TextBox();
                workerOsha10TB.Text = "";
                workerOsha10TB.ID = "oshaTB";
                cc.Controls.Add(workerOsha10TB);
                cc.Width = Unit.Percentage(20);
                rr.Cells.Add(cc);

                TableCell dd = new TableCell();
                workerLevelDD = new DropDownList();
                ListItem[] levelItemsx = { new ListItem("General Foreman"), new ListItem("Foreman"), new ListItem("Journeyman"), new ListItem("Apprentice") };
                workerLevelDD.Items.AddRange(levelItemsx);
                workerLevelDD.ID = "workerlevelDD";
                dd.Controls.Add(workerLevelDD);
               // levelDropDownList[WorkerRowCount + 1].SelectedValue = sumData[3, j];
                dd.Width = Unit.Percentage(20);
                rr.Cells.Add(dd);

                TableCell ee = new TableCell();
                var addNewWorkerButton = new Button();
                addNewWorkerButton.Text = "Add this row";
                addNewWorkerButton.Click += new EventHandler(addNewWorkerButtonClick);
                addNewWorkerButton.ID = "5";
                addNewWorkerButton.ToolTip = "Add the needed data and then click add this row";
                ee.Controls.Add(addNewWorkerButton);
                ee.Width = Unit.Percentage(20);
                rr.Cells.Add(ee);

                //workerTable.Rows.Add(rr);


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getSectionTable(String siteName)
        {
            string[,] sumData = new string[4, 100];

            try
            {
                sumData = getSiteSectionsData(companyName, globalSiteName);
                // this section is for creatin the section update buttons

                sectionUpdateButtons = new Button[sectionRowCount];
                sectionDeleteButtons = new Button[sectionRowCount];

                sectionSiteLabels = new Label[sectionRowCount];
                sectionTextBoxes = new TextBox[sectionRowCount];
                sectionCheckFW = new CheckBox[sectionRowCount];

                for (int i = 0; i < sectionRowCount; i++)
                {
                    var a = new Label();
                    sectionSiteLabels[i] = a;

                    var b = new TextBox();
                    sectionTextBoxes[i] = b;

                    var  c = new CheckBox();
                    sectionCheckFW[i] = c;

                    var d = new Button();
                    sectionUpdateButtons[i] = d;

                    var e = new Button();
                    sectionDeleteButtons[i] = e;

                }

                sectionTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Site";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Section";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Field Work";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Update";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Delete";
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
                sectionTable.Rows.AddAt(0, headerRow);

                int numrows = sectionRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //worker name
                    sectionSiteLabels[j].Text = sumData[0, j];
                    sectionSiteLabels[j].ID = "1|" + j.ToString() + "|" + sumData[4, j] + "|S";
                    a.Controls.Add(sectionSiteLabels[j]);
                    a.Width = Unit.Percentage(20);
                    r.Cells.Add(a);

                    TableCell b = new TableCell(); //worker name
                    sectionTextBoxes[j].Text = sumData[2, j];
                    sectionTextBoxes[j].ID = "2|" + j.ToString() + "|" + sumData[4, j] + "|S";
                    b.Controls.Add(sectionTextBoxes[j]);
                    b.Width = Unit.Percentage(20);
                    r.Cells.Add(b);

                    TableCell c = new TableCell();
                    string tempxxxx = sumData[3, j];
                    sectionCheckFW[j].Checked = sumData[3, j] == "True" ? true : false;
                    sectionCheckFW[j].ID = "3|" + j.ToString() + "|" + sumData[4, j] + "|S";
                    sectionCheckFW[j].ToolTip = "Check the box if this is field work and then click update";
                    c.Controls.Add(sectionCheckFW[j]);
                    c.Width = Unit.Percentage(20);
                    r.Cells.Add(c);

                    TableCell d = new TableCell();
                    sectionUpdateButtons[j].Text = "Update this row";
                    sectionUpdateButtons[j].Click += new EventHandler(OnSectionUpdateButtonClick);
                    sectionUpdateButtons[j].ID = "4|" + j.ToString() + "|" + sumData[4, j] + "|S";
                    sectionUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    d.Controls.Add(sectionUpdateButtons[j]);
                    d.Width = Unit.Percentage(20);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    sectionDeleteButtons[j].Text = "Delete this row";
                    sectionDeleteButtons[j].Click += new EventHandler(OnSectionDeleteButtonClick);
                    sectionDeleteButtons[j].ID = "5|" + j.ToString() + "|" + sumData[4, j] + "|S";
                    sectionDeleteButtons[j].ToolTip = "Click delete to remove this section";
                    e.Controls.Add(sectionDeleteButtons[j]);
                    e.Width = Unit.Percentage(20);
                    r.Cells.Add(e);


                    //   }
                    sectionTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //osha 10
                sectionSiteLabel = new Label();
                sectionSiteLabel.Text = siteName;
                sectionSiteLabel.ID = "sectionLabel";
                aa.Controls.Add(sectionSiteLabel);
                aa.Width = Unit.Percentage(25);
                rr.Cells.Add(aa);

                TableCell bb = new TableCell(); //osha 10
                sectionTB = new TextBox();
                sectionTB.Text = "";
                sectionTB.ID = "sectionTB";
                bb.Controls.Add(sectionTB);
                bb.Width = Unit.Percentage(25);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                sectionCB = new CheckBox();
                sectionCB.Checked = false;
                sectionCB.ID = "sectionCB";
                cc.Controls.Add(sectionCB);
                cc.Width = Unit.Percentage(25);
                rr.Cells.Add(cc);

                TableCell dd = new TableCell();                
                addNewSectionButton.Text = "Add this row";
                addNewSectionButton.Click += new EventHandler(addNewSectionButtonClick);
                addNewSectionButton.ID = "55aaaaaaaa";
                addNewSectionButton.ToolTip = "Add the needed data and then click add this row";
                dd.Controls.Add(addNewSectionButton);
                dd.Width = Unit.Percentage(25);
                rr.Cells.Add(dd);

                sectionTable.Rows.Add(rr);

                //UpdatePanel6.Update();
                /*
                                var ClickButton = UpdatePanel6.FindControl("addNewSectionButton") as Button;


                                var trigger = new PostBackTrigger();
                                trigger.ControlID = ClickButton.UniqueID.ToString();
                                UpdatePanel6.Triggers.Add(trigger);

                                UpdatePanel6.Update();

                                UpdatePanel6.Triggers.Add(new AsyncPostBackTrigger()
                                {
                                    ControlID = updateSiteButton.UniqueID,
                                   // EventName = "SelectedIndexChanged", // this may be optional
                                });

                                */
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getConstructionTable(String siteName)
        {
            string[,] sumData = new string[5, 100];
            String[,] sectionData = new string[4, 100];
            List<string> constructionDistinctList = new List<string>();

            try
            {
                sumData = getConstructionData(companyName, siteName);
                // this section is for creatin the section update buttons

                sectionData = getSiteSectionsData(companyName, siteName);
                constructionDistinctList = getDistinctConstructionData(companyName);

                constructionSiteNameLabels = new Label[constructionRowCount];
                constructionSectionNameDropDownList = new DropDownList[constructionRowCount];
                constructionDropDown = new DropDownList[constructionRowCount];
                constructionHoursTextBoxes = new TextBox[constructionRowCount];
                constructionLabelFW = new Label[constructionRowCount];

                constructionUpdateButtons = new Button[constructionRowCount];
                constructionDeleteButtons = new Button[constructionRowCount];


                for (int i = 0; i < constructionRowCount; i++)
                {
                    var a = new Label();
                    constructionSiteNameLabels[i] = a;

                    var b = new DropDownList();
                    constructionSectionNameDropDownList[i] = b;

                 //   var c = new TextBox();
                    constructionDropDown[i] = new DropDownList();

                    var d = new TextBox();
                    constructionHoursTextBoxes[i] = d;

                    var e = new Label();
                    constructionLabelFW[i] = e;

                    var f = new Button();
                    constructionUpdateButtons[i] = f;

                    var g = new Button();
                    constructionDeleteButtons[i] = g;

                }

                constructionTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Site";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Section";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Construction";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Hours";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Field Work";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Update";
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = "Delete";
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
                constructionTable.Rows.AddAt(0, headerRow);

                int secRows = sectionRowCount;

                ConstructionSectionItems.Clear();

                for (int z = 0; z < sectionRowCount; z++)
                {
                    string temp = sectionData[2, z];
                    ConstructionSectionItems.Add( new ListItem(sectionData[2, z]));
                }
               // ListItem[] tradeItems = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                int numrows = constructionRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //site name
                    constructionSiteNameLabels[j].Text = sumData[0, j];
                    constructionSiteNameLabels[j].ID = "1|" + j.ToString() + "|" + sumData[5, j] + "|C";
                    a.Controls.Add(constructionSiteNameLabels[j]);
                    a.Width = Unit.Percentage(14.3);
                    r.Cells.Add(a);

                    TableCell b = new TableCell(); //site type   
                    constructionSectionNameDropDownList[j].Items.Clear();
                    constructionSectionNameDropDownList[j].DataSource = ConstructionSectionItems;
                    constructionSectionNameDropDownList[j].DataBind();
                    constructionSectionNameDropDownList[j].ID = "2|" + j.ToString() + "|" + sumData[5, j] + "|C" + "|" + sumData[1, j];
                    constructionSectionNameDropDownList[j].SelectedIndexChanged += new EventHandler(constructionSectionMain_SelectedIndexChanged);
                    constructionSectionNameDropDownList[j].AutoPostBack = true;
                    b.Controls.Add(constructionSectionNameDropDownList[j]);
                    constructionSectionNameDropDownList[j].SelectedValue = sumData[1, j];
                    b.Width = Unit.Percentage(14.3);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //worker name
                    constructionDropDown[j].Items.Clear();
                    constructionDropDown[j].DataSource = constructionDistinctList;
                    constructionDropDown[j].DataBind();
                    constructionDropDown[j].ID = "3|" + j.ToString() + "|" + sumData[5, j] + "|C";
                    constructionDropDown[j].SelectedValue = sumData[2, j];
                    c.Controls.Add(constructionDropDown[j]);
                    c.Width = Unit.Percentage(14.3);
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    constructionHoursTextBoxes[j].Text = sumData[3, j];
                    constructionHoursTextBoxes[j].ID = "4|" + j.ToString() + "|" + sumData[5, j] + "|C";
                    d.Controls.Add(constructionHoursTextBoxes[j]);
                    d.Width = Unit.Percentage(14.3);
                    r.Cells.Add(d);

                    TableCell e = new TableCell(); //this is the checkbox
                   // constructionLabelFW[j].Text = sumData[4, j] == "1" ? "true" : "false";
                    constructionLabelFW[j].Text = getSectionsFWstatus(companyName, constructionSectionNameDropDownList[j].SelectedValue) == "True" ? "True" : "False";
                    constructionLabelFW[j].ID = "5|" + j.ToString() + "|" + sumData[5, j] + "|C";
                    //string emptemp =  getSectionsFWstatus(companyName, constructionSectionNameDropDownList[j].SelectedValue) == "1" ? "true" : "false";
                    e.HorizontalAlign = HorizontalAlign.Center;
                    e.Controls.Add(constructionLabelFW[j]);
                    e.Width = Unit.Percentage(14.3);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    constructionUpdateButtons[j].Text = "Update this row";
                    constructionUpdateButtons[j].Click += new EventHandler(OnConstructionUpdateButtonClick);
                    constructionUpdateButtons[j].ID = "6|" + j.ToString() + "|" + sumData[5, j] + "|C";
                    constructionUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    f.Controls.Add(constructionUpdateButtons[j]);
                    f.Width = Unit.Percentage(14.3);
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    constructionDeleteButtons[j].Text = "Delete this row";
                    constructionDeleteButtons[j].Click += new EventHandler(OnConstructionDeleteButtonClick);
                    constructionDeleteButtons[j].ID = "7|" + j.ToString() + "|" + sumData[5, j] + "|C";
                    constructionDeleteButtons[j].ToolTip = "Click delete to remove this section";
                    g.Controls.Add(constructionDeleteButtons[j]);
                    g.Width = Unit.Percentage(14.3);
                    r.Cells.Add(g);


                    //   }
                    constructionTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                if (sectionData[0,0] == "-1")
                {
                    TableCell xx = new TableCell(); //osha 10
                    Label noSectionLabel = new Label();
                    noSectionLabel.Text = "Create a work section above";
                    noSectionLabel.ID = "noSectionLabel_ID";
                    xx.Controls.Add(noSectionLabel);
                    xx.Width = Unit.Percentage(100);
                    rr.Cells.Add(xx);
                }

          //      Label constructionSiteNameLB;
           //     DropDownList constructionSectionDD;
           //     TextBox constructionTB;
          //      TextBox constructionHoursTB;taskSectionMain_SelectedIndexChanged

                TableCell aa = new TableCell(); //osha 10
                constructionSiteNameLB = new Label();
                constructionSiteNameLB.Text = globalSiteName;
                constructionSiteNameLB.ID = "constructionSiteNameLB";
                aa.Controls.Add(constructionSiteNameLB);
                aa.Width = Unit.Percentage(16.6);
                rr.Cells.Add(aa);

                TableCell bb = new TableCell(); //site type  
                constructionSectionDD = new DropDownList();
                constructionSectionDD.Items.Clear();
                if (constructionDistinctList.Count > 0) { constructionSectionDD.DataSource =  ConstructionSectionItems; constructionSectionDD.DataBind(); }
                if (constructionDistinctList.Count == 0) { constructionSectionDD.Items.Add(new ListItem("add Construction above")); }
                constructionSectionDD.ID = "constructionSectionDD";
                constructionSectionDD.SelectedIndexChanged += new EventHandler(constructionSectionDD_SelectedIndexChanged); //
                constructionSectionDD.SelectedIndex = 0;
                constructionSectionDD.AutoPostBack = true;
                //string tempSection = constructionSectionDD.SelectedValue;
                bb.Controls.Add(constructionSectionDD);
                bb.Width = Unit.Percentage(16.6);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                constructionDD = new DropDownList();
                constructionDD.DataSource = constructionDistinctList;
                constructionDD.DataBind();
                constructionDD.ID = "constructionDD";
                constructionDD.Items.Add("Choose construction type");
                constructionDD.SelectedValue = "Choose construction type";
                cc.Controls.Add(constructionDD);
                cc.Width = Unit.Percentage(16.6);
                rr.Cells.Add(cc);

                TableCell dd = new TableCell(); //osha 10
                constructionHoursTB = new TextBox();
                constructionHoursTB.Text = "";
                constructionHoursTB.ID = "constructionHoursTB";
                dd.Controls.Add(constructionHoursTB);
                dd.Width = Unit.Percentage(16.6);
                rr.Cells.Add(dd);

                TableCell ee = new TableCell(); //osha 10
                constructionFWLB = new Label();
                constructionFWLB.Text = getSectionsFWstatus(companyName, constructionSectionDD.SelectedValue) == "True" ? "True" : "False";
                constructionFWLB.ID = "constructionFWLB";
                ee.HorizontalAlign = HorizontalAlign.Center;
                ee.Controls.Add(constructionFWLB);
                ee.Width = Unit.Percentage(16.6);
                rr.Cells.Add(ee);

                TableCell ff = new TableCell();
                Button addNewConstructionButton = new Button();
                addNewConstructionButton.Text = "Add this row";
                addNewConstructionButton.Click += new EventHandler(addNewConstructionButtonClick);
                addNewConstructionButton.ID = "66";
                addNewConstructionButton.ToolTip = "Add the needed data and then click add this row";
                ff.Controls.Add(addNewConstructionButton);
                ff.Width = Unit.Percentage(16.6);
                rr.Cells.Add(ff);

                constructionTable.Rows.Add(rr);

                //UpdatePanel6.Update();
                /*
                                var ClickButton = UpdatePanel6.FindControl("addNewSectionButton") as Button;


                                var trigger = new PostBackTrigger();
                                trigger.ControlID = ClickButton.UniqueID.ToString();
                                UpdatePanel6.Triggers.Add(trigger);

                                UpdatePanel6.Update();

                                UpdatePanel6.Triggers.Add(new AsyncPostBackTrigger()
                                {
                                    ControlID = updateSiteButton.UniqueID,
                                   // EventName = "SelectedIndexChanged", // this may be optional
                                });

                                */
            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getTaskTable()
        {
            String[,] sumData = new String[7, 100];
            String[,] sectionData = new String[5, 100];
            String[,] constructData = new String[6, 500];
            String[,] constructDataBySection = new String[6, 500];

            List<string> constructionDistinctList = new List<string>();
            List<string> taskDistinctList = new List<string>();
            List<string> measureDistinctList = new List<string>();

            try
            {
                sumData = getTaskData(companyName, globalSiteName);
                sectionData = getSiteSectionsData(companyName, globalSiteName);
                constructData = getConstructionData(companyName, globalSiteName);

                constructionDistinctList = getDistinctConstructionData(companyName);
                taskDistinctList = getDistinctTaskData(companyName);
                measureDistinctList = getDistinctMeasureData(companyName);


                if (sectionRowCount == 1)
                {
                    constructDataBySection = getConstructionDataBySection(companyName, globalSiteName, sectionData[2, 0]);
                }

                //need something to get construction choices based on section
                // this section is for creatin the worker update buttons
                taskUpdateButtons = new Button[taskRowCount];
                taskDeleteButtons = new Button[taskRowCount];

                taskDropDown = new DropDownList[taskRowCount];
                taskMeasureDropDownList = new DropDownList[taskRowCount];

                taskSectionNameDropDownList = new DropDownList[taskRowCount];
                taskConstructionDropDownList = new DropDownList[taskRowCount];

                taskFWLabel = new Label[taskRowCount];


                for (int i = 0; i < taskRowCount; i++)
                {
                    var a = new DropDownList();
                    taskSectionNameDropDownList[i] = a;

                    var b = new DropDownList();
                    taskConstructionDropDownList[i] = b;

                    taskDropDown[i] = new DropDownList();

                    var d = new DropDownList();
                    taskMeasureDropDownList[i] = d;

                    var e = new Label();
                    taskFWLabel[i] = e;

                    var f = new Button();
                    taskUpdateButtons[i] = f;

                    var g = new Button();
                    taskDeleteButtons[i] = g;
                }

                taskTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Section";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Construction";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Task";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Measure";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Fieldwork";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Delete";
                header6.Font.Bold = true;
                header6.BackColor = Color.LightGray;
                header6.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header7 = new TableHeaderCell();
                header7.Text = "Update";
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
                taskTable.Rows.AddAt(0, headerRow);

                taskSectionItems.Clear();

                for (int z = 0; z < sectionRowCount; z++)
                {
                    taskSectionItems.Add( new ListItem(sectionData[2, z]));
                }
                tempConstrutListItems.Clear();

                if (sectionRowCount == 1)
                {
                    for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
                    {
                        string temp111 = constructDataBySection[2, z];
                        tempConstrutListItems.Add(new ListItem(constructDataBySection[2, z]));
                    }
                }

                int numrows = taskRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type
                    taskSectionNameDropDownList[j].Items.Clear();
                    taskSectionNameDropDownList[j].DataSource = taskSectionItems;
                    taskSectionNameDropDownList[j].DataBind();
                    taskSectionNameDropDownList[j].ID = "1|" + j.ToString() + "|" + sumData[6, j] + "|T";
                    taskSectionNameDropDownList[j].SelectedIndexChanged += new EventHandler(taskSectionMain_SelectedIndexChanged);
                    taskSectionNameDropDownList[j].AutoPostBack = true;
                    a.Controls.Add(taskSectionNameDropDownList[j]);
                    taskSectionNameDropDownList[j].SelectedValue = sumData[0, j];
                    a.Width = Unit.Percentage(14.2);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    taskConstructionDropDownList[j].Items.Clear();
                    taskConstructionDropDownList[j].DataSource = constructionDistinctList;
                    taskConstructionDropDownList[j].DataBind();                  
                    taskConstructionDropDownList[j].ID = "2|" + j.ToString() + "|" + sumData[6, j] + "|T";
                    taskConstructionDropDownList[j].SelectedValue = sumData[2, j];
                    b.Controls.Add(taskConstructionDropDownList[j]);
                    b.Width = Unit.Percentage(14.2);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //task values
                    taskDropDown[j].Items.Clear();
                    taskDropDown[j].DataSource = taskDistinctList;
                    taskDropDown[j].DataBind();
                    taskDropDown[j].Text = sumData[3, j];
                    taskDropDown[j].ID = "3|" + j.ToString() + "|" + sumData[6, j] + "|T";
                    c.Controls.Add(taskDropDown[j]);
                    c.Width = Unit.Percentage(14.2);
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    taskMeasureDropDownList[j].Items.Clear();
                    taskMeasureDropDownList[j].DataSource = measureDistinctList;
                    taskMeasureDropDownList[j].DataBind();
                    taskMeasureDropDownList[j].ID = "4|" + j.ToString() + "|" + sumData[6, j] + "|T";
                    taskMeasureDropDownList[j].SelectedValue = sumData[4, j];
                    d.Controls.Add(taskMeasureDropDownList[j]);
                    d.Width = Unit.Percentage(14.2);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    taskFWLabel[j].Text = getSectionsFWstatus(companyName, taskSectionNameDropDownList[j].SelectedValue) == "True" ? "True" : "False";
                    taskFWLabel[j].ID = "5|" + j.ToString() + "|" + sumData[6, j] + "|T";
                    e.Controls.Add(taskFWLabel[j]);
                    e.Width = Unit.Percentage(14.2);
                    r.Cells.Add(e);
                    TableCell f = new TableCell();
                    taskUpdateButtons[j].Text = "Update this row";
                    taskUpdateButtons[j].Click += new EventHandler(OnTaskUpdateButtonClick);
                    taskUpdateButtons[j].ID = "6|" + j.ToString() + "|" + sumData[6, j] + "|T";
                    taskUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    f.Controls.Add(taskUpdateButtons[j]);
                    f.Width = Unit.Percentage(14.2);
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    taskDeleteButtons[j].Text = "Delete this row";
                    taskDeleteButtons[j].Click += new EventHandler(OnTaskDeleteButtonClick);
                    taskDeleteButtons[j].ID = "7|" + j.ToString() + "|" + sumData[6, j] + "|T";
                    taskDeleteButtons[j].ToolTip = "Click delete to remove this task";
                    g.Controls.Add(taskDeleteButtons[j]);
                    g.Width = Unit.Percentage(14.2);
                    r.Cells.Add(g);


                    //   }
                    taskTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //trade type
                taskSectionDD = new DropDownList();
                taskSectionDD.Items.Clear();
                if (taskSectionItems.Count > 0) { taskSectionDD.DataSource = taskSectionItems; taskSectionDD.DataBind();  }
                if (taskSectionItems.Count == 0) { taskSectionDD.Items.Add(new ListItem("add section above")); }
              //  taskSectionDD.Items.AddRange(taskSectionItems);
                taskSectionDD.ID = "taskSectionDD";
                taskSectionDD.SelectedIndexChanged += new EventHandler(taskSectionDD_SelectedIndexChanged);
                taskSectionDD.AutoPostBack = true;
                taskSectionDD.SelectedIndex = 0;
                //String tempSection = taskSectionDD.SelectedValue;
                aa.Controls.Add(taskSectionDD);
                aa.Width = Unit.Percentage(16.6);
                rr.Cells.Add(aa);

                //sectionSelectedConstructionRowCount
                TableCell bb = new TableCell();
                taskConstructionDD = new DropDownList();
                taskConstructionDD.Items.Clear();
                string[,] tempSections = getConstructionDataBySection(companyName, globalSiteName, Convert.ToString(taskSectionDD.Items[0].Text));

                for(int x = 0; x < 500; x++)
                {
                    if(tempSections[2,x] != null)
                    {
                        taskConstructionDD.Items.Add(tempSections[2, x]);
                    }
                    if (tempSections[2, x] == null)
                    {
                        x = 501;
                    }

                }

                if (taskConstructionDD.Items.Count == 0)
                {
                    ListItem[] typeItems = { new ListItem("Choose Section") };
                    taskConstructionDD.Items.AddRange(typeItems);
                }
                
                taskConstructionDD.ID = "taskConstructionDD";
                bb.Controls.Add(taskConstructionDD);
                bb.Width = Unit.Percentage(16.6);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                taskDD = new DropDownList();
                taskDD.Items.Clear();
                taskDD.DataSource = taskDistinctList;
                taskDD.DataBind();
                taskDD.Items.Add("Choose Task");
                taskDD.SelectedValue = "Choose Task";
                taskDD.ID = "taskDD";
                cc.Controls.Add(taskDD);
                cc.Width = Unit.Percentage(16.6);
                rr.Cells.Add(cc);

                TableCell dd = new TableCell(); //osha 10
                taskMeasureDD = new DropDownList();
                taskMeasureDD.Items.Clear();
                taskMeasureDD.DataSource = measureDistinctList;
                taskMeasureDD.DataBind();
                taskMeasureDD.ID = "taskMeasureDD";
                taskMeasureDD.Items.Add("Choose Measure");
                taskMeasureDD.SelectedValue = "Choose Measure";
                dd.Controls.Add(taskMeasureDD);
                dd.Width = Unit.Percentage(16.6);
                rr.Cells.Add(dd);

                TableCell ee = new TableCell(); //osha 10
                taskFWLab = new Label();
                taskFWLab.Text = getSectionsFWstatus(companyName, taskSectionDD.SelectedValue) == "True" ? "True" : "False";
                taskFWLab.ID = "taskFWLab";
                ee.Controls.Add(taskFWLab);
                ee.Width = Unit.Percentage(16.6);
                rr.Cells.Add(ee);

                TableCell ff = new TableCell();
                var addNewTaskButton = new Button();
                addNewTaskButton.Text = "Add this row";
                addNewTaskButton.Click += new EventHandler(addNewTaskButtonClick);
                addNewTaskButton.ID = "55";
                addNewTaskButton.ToolTip = "Add the needed data and then click add this row";
                ff.Controls.Add(addNewTaskButton);
                ff.Width = Unit.Percentage(16.6);
                rr.Cells.Add(ff);

                taskTable.Rows.Add(rr);


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getDeliveryTable()
        {
            string[,] sumData = new string[5, 100];
            String[,] sectionData = new String[4, 100];
            String[,] constructData = new String[5, 500];
            String[,] constructDataBySection = new String[5, 500];

            List<string> constructionDistinctList = new List<string>();
            List<string> taskDistinctList = new List<string>();
            List<string> measureDistinctList = new List<string>();
            List<string> deliveryDistinctList = new List<string>();

            try
            {
                sumData = getDeliveryData(companyName, globalSiteName);

                sectionData = getSiteSectionsData(companyName, globalSiteName);
                constructData = getConstructionData(companyName, globalSiteName);

                constructionDistinctList = getDistinctConstructionData(companyName);
                deliveryDistinctList = getDistinctDeliveryData(companyName);
                measureDistinctList = getDistinctMeasureData(companyName);

                if (sectionRowCount == 1)
                {
                    constructDataBySection = getConstructionDataBySection(companyName, globalSiteName, sectionData[2, 0]);
                }
                // this section is for creatin the worker update buttons

                deliveryTextBoxes = new TextBox[deliveryRowCount];

                deliverySectionNameDropDownList = new DropDownList[deliveryRowCount];
                deliveryConstructionDropDownList = new DropDownList[deliveryRowCount];
                deliveryMeasureDropDownList = new DropDownList[deliveryRowCount];

                deliveryUpdateButtons = new Button[deliveryRowCount];
                deliveryDeleteButtons = new Button[deliveryRowCount];


                for (int i = 0; i < deliveryRowCount; i++)
                {
                    var a = new DropDownList();
                    deliverySectionNameDropDownList[i] = a;

                    var b = new DropDownList();
                    deliveryConstructionDropDownList[i] = b;

                    var c = new TextBox();
                    deliveryTextBoxes[i] = c;

                    var d = new DropDownList();
                    deliveryMeasureDropDownList[i] = d;

                  //  var e = new Button();
                    deliveryUpdateButtons[i] = new Button();

                  //  var e = new Button();
                    deliveryDeleteButtons[i] = new Button();
                }

                deliveryTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Section";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Construction";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Material";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Measure";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Delete";
                header5.Font.Bold = true;
                header5.BackColor = Color.LightGray;
                header5.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header6 = new TableHeaderCell();
                header6.Text = "Update";
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

                deliverySectionItems.Clear();
                tempDeliveryConstrutListItems.Clear();

                for (int z = 0; z < sectionRowCount; z++)
                {
                    deliverySectionItems.Add(new ListItem(sectionData[2, z]));
                }

                if (sectionRowCount == 1)
                {
                    for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
                    {
                        tempDeliveryConstrutListItems.Add( new ListItem(constructDataBySection[2, z]));
                    }
                }



                int numrows = deliveryRowCount;
                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type
                    deliverySectionNameDropDownList[j].Items.Clear();

                    deliverySectionNameDropDownList[j].DataSource = deliverySectionItems;
                    deliverySectionNameDropDownList[j].DataBind();
                    deliverySectionNameDropDownList[j].ID = "1|" + j.ToString() + "|" + sumData[5, j] + "|T";
                    deliverySectionNameDropDownList[j].SelectedIndexChanged += new EventHandler(deliverySectionMain_SelectedIndexChanged);
                    deliverySectionNameDropDownList[j].AutoPostBack = true;
                    a.Controls.Add(deliverySectionNameDropDownList[j]);
                    deliverySectionNameDropDownList[j].SelectedValue = sumData[0, j];
                    a.Width = Unit.Percentage(16.6);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    if (sectionRowCount == 1)
                    {
                        deliveryConstructionDropDownList[j].DataSource = tempDeliveryConstrutListItems;
                        deliveryConstructionDropDownList[j].DataBind();
                    }

                    else
                    {
                        ListItem[] typeItems = { new ListItem(sumData[1, j]) };
                        deliveryConstructionDropDownList[j].Items.AddRange(typeItems);
                    }

                    deliveryConstructionDropDownList[j].ID = "2|" + j.ToString() + "|" + sumData[5, j] + "|T";
                    deliveryConstructionDropDownList[j].SelectedValue = sumData[2, j];
                    b.Controls.Add(deliveryConstructionDropDownList[j]);
                    b.Width = Unit.Percentage(16.6);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //worker name
                    deliveryTextBoxes[j].Text = sumData[3, j];
                    deliveryTextBoxes[j].ID = "3|" + j.ToString() + "|" + sumData[5, j] + "|D";
                    c.Controls.Add(deliveryTextBoxes[j]);
                    c.Width = Unit.Percentage(16.6);
                    r.Cells.Add(c);

                    TableCell d = new TableCell();
                    //ListItem[] measureDItems = { new ListItem("Pieces"), new ListItem("Trucks"), new ListItem("Container"), new ListItem("Pounds"), new ListItem("Tons"), new ListItem("Linear Feet"), new ListItem("Square Feet"), new ListItem("Rolls") };
                    deliveryMeasureDropDownList[j].DataSource = measureDistinctList;
                    deliveryMeasureDropDownList[j].DataBind();
                    deliveryMeasureDropDownList[j].ID = "4|" + j.ToString() + "|" + sumData[5, j] + "|D";
                    deliveryMeasureDropDownList[j].SelectedValue = sumData[4, j];
                    d.Controls.Add(deliveryMeasureDropDownList[j]);
                    d.Width = Unit.Percentage(16.6);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    deliveryUpdateButtons[j].Text = "Update this row";
                    deliveryUpdateButtons[j].Click += new EventHandler(OnDeliveryUpdateButtonClick);
                    deliveryUpdateButtons[j].ID = "5|" + j.ToString() + "|" + sumData[5, j] + "|D";
                    deliveryUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    e.Controls.Add(deliveryUpdateButtons[j]);
                    e.Width = Unit.Percentage(16.6);
                    r.Cells.Add(e);

                    TableCell f = new TableCell();
                    deliveryDeleteButtons[j].Text = "Delete this row";
                    deliveryDeleteButtons[j].Click += new EventHandler(OnDeliveryDeleteButtonClick);
                    deliveryDeleteButtons[j].ID = "6|" + j.ToString() + "|" + sumData[5, j] + "|D";
                    deliveryDeleteButtons[j].ToolTip = "Click delete to remove this worker";
                    f.Controls.Add(deliveryDeleteButtons[j]);
                    f.Width = Unit.Percentage(16.6);
                    r.Cells.Add(f);


                    //   }
                    deliveryTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //trade type
                deliverySectionDD = new DropDownList();
                if (deliveryDistinctList.Count > 0) { deliverySectionDD.DataSource = deliverySectionItems; deliverySectionDD.DataBind(); }
                if (deliveryDistinctList.Count == 0) { deliverySectionDD.Items.Add(new ListItem("add section above")); }
              //  deliverySectionDD.Items.AddRange(deliverySectionItems);
                deliverySectionDD.ID = "deliverySectionDD";
                deliverySectionDD.SelectedIndexChanged += new EventHandler(deliverySectionDD_SelectedIndexChanged);
                deliverySectionDD.AutoPostBack = true;
                aa.Controls.Add(deliverySectionDD);
                aa.Width = Unit.Percentage(20);
                rr.Cells.Add(aa);

                //sectionSelectedConstructionRowCount
                TableCell bb = new TableCell();
                deliveryConstructionDD = new DropDownList();
                string[,] tempSections = getConstructionDataBySection(companyName, globalSiteName, Convert.ToString(taskSectionDD.Items[0].Text));

                for (int x = 0; x < 500; x++)
                {
                    if (tempSections[2, x] != null)
                    {
                        deliveryConstructionDD.Items.Add(tempSections[2, x]);
                    }
                    if (tempSections[2, x] == null)
                    {
                        x = 501;
                    }

                }

                if (deliveryConstructionDD.Items.Count == 0)
                {
                    ListItem[] typeItems = { new ListItem("Choose Section") };
                    deliveryConstructionDD.Items.AddRange(typeItems);
                }

                deliveryConstructionDD.ID = "deliveryConstructionDD";
                bb.Controls.Add(deliveryConstructionDD);
                bb.Width = Unit.Percentage(20);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                deliveryTB = new TextBox();
                deliveryTB.Text = "";
                deliveryTB.ID = "deliveryTB";
                cc.Controls.Add(deliveryTB);
                cc.Width = Unit.Percentage(20);
                rr.Cells.Add(cc);

                TableCell dd = new TableCell();
                deliveryMeasureDD = new DropDownList();
                //ListItem[] measureDItemsx = { new ListItem("Pieces"), new ListItem("Trucks"), new ListItem("Container"), new ListItem("Pounds"), new ListItem("Tons"), new ListItem("Linear Feet"), new ListItem("Square Feet"), new ListItem("Rolls") };
                deliveryMeasureDD.DataSource = measureDistinctList;
                deliveryMeasureDD.DataBind();
                deliveryMeasureDD.ID = "deliveryMeasureDD";
                dd.Controls.Add(deliveryMeasureDD);
                dd.Width = Unit.Percentage(20);
                rr.Cells.Add(dd);

                TableCell ee = new TableCell();
                var addNewDeliveryButton = new Button();
                addNewDeliveryButton.Text = "Add this row";
                addNewDeliveryButton.Click += new EventHandler(addNewDeliveryButtonClick);
                addNewDeliveryButton.ID = "5x";
                addNewDeliveryButton.ToolTip = "Add the needed data and then click add this row";
                ee.Controls.Add(addNewDeliveryButton);
                ee.Width = Unit.Percentage(20);
                rr.Cells.Add(ee);

                deliveryTable.Rows.Add(rr);


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getCommentTable()
        {
            string[,] sumData = new string[6, 100];

            try
            {
                sumData = getCommentData(companyName);
                // this section is for creatin the worker update buttons

                commentTextBoxes = new TextBox[commentRowCount];

                commentTradeNameDropDownList = new DropDownList[commentRowCount];
                commentTypeDropDownList = new DropDownList[commentRowCount];

                commentUpdateButtons = new Button[commentRowCount];
                commentDeleteButtons = new Button[commentRowCount];


                for (int i = 0; i < commentRowCount; i++)
                {
                    var a = new DropDownList();
                    commentTradeNameDropDownList[i] = a;

                    var b = new DropDownList();
                    commentTypeDropDownList[i] = b;

                    var c = new TextBox();
                    commentTextBoxes[i] = c;

                    var d = new Button();
                    commentUpdateButtons[i] = d;

                    var e = new Button();
                    commentDeleteButtons[i] = e;
                }


                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Trade";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Type";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Comment";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "Delete";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Update";
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
                //commentTable.Rows.AddAt(0, headerRow);

                int numrows = commentRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type
                    ListItem[] tradeItems = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                    commentTradeNameDropDownList[j].Items.AddRange(tradeItems);
                    commentTradeNameDropDownList[j].ID = "1|" + j.ToString() + "|" + sumData[4, j];
                    a.Controls.Add(commentTradeNameDropDownList[j]);
                    commentTradeNameDropDownList[j].SelectedValue = sumData[0, j];
                    a.Width = Unit.Percentage(20);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    ListItem[] typeItems = { new ListItem("Worker"), new ListItem("Task"), new ListItem("Delivery") };
                    commentTypeDropDownList[j].Items.AddRange(typeItems);
                    commentTypeDropDownList[j].ID = "2|" + j.ToString() + "|" + sumData[4, j];
                    commentTypeDropDownList[j].SelectedValue = sumData[1, j];
                    b.Controls.Add(commentTypeDropDownList[j]);
                    b.Width = Unit.Percentage(20);
                    r.Cells.Add(b);

                    TableCell c = new TableCell(); //osha 10
                    commentTextBoxes[j].Text = sumData[2, j];
                    commentTextBoxes[j].ID = "3|" + j.ToString() + "|" + sumData[4, j];
                    c.Controls.Add(commentTextBoxes[j]);
                    c.Width = Unit.Percentage(20);
                    r.Cells.Add(c);

                    TableCell d = new TableCell();
                    commentUpdateButtons[j].Text = "Update this row";
                    commentUpdateButtons[j].Click += new EventHandler(OnCommentUpdateButtonClick);
                    commentUpdateButtons[j].ID = "4|" + j.ToString() + "|" + sumData[4, j];
                    commentUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    d.Controls.Add(commentUpdateButtons[j]);
                    d.Width = Unit.Percentage(20);
                    r.Cells.Add(d);

                    TableCell e = new TableCell();
                    commentDeleteButtons[j].Text = "Delete this row";
                    commentDeleteButtons[j].Click += new EventHandler(OnCommentDeleteButtonClick);
                    commentDeleteButtons[j].ID = "5|" + j.ToString() + "|" + sumData[4, j];
                    commentDeleteButtons[j].ToolTip = "Click delete to remove this comment";
                    e.Controls.Add(commentDeleteButtons[j]);
                    e.Width = Unit.Percentage(20);
                    r.Cells.Add(e);


                    //   }
                    //commentTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //trade type
                commentTradeDD = new DropDownList();
                ListItem[] tradeItemsx = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                commentTradeDD.Items.AddRange(tradeItemsx);
                commentTradeDD.ID = "commentTradeDD";
                aa.Controls.Add(commentTradeDD);
                commentTradeDD.SelectedValue = "Ironworker";
                aa.Width = Unit.Percentage(20);
                rr.Cells.Add(aa);

                TableCell bb = new TableCell();
                commentConstructionTypeDD = new DropDownList();
                ListItem[] typeItemsx = { new ListItem("Worker"), new ListItem("Task"), new ListItem("Delivery") };
                commentConstructionTypeDD.Items.AddRange(typeItemsx);
                commentConstructionTypeDD.ID = "commentDD";
                bb.Controls.Add(commentConstructionTypeDD);
                // levelDropDownList[WorkerRowCount + 1].SelectedValue = sumData[3, j];
                bb.Width = Unit.Percentage(20);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                commentTB = new TextBox();
                commentTB.Text = "";
                commentTB.ID = "commentTB";
                cc.Controls.Add(commentTB);
                cc.Width = Unit.Percentage(20);
                rr.Cells.Add(cc);

                TableCell ee = new TableCell();
                var addNewCommentButton = new Button();
                addNewCommentButton.Text = "Add this row";
                addNewCommentButton.Click += new EventHandler(addNewCommentButtonClick);
                addNewCommentButton.ID = "58";
                addNewCommentButton.ToolTip = "Add the needed data and then click add this row";
                ee.Controls.Add(addNewCommentButton);
                ee.Width = Unit.Percentage(20);
                rr.Cells.Add(ee);

                //commentTable.Rows.Add(rr);


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getDocumentTable()
        {
            string[,] sumData = new string[4, 100];

            try
            {
                sumData = getDocumentData(companyName);
                // this section is for creatin the worker update buttons


                docNameTextBoxes = new TextBox[documentRowCount];
                docDateTextBoxes = new TextBox[documentRowCount];
                documentDeleteButtons = new Button[documentRowCount];


                for (int i = 0; i < documentRowCount; i++)
                {
                    var a = new TextBox();
                    docNameTextBoxes[i] = a;

                    var b = new TextBox();
                    docDateTextBoxes[i] = b;

                    var c = new Button();
                    documentDeleteButtons[i] = c;
                }

                //fileTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(1000);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Document";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Upload Date";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Delete";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                // Add the header to a new row.
                TableRow headerRow = new TableRow();
                headerRow.Cells.Add(header1);
                headerRow.Cells.Add(header2);
                headerRow.Cells.Add(header3);

                // Add the header row to the table.
                //fileTable.Rows.AddAt(0, headerRow);

                int numrows = documentRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //osha 10
                    docNameTextBoxes[j].Text = sumData[0, j];
                    docNameTextBoxes[j].ID = "1|" + j.ToString() + "|" + sumData[3, j] + "|D";
                    docNameTextBoxes[j].Width = Unit.Pixel(400);
                    a.Controls.Add(docNameTextBoxes[j]);
                    a.Width = Unit.Percentage(50);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    docDateTextBoxes[j].Text = sumData[2, j];
                    docDateTextBoxes[j].ID = "2|" + j.ToString() + "|" + sumData[3, j] + "|D";
                    b.Controls.Add(docDateTextBoxes[j]);
                    b.Width = Unit.Percentage(30);
                    r.Cells.Add(b);

                    TableCell c = new TableCell();
                    documentDeleteButtons[j].Text = "Delete this row";
                    documentDeleteButtons[j].Click += new EventHandler(OnDocumentDeleteButtonClick);
                    documentDeleteButtons[j].ID = "3|" + j.ToString() + "|" + sumData[3, j] + "|D";
                    documentDeleteButtons[j].ToolTip = "Click delete to remove this document";
                    c.Controls.Add(documentDeleteButtons[j]);
                    c.Width = Unit.Percentage(20);
                    r.Cells.Add(c);


                    //   }
                    //fileTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        private int getmySqlID(string tableName, string comp)
        {
            int returnValue = 1;

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            deliveryRowCount = 0;
            try
            {


                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT MAX(mySqlID) FROM wcf_data." + tableName + " WHERE company = '" + comp + "' ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnValue = rdr.IsDBNull(0) ? 1 : rdr.GetInt32(0) + 1;
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

            return returnValue;
        }

        private void OnWorkerUpdateButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

            String insertString = "";
            if (workerTradeNameDropDownList[Convert.ToInt32(idParts[1])].SelectedIndex < 0) { workerTradeNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue = "Ironworker"; }
            String trade = workerTradeNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            String name = HttpUtility.HtmlEncode(workerNameTextBoxes[Convert.ToInt32(idParts[1])].Text);
            String osha = HttpUtility.HtmlEncode(workerOsha10TextBoxes[Convert.ToInt32(idParts[1])].Text);
            String level = workerLevelDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;



            insertString = "UPDATE wcf_data.worker " +
                " SET trade = '" + trade + "', " +
                " name = '" + name + "', " +
                " osha10 = '" + osha + "', " +
                " level = '" + level + "', " +
                " updateDate = '" + dateToday + "' " +

                " WHERE mySqlID = '" + idParts[2] + "'";


            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //  string workerWipSQL = "SELECT DISTINCT trade, name, osha10, level, updateDate, mySqlID FROM wcf_data.worker where company = '" + comp + "';";

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }
                Response.Redirect(Request.RawUrl, false);
            }
        }

        private void OnWorkerDeleteButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);
            String insertString = "";
          //  String updateString = "";

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

         


            insertString = "DELETE FROM wcf_data.worker " +

                " WHERE mySqlID = '" + idParts[2] + "'";
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);
            MySqlDataReader rdr = null;
            

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }

            try
            {
                String nameStr = "";
                String oshaStr = "";
                Boolean hasRows = false;
                conn = new MySqlConnection(cs);
                conn.Open();

                String getUpdateRowSQL = "SELECT DISTINCT name, osha10 FROM wcf_data.worker AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.worker AS b WHERE a.name = b.name AND a.osha10 = b.osha10 ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if(rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        nameStr = rdr.GetString(0);
                        oshaStr = rdr.GetString(1);
                    }

                }

                if (rdr.HasRows == false)
                {
                        hasRows = false;
                        nameStr = "no rows";
                        oshaStr = "00000000";
                }

                rdr.Close();

                if(hasRows == true)
                {
                    insertString = "UPDATE wcf_data.worker " +
               " SET  updateDate = '" + dateToday + "' " +
              " WHERE name = '" + nameStr + "' AND osha10 = '" + oshaStr +  "';";
                    cmd = new MySqlCommand(insertString, conn);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            Response.Redirect(Request.RawUrl, false);
        }

        private void addNewWorkerButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();

            try
            {

             string id = Convert.ToString(getmySqlID("worker", companyName));

             String insertString = "";
            if(workerTradeDD.SelectedIndex < 0) { workerTradeDD.SelectedValue = "Ironworker"; }
            String trade = workerTradeDD.SelectedValue;
            String name = HttpUtility.HtmlEncode(workerNameTB.Text);
            String osha = HttpUtility.HtmlEncode(workerOsha10TB.Text);
            String level = workerLevelDD.SelectedValue;

            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.worker (company, trade, name, osha10, level, updateDate, mySqlID) " +
                "VALUES('" + companyName + "', '" + trade + "', '" + name + "', '" + osha + "', '" + level + "', '" + dateToday + "', '" + id + "')";

                string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            Response.Redirect(Request.RawUrl, false);
           // Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }

        private void OnSectionUpdateButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

            String insertString = "";

            String site = sectionSiteLabel.Text;
            String section = HttpUtility.HtmlEncode(sectionTextBoxes[Convert.ToInt32(idParts[1])].Text);


            insertString = "UPDATE wcf_data.site_sections " +
                " SET company = '" + companyName + "', " +
                " siteName = '" + site + "', " +
                " siteTown = '" + globalTownName + "', " +
                " sectionName = '" + section + "', " +
                " updateDate = '" + dateToday + "' " +
                " WHERE mySqlID = '" + idParts[2] + "'";


            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getSectionTable(site);
          //  Response.Redirect(Request.RawUrl, false);
        }

        private void OnSectionDeleteButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);
            String insertString = "";

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');




            insertString = "DELETE FROM wcf_data.site_sections " +

                " WHERE mySqlID = '" + idParts[2] + "'";
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }

            try
            {
                String siteNameStr = "";
                String sectionNameStr = "";
                Boolean hasRows = false;
                MySqlDataReader rdr = null;
                conn = new MySqlConnection(cs);
                conn.Open();

                String getUpdateRowSQL = "SELECT DISTINCT  siteName, sectionName FROM wcf_data.site_sections AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.site_sections AS b WHERE a.siteName = b.siteName AND a.sectionName = b.sectionName ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if (rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        siteNameStr = rdr.GetString(0);
                        sectionNameStr = rdr.GetString(1);
                    }

                }

                if (rdr.HasRows == false)
                {
                    hasRows = false;
                    siteNameStr = "no rows";
                    sectionNameStr = "00000000";
                }

                rdr.Close();

                if (hasRows == true)
                {
                    insertString = "UPDATE wcf_data.site_sections " +
               " SET  updateDate = '" + dateToday + "' " +
              " WHERE siteName = '" + siteNameStr + "' AND sectionName = '" + sectionNameStr + "';";
                    cmd = new MySqlCommand(insertString, conn);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getSectionTable(globalSiteName);
        }

        protected void addNewSectionButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();

            try
            {

                String site = sectionSiteLabel.Text;
                String section = HttpUtility.HtmlEncode(sectionTB.Text);
                int fwCheckbox = sectionCB.Checked ? 1 : 0;

                string id = Convert.ToString(getmySqlID("site_sections", companyName));

                if (site.Contains("-"))
            {
                String temptown = site;
                String[] tempTownArray = temptown.Split('-');
                site = tempTownArray[0];

            }

            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.site_sections(company, siteName, siteTown, sectionName, fieldWork, updateDate, mySqlID) " +
                "VALUES('" + companyName + "', '" + site + "', '" + globalTownName + "', '" + section + "', '" + fwCheckbox + "', '" + dateToday + "', '" + id + "')";

             string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

             conn = new MySqlConnection(cs);
             conn.Open();

             cmd = new MySqlCommand(insertString, conn);
             cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getSectionTable(globalSiteName);
            getConstructionTable(globalSiteName);
            getTaskTable();
            getDeliveryTable();

        }

        private void OnConstructionUpdateButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

            String insertString = "";

            String site = constructionSiteNameLabels[Convert.ToInt32(idParts[1])].Text;
            String section = constructionSectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue.ToString();
            String construct = HttpUtility.HtmlEncode(constructionDropDown[Convert.ToInt32(idParts[1])].Text);
            String constructHours = HttpUtility.HtmlEncode(constructionHoursTextBoxes[Convert.ToInt32(idParts[1])].Text);


            insertString = "UPDATE wcf_data.construction " +
                " SET company = '" + companyName + "', " +
                " siteName = '" + site + "', " +
                " sectionName = '" + section + "', " +
                " construction = '" + construct + "', " +
                " hours = '" + constructHours + "', " +
                " updateDate = '" + dateToday + "' " +

                " WHERE mySqlID = '" + idParts[2] + "'";


            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getSectionTable(site);
            //  Response.Redirect(Request.RawUrl, false);
        }

        private void OnConstructionDeleteButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);
            String insertString = "";

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');


            insertString = "DELETE FROM wcf_data.construction " +

                " WHERE mySqlID = '" + idParts[2] + "'";
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }

            try
            {
                String sectionNameStr = "";
                String constructionStr = "";
                Boolean hasRows = false;
                MySqlDataReader rdr = null;
                conn = new MySqlConnection(cs);
                conn.Open();

                String getUpdateRowSQL = "SELECT DISTINCT sectionName, construction FROM wcf_data.construction AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.construction AS b WHERE a.sectionName = b.sectionName AND a.construction = b.construction ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if (rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        sectionNameStr = rdr.GetString(0);
                        constructionStr = rdr.GetString(1);
                    }

                }

                if (rdr.HasRows == false)
                {
                    hasRows = false;
                    sectionNameStr = "no rows";
                    constructionStr = "00000000";
                }

                rdr.Close();

                if (hasRows == true)
                {
                    insertString = "UPDATE wcf_data.construction " +
               " SET  updateDate = '" + dateToday + "' " +
              " WHERE sectionName = '" + sectionNameStr + "' AND construction = '" + constructionStr + "';";
                    cmd = new MySqlCommand(insertString, conn);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getSectionTable(globalSiteName);
        }

        protected void addNewConstructionButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
            string id = Convert.ToString(getmySqlID("construction", companyName));

            String site = constructionSiteNameLB.Text;
            String section = HttpUtility.HtmlEncode(constructionSectionDD.SelectedValue.ToString());
            String construct = HttpUtility.HtmlEncode(constructionDD.SelectedValue.ToString());
            String constructHours = HttpUtility.HtmlEncode(constructionHoursTB.Text);


            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.construction (company, siteName, sectionName, construction, hours, fieldWork, updateDate, mySqlID) " +
                "VALUES('" + companyName + "', '" + site + "', '" + section + "', '" + construct + "', '" + constructHours + "', '" + "0" + "', '" + dateToday + "', '" + id + "')";

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            conn = new MySqlConnection(cs);
            conn.Open();

            cmd = new MySqlCommand(insertString, conn);
            cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            //getSectionTable(globalSiteName);
            getConstructionTable(globalSiteName);
            getTaskTable();
            getDeliveryTable();
        }

        private void OnTaskUpdateButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

            String insertString = "";
           // if (taskSectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedIndex < 0) { taskSectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue = "Ironworker"; }
            String section = taskSectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            String constructionType = taskConstructionDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            
            String task = HttpUtility.HtmlEncode(taskDropDown[Convert.ToInt32(idParts[1])].SelectedValue);
            String measure = taskMeasureDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;



            insertString = "UPDATE wcf_data.tasks " +
                " SET sectionName= '" + section + "', " +
                " construction = '" + constructionType + "', " +
                " task = '" + task + "', " +
                " measure = '" + measure + "', " +
                " updateDate = '" + dateToday + "' " +

                " WHERE mySqlID = '" + idParts[2] + "'";


            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getTaskTable();
        }

        private void OnTaskDeleteButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);
            String insertString = "";

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');


            insertString = "DELETE FROM wcf_data.tasks " +

                " WHERE mySqlID = '" + idParts[2] + "'";
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }

            try
            {
                String sectionNameStr = "";
                String constructionStr = "";
                String taskStr = "";
                Boolean hasRows = false;
                MySqlDataReader rdr = null;
                conn = new MySqlConnection(cs);
                conn.Open();

                String getUpdateRowSQL = "SELECT DISTINCT sectionName, construction, task FROM wcf_data.tasks AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.tasks AS b WHERE a.sectionName = b.sectionName AND a.construction = b.construction AND a.task = b.task ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if (rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        sectionNameStr = rdr.GetString(0);
                        constructionStr = rdr.GetString(1);
                        taskStr = rdr.GetString(2);
                    }

                }

                if (rdr.HasRows == false)
                {
                    hasRows = false;
                    sectionNameStr = "no rows";
                    constructionStr = "00000000";
                }

                rdr.Close();

                if (hasRows == true)
                {
                    insertString = "UPDATE wcf_data.tasks " +
               " SET  updateDate = '" + dateToday + "' " +
              " WHERE sectionName = '" + sectionNameStr + "' AND construction = '" + constructionStr + "' AND task = '" + taskStr + "';";
                    cmd = new MySqlCommand(insertString, conn);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
        }

        private void addNewTaskButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
            
                string id = Convert.ToString(getmySqlID("tasks", companyName));

            String section = HttpUtility.HtmlEncode(taskSectionDD.SelectedValue);
            String construct = HttpUtility.HtmlEncode(taskConstructionDD.SelectedValue);

            String task = HttpUtility.HtmlEncode(taskDD.SelectedValue);
            String measure = taskMeasureDD.SelectedValue;

            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.tasks (company, siteName, sectionName, construction, task, measure, fieldWork, updateDate, mySqlID ) " +
                "VALUES('" + companyName + "', '" + globalSiteName + "', '" + section + "', '" + construct + "', '" + task + "', '" + measure + "', '" + "0" + "', '" + dateToday + "', '" + id + "')";

                string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            conn = new MySqlConnection(cs);
             conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }

            getTaskTable();
        }

        private void OnDeliveryUpdateButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

            String insertString = "";
      //      if (deliverySectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedIndex < 0) { deliverySectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue = "Ironworker"; }
            String section = deliverySectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            String construct = deliveryConstructionDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            String delivery = HttpUtility.HtmlEncode(deliveryTextBoxes[Convert.ToInt32(idParts[1])].Text);
            String measure = deliveryMeasureDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;



            insertString = "UPDATE wcf_data.delivery " +
                " SET sectionName = '" + section + "', " +
                " construction = '" + construct + "', " +
                " material = '" + delivery + "', " +
                " measure = '" + measure + "', " +
                " updateDate = '" + dateToday + "' " +

                " WHERE mySqlID = '" + idParts[2] + "'";


            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getDeliveryTable();
        }

        private void OnDeliveryDeleteButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);
            String insertString = "";

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');




            insertString = "DELETE FROM wcf_data.delivery " +

                " WHERE mySqlID = '" + idParts[2] + "'";
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }

            try
            {
                String sectionNameStr = "";
                String constructionStr = "";
                String materialStr = "";
                Boolean hasRows = false;
                MySqlDataReader rdr = null;
                conn = new MySqlConnection(cs);
                conn.Open();

                String getUpdateRowSQL = "SELECT DISTINCT sectionName, construction, material FROM wcf_data.delivery AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.delivery AS b WHERE a.sectionName = b.sectionName AND a.construction = b.construction AND a.material = b.material ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if (rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        sectionNameStr = rdr.GetString(0);
                        constructionStr = rdr.GetString(1);
                        materialStr = rdr.GetString(2);
                    }

                }

                if (rdr.HasRows == false)
                {
                    hasRows = false;
                    sectionNameStr = "no rows";
                    constructionStr = "00000000";
                }

                rdr.Close();

                if (hasRows == true)
                {
                    insertString = "UPDATE wcf_data.delivery " +
               " SET  updateDate = '" + dateToday + "' " +
              " WHERE sectionName = '" + sectionNameStr + "' AND construction = '" + constructionStr + "' AND measure = '" + materialStr + "';";
                    cmd = new MySqlCommand(insertString, conn);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getDeliveryTable();
        }

        private void addNewDeliveryButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {

                String insertString = "";
            if (deliverySectionDD.SelectedIndex < 0) { deliverySectionDD.SelectedValue = "Ironworker"; }
            String section = deliverySectionDD.SelectedValue;
            String construct = deliveryConstructionDD.SelectedValue;
            String delivery = deliveryTB.Text;
            String measure = deliveryMeasureDD.SelectedValue;

                string id = Convert.ToString(getmySqlID("delivery", companyName));

                var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.delivery (company, siteName, sectionName, construction, material, measure, updateDate, mySqlID) " +
                "VALUES('" + companyName + "', '" + globalSiteName + "', '" + section + "', '" + construct  + "', '" + delivery +"', '" + measure + "', '" + dateToday + "', '" + id + "')";

                string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;


                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getDeliveryTable();
        }

        private void OnCommentUpdateButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

            String insertString = "";
            if (commentTradeNameDropDownList[Convert.ToInt32(idParts[1])].SelectedIndex < 0) { commentTradeNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue = "Ironworker"; }
            String trade = commentTradeNameDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            String commentType = commentTypeDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            String comment = commentTextBoxes[Convert.ToInt32(idParts[1])].Text;



            insertString = "UPDATE wcf_data.comment " +
                " SET trade = '" + trade + "', " +
                " commentType = '" + commentType + "', " +
                " comment = '" + comment + "', " +
                " updateDate = '" + dateToday + "' " +

                " WHERE mySqlID = '" + idParts[2] + "'";


            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
        }

        private void OnCommentDeleteButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);
            String insertString = "";

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');




            insertString = "DELETE FROM wcf_data.comment " +

                " WHERE mySqlID = '" + idParts[2] + "'";
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }

            try
            {
                String commentTypeStr = "";
                String commentStr = "";
                Boolean hasRows = false;
                MySqlDataReader rdr = null;
                conn = new MySqlConnection(cs);
                conn.Open();

                String getUpdateRowSQL = "SELECT DISTINCT commentType, comment FROM wcf_data.comment AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.comment AS b WHERE a.commentType = b.commentType AND a.comment = b.comment ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if (rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        commentTypeStr = rdr.GetString(0);
                        commentStr = rdr.GetString(1);
                    }

                }

                if (rdr.HasRows == false)
                {
                    hasRows = false;
                    commentTypeStr = "no rows";
                    commentStr = "00000000";
                }

                rdr.Close();

                if (hasRows == true)
                {
                    insertString = "UPDATE wcf_data.comment " +
               " SET  updateDate = '" + dateToday + "' " +
              " WHERE commentType = '" + commentTypeStr + "' AND comment = '" + commentStr + "';";
                    cmd = new MySqlCommand(insertString, conn);
                    cmd.ExecuteNonQuery();
                }


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
        }

        private void addNewCommentButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                
            String insertString = "";
            if (commentTradeDD.SelectedIndex < 0) { commentTradeDD.SelectedValue = "Ironworker"; }
            String trade = commentTradeDD.SelectedValue;
            String type = taskConstructionDD.SelectedValue;
            String comment = commentTB.Text;

            var whichButton = (Button)sender;

                string id = Convert.ToString(getmySqlID("comment", companyName));

                insertString = "INSERT INTO wcf_data.comment (company, trade, commentType, comment, updateDate, mySqlID ) " +
                "VALUES('" + companyName + "', '" + trade + "', '" + type + "', '" +  comment + "', '" + dateToday + "', '" + id + "')";

                string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }

        }

        protected void OnRowSiteAddButtonClick(object sender, EventArgs e)
        {

            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";
            String siteName = siteNameTB.Text;
            String siteTown = siteTownTB.Text;

            DateTime startDate;
            DateTime endDate;
            String startDateStr = "";
            String endDateStr = ""; ;

            try
            {
                startDate = Convert.ToDateTime(siteStartDateCell.Text);
                endDate = Convert.ToDateTime(siteEndDateCell.Text);

                startDateStr = startDate.ToString("yyyy-MM-dd");
                endDateStr = endDate.ToString("yyyy-MM-dd");

            }
            catch(Exception ex)
            {

            }

            // var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.sites (company, siteName, siteTown, startDate, endDate, updateDate ) " +
                "VALUES('" + companyName + "', '" + siteName + "', '" + siteTown + "', '" + startDateStr + "', '" + endDateStr + "', '" + dateToday + "')";

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //  string workerWipSQL = "SELECT DISTINCT trade, name, osha10, level, updateDate, mySqlID FROM wcf_data.worker where company = '" + comp + "';";

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }

            loadSiteDropDown();
        }

        protected void OnRowSiteUpdateButtonClick(object sender, EventArgs e)
        {
            int x = 0;
        }

        protected void updateSiteButtonClick(object sender, EventArgs e)
        {
            //   String noSiteError = "No site was choosen to update";
            //   ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + noSiteError + "');", true);

            // ClientScript.RegisterStartupScript(this.GetType(), "RefreshOpener", "f2();", true); expects a javascript function named f2();

            String temptown = ChooseSiteDropDownList.SelectedItem.Text.ToString();

            globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();

          //  String[] tempTownArray = temptown.Split('-');

         //   globalSiteName = tempTownArray[0];
           // globalTownName = tempTownArray[1];

            String[] siteData = new String[6];

            siteData = getOneSiteData(companyName, globalSiteName);

            DateTime startDate = Convert.ToDateTime(siteData[2]);
            DateTime endDate = Convert.ToDateTime(siteData[3]);

            siteNameTB.Text = siteData[0];
            siteTownTB.Text = siteData[1];
            siteStartDateCell.Text = startDate.Date.ToString("MM/dd/yyyy");
            siteEndDateCell.Text = endDate.Date.ToString("MM/dd/yyyy");

            calStart.SelectedDate = startDate.Date;
            calEnd.SelectedDate = endDate.Date;


            siteButton.Visible = false;
            siteButtonUpdate.Visible = true;
            Table1.Visible = true;

            calStart.Visible = true;
            calEnd.Visible = true;

            if(siteData[4] == "False") { CheckBox1.Checked = false; }
            if (siteData[4] == "True") { CheckBox1.Checked = true; }

            //  UpdatePanel6.Update();

            // below this is to load the sections
            //need a function call to do this
            getSectionTable(globalSiteName);
            getConstructionTable(globalSiteName);
            getTaskTable();
            getDeliveryTable();
            /*

            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";
            if (commentTradeDD.SelectedIndex < 0) { commentTradeDD.SelectedValue = "Ironworker"; }
            String trade = commentTradeDD.SelectedValue;
            String type = taskConstructionDD.SelectedValue;
            String comment = commentTB.Text;



            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.comment (company, trade, commentType, comment, updateDate ) " +
                "VALUES('" + companyName + "', '" + trade + "', '" + type + "', '" + comment + "', '" + dateToday + "')";

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                //  string workerWipSQL = "SELECT DISTINCT trade, name, osha10, level, updateDate, mySqlID FROM wcf_data.worker where company = '" + comp + "';";

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
*/
        }

        protected void addNewSiteButtonClick(object sender, EventArgs e)
        {

            
            try
            {
                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);
                Table1.Style.Equals(tableStyle);
               // Table1.Width = Unit.Pixel(550);

                siteNameCell.Width = Unit.Percentage(20);
                siteTownCell.Width = Unit.Percentage(20);
                siteStartDateCell.Width = Unit.Percentage(20);
                siteEndDateCell.Width = Unit.Percentage(20);
                siteButtonCell.Width = Unit.Percentage(20);

                siteButton.Text = "Add Site";
                Table1.Visible = true;

                calStart.Visible = true;
                calEnd.Visible = true;

                /*
                                siteNameTextBoxes = new TextBox[1];
                                siteTownTextBoxes = new TextBox[1];
                                siteStartDateTextBoxes = new TextBox[1];
                                siteEndDateTextBoxes = new TextBox[1];

                                siteAddButtons = new Button[1];

                                siteStartDates = new Calendar[1];
                                siteEndDates = new Calendar[1];


                                //     var b = new TextBox();
                                siteNameTextBoxes[0] = new TextBox();
                                siteTownTextBoxes[0] = new TextBox();
                                siteStartDateTextBoxes[0] = new TextBox();
                                siteEndDateTextBoxes[0] = new TextBox();
                                */
                //     Button rowSiteUpdateButton = new Button();
                //    Button rowsiteDeleteButton = new Button();

                //      var c = new TextBox();
                //      workerOsha10TextBoxes[i] = c;

                //       var d = new DropDownList();
                //      workerLevelDropDownList[i] = d;

                //        var ee = new Button();
                //        workerUpdateButtons[i] = ee;

                //     var f = new Button();
                //     siteAddButtons[0] = new Button();
                /*
                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Name";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Town";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                //  header.VerticalAlign = VerticalAlign.Middle;

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Start Date";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header4 = new TableHeaderCell();
                header4.Text = "End Date";
                header4.Font.Bold = true;
                header4.BackColor = Color.LightGray;
                header4.HorizontalAlign = HorizontalAlign.Center;

                TableHeaderCell header5 = new TableHeaderCell();
                header5.Text = "Add";
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
                siteTable.Rows.AddAt(0, headerRow);


                TableRow r = new TableRow();

                TableCell a = new TableCell(); //trade type
                siteNameTextBox.Text = "";
                siteNameTextBox.TextChanged += new EventHandler(siteNameTextBox_TextChanged);
                a.Controls.Add(siteNameTextBox);
                a.Width = Unit.Percentage(20);
                r.Cells.Add(a);

                TableCell b = new TableCell(); //worker name
                siteTownTextBox.Text = "";
                siteTownTextBox.TextChanged += new EventHandler(siteTownTextBox_TextChanged);
                b.Controls.Add(siteTownTextBox);
                b.Width = Unit.Percentage(20);
                r.Cells.Add(b);

                TableCell c = new TableCell(); //osha 10
                siteStartDateTextBox.Text = "Choose in calendar";
                c.Controls.Add(siteStartDateTextBox);
                c.Width = Unit.Percentage(20);
                r.Cells.Add(c);

                TableCell d = new TableCell();
                siteEndDateTextBox.Text = "Choose in calendar";
                d.Controls.Add(siteEndDateTextBox);
                d.Width = Unit.Percentage(20);
                r.Cells.Add(d);

                TableCell ee = new TableCell();
                rowSiteUpdateButton.Text = "Update this row";
                rowSiteUpdateButton.Click += new EventHandler(OnRowSiteUpdateButtonClick);
                rowSiteUpdateButton.ToolTip = "Make any needed changes in the row and then click update";
                ee.Controls.Add(rowSiteUpdateButton);
                ee.Width = Unit.Percentage(20);
                r.Cells.Add(ee);
                siteTable.Rows.Add(r);
                calStart.Visible = true;
                calEnd.Visible = true;


                //   }
                siteTable.Rows.Add(r);
                */
                getSectionTable(globalSiteName);
                getConstructionTable(globalSiteName);
                getTaskTable();
                getDeliveryTable();

            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        protected void taskSectionMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

            string xxx = taskSectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedItem.Text;


            returnData = getConstructionDataBySection(companyName, globalSiteName, taskSectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedItem.Text);

            tempConstrutListItems.Clear();

            int tempRow = Convert.ToInt32(idParts[1]);
            taskConstructionDropDownList[tempRow].Items.Clear();

                taskFWLabel[tempRow].Text = getSectionsFWstatus(companyName, xxx) == "True" ? "True" : "False";

                for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
              //  tempConstrutListItems.Add( new ListItem(returnData[2, z]));
                taskConstructionDropDownList[tempRow].Items.Add(new ListItem(returnData[2, z]));
               // taskConstructionDropDownList[tempRow].Items.Add
            }

            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + temp + "');", true);
                //  this Page page;
                //MessageBox.Show(this, temp);
            }
            //  taskConstructionDropDownList[tempRow].Items.Add()   //DataSource = tempConstrutListItems;
            //  taskConstructionDropDownList[tempRow].DataBind();
        }

        protected void taskSectionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

            returnData = getConstructionDataBySection(companyName, globalSiteName, taskSectionDD.SelectedItem.Text);

            // sectionSelectedConstructionRowCount

            tempConstrutListItems.Clear();

            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
                tempConstrutListItems.Add( new ListItem(returnData[2, z]));
            }

            if(tempConstrutListItems.Count == 0)
             {
                    tempConstrutListItems.Add(new ListItem("Add Construction Above"));
             }

            taskConstructionDD.Items.Clear();
            taskConstructionDD.DataSource = tempConstrutListItems;
            taskConstructionDD.DataBind();
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
                // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + temp + "');", true);
                //  this Page page;
                //MessageBox.Show(this, temp);
            }

        }

        protected void deliverySectionMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { 
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

            returnData = getConstructionDataBySection(companyName, globalSiteName, deliverySectionNameDropDownList[Convert.ToInt32(idParts[1])].SelectedItem.Text);

           // tempDeliveryConstrutListItems.Clear();
            deliveryConstructionDropDownList[Convert.ToInt32(idParts[1])].Items.Clear();

            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
             //   tempDeliveryConstrutListItems.Add( new ListItem(returnData[2, z]));
                deliveryConstructionDropDownList[Convert.ToInt32(idParts[1])].Items.Add(returnData[2, z]);
            }

            // deliveryConstructionDropDownList[Convert.ToInt32(idParts[1])].Items.Clear();
            // deliveryConstructionDropDownList[Convert.ToInt32(idParts[1])].DataSource = tempDeliveryConstrutListItems;
            //  deliveryConstructionDropDownList[Convert.ToInt32(idParts[1])].DataBind();

        }
            catch(Exception ex)
            {
                string temp = ex.Message.ToString();
        // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + temp + "');", true);
        //  this Page page;
        //MessageBox.Show(this, temp);
        }
    }

        protected void constructionSectionMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            string temp = "";
            int tempInt = 0;
            try
            {
                var whichDD = (DropDownList)sender;
                String buttonID = whichDD.ID;
                String[] idParts;
                idParts = buttonID.Split('|'); //part two has the item number


                if (idParts.Length >= 4)
                {
                  //  temp = idParts[4];
                    tempInt = Convert.ToInt32(idParts[1]);
                   temp = constructionSectionNameDropDownList[tempInt].SelectedValue;
                }
            }
            catch(Exception ex)
            {
                string tempEX = ex.Message.ToString();
            }
          //  String[,] returnData = new String[6, 500];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT fieldWork FROM wcf_data.site_sections where company = '" + companyName + "' AND siteName = '" + globalSiteName + "' AND sectionName = '" + temp + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

                if (!rdr.HasRows) { constructionLabelFW[tempInt].Text = "false"; }


                while (rdr.Read())
                {
                    constructionLabelFW[tempInt].Text = rdr.GetInt32(0) == 1 ? "true" : "false";
                }


            }
            catch (MySqlException ex)
            {
                string tempxx = ex.ToString();

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
        }

        protected void constructionSectionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            string temp = "";
            int tempInt = 0;

            temp = constructionSectionDD.SelectedValue;

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT fieldWork FROM wcf_data.site_sections where company = '" + companyName + "' AND siteName = '" + globalSiteName + "' AND sectionName = '" + temp + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

                if (!rdr.HasRows) { constructionLabelFW[tempInt].Text = "false"; }


                while (rdr.Read())
                {
                  //  constructionLabelFW[tempInt].Text = rdr.GetInt32(0) == 1 ? "true" : "false";
                    constructionFWLB.Text = rdr.GetInt32(0) == 1 ? "true" : "false";
                }


            }
            catch (MySqlException ex)
            {
                string tempxx = ex.ToString();

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

        }

        protected void deliverySectionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                String[,] returnData = new String[5, 500];

                var whichDD = (DropDownList)sender;
                String buttonID = whichDD.ID;
                String[] idParts;
                idParts = buttonID.Split('|'); //part two has the item number

                returnData = getConstructionDataBySection(companyName, globalSiteName, deliverySectionDD.SelectedItem.Text);

                tempConstrutListItems.Clear();


                for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
                {
                    tempConstrutListItems.Add(new ListItem(returnData[2, z]));
                }
                if (tempConstrutListItems.Count == 0)
                {
                    tempConstrutListItems.Add(new ListItem("Add Construction Above"));
                }
                deliveryConstructionDD.Items.Clear();
                deliveryConstructionDD.DataSource = tempConstrutListItems;
                deliveryConstructionDD.DataBind();
            }
            catch(Exception ex)
            {
                string temp = ex.Message.ToString();
               // ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + temp + "');", true);
                //  this Page page;
                //MessageBox.Show(this, temp);
            }
        }

        protected void calStart_SelectionChanged(object sender, EventArgs e)
        {

            siteStartDateCell.Text = calStart.SelectedDate.Date.ToString("MM/dd/yyyy");
        }

        protected void calEnd_SelectionChanged(object sender, EventArgs e)
        {
            siteEndDateCell.Text = calEnd.SelectedDate.Date.ToString("MM/dd/yyyy");
        }

        protected void siteNameTextBox_TextChanged(object sender, EventArgs e)
        {
            siteNameStr = siteTownTextBox.Text;
        }

        protected void siteTownTextBox_TextChanged(object sender, EventArgs e)
        {
            siteTownStr = siteTownTextBox.Text;
        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {/*
            // Save the uploaded file to an "Uploads" directory
            // that already exists in the file system of the 
            // currently executing ASP.NET application.  
            // Creating an "Uploads" directory isolates uploaded 
            // files in a separate directory. This helps prevent
            // users from overwriting existing application files by
            // uploading files with names like "Web.config".
            string saveDir = @"\Uploads\";

          //  string extension = Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

            // Get the physical file system path for the currently
            // executing application.
            string appPath = Request.PhysicalApplicationPath;

            var temp = FileUpload1.HasFile;


            // Before attempting to save the file, verify
            // that the FileUpload control contains a file.
            if (FileUpload1.HasFile && extension == "pdf")
            {
                string savePath = appPath + saveDir + Server.HtmlEncode(FileUpload1.FileName);

                // Call the SaveAs method to save the 
                // uploaded file to the specified path.
                // This example does not perform all
                // the necessary error checking.               
                // If a file with the same name
                // already exists in the specified path,  
                // the uploaded file overwrites it.
                FileUpload1.SaveAs(savePath);

                savePath = savePath.Replace("\\", "\\\\");

                FileUpload1.Dispose();

                DateTime now = DateTime.Now;
                String todayFormat = "yyyy-MM-dd HH:mm:ss";
                String dateToday = now.ToString(todayFormat);

                String insertString = "";

                insertString = "INSERT INTO wcf_data.documents (company, docName, docPath, uploadDate ) " +
                    "VALUES('" + companyName + "', '" + FileUpload1.FileName + "', '" + savePath + "', '" + dateToday + "')";

                string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

                MySqlConnection conn = null;
                MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

                try
                {
                    conn = new MySqlConnection(cs);
                    conn.Open();

                    //  string workerWipSQL = "SELECT DISTINCT trade, name, osha10, level, updateDate, mySqlID FROM wcf_data.worker where company = '" + comp + "';";

                    cmd = new MySqlCommand(insertString, conn);
                    cmd.ExecuteNonQuery();


                }
                catch (MySqlException ex)
                {
                    string tempStr = ex.ToString();

                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Dispose();
                    }
                    if (conn != null)
                    {
                        conn.Close();
                    }

                }

                // Notify the user that the file was uploaded successfully.
                //  UploadStatusLabel.Text = "Your file was uploaded successfully.";

            }
            else
            {
                MessageBox.Show(this, "File did not upload");
                // Notify the user that a file was not uploaded.
                //  UploadStatusLabel.Text = "You did not specify a file to upload.";
            }
            getDocumentTable();
            */
        }

        private void OnDocumentDeleteButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
         //   String todayFormat = "yyyy-MM-dd HH:mm:ss";
         //   String dateToday = now.ToString(todayFormat);
            String insertString = "";

            var whichButton = (Button)sender;
            String buttonID = whichButton.ID;
            String[] idParts;
            idParts = buttonID.Split('|');

            String docName = docNameTextBoxes[Convert.ToInt32(idParts[1])].Text;

            string saveDir = @"\Uploads\";


            string appPath = Request.PhysicalApplicationPath;

            string savePath = appPath + saveDir + Server.HtmlEncode(docName);

            


            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }


            insertString = "DELETE FROM wcf_data.documents " +

                " WHERE mySqlID = '" + idParts[2] + "'";
            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();
                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }

   
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }
            getDocumentTable();
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            string intBool = "";

            String insertString = "";

            if (CheckBox1.Checked == true) { intBool = "1"; }
            if (CheckBox1.Checked == false) { intBool = "0"; }

            insertString = "UPDATE wcf_data.sites " +
                " SET isActive = '" + intBool + "' AND " +
                 " updateDate = '" + dateToday + "' " +

                " WHERE siteName = '" + globalSiteName + "'";


            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                cmd = new MySqlCommand(insertString, conn);
                cmd.ExecuteNonQuery();


            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }
                if (conn != null)
                {
                    conn.Close();
                }

            }



        }
    }

    public static class MessageBox
    {
        public static void Show(this Page Page, String Message)
        {
            Page.ClientScript.RegisterStartupScript(
               Page.GetType(),
               "MessageBox",
               "<script language='javascript'>alert('" + Message + "');</script>"
            );
        }
    }

}