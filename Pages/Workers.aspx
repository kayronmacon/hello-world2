<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Workers.aspx.cs" Inherits="Act_site.siteWorkers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      
    <style type="text/css">

        .CssStyleRightBorder {
    border-right: 2px solid black;
    text-align: center
}

.CssStyleLeftBorder {
    border-left: 2px solid black;
    text-align: center
}

.CssStyleBothBorder {
    border-right: 2px solid black;
    border-left: 2px solid black;
    text-align: center
}

.CssStyleBottomBorder {
    border-bottom: 2px solid black;
    text-align: center
}

.CssStyleBottomBorderNTA {
    border-bottom: 2px solid black;
}

.CssStyleNoBorderTA {
    text-align: center
}
    </style>

    <div style="width:1110px;">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                             
                        <ContentTemplate><fieldset>
                        <legend><h2><asp:Label ID="siteNameLabel" Text="Choose Site" runat="server"></asp:Label></h2></legend>
                            <asp:DropDownList ID="ChooseSiteDropDownList" runat="server" OnSelectedIndexChanged="ChooseSiteDropDownList_SelectedIndexChanged" autoPostBack="true"></asp:DropDownList>

                            &nbsp;&nbsp;

                             <legend><h2>Choose week begin date</h2></legend>
                            <asp:DropDownList ID="PayrollViewDropDownList" runat="server" OnSelectedIndexChanged="PayrollViewDropDownList_SelectedIndexChanged" autoPostBack="true"></asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:Table ID="payrollViewTable" runat="server"></asp:Table>
                        
       

                            <div style="width: 1000px;"> </div>
                            <legend><h2>Choose Worker</h2></legend>
                            
                             
              
                        
                            <asp:DropDownList ID="WorkerDropDownList" runat="server" OnSelectedIndexChanged="WorkerDropDownList_SelectedIndexChanged" autoPostBack="true"></asp:DropDownList>

                            &nbsp;&nbsp;
                            <h2>Summary of hours worked by task</h2>
                          <asp:Table ID="workerViewTable" runat="server"></asp:Table>             
                                       
               <!-- was here -->

                            </fieldset></ContentTemplate></asp:UpdatePanel>
                        </div>
</asp:Content>
