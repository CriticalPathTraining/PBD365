<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmbedDashboard.aspx.cs" Inherits="PowerBIEmbedding.EmbedDashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="/scripts/powerbi.js"></script>
    <form id="form1" runat="server">
        <asp:DropDownList ID="ddlDashboard" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Page_Load" />
        <div id="embedDiv" style="height: 600px; width: 100%;" />
        <script>
            // Read embed token
            var embedToken = "<% =this.embedToken %>";

            // Read embed URL
            var embedUrl = "<% = this.embedUrl %>";

            // Read dashboard Id
            var dashboardId = "<% = this.dashboardId %>";

            // Get models (models contains enums)
            var models = window['powerbi-client'].models;

            // Embed configuration is used to describe what and how to embed
            // This object is used when calling powerbi.embed
            // It can also includes settings and options such as filters
            var config = {
                type: 'dashboard',
                tokenType: models.TokenType.Embed,
                accessToken: embedToken,
                embedUrl: embedUrl,
                id: dashboardId,
                pageView: "oneColumn"
            };

            // Embed the dashboard within the div element
            var dashboard = powerbi.embed(embedDiv, config);
        </script>
    </form>
</body>
</html>
