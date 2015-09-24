<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="imageUpload.aspx.cs" Inherits="RM.MailshotsOnline.Web.imageUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="/css/temp.css" type="text/css" rel="stylesheet"/>
    <script type="text/javascript" src="/canvas/js/vendor/jquery/dist/jquery.min.js"></script>
</head>
<body>
    <div id="loginError" runat="server" visible="false">
        <p>You must be logged in to save an image.</p>
    </div>
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div id="formArea" runat="server">
            <p id="errorMessageParagraph" runat="server"></p>
            <div>
                <label for="<%= nameInput.ClientID %>">Image name</label>
                <input type="text" id="nameInput" runat="server" />
            </div>
            <div>
                <label for="<%= fileUpload.ClientID %>">File</label>
                <input type="file" id="fileUpload" runat="server" />
            </div>
            <div>
                <input type="submit" value="Save image" id="saveButton" runat="server" />
            </div>
        </div>
        <div id="success" runat="server" visible="false">
            <input type="hidden" id="imageResult" runat="server" />
            <input type="hidden" id="imageResultSmall" runat="server" />
        </div>
    </form>
    
    <script type="text/javascript">
        $(document).ready( function() {
            $('#fileUpload').change(function() {
                if ($(this).val() !== '') {
                    window.parent.fireUpload();
                    $('#form1').submit();
                }
            })
        })
    </script>
</body>
</html>
