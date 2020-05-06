<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="inUseData.aspx.cs" Inherits="Act_site.siteInUseData" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
                         
                    
                  <div class="col-sm-4" style="width: 1000px;">
                      <h2>List of workers, progress measures and delivery measures</h2>
                      <asp:UpdatePanel ID="UpdatePanel1" runat="server" >                             
                        <ContentTemplate><fieldset>
                        <legend><h2>Choose site to see deliveries</h2></legend>
                        <asp:DropDownList ID="ChooseSiteDropDownList" runat="server" OnSelectedIndexChanged="ChooseSiteDropDownList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                           
                        </fieldset> 
                    <div style="width: 1210px; min-height: 800px; border: 1px solid black; ">
                    
                        <div style="width: 400px;  min-height: 800px; height: 100%; border: 1px solid black; float: left;">            
                        
                        <fieldset>
                        
                                              
                        <legend><h2>Workers</h2></legend>
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                        <legend><asp:DropDownList ID="ChooseWorkerDate" runat="server" OnSelectedIndexChanged="ChooseWorkerDate_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList> </legend>
                        <asp:Table ID="workerTable" runat="server"></asp:Table>
  </fieldset>

                    </div>
                    <div style="width: 450px; min-height: 800px; height: 100%; border: 1px solid black; float: left;">
                     <fieldset>
                        <legend><h2>Progress Measures</h2></legend>
                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                        <legend><asp:DropDownList ID="ChooseProgressDate" runat="server" OnSelectedIndexChanged="ChooseProgressDate_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList> </legend>
                        <asp:Table ID="progressTable" runat="server"></asp:Table>

                         </fieldset>
                    </div>
                    <div style="width: 350px;  height: 100%; border: 1px solid black; float: left;">                        
                     <fieldset>
                        <legend><h2>Delivery Measures</h2></legend>
                        <asp:Literal ID="Literal3" runat="server"></asp:Literal>
                        <legend><asp:DropDownList ID="ChooseDeliveryDate" runat="server" OnSelectedIndexChanged="ChooseDeliveryDate_SelectedIndexChanged" AutoPostBack="true" ></asp:DropDownList> </legend>
                        <asp:Table ID="deliveryTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate>
                        </asp:UpdatePanel> </div>
                        </div>


                  </div>



    <div>


    </div>
 </asp:Content>



