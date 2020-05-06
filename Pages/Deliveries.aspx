<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Deliveries.aspx.cs" Inherits="Act_site.siteDeliveries" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
                         
                    
                  <div>

                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
                             
                        <ContentTemplate ><fieldset>
                        <legend><h2><asp:Label ID="siteNameLabel" Text="Choose Site" runat="server"></asp:Label></h2></legend>
                        <asp:DropDownList ID="ChooseSiteDropDownList" runat="server" OnSelectedIndexChanged="ChooseSiteDropDownList_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>                           
                        </fieldset>

                        
                        <fieldset>                       
                        <legend><h2>Summary of deliveries</h2></legend>
                        <asp:Table ID="summaryTable" runat="server"></asp:Table>
                        </fieldset>              
                      

                        
                        
                        <fieldset>
                        <legend><h2>Detailed list of deliveries</h2></legend>
                        <asp:DropDownList ID="DeliveryViewDropDownList" runat="server" OnSelectedIndexChanged="DeliveryViewDropDownList_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <asp:Table ID="detailTable" runat="server"></asp:Table>
                        </fieldset></ContentTemplate></asp:UpdatePanel>
                      <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" ShowGroupPanel="true" AllowSorting="true"
        DataSourceID="SqlDataSource1" AllowPaging="true" PageSize="30" AutoGenerateColumns="false" Width="100%">
        <ClientSettings AllowColumnsReorder="true" AllowDragToGroup="true" ReorderColumnsOnClient="true">
            <Scrolling AllowScroll="true" UseStaticHeaders="true"></Scrolling>
            <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
        </ClientSettings>
        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
        <MasterTableView EnableHeaderContextMenu="true">
             <ColumnGroups>
                <telerik:GridColumnGroup HeaderText="Monday" Name="Monday" >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Tuesday" Name="Tuesday" >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Wednesday" Name="Wednesday" >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Thursday" Name="Thursday" >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Friday" Name="Friday" >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Saturday" Name="Saturday" >
                </telerik:GridColumnGroup>
                <telerik:GridColumnGroup HeaderText="Sunday" Name="Sunday" >
                </telerik:GridColumnGroup>
            </ColumnGroups> 
            <Columns>
                 <telerik:GridBoundColumn UniqueName="Name" DataField="Name" HeaderText="Name">
                </telerik:GridBoundColumn>
                <telerik:GridNumericColumn UniqueName="Osha10" DataField="Osha10" HeaderText="Osha 10">
                </telerik:GridNumericColumn>
               
                <telerik:GridDateTimeColumn UniqueName="weekBeginning" DataField="weekBeginning" HeaderText="Week Beginning">
                </telerik:GridDateTimeColumn>
                <telerik:GridNumericColumn UniqueName="monday_Str" DataField="monday_Str" ColumnGroupName="Monday"
                    HeaderText="Str">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="monday_half" DataField="monday_half" ColumnGroupName="Monday"
                    HeaderText="1.5">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="monday_double" DataField="monday_double" ColumnGroupName="Monday"
                    HeaderText="2.0">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="monday_Cir" DataField="monday_Cir" ColumnGroupName="Monday"
                    HeaderText="Cir">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="tuesday_Str" DataField="tuesday_Str" ColumnGroupName="Tuesday"
                    HeaderText="Str">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="tuesday_half" DataField="tuesday_half" ColumnGroupName="Tuesday"
                    HeaderText="1.5">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn UniqueName="tuesday_double" DataField="tuesday_double" ColumnGroupName="Tuesday"
                    HeaderText="2.0">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="tuesday_Cir" DataField="tuesday_Cir" ColumnGroupName="Tuesday"
                    HeaderText="Cir">
                </telerik:GridNumericColumn>
                 <telerik:GridNumericColumn UniqueName="wednesday_Str" DataField="wednesday_Str" ColumnGroupName="Wednesday"
                    HeaderText="Str">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="wednesday_half" DataField="wednesday_half" ColumnGroupName="Wednesday"
                    HeaderText="1.5">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn UniqueName="wednesday_double" DataField="wednesday_double" ColumnGroupName="Wednesday"
                    HeaderText="2.0">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="wednesday_Cir" DataField="wednesday_Cir" ColumnGroupName="Wednesday"
                    HeaderText="Cir">
                </telerik:GridNumericColumn>
                 <telerik:GridNumericColumn UniqueName="thursday_Str" DataField="thursday_Str" ColumnGroupName="Thursday"
                    HeaderText="Str">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="thursday_half" DataField="thursday_half" ColumnGroupName="Thursday"
                    HeaderText="1.5">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn UniqueName="thursday_double" DataField="thursday_double" ColumnGroupName="Thursday"
                    HeaderText="2.0">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="thursday_Cir" DataField="thursday_Cir" ColumnGroupName="Thursday"
                    HeaderText="Cir">
                </telerik:GridNumericColumn>
                 <telerik:GridNumericColumn UniqueName="friday_Str" DataField="friday_Str" ColumnGroupName="Friday"
                    HeaderText="Str">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="friday_half" DataField="friday_half" ColumnGroupName="Friday"
                    HeaderText="1.5">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn UniqueName="friday_double" DataField="friday_double" ColumnGroupName="Friday"
                    HeaderText="2.0">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="friday_Cir" DataField="friday_Cir" ColumnGroupName="Friday"
                    HeaderText="Cir">
                </telerik:GridNumericColumn>
                 <telerik:GridNumericColumn UniqueName="saturday_Str" DataField="saturday_Str" ColumnGroupName="Saturday"
                    HeaderText="Str">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="saturday_half" DataField="saturday_half" ColumnGroupName="Saturday"
                    HeaderText="1.5">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn UniqueName="saturday_double" DataField="saturday_double" ColumnGroupName="Saturday"
                    HeaderText="2.0">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="saturday_Cir" DataField="saturday_Cir" ColumnGroupName="Saturday"
                    HeaderText="Cir">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="sunday_Str" DataField="sunday_Str" ColumnGroupName="Sunday"
                    HeaderText="Str">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="sunday_half" DataField="sunday_half" ColumnGroupName="Sunday"
                    HeaderText="1.5">
                </telerik:GridNumericColumn>  
                <telerik:GridNumericColumn UniqueName="sunday_double" DataField="sunday_double" ColumnGroupName="Sunday"
                    HeaderText="2.0">
                </telerik:GridNumericColumn>
                <telerik:GridNumericColumn UniqueName="sunday_Cir" DataField="sunday_Cir" ColumnGroupName="Sunday"
                    HeaderText="Cir">
                </telerik:GridNumericColumn>
                </Columns>

             <NestedViewTemplate>
      <asp:Panel ID="NestedViewPanel" runat="server" CssClass="viewWrap">
  
        <div class="contactWrap">
          <fieldset style="padding: 10px;">
            <legend style="padding: 5px;"><b>Detail info for tablet:<%#Eval("tabletID") %></b> 
            </legend>
            <table>
  <tr>
    <th>Name</th>
    <th>Osha</th>
    <th>Week Beginning</th>
    <th colspan="2">Monday</th>
    <th colspan="2">Tuesday</th>
    <th colspan="2">Wednesday</th>
    <th colspan="2">Thursday</th>
    <th colspan="2">Friday</th>
    <th colspan="2">Saturday</th>
    <th colspan="2">Sunday</th>
  </tr>
  <tr>
    <td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
    <td><%task1%></td>
    <td><%8%></td>
    <td><%task1%></td>
    <td><%8%></td>
    <td><%task1%></td>
    <td><%8%></td>
    <td><%task1%></td>
    <td><%8%></td>
    <td><%task1%></td>
    <td><%8%></td>
    <td><%task1%></td>
    <td><%8%></td>
    <td><%task1%></td>
    <td><%8%></td>
  </tr>
  <tr>
    <td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
    <td><%task2%></td>
    <td><%8%></td>
    <td><%task2%></td>
    <td><%8%></td>
    <td><%task2%></td>
    <td><%8%></td>
    <td><%task2%></td>
    <td><%8%></td>
    <td><%task2%></td>
    <td><%8%></td>
    <td><%task2%></td>
    <td><%8%></td>
    <td><%task2%></td>
    <td><%8%></td>
  </tr>
  <tr>
    <td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
    <td><%task3%></td>
    <td><%8%></td>
    <td><%task3%></td>
    <td><%8%></td>
    <td><%task3%></td>
    <td><%8%></td>
    <td><%task3%></td>
    <td><%8%></td>
    <td><%task3%></td>
    <td><%8%></td>
    <td><%task3%></td>
    <td><%8%></td>
    <td><%task3%></td>
    <td><%8%></td>
  </tr>
  <tr>
    <td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
    <td><%task4%></td>
    <td><%8%></td>
    <td><%task4%></td>
    <td><%8%></td>
    <td><%task4%></td>
    <td><%8%></td>
    <td><%task4%></td>
    <td><%8%></td>
    <td><%task4%></td>
    <td><%8%></td>
    <td><%task4%></td>
    <td><%8%></td>
    <td><%task4%></td>
    <td><%8%></td>
  </tr>
  <tr>
    <td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
    <td colspan="2"><%Comment1%></td>
    <td colspan="2"><%Comment1%></td>
    <td colspan="2"><%Comment1%></td>
    <td colspan="2"><%Comment1%></td>
    <td colspan="2"><%Comment1%></td>
    <td colspan="2"><%Comment1%></td>
    <td colspan="2"><%Comment1%></td>
    
  </tr>
  <tr>
    <td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
    <td colspan="2"><%Comment2%></td>
    <td colspan="2"><%Comment2%></td>
    <td colspan="2"><%Comment2%></td>
    <td colspan="2"><%Comment2%></td>
    <td colspan="2"><%Comment2%></td>
    <td colspan="2"><%Comment2%></td>
    <td colspan="2"><%Comment2%></td>
          
          
  </tr>
 <tr>
 <td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
 	<td colspan="2"><%Approver%></td>
    <td colspan="2"><%Approver%></td>
    <td colspan="2"><%Approver%></td>
    <td colspan="2"><%Approver%></td>
    <td colspan="2"><%Approver%></td>
    <td colspan="2"><%Approver%></td>
    <td colspan="2"><%Approver%></td>
 </tr>
 
 <tr>
 	<td><%Name%></td>
    <td><%0123456789%></td>
    <td><%weekbeg%></td>
    	  <td colspan="2"><%Signature%></td>
          <td colspan="2"><%Signature%></td>
          <td colspan="2"><%Signature%></td>
          <td colspan="2"><%Signature%></td>
          <td colspan="2"><%Signature%></td>
          <td colspan="2"><%Signature%></td>
          <td colspan="2"><%Signature%></td>
 </tr>
 

</table>
          </fieldset>
        </div>
      </asp:Panel>
    </NestedViewTemplate>
        </MasterTableView>
    </telerik:RadGrid>
       </form>     
                  </div>
    <div>


    </div>
 </asp:Content>

