<%@ Page Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="CsvUpload.aspx.cs" Inherits="RM.MailshotsOnline.Web.CsvUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/temp.css" type="text/css" rel="stylesheet"/>
</head>
<body>
    <form id="frmUploadCsv" runat="server">
        <asp:HiddenField runat="server" id="hdnDistributionListId" />
        <asp:HiddenField runat="server" ID="hdnListName" />
    <asp:panel id="pnlLoginError" runat="server" visible="false">
        <p>You must be logged in to upload a CSV.</p>
    </asp:panel>
        <asp:panel ID="pnlFormArea" runat="server">
            <asp:Literal runat="server" ID="ltlErrorMessages"></asp:Literal>
            <div>
                <asp:label runat="server" AssociatedControlID="fuUploadCsv">Upload CSV file</asp:label>
                <asp:FileUpload runat="server" ID="fuUploadCsv"/>
            </div>
            <div>
                <input type="submit" value="Upload CSV File" id="btnUpload" runat="server"/>
            </div>
        </asp:panel>
        <asp:panel runat="server" id="pnlSuccess" Visible="false">
            <asp:HiddenField runat="server" ID="hdnSuccessState" />
        </asp:panel>
    </form>
    
    <script type="text/javascript">
        $(document).ready( function() {
            $('#fuUploadCsv').change(function() {
                if ($(this).val() !== '') {
                    window.parent.fireUpload();
                    $('#uploadCsvForm').submit();
                }
            });
        })
    </script>
</body>
</html>
