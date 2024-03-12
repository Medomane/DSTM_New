<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Track.aspx.cs" Inherits="DSTM.Track" Title="Suivie" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container p-5">
        <dx:ASPxTabControl ID="ActionsTabControl" runat="server" Width="100%" TabAlign="Justify" Paddings-Padding="0">
            <Tabs>
                <dx:Tab Text="Créer une nouvelle demande" NavigateUrl="/Default.aspx"></dx:Tab>
                <dx:Tab Text="Suivre le statut des demandes en cours" NavigateUrl="/Track.aspx"></dx:Tab>
            </Tabs>
        </dx:ASPxTabControl>
        <dx:ASPxGridView ID="GridView" ClientInstanceName="GridView" KeyFieldName="Id" runat="server" Width="100%" >
            <SettingsPager Mode="ShowPager" Visible="True" PageSize="20" />
            <SettingsBehavior AllowEllipsisInText="True" AllowSort="True" AllowDragDrop="False"/>
            <Styles AlternatingRow-Enabled="True"/>
            <Columns>
                <dx:GridViewDataTextColumn FieldName="Id" Visible="False" />
                <dx:GridViewDataTextColumn FieldName="Num" Caption="N° de la Demande" Width="150" />
                <dx:GridViewDataDateColumn FieldName="DemandDate" Caption="Date de la Demande" Width="150" />
                <dx:GridViewDataTextColumn FieldName="DemandSubject" Caption="Objet de la demande" />
                <dx:GridViewDataDateColumn FieldName="DeliveryDate" Caption="Date de liv. souaitée" Width="150" />
                <dx:GridViewDataTextColumn FieldName="DemandStatus" Caption="Statut" Width="150" />
            </Columns>
        </dx:ASPxGridView>
    </div>
</asp:Content>