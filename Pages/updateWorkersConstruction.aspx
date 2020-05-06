<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="updateWorkersConstruction.aspx.cs" Inherits="Act_site.action.siteUpdateWorkersConstruction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
                         
                    
                  <div class="col-sm-4" style="width: 1000px;">
                   

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

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate><fieldset>
                        <legend><h2>Add, update and delete workers</h2></legend>
                        <asp:Table ID="workerTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate></asp:UpdatePanel>

                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate><fieldset>
                        <legend><h2>Add, update and delete comments</h2></legend>
                        <asp:Table ID="commentTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate></asp:UpdatePanel>
                      
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                        <ContentTemplate><fieldset>
                        <legend><h2>Add and delete PDF documents</h2></legend>
                        <asp:Table ID="fileTable" runat="server"></asp:Table>

                        </fieldset></ContentTemplate></asp:UpdatePanel>
                          <asp:FileUpload ID="FileUpload1" runat="server" accept=".pdf" /> <br />    
                        <asp:Button id="UploadButton" Text="Upload file" OnClick="UploadButton_Click"  runat="server"> </asp:Button>

                  </div>



    <div>


    </div>
 </asp:Content>
