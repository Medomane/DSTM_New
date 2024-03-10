<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="GridView.aspx.cs" Inherits="DSTM.GridViewModule" Title="GridView" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <link rel="stylesheet" type="text/css" href='<%# ResolveUrl("~/Content/GridView.css") %>' />
    <script type="text/javascript" src='<%# ResolveUrl("~/Content/GridView.js") %>'></script>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <dx:ASPxGridView ID="GridView" ClientInstanceName="GridView" runat="server" KeyFieldName="id" Width="100%">
        <ClientSideEvents Init="onControlInit"/>
        <SettingsFilterControl ShowOperandTypeButton="True"/>
        <SettingsExport EnableClientSideExportAPI="True" ExcelExportMode="WYSIWYG"/>
        <SettingsContextMenu Enabled="True" EnableColumnMenu="True" EnableFooterMenu="True" EnableGroupFooterMenu="True" EnableGroupPanelMenu="True" EnableRowMenu="True">
            <RowMenuItemVisibility Refresh="False" ExportMenu-Visible="True" />
        </SettingsContextMenu>
        <SettingsCustomizationDialog Enabled="True"/>
        <SettingsPager PageSize="50" EnableAdaptivity="true" PageSizeItemSettings-Visible="True" />
        <SettingsBehavior AllowEllipsisInText="True" SortMode="Value" EnableCustomizationWindow="True"/>
        <Settings ShowFilterRow="false" ShowFilterRowMenu="true" ShowFooter="true" VerticalScrollBarMode="Auto" ShowHeaderFilterButton="true" />
        <SettingsResizing ColumnResizeMode="NextColumn"/>
        <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False"/>
        <Styles AlternatingRow-Enabled="True"/>
    </dx:ASPxGridView>

</asp:Content>