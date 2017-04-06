<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Snmp_Web_Client._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <h3 align="center">SNMP CLIENT SEARCH</h3>
        <div class="row" style="margin-bottom: 10px" align="center">
            <label>Enter search value:</label>
            <asp:TextBox ID="txtSearch" runat="server" Width="460px"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="Search" Width="158px" OnClick="btnSearch_Click" />
        </div>

        <div>
            <div align="center">
                <asp:GridView ID="dgLog" runat="server">
                </asp:GridView>
            </div>

        </div>
    </div>




</asp:Content>
