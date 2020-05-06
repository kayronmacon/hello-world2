<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="updateProjects.aspx.cs" Inherits="Act_site.action.siteUpdateProjects" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
                         
                    
                  <div class="col-sm-4" style="width: 1000px;">

                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" >
                             
                        <ContentTemplate><fieldset>
                        <legend><h2>Add new sites and jobs</h2></legend>
                            <asp:DropDownList ID="ChooseSiteDropDownList" runat="server"  ></asp:DropDownList>
                            <asp:Button ID="updateSiteButton" runat="server" Text="Update Site" OnClick="updateSiteButtonClick" />&nbsp;&nbsp;
                            <asp:Button ID="addNewSiteButton" runat="server" Text="Add new site" OnClick="addNewSiteButtonClick" />

                            <asp:Table ID="Table1" 
            runat="server" 
            Font-Size="Medium" 
            Font-Names="Palatino"
            BackColor="White"
            BorderWidth="1"
            ForeColor="Black"
            CellPadding="5"
            CellSpacing="5"
            visible="false">
            <asp:TableHeaderRow 
                runat="server" 
                BackColor="LightGray"
                Font-Bold="true"
                >
                <asp:TableHeaderCell>Site Name</asp:TableHeaderCell>
                <asp:TableHeaderCell>Town</asp:TableHeaderCell>
                <asp:TableHeaderCell>Start Date</asp:TableHeaderCell>
                <asp:TableHeaderCell>End Date</asp:TableHeaderCell>
                <asp:TableHeaderCell>Add or update</asp:TableHeaderCell>
                <asp:TableHeaderCell>Active Site</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow 
                ID="TableRow1" 
                runat="server" 
                   >
                <asp:TableCell ID="siteNameCell" ><asp:TextBox ID="siteNameTB" runat="server"></asp:TextBox></asp:TableCell>
                <asp:TableCell ID="siteTownCell" ><asp:TextBox ID="siteTownTB" runat="server"></asp:TextBox></asp:TableCell>
                <asp:TableCell ID="siteStartDateCell" >Choose date in calendar</asp:TableCell>
                <asp:TableCell ID="siteEndDateCell" >Choose date in calendar</asp:TableCell>
                <asp:TableCell ID="siteButtonCell" ><asp:Button ID="siteButton" runat="server" Text="Add" OnClick="OnRowSiteAddButtonClick" /><asp:Button ID="siteButtonUpdate" visible="false" runat="server" Text="Update" OnClick="OnRowSiteUpdateButtonClick" /></asp:TableCell>
                <asp:TableCell ID="siteActiveCell" ><asp:CheckBox ID="CheckBox1" runat="server" Text="Check if Active" ToolTip="If this is checked, the project will load to the tablets" OnCheckedChanged="CheckBox1_CheckedChanged" /></asp:TableCell>
            </asp:TableRow>
                                </asp:Table>

                         <asp:Table ID="siteTable" runat="server"></asp:Table>


                            <table style="width:80%">

  <tr>
    <td><asp:Calendar ID="calStart" runat="server" SelectionMode="Day" Caption="Start Date" Visible="false" OnSelectionChanged="calStart_SelectionChanged" ></asp:Calendar></td>
    <td> <asp:Calendar ID="calEnd" runat="server" SelectionMode="Day" Caption="End Date" Visible="false" OnSelectionChanged="calEnd_SelectionChanged" ></asp:Calendar></td> 

  </tr>

</table>

                         
                       
                            </fieldset></ContentTemplate> </asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel6" runat="server" >
                        <ContentTemplate><fieldset>                       
                        <legend><h2>Add, update and delete site sections</h2></legend>
                        <asp:Table ID="sectionTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate>
                        </asp:UpdatePanel>              

                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                        <ContentTemplate><fieldset>
                        <legend><h2>Add, update and delete construction types</h2></legend>
                        <asp:Table ID="constructionTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate></asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate><fieldset>
                        <legend><h2>Add, update and delete tasks</h2></legend>
                        <asp:Table ID="taskTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate></asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate><fieldset>
                        <legend><h2>Add, update and delete deliveries</h2></legend>
                        <asp:Table ID="deliveryTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate></asp:UpdatePanel>
                      <!--
                          old items were here...
                      -->
                  </div>



    <div>


    </div>
 </asp:Content>
