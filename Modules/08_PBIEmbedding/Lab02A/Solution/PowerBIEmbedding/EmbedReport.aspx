<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmbedReport.aspx.cs" Inherits="PowerBIEmbedding.EmbedReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <script src="/scripts/powerbi.js"></script>
    <form id="form1" runat="server">
        <asp:DropDownList ID="ddlReport" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Page_Load" />
        <div id="embedDiv" style="height: 600px; width: 100%; max-width: 1000px;" />
        <script>
            // Read embed token
            var embedToken = "<% =this.embedToken %>";

            // Read embed URL
            var embedUrl = "<% = this.embedUrl %>";

            // Read report Id
            var reportId = "<% = this.reportId %>";

            // Get models (models contains enums)
            var models = window['powerbi-client'].models;

            // Embed configuration is used to describe what and how to embed
            // This object is used when calling powerbi.embed
            // It can also includes settings and options such as filters
            var config = {
                type: 'report',
                tokenType: models.TokenType.Embed,
                accessToken: embedToken,
                embedUrl: embedUrl,
                id: reportId,
                settings: {
                    filterPaneEnabled: true,
                    navContentPaneEnabled: true
                }
            };

            // Embed the report within the div element
            var report = powerbi.embed(embedDiv, config);
        </script>
    </form>
</body>
</html>
