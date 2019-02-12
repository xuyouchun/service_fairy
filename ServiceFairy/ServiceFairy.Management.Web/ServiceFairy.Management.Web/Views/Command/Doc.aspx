<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	接口文档
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>接口文档</h2>
    
    <table style="width:100%; border:1px solid red;">
        <tr>
            <td style="padding-left:0px; vertical-align:top; width:230px; padding:5px;">
                <ul id="tree" style="list-style-type:square; margin:0px;">

                </ul>    
            </td>
            <td style="vertical-align:top; text-align:left">
                <div id="doc">
                </div>
            </td>
        </tr>
    </table>

    <script type="text/javascript">

        $('#tree').showCommandTree($('#doc'));
    
    </script>

</asp:Content>
