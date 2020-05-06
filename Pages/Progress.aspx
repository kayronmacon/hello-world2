<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Progress.aspx.cs" Inherits="Act_site.siteProgress" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
        <div class="col-sm-4" style="width: 1110px;">
                            <asp:UpdatePanel ID="UpdatePanel5" runat="server" >
                             
                        <ContentTemplate><fieldset>
                        <legend><h2><asp:Label ID="siteNameLabel" Text="Choose Site" runat="server"></asp:Label></h2></legend>
                            <asp:DropDownList ID="ChooseSiteDropDownList" runat="server" OnSelectedIndexChanged="ChooseSiteDropDownList_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList>

                            &nbsp;&nbsp;

                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

                             <legend><h3>Choose week begin date</h3></legend>
                            <asp:DropDownList ID="PayrollViewDropDownList" runat="server" OnSelectedIndexChanged="PayrollViewDropDownList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            &nbsp;&nbsp;
                          
                            <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                
                        </fieldset></ContentTemplate></asp:UpdatePanel>
        </div>


</asp:Content>
