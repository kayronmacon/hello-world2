using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Act_site.Pages
{
    public partial class siteReportsAndFieldwork : System.Web.UI.Page
    {
        String companyName = "Action";
        String globalSiteName = "Choose a site";

      //  Response.ContentType = "application/pdf";  
//Response.AppendHeader("Content-Disposition", "attachment; filename=MyFile.pdf");  
//Response.TransmitFile(Server.MapPath("~/Files/MyFile.pdf"));  
//Response.End(); 

  //          .htm, .html Response.ContentType = "text/HTML";
//.txt Response.ContentType = "text/plain";
//.doc, .rtf, .docx Response.ContentType = "Application/msword";
//.xls, .xlsx Response.ContentType = "Application/x-msexcel";
//.jpg, .jpeg Response.ContentType = "image/jpeg";
//.gif Response.ContentType =  "image/GIF";
//.pdf Response.ContentType = "application/pdf";

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
                    ChooseSiteDropDownList.Items.FindByValue(globalSiteName).Selected = true;
                    siteNameLabel.Text = globalSiteName;

                }
                if (!Page.IsPostBack)
                {
                    loadSiteDropDown(companyName);
                }
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }

        public Tuple<int, List<string>> getDistinctSiteList(String comp)
        {
            List<string> siteDistinctList = new List<string>();

            string cs = ConfigurationManager.ConnectionStrings["reportConnection"].ConnectionString;

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string sectionSQL = "SELECT DISTINCT site FROM daily_reports.reports WHERE company = '" + comp + "';";

                MySqlCommand cmd = new MySqlCommand(sectionSQL, conn);
                rdr = cmd.ExecuteReader();


                while (rdr.Read())
                {
                    siteDistinctList.Add(rdr.GetString(0));
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

            Tuple<int, List<string>> returnValue = new Tuple<int, List<string>>(siteDistinctList.Count, siteDistinctList);
           // return author;
            return returnValue;
        }

        protected void loadSiteDropDown(string comp)
        {

            try
            {
                Tuple<int, List<string>> siteList = getDistinctSiteList(comp);

                ChooseSiteDropDownList.Items.Clear();

                if (siteList.Item1 == 0)
                {
                    ChooseSiteDropDownList.Items.Add(new ListItem("No current sites"));
                }

                for (int x = 0; x < siteList.Item1; x++)
                    {
                        ChooseSiteDropDownList.Items.Add(new ListItem(siteList.Item2[x], siteList.Item2[x]));
                    }

                getReportTable(companyName, Convert.ToString(ChooseSiteDropDownList.Items[0]));
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }

        }

        protected void ChooseSiteDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {


                globalSiteName = ChooseSiteDropDownList.SelectedValue.ToString();


                Session["Site"] = globalSiteName;

                siteNameLabel.Text = globalSiteName;

                getReportTable(companyName, globalSiteName);

                //  ChooseSiteDropDownList.ClearSelection();
            }
            catch (Exception ex)
            {
                string temp = ex.Message.ToString();
            }
        }


        protected void getReportTable(string company, string site)
        {
            


            try
            {
                string baseUrlDaily = "~/Reports/" + site + "/daily/";
                string baseUrlFW = "~/Reports/" + site + "/fieldWork";

                //string a = HttpContext.Current.Server.MapPath(baseUrlDaily);

                Tuple<int, int, List<string>, List<string>, List<string>, List<string>> returnData = getReportData(company, site);

                HyperLink[] reportLinks = new HyperLink[returnData.Item1];
                HyperLink[] fwLinks = new HyperLink[returnData.Item2];

                reportViewTable.Rows.Clear();

                TableItemStyle tableStyle = new TableItemStyle();
                tableStyle.HorizontalAlign = HorizontalAlign.Center;
                tableStyle.VerticalAlign = VerticalAlign.Middle;
                tableStyle.Width = Unit.Pixel(800);

                TableHeaderCell header1 = new TableHeaderCell();
                header1.Text = "Daily Reports";
                header1.Font.Bold = true;
                header1.BackColor = Color.LightGray;
                header1.HorizontalAlign = HorizontalAlign.Center;
                header1.Width = Unit.Percentage(50);
                header1.CssClass = "CssStyleBothBorder"; //"CssStyleBothBorder CssStyleBottomBorder"


                TableHeaderCell header2 = new TableHeaderCell();
                header2.Text = "Field Work Reports";
                header2.Font.Bold = true;
                header2.BackColor = Color.LightGray;
                header2.HorizontalAlign = HorizontalAlign.Center;
                header2.Width = Unit.Percentage(50);
                header2.CssClass = "CssStyleBothBorder";

                


                // Add the header to a new row.
                TableRow headerRow = new TableRow();
                headerRow.Cells.Add(header1);
                headerRow.Cells.Add(header2);

                // Add the header row to the table.
                reportViewTable.Rows.AddAt(0, headerRow);


                //string link = HttpContext.Current.Server.MapPath("~/App_Data/Example.xml");

                int numrows = Math.Max(returnData.Item1, returnData.Item2);

                for (int j = 0; j < numrows; j++)
                {

                    TableRow r = new TableRow();

                    TableCell a = new TableCell(); //trade type
                    if (j < returnData.Item1)
                    {
                        reportLinks[j] = new HyperLink();
                        //  MapURL(returnData.Item3[j]);
                        string link = baseUrlDaily + returnData.Item4[j]; 

                        link = link.Replace(@"\", "/");

                        reportLinks[j].NavigateUrl = link;
                        reportLinks[j].Target = "_blank";
                        reportLinks[j].Text = returnData.Item4[j];
                        a.HorizontalAlign = HorizontalAlign.Center;
                        a.Width = Unit.Percentage(50);
                        a.Wrap = true;
                        a.Controls.Add(reportLinks[j]);
                        r.Cells.Add(a);
                    }

                    TableCell b = new TableCell(); //trade type
                    if (j < returnData.Item2)
                    {
                        fwLinks[j] = new HyperLink();
                        string link = baseUrlFW + returnData.Item6[j];

                        link = link.Replace(@"\", "/");

                        fwLinks[j].NavigateUrl = link;
                        fwLinks[j].Target = "_blank";
                        fwLinks[j].Text = returnData.Item6[j];
                        a.HorizontalAlign = HorizontalAlign.Center;
                        a.Width = Unit.Percentage(50);
                        a.Wrap = true;
                        a.Controls.Add(fwLinks[j]);
                        r.Cells.Add(a);
                    }




                    reportViewTable.Rows.Add(r);
                }


            }
            catch (Exception ex)
            {
                String temp = ex.Message.ToString();
            }

        }

        private string MapURL(string path)
        {
            string appPath = Server.MapPath("/").ToLower();
            return string.Format("/{0}", path.ToLower().Replace(appPath, "").Replace(@"\", "/"));
        }

        protected Tuple <int, int, List<string>, List<string>, List<string>, List<string>> getReportData(string company, string site)
        {
            int reportCount = 0;
            int fwCount = 0;
            List<string> reportLinks = new List<string>();
            List<string> reportTitles = new List<string>();
            List<string> fwLinks = new List<string>();
            List<string> fwTitles = new List<string>();

          //  Tuple <int, int, List<string>, List<string>, List<string>, List<string>> returnData = new Tuple<int, int, List<string>, List<string>, List<string>, List<string>>(reportCount, fwCount, reportLinks, reportTitles, fwLinks, fwTitles);


            string cs = ConfigurationManager.ConnectionStrings["reportConnection"].ConnectionString;
            //  string image_path = ConfigurationManager.AppSettings["Test1"];

            MySqlConnection conn = null;
            MySqlDataReader rdr = null;

            try
            {
                conn = new MySqlConnection(cs);
                conn.Open();

                string reportSQL = "SELECT DISTINCT pdfLink, pdfFileName FROM daily_reports.reports WHERE company = '" + company + "' AND site = '" + site + "';";

                MySqlCommand cmd = new MySqlCommand(reportSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    reportLinks.Add(rdr.IsDBNull(0) == true ? "NA" : rdr.GetString(0));
                    reportTitles.Add(rdr.IsDBNull(1) == true ? "NA" : rdr.GetString(1));
                    reportCount++;
                }

                conn.Close();

            }
            catch (MySqlException ex)
            {
                string temp = ex.ToString();
            }


            try
            {
             //   conn = new MySqlConnection(cs);
             /*
                conn.Open();

                string reportSQL = "SELECT DISTINCT pdfLink, pdfFileName FROM daily_reports.reports WHERE company = '" + company + "' AND site = '" + site + "';";

                MySqlCommand cmd = new MySqlCommand(reportSQL, conn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    reportLinks.Add(rdr.IsDBNull(0) == true ? "NA" : rdr.GetString(0));
                    reportTitles.Add(rdr.IsDBNull(1) == true ? "NA" : rdr.GetString(1));
                    reportCount++;
                }

                conn.Close();
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

            Tuple<int, int, List<string>, List<string>, List<string>, List<string>> returnData = new Tuple<int, int, List<string>, List<string>, List<string>, List<string>>(reportCount, fwCount, reportLinks, reportTitles, fwLinks, fwTitles);

            return returnData;
        }



    }
}