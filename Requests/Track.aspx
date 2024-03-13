<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Track.aspx.cs" Inherits="DSTM.Track" Title="Suivie" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container p-5">
        <dx:ASPxTabControl ID="ActionsTabControl" runat="server" Width="100%" TabAlign="Justify" Paddings-Padding="0">
            <Tabs>
                <dx:Tab Text="Créer une nouvelle demande" NavigateUrl="/Default.aspx" TabStyle-Font-Bold="True"></dx:Tab>
                <dx:Tab Text="Suivre le statut des demandes en cours" NavigateUrl="/Track.aspx" TabStyle-Font-Bold="True"></dx:Tab>
            </Tabs>
        </dx:ASPxTabControl>
        <dx:ASPxGridView ID="GridView" ClientInstanceName="GridView" KeyFieldName="Id" runat="server" Width="100%" OnHtmlRowPrepared="GridView_OnHtmlRowPrepared">
            <SettingsPager Visible="True" Mode="ShowPager" PageSize="15" EnableAdaptivity="true" PageSizeItemSettings-Visible="True">
                <PageSizeItemSettings Items="10, 15, 20, 50" />
            </SettingsPager>
            <%--<Columns>
                <dx:GridViewDataTextColumn FieldName="DO_Piece" Caption="N° de la Demande" />
                <dx:GridViewDataTextColumn FieldName="DO_Tiers" Caption="Tiers" />
                <dx:GridViewDataTextColumn FieldName="DO_Statut" Caption="Statut" />
                <dx:GridViewDataDateColumn FieldName="DO_Date" Caption="Date de la Demande" />
                <dx:GridViewDataTextColumn FieldName="AR_Ref" Caption="Référence" />
                <dx:GridViewDataTextColumn FieldName="DL_Design" Caption="Désignation" />
                <dx:GridViewDataDateColumn FieldName="DO_DateLivr" Caption="Date de livraison" />
            </Columns>--%>
            <SettingsFilterControl ShowOperandTypeButton="True"/>
            <SettingsExport EnableClientSideExportAPI="True" ExcelExportMode="WYSIWYG"/>
            <SettingsContextMenu Enabled="True" EnableColumnMenu="True" EnableFooterMenu="True" EnableGroupFooterMenu="True" EnableGroupPanelMenu="True" EnableRowMenu="True">
                <RowMenuItemVisibility Refresh="False" ExportMenu-Visible="True" />
            </SettingsContextMenu>
            <SettingsCustomizationDialog Enabled="True"/>
            <SettingsBehavior AllowEllipsisInText="True" SortMode="Value" EnableCustomizationWindow="True"/>
            <Settings ShowFilterRow="True" ShowFilterRowMenu="true" ShowFooter="true" ShowHeaderFilterButton="true" />
            <SettingsResizing ColumnResizeMode="NextColumn"/>
            <SettingsDataSecurity AllowDelete="False" AllowEdit="False" AllowInsert="False"/>
        </dx:ASPxGridView>
    </div>
</asp:Content>