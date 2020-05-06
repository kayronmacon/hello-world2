<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="reportsandfieldwork.aspx.cs" Inherits="Act_site.Pages.siteReportsAndFieldwork" %>
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

    <div class="col-sm-4" style="width: 1110px;">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                             
                        <ContentTemplate><fieldset>
                        <legend><h2><asp:Label ID="siteNameLabel" Text="Choose Site" runat="server"></asp:Label></h2></legend>
                            <asp:DropDownList ID="ChooseSiteDropDownList" runat="server" OnSelectedIndexChanged="ChooseSiteDropDownList_SelectedIndexChanged" autoPostBack="true"></asp:DropDownList>


                            <h2>Reports availible for the site</h2>
                          <asp:Table ID="reportViewTable" runat="server"></asp:Table>             
                                       
               <!-- was here -->

                            </fieldset></ContentTemplate></asp:UpdatePanel>
                        </div>
</asp:Content>
