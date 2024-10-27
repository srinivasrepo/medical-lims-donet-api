<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptViewer.aspx.cs" Inherits="MedicalLIMSApi.Web.Reports.rptViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=B03F5F7F11D50A3A" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td align="center" style="color: Red; margin-top: 800px; font-size: larger">
                    <asp:Literal ID="ltErr" runat="server"></asp:Literal>
                </td>
            </tr>
        </table>
        <rsweb:reportviewer runat="server" id="rvRequestReport">
    </rsweb:reportviewer>
    </form>
</body>
</html>
