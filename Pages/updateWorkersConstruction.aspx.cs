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
    public partial class siteUpdateWorkersConstruction : System.Web.UI.Page
    {
        String companyName = "Action";

        String globalSiteName = "Choose a site";
     //   String globalTownName = "";

        int measureRowCount = 0;
        int sectionRowCount = 0;
        int constructionRowCount = 0;
       // int distinctConstructionRowCount = 0;
        int WorkerRowCount = 0;
        int taskRowCount = 0;
        int deliveryRowCount = 0;
        int commentRowCount = 0;
        int documentRowCount = 0;

        int sectionSelectedConstructionRowCount = 0;

        // section area

     //   Button[] sectionUpdateButtons;
     //   Button[] sectionDeleteButtons;

     //   Label[] sectionSiteLabels;
     //   TextBox[] sectionTextBoxes;
      //  CheckBox[] sectionCheckFW;

     //   Label sectionSiteLabel;
     //   TextBox sectionTB;
     //   CheckBox sectionCB;


       // Button addNewSectionButton = new Button();

        //*******************************************
        //construction type

        Button[] constructionUpdateButtons;
        Button[] constructionDeleteButtons;

        TextBox[] constructionTextBoxes;

        Label[] constructionLabelFW;


        TextBox constructionTB;
 
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

        TextBox[] taskTextBoxes;
        
      //  DropDownList[] taskSectionNameDropDownList;
        DropDownList[] taskConstructionDropDownList;
        DropDownList[] taskMeasureDropDownList;
  

        DropDownList taskSectionDD;
        DropDownList taskConstructionDD;
        DropDownList taskMeasureDD;


        TextBox taskTB;

        ListItemCollection taskSectionItems = new ListItemCollection();

        ListItemCollection tempConstrutListItems = new ListItemCollection();

        // the array of buttons etc for the delivery section
        private Button[] deliveryUpdateButtons;
        private Button[] deliveryDeleteButtons;

        private TextBox[] deliveryTextBoxes;

        private DropDownList[] deliveryDropDownList;
        private DropDownList[] deliveryMeasureDropDownList;

     //   DropDownList deliverySectionDD;
        DropDownList deliveryDD;
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

     //   DateTime siteStartDate = new DateTime();
      //  DateTime siteEndDate = new DateTime();

        String siteNameStr = "";
        String siteTownStr = "";
     //   String siteStartDateStr = "";
     //   String siteEndDateStr = "";



        //site info creation
      //  private Button[] siteUpdateButtons;
       // private Button[] siteDeleteButtons;
     //   private Button[] siteAddButtons;

     //   private TextBox[] siteNameTextBoxes;
     //   private TextBox[] siteTownTextBoxes;
     //   private TextBox[] siteStartDateTextBoxes;
     //   private TextBox[] siteEndDateTextBoxes;

     //   private Calendar[] siteStartDates;
      //  private Calendar[] siteEndDates;


        protected void Page_Load(object sender, EventArgs e)
        {
                getConstructionTable(globalSiteName);
                getWorkerTable();
                getTaskTable();
                getDeliveryTable();
                getCommentTable();
                getDocumentTable();
        }

        public List<string> getDistinctConstructionData(String comp)
        {
            List<string> constructionDistinctList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

           // distinctConstructionRowCount = 0;

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

        public String[,] getConstructionData(String comp)
        {
            String[,] returnData = new String[4, 500];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            constructionRowCount = 0;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT company, constructionType, updateDate, mySqlID FROM wcf_data.constructiontypes where company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();

                if (rdr.FieldCount == 0) { returnData[0, 0] = "-1"; constructionRowCount = 0; }


                while (rdr.Read())
                {
                    returnData[0, constructionRowCount] = rdr.GetString(0);
                    returnData[1, constructionRowCount] = rdr.GetString(1);
                    returnData[2, constructionRowCount] = rdr.GetString(2);
                    returnData[3, constructionRowCount] = rdr.GetString(3);
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

        public String[,] getWorkerData(String comp)
        {
            WorkerRowCount = 0;
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

        public String[,] getTaskData(String comp)
        {
            String[,] returnData = new String[6, 100];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            taskRowCount = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT company, construction, task, measure, updateDate, mySqlID FROM wcf_data.tasktypes where company = '" + comp + "' ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, taskRowCount] = rdr.GetString(0);
                    returnData[1, taskRowCount] = rdr.GetString(1);
                    returnData[2, taskRowCount] = rdr.GetString(2);
                    returnData[3, taskRowCount] = rdr.GetString(3);
                    returnData[4, taskRowCount] = rdr.GetString(4);
                    returnData[5, taskRowCount] = rdr.GetString(5);

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

        public String[,] getMeasureData(String comp)
        {
            String[,] returnData = new String[4,100];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            measureRowCount = 0;
            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string tasksSQL = "SELECT DISTINCT company, measure, updateDate, mySqlID FROM wcf_data.measuretypes where company = '" + comp + "' ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, measureRowCount] = rdr.GetString(0);
                    returnData[1, measureRowCount] = rdr.GetString(1);
                    returnData[2, measureRowCount] = rdr.GetString(2);
                    returnData[3, measureRowCount] = rdr.GetString(3);

                    measureRowCount++;
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

        public String[,] getDeliveryData(String comp)
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

                string tasksSQL = "SELECT DISTINCT company, material, measure, mySqlID FROM wcf_data.deliverytypes where company = '" + comp + "' ;";

                MySqlCommand cmd = new MySqlCommand(tasksSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    returnData[0, deliveryRowCount] = rdr.GetString(0);
                    returnData[1, deliveryRowCount] = rdr.GetString(1);
                    returnData[2, deliveryRowCount] = rdr.GetString(2);
                    returnData[3, deliveryRowCount] = rdr.GetString(3);
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

                 //   deliveryRowCount++;
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

        public String[,] getCommentData(String comp)
        {
            String[,] returnData = new String[5, 100];

            string cs = ConfigurationManager.ConnectionStrings["WCFConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;
            commentRowCount = 0;
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
                    returnData[0, documentRowCount] = rdr.IsDBNull(0) == true ? "NA" : rdr.GetString(0);
                    returnData[1, documentRowCount] = rdr.IsDBNull(1) == true ? "NA" : rdr.GetString(1);
                    returnData[2, documentRowCount] = rdr.IsDBNull(2) == true ? "NA" : Convert.ToString(rdr.GetDateTime(2));
                    returnData[3, documentRowCount] = rdr.IsDBNull(3) == true ? "NA" : rdr.GetString(3);

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

                workerTable.Rows.Clear();

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
                workerTable.Rows.AddAt(0, headerRow);

                int numrows = WorkerRowCount;
                for (int j = 0; j < numrows; j++)
                {

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
                    workerTable.Rows.Add(r);
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

                workerTable.Rows.Add(rr);


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

            try
            {
                sumData = getConstructionData(companyName);
                constructionTextBoxes = new TextBox[constructionRowCount];
                constructionLabelFW = new Label[constructionRowCount];

                constructionUpdateButtons = new Button[constructionRowCount];
                constructionDeleteButtons = new Button[constructionRowCount];


                for (int i = 0; i < constructionRowCount; i++)
                {
                    var c = new TextBox();
                    constructionTextBoxes[i] = c;

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

                TableHeaderCell header3 = new TableHeaderCell();
                header3.Text = "Construction";
                header3.Font.Bold = true;
                header3.BackColor = Color.LightGray;
                header3.HorizontalAlign = HorizontalAlign.Center;

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
                headerRow.Cells.Add(header3);
                headerRow.Cells.Add(header6);
                headerRow.Cells.Add(header7);

                // Add the header row to the table.
                constructionTable.Rows.AddAt(0, headerRow);

                int secRows = sectionRowCount;

                //ConstructionSectionItems.Clear();

                for (int z = 0; z < sectionRowCount; z++)
                {
                    string temp = sectionData[2, z];
                  //  ConstructionSectionItems.Add( new ListItem(sectionData[2, z]));
                }
               // ListItem[] tradeItems = { new ListItem("Ironworker"), new ListItem("Laborer"), new ListItem("Other") };
                int numrows = constructionRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell c = new TableCell(); //worker name
                    constructionTextBoxes[j].Text = sumData[1, j];
                    constructionTextBoxes[j].ID = "1|" + j.ToString() + "|" + sumData[3, j] + "|C";
                    c.Controls.Add(constructionTextBoxes[j]);
                    c.Width = Unit.Percentage(33.3);
                    r.Cells.Add(c);

                    TableCell f = new TableCell();
                    constructionUpdateButtons[j].Text = "Update this row";
                    constructionUpdateButtons[j].Click += new EventHandler(OnConstructionUpdateButtonClick);
                    constructionUpdateButtons[j].ID = "2|" + j.ToString() + "|" + sumData[3, j] + "|C";
                    constructionUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    f.Controls.Add(constructionUpdateButtons[j]);
                    f.Width = Unit.Percentage(33.3);
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    constructionDeleteButtons[j].Text = "Delete this row";
                    constructionDeleteButtons[j].Click += new EventHandler(OnConstructionDeleteButtonClick);
                    constructionDeleteButtons[j].ID = "3|" + j.ToString() + "|" + sumData[3, j] + "|C";
                    constructionDeleteButtons[j].ToolTip = "Click delete to remove this section";
                    g.Controls.Add(constructionDeleteButtons[j]);
                    g.Width = Unit.Percentage(33.3);
                    r.Cells.Add(g);


                    //   }
                    constructionTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell cc = new TableCell(); //osha 10
                constructionTB = new TextBox();
                constructionTB.Text = "";
                constructionTB.ID = "constructionTB";
                cc.Controls.Add(constructionTB);
                cc.Width = Unit.Percentage(33.3);
                rr.Cells.Add(cc);


                TableCell ff = new TableCell();
                Button addNewConstructionButton = new Button();
                addNewConstructionButton.Text = "Add this row";
                addNewConstructionButton.Click += new EventHandler(addNewConstructionButtonClick);
                addNewConstructionButton.ID = "66";
                addNewConstructionButton.ToolTip = "Add the needed data and then click add this row";
                ff.Controls.Add(addNewConstructionButton);
                ff.Width = Unit.Percentage(33.3);
                rr.Cells.Add(ff);

                constructionTable.Rows.Add(rr);

            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

        }

        protected void getTaskTable()
        {
            String[,] sumData = new String[7, 100];
            String[,] measureData = new String[5, 100];
            String[,] constructData = new String[6, 500];
            List<string> constructionDistinctList = new List<string>();
            List<string> measureDistinctList = new List<string>();

            try
            {
                sumData = getTaskData(companyName);
                measureData = getMeasureData(companyName);
                constructData = getConstructionData(companyName);
                constructionDistinctList = getDistinctConstructionData(companyName);
                measureDistinctList = getDistinctMeasureData(companyName);

                taskUpdateButtons = new Button[taskRowCount];
                taskDeleteButtons = new Button[taskRowCount];

                taskTextBoxes = new TextBox[taskRowCount];
                taskMeasureDropDownList = new DropDownList[taskRowCount];

                taskConstructionDropDownList = new DropDownList[taskRowCount];

                //  taskFWLabel = new Label[taskRowCount];

                ListItem[] measureTItems = new ListItem[measureRowCount];

                for(int z = 0; z < measureRowCount; z++)
                {
                    measureTItems[z] = new ListItem(measureData[1, z]);
                }


                for (int i = 0; i < taskRowCount; i++)
                {
                    var b = new DropDownList();
                    taskConstructionDropDownList[i] = b;

                    var c = new TextBox();
                    taskTextBoxes[i] = c;

                    var d = new DropDownList();
                    taskMeasureDropDownList[i] = d;

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
                headerRow.Cells.Add(header2);
                headerRow.Cells.Add(header3);
                headerRow.Cells.Add(header4);
                headerRow.Cells.Add(header6);
                headerRow.Cells.Add(header7);

                // Add the header row to the table.
                taskTable.Rows.AddAt(0, headerRow);



                tempConstrutListItems.Clear();


                int numrows = taskRowCount;

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell b = new TableCell();
                    taskConstructionDropDownList[j].DataSource = constructionDistinctList;
                    taskConstructionDropDownList[j].DataBind();
                    taskConstructionDropDownList[j].ID = "1|" + j.ToString() + "|" + sumData[5, j] + "|Tt";                                  
                    taskConstructionDropDownList[j].SelectedValue = sumData[1, j];
                    b.Controls.Add(taskConstructionDropDownList[j]);
                    b.Width = Unit.Percentage(20);
                    r.Cells.Add(b);
      
                    TableCell c = new TableCell(); //worker name
                    taskTextBoxes[j].Text = sumData[2, j];
                    taskTextBoxes[j].ID = "2|" + j.ToString() + "|" + sumData[5, j] + "|T";
                    c.Controls.Add(taskTextBoxes[j]);
                    c.Width = Unit.Percentage(20);
                    r.Cells.Add(c);

                    TableCell d = new TableCell(); //worker name
                    taskMeasureDropDownList[j].DataSource = measureDistinctList;
                    taskMeasureDropDownList[j].DataBind();
                    taskMeasureDropDownList[j].ID = "3|" + j.ToString() + "|" + sumData[5, j] + "|T";
                    taskMeasureDropDownList[j].SelectedValue = sumData[3, j];
                    d.Controls.Add(taskMeasureDropDownList[j]);
                    d.Width = Unit.Percentage(20);
                    r.Cells.Add(d);

                    TableCell f = new TableCell();
                    taskUpdateButtons[j].Text = "Update this row";
                    taskUpdateButtons[j].Click += new EventHandler(OnTaskUpdateButtonClick);
                    taskUpdateButtons[j].ID = "4|" + j.ToString() + "|" + sumData[5, j] + "|T";
                    taskUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    f.Controls.Add(taskUpdateButtons[j]);
                    f.Width = Unit.Percentage(20);
                    r.Cells.Add(f);

                    TableCell g = new TableCell();
                    taskDeleteButtons[j].Text = "Delete this row";
                    taskDeleteButtons[j].Click += new EventHandler(OnTaskDeleteButtonClick);
                    taskDeleteButtons[j].ID = "5|" + j.ToString() + "|" + sumData[5, j] + "|T";
                    taskDeleteButtons[j].ToolTip = "Click delete to remove this task";
                    g.Controls.Add(taskDeleteButtons[j]);
                    g.Width = Unit.Percentage(20);
                    r.Cells.Add(g);


 
                    taskTable.Rows.Add(r);

                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //trade type
                taskConstructionDD = new DropDownList();
                taskConstructionDD.Items.Clear();
                if (constructionDistinctList.Count > 0) { taskConstructionDD.DataSource = constructionDistinctList; taskConstructionDD.DataBind();  }
                if (constructionDistinctList.Count == 0) { taskConstructionDD.Items.Add(new ListItem("Add construction above")); }
                //  taskSectionDD.Items.AddRange(taskSectionItems);
                taskConstructionDD.ID = "taskConstructionDD";
                // taskSectionDD.SelectedIndexChanged += new EventHandler(taskSectionDD_SelectedIndexChanged);
                taskConstructionDD.SelectedIndex = 0;
                String tempSection = taskConstructionDD.SelectedValue;
                aa.Controls.Add(taskConstructionDD);
                aa.Width = Unit.Percentage(16.6);
                rr.Cells.Add(aa);


                TableCell bb = new TableCell(); //osha 10
                taskTB = new TextBox();
                taskTB.Text = "";
                taskTB.ID = "taskTB";
                bb.Controls.Add(taskTB);
                bb.Width = Unit.Percentage(16.6);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell(); //osha 10
                taskMeasureDD = new DropDownList();
                taskMeasureDD.Items.AddRange(measureTItems);
                taskMeasureDD.ID = "taskMeasureCC";
                cc.Controls.Add(taskMeasureDD);
                cc.Width = Unit.Percentage(16.6);
                rr.Cells.Add(cc);

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
            string[,] sumData = new string[4, 100];
            string[,] measureData = new String[5, 100];
            List<string> deliveryDistinctList = new List<string>();
            List<string> measureDistinctList = new List<string>();

            try
            {
                sumData = getDeliveryData(companyName);
                measureData = getMeasureData(companyName);
                deliveryDistinctList = getDistinctDeliveryData(companyName);
                measureDistinctList = getDistinctMeasureData(companyName);

                deliveryTextBoxes = new TextBox[deliveryRowCount];

              //  deliverySectionNameDropDownList = new DropDownList[deliveryRowCount];
                deliveryDropDownList = new DropDownList[deliveryRowCount];
                deliveryMeasureDropDownList = new DropDownList[deliveryRowCount];

                deliveryUpdateButtons = new Button[deliveryRowCount];
                deliveryDeleteButtons = new Button[deliveryRowCount];


                for (int i = 0; i < deliveryRowCount; i++)
                {
                    deliveryTextBoxes[i] = new TextBox();

                    deliveryMeasureDropDownList[i] = new DropDownList();

                    deliveryUpdateButtons[i] = new Button();

                    deliveryDeleteButtons[i] = new Button();
                }

              //  ListItem[] measureDItems;// = new ListItem[measureRowCount];
                List<ListItem> measureList = new List<ListItem>();
                ListItemCollection listBoxData = new ListItemCollection();
                for (int z = 0; z < measureRowCount; z++)
                {
                    listBoxData.Add(new ListItem(measureData[1, z]));
                    measureList.Add( new ListItem(measureData[1, z]));
                }

                deliveryTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(500);

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
                headerRow.Cells.Add(header3);
                headerRow.Cells.Add(header4);
                headerRow.Cells.Add(header5);
                headerRow.Cells.Add(header6);

                // Add the header row to the table.
                deliveryTable.Rows.AddAt(0, headerRow);

            //    deliverySectionItems.Clear();
             //   tempDeliveryConstrutListItems.Clear();




                int numrows = deliveryRowCount;
                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();
    
                    TableCell a = new TableCell(); //worker name
                    deliveryTextBoxes[j].Text = sumData[1, j];
                    deliveryTextBoxes[j].ID = "1|" + j.ToString() + "|" + sumData[3, j] + "|D";
                    a.Controls.Add(deliveryTextBoxes[j]);
                    a.Width = Unit.Percentage(25);
                    r.Cells.Add(a);

                    TableCell b = new TableCell();
                    deliveryMeasureDropDownList[j].DataSource = measureDistinctList;
                    deliveryMeasureDropDownList[j].DataBind();
                    deliveryMeasureDropDownList[j].ID = "2|" + j.ToString() + "|" + sumData[3, j] + "|D";
                    deliveryMeasureDropDownList[j].DataTextField = "Text";
                    deliveryMeasureDropDownList[j].SelectedValue = sumData[2, j];
                    b.Controls.Add(deliveryMeasureDropDownList[j]);
                    b.Width = Unit.Percentage(25);
                    r.Cells.Add(b);

                    TableCell c = new TableCell();
                    deliveryUpdateButtons[j].Text = "Update this row";
                    deliveryUpdateButtons[j].Click += new EventHandler(OnDeliveryUpdateButtonClick);
                    deliveryUpdateButtons[j].ID = "3|" + j.ToString() + "|" + sumData[3, j] + "|D";
                    deliveryUpdateButtons[j].ToolTip = "Make any needed changes in the row and then click update";
                    c.Controls.Add(deliveryUpdateButtons[j]);
                    c.Width = Unit.Percentage(25);
                    r.Cells.Add(c);

                    TableCell d = new TableCell();
                    deliveryDeleteButtons[j].Text = "Delete this row";
                    deliveryDeleteButtons[j].Click += new EventHandler(OnDeliveryDeleteButtonClick);
                    deliveryDeleteButtons[j].ID = "4|" + j.ToString() + "|" + sumData[3, j] + "|D";
                    deliveryDeleteButtons[j].ToolTip = "Click delete to remove this worker";
                    d.Controls.Add(deliveryDeleteButtons[j]);
                    d.Width = Unit.Percentage(25);
                    r.Cells.Add(d);


                    //   }
                    deliveryTable.Rows.Add(r);
                }

                TableRow rr = new TableRow();

                TableCell aa = new TableCell(); //osha 10
                deliveryTB = new TextBox();
                deliveryTB.Text = "";
                deliveryTB.ID = "deliveryTB";
                aa.Controls.Add(deliveryTB);
                aa.Width = Unit.Percentage(20);
                rr.Cells.Add(aa);

                TableCell bb = new TableCell();
                deliveryMeasureDD = new DropDownList();
                ListItem[] measureDItemsx = { new ListItem("Pieces"), new ListItem("Trucks"), new ListItem("Container"), new ListItem("Pounds"), new ListItem("Tons"), new ListItem("Linear Feet"), new ListItem("Square Feet"), new ListItem("Rolls") };
                deliveryMeasureDD.Items.AddRange(measureDItemsx);
                deliveryMeasureDD.ID = "deliveryMeasureDD";
                bb.Controls.Add(deliveryMeasureDD);
                bb.Width = Unit.Percentage(20);
                rr.Cells.Add(bb);

                TableCell cc = new TableCell();
                var addNewDeliveryButton = new Button();
                addNewDeliveryButton.Text = "Add this row";
                addNewDeliveryButton.Click += new EventHandler(addNewDeliveryButtonClick);
                addNewDeliveryButton.ID = "5x";
                addNewDeliveryButton.ToolTip = "Add the needed data and then click add this row";
                cc.Controls.Add(addNewDeliveryButton);
                cc.Width = Unit.Percentage(20);
                rr.Cells.Add(cc);

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

                commentTable.Rows.Clear();

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
                commentTable.Rows.AddAt(0, headerRow);

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
                    commentTable.Rows.Add(r);
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

                commentTable.Rows.Add(rr);


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

                fileTable.Rows.Clear();

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
                fileTable.Rows.AddAt(0, headerRow);

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
                    fileTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                string temp = ex.ToString();

            }

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
                
            }

            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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

                //  string workerWipSQL = "SELECT DISTINCT trade, name, osha10, level, updateDate, mySqlID FROM wcf_data.worker where company = '" + comp + "';";

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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
        }

        private void addNewWorkerButtonClick(object sender, EventArgs e)
        {
            
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);
            try
            {
            
            String insertString = "";
            if(workerTradeDD.SelectedIndex < 0) { workerTradeDD.SelectedValue = "Ironworker"; }
            String trade = workerTradeDD.SelectedValue;
            String name = HttpUtility.HtmlEncode(workerNameTB.Text);
            String osha = HttpUtility.HtmlEncode(workerOsha10TB.Text);
            String level = workerLevelDD.SelectedValue;

            string id = Convert.ToString(getmySqlID("worker",  companyName));
            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.worker (company, trade, name, osha10, level, updateDate, mySqlID ) " +
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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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

            String construct = HttpUtility.HtmlEncode(constructionTextBoxes[Convert.ToInt32(idParts[1])].Text);


            insertString = "UPDATE wcf_data.constructiontypes " +
                " SET company = '" + companyName + "', " +
                " construction = '" + construct + "', " +
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
            //getSectionTable(site);
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


            insertString = "DELETE FROM wcf_data.constructiontypes " +

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
            //getSectionTable(globalSiteName);
            getConstructionTable(globalSiteName);
            getTaskTable();
            getDeliveryTable();
        }

        protected void addNewConstructionButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();

            try
            {
           
            String construct = HttpUtility.HtmlEncode(constructionTB.Text);

            string id = Convert.ToString(getmySqlID("constructiontypes", companyName));

            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.constructiontypes (company, constructionType, updateDate, mySqlID ) " +
                "VALUES('" + companyName + "', '"  + construct + "', '" + dateToday + "', '" + id + "')";

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

            String constructionType = taskConstructionDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;
            
            String task = HttpUtility.HtmlEncode(taskTextBoxes[Convert.ToInt32(idParts[1])].Text);
            String measure = taskMeasureDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;



            insertString = "UPDATE wcf_data.tasktypes " +
                " SET construction = '" + constructionType + "', " +
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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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


            insertString = "DELETE FROM wcf_data.tasktypes " +

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

                String getUpdateRowSQL = "SELECT DISTINCT construction, task FROM wcf_data.tasktypes AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.tasktypes AS b WHERE a.construction = b.construction AND a.task = b.task ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if (rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        constructionStr = rdr.GetString(0);
                        taskStr = rdr.GetString(1);
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
                    insertString = "UPDATE wcf_data.tasktypes " +
                     " SET  updateDate = '" + dateToday + "' " +
                      " WHERE construction = '" + constructionStr + "' AND task = '" + taskStr + "';";
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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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

           // String section = taskSectionDD.SelectedValue;
            String construct = taskConstructionDD.SelectedValue;

            String task = HttpUtility.HtmlEncode(taskTB.Text);
            String measure = taskMeasureDD.SelectedValue;

            string id = Convert.ToString(getmySqlID("tasktypes", companyName));

            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.tasktypes (company, construction, task, measure, updateDate, mySqlID) " +
                "VALUES('" + companyName + "', '" +  construct + "', '" + task + "', '" + measure + "', '" + dateToday + "', '" + id + "')";

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

            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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

            String delivery = HttpUtility.HtmlEncode(deliveryTextBoxes[Convert.ToInt32(idParts[1])].Text);
            String measure = deliveryMeasureDropDownList[Convert.ToInt32(idParts[1])].SelectedValue;



            insertString = "UPDATE wcf_data.deliverytypes " +
                " SET material = '" + delivery + "', " +
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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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




            insertString = "DELETE FROM wcf_data.deliverytypes " +

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

                String getUpdateRowSQL = "SELECT DISTINCT material FROM wcf_data.delivery AS a WHERE updateDate = (SELECT MIN(updateDate) FROM wcf_data.deliverytypes AS b WHERE  a.material = b.material ) LIMIT 1;";

                MySqlCommand cmd1 = new MySqlCommand(getUpdateRowSQL, conn);
                rdr = cmd1.ExecuteReader();

                if (rdr.HasRows == true)
                {
                    hasRows = true;
                    while (rdr.Read())
                    {
                        constructionStr = rdr.GetString(0);
                        materialStr = rdr.GetString(1);
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
                    insertString = "UPDATE wcf_data.deliverytypes " +
               " SET  updateDate = '" + dateToday + "' " +
              " WHERE measure = '" + materialStr + "';";
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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
        }

        private void addNewDeliveryButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";
            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();

            try
            {

            string id = Convert.ToString(getmySqlID("deliverytypes", companyName));

           // if (deliveryDD.SelectedIndex < 0) { deliveryDD.SelectedValue = "Steel"; }
            //String construct = deliveryDD.SelectedValue;
            String delivery = deliveryTB.Text;
            String measure = deliveryMeasureDD.SelectedValue;

            var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.deliverytypes (company, material, measure, updateDate, mySqlID) " +
                "VALUES('" + companyName + "', '" + delivery +"', '" + measure + "', '" + dateToday + "', '" + id + "')";

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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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

            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();
        }

        private void addNewCommentButtonClick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            String todayFormat = "yyyy-MM-dd HH:mm:ss";
            String dateToday = now.ToString(todayFormat);

            String insertString = "";

            MySqlConnection conn = null;
            MySqlCommand cmd = new MySqlCommand();// (insertString, conn);

            try
            {

            if (commentTradeDD.SelectedIndex < 0) { commentTradeDD.SelectedValue = "Ironworker"; }
            String trade = commentTradeDD.SelectedValue;
            String type = taskConstructionDD.SelectedValue;
            String comment = commentTB.Text;

                string id = Convert.ToString(getmySqlID("comment", companyName));

                var whichButton = (Button)sender;

            insertString = "INSERT INTO wcf_data.comment (company, trade, commentType, comment, updateDate, mySqlID) " +
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
            getConstructionTable(globalSiteName);
            getWorkerTable();
            getTaskTable();
            getDeliveryTable();
            getCommentTable();
            getDocumentTable();

        }
        /*
        protected void taskSectionMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

         //   returnData = getConstructionDataBySection(companyName, globalSiteName, taskSectionNameDropDownList[Convert.ToInt32(idParts[2])].SelectedItem.Text);

            tempConstrutListItems.Clear();


            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
                tempConstrutListItems.Add( new ListItem(returnData[2, z]));
            }

            taskConstructionDropDownList[Convert.ToInt32(idParts[2])].Items.Clear();
            taskConstructionDropDownList[Convert.ToInt32(idParts[2])].DataSource = tempConstrutListItems;
            taskConstructionDropDownList[Convert.ToInt32(idParts[2])].DataBind();
        }

        protected void taskSectionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

         //  returnData = getConstructionDataBySection(companyName, globalSiteName, taskSectionDD.SelectedItem.Text);

            // sectionSelectedConstructionRowCount

            tempConstrutListItems.Clear();

            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
                tempConstrutListItems.Add( new ListItem(returnData[2, z]));
            }

            taskConstructionDD.Items.Clear();
            taskConstructionDD.DataSource = tempConstrutListItems;
            taskConstructionDD.DataBind();

        }

        protected void deliverySectionMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

         //  returnData = getConstructionDataBySection(companyName, globalSiteName, deliverySectionNameDropDownList[Convert.ToInt32(idParts[2])].SelectedItem.Text);

            tempDeliveryConstrutListItems.Clear();

            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
                tempDeliveryConstrutListItems.Add( new ListItem(returnData[2, z]));
            }

            deliveryDropDownList[Convert.ToInt32(idParts[2])].Items.Clear();
            deliveryDropDownList[Convert.ToInt32(idParts[2])].DataSource =tempDeliveryConstrutListItems;
            deliveryDropDownList[Convert.ToInt32(idParts[2])].DataBind();
        }

        protected void constructionSectionMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

            returnData = getConstructionDataBySection(companyName, globalSiteName, deliverySectionNameDropDownList[Convert.ToInt32(idParts[2])].SelectedItem.Text);

            tempDeliveryConstrutListItems.Clear();

            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
                tempDeliveryConstrutListItems.Add(new ListItem(returnData[2, z]));
            }

            deliveryConstructionDropDownList[Convert.ToInt32(idParts[2])].Items.Clear();
            deliveryConstructionDropDownList[Convert.ToInt32(idParts[2])].DataSource = tempDeliveryConstrutListItems;
            deliveryConstructionDropDownList[Convert.ToInt32(idParts[2])].DataBind();
        }

        protected void constructionSectionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

           // returnData = getConstructionDataBySection(companyName, globalSiteName, taskSectionDD.SelectedItem.Text);

            // sectionSelectedConstructionRowCount

            tempConstrutListItems.Clear();

            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
                tempConstrutListItems.Add(new ListItem(returnData[2, z]));
            }

            taskConstructionDD.Items.Clear();
            taskConstructionDD.DataSource = tempConstrutListItems;
            taskConstructionDD.DataBind();

       }

        protected void deliverySectionDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            String[,] returnData = new String[5, 500];

            var whichDD = (DropDownList)sender;
            String buttonID = whichDD.ID;
            String[] idParts;
            idParts = buttonID.Split('|'); //part two has the item number

          //  returnData = getConstructionDataBySection(companyName, globalSiteName, deliverySectionDD.SelectedItem.Text);

            tempConstrutListItems.Clear();

            for (int z = 0; z < sectionSelectedConstructionRowCount; z++)
            {
                tempConstrutListItems.Add(new ListItem(returnData[2, z]));
            }

            deliveryDD.Items.Clear();
            deliveryDD.DataSource = tempConstrutListItems;
            deliveryDD.DataBind();
        }

        protected void siteNameTextBox_TextChanged(object sender, EventArgs e)
        {
            siteNameStr = siteTownTextBox.Text;
        }

        protected void siteTownTextBox_TextChanged(object sender, EventArgs e)
        {
            siteTownStr = siteTownTextBox.Text;
        }
    */
        protected void UploadButton_Click(object sender, EventArgs e)
        {
            // Save the uploaded file to an "Uploads" directory
            // that already exists in the file system of the 
            // currently executing ASP.NET application.  
            // Creating an "Uploads" directory isolates uploaded 
            // files in a separate directory. This helps prevent
            // users from overwriting existing application files by
            // uploading files with names like "Web.config".
            string saveDir = @"\Uploads\";

            string extension = Path.GetExtension(FileUpload1.PostedFile.FileName).Substring(1);

            extension = extension.ToLower();

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

    }
    public static class MessageBox1
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