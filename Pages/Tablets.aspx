<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tablets.aspx.cs" Inherits="Act_site.siteTablets" %>
<asp:Content runat="server" ID="HeadContent1" ContentPlaceHolderID="HeadContent">
    <script type='text/javascript' src='http://www.bing.com/api/maps/mapcontrol' ></script>                  
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       
    <div id="#parentUpdateContent">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>

         <div class="leftUpdateContent">
        <style type="text/css"> label { display: inline-block; }</style>
    <asp:CheckBox ID="CheckBoxShowChoice" runat="server" Text="  Choose" TextAlign="Right"
        OnCheckedChanged="CheckBoxShowChoice_CheckedChanged" AutoPostBack="True"/>
        <asp:DropDownList ID="DropDownListTab" runat="server"></asp:DropDownList>
        <asp:Button ID="ButtonUpdateChoice" runat="server" Text="Update" OnClick="ButtonUpdateChoice_Click"/>
             </div><div class="middleUpdateContent">
              <asp:Calendar ID="calStart" runat="server" SelectionMode="Day" Caption="Start Date" ></asp:Calendar>
            </div><div class="rightUpdateContent">
               <asp:Calendar ID="calEnd" runat="server" SelectionMode="Day" Caption="End Date"></asp:Calendar>
                </div>
        </ContentTemplate>
         <Triggers>
         <asp:AsyncPostBackTrigger ControlID="CheckBoxShowChoice" EventName="CheckedChanged" />
         </Triggers>
       </asp:UpdatePanel>
    </div>

                                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                             
                        <ContentTemplate><fieldset>
                        <legend><h2><asp:Label ID="siteNameLabel" Text="Choose Site" runat="server"></asp:Label></h2></legend>
                            <asp:DropDownList ID="ChooseSiteDropDownList" runat="server" OnSelectedIndexChanged="ChooseSiteDropDownList_SelectedIndexChanged" autoPostBack="true"></asp:DropDownList>

                            &nbsp;&nbsp;

                             <legend><h2>Choose week begin date</h2></legend>
                            <asp:DropDownList ID="weekViewDropDownList" runat="server" OnSelectedIndexChanged="WeekViewDropDownList_SelectedIndexChanged" autoPostBack="true"></asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:Table ID="tabletViewTable" runat="server"></asp:Table>
                        
       

            
                                       
               <!-- was here -->

                            </fieldset></ContentTemplate></asp:UpdatePanel>



    <style type="text/css">
      div#pop-up {
        display: none;
        position: absolute;
        width: 420px;
        padding: 10px;
        background: #eeeeee;
        color: #000000;
        border: 1px solid #1a1a1a;
        font-size: 90%;
      }
    </style>
       
    <div style="width:1000px;">
        <% =getBasicTable()%>.
        </div> 

      <!-- HIDDEN / POP-UP DIV -->
      <div id="pop-up">
    <div id="mapDiv" style="position:relative;width:400px;height:400px;">
      </div>
          </div>

     <script type="text/javascript">
        // $(function () {
             var moveLeft = 20;
             var moveDown = -200;
             var latLon;

         $(".tempxx")
          .mouseenter(function () {
              var latLon = event.target.innerHTML; //jQuery(this).attr("innerHTML");
             // var c = parseFloat("10.33")
              var res = latLon.split(",");
              lat = res[1]; lon = res[2]; //event.target.innerHTML;
       //   $("#tempx").html(event.srcElement.innerHTML);
          // $("#tempx").html($.fn.jquery);

          $('div#pop-up').show()
            .css('top', event.pageY + moveDown)
            .css('left', event.pageX + moveLeft)
            .appendTo('body');
          GetMapTabHover(lat, lon);

      })

      .mouseleave(function () {
          $('div#pop-up').hide();
          disposeMapTabHover();

      });


    </script>

<script>
    var map;
    var ready;
    var readyStateCheckInterval = setInterval(function () {
        if (document.readyState === "complete") {
            clearInterval(readyStateCheckInterval);
            ready = true;
            
        }
    }, 10);


    function GetMapTabHover(lat, lon) {

        try{

            if (ready == true) {

                map = new Microsoft.Maps.Map('#mapDiv', {
                    credentials: 'AjpI3HF9ZbqXgYHCr6nEOvadU-sN97AY_9Jjun3G0z6-IWFwU3-cIZBfQPjjrhBT',
                    showScalebar: false,
                    showMapTypeSelector: false,
                    disableZooming: true,
                    mapTypeId: Microsoft.Maps.MapTypeId.road,
                    height: 200,
                    width: 200,
                    center: new Microsoft.Maps.Location(lat, lon),
                    mapTypeId: Microsoft.Maps.MapTypeId.aerial,
                    zoom: 15
                });
                var center = map.getCenter();

                var pin = new Microsoft.Maps.Pushpin(center);
                map.entities.push(pin);
            }
        }
        catch (err) {
        }
    }

    function disposeMapTabHover() {
        try{
            map.dispose();
        }
        catch(err)
        { alert(err.message);}
    }

</script>

</asp:Content>
