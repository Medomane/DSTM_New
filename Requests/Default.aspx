<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Default.aspx.cs" Inherits="DSTM.Default" Title="Accueil" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container p-5">
        <dx:ASPxTabControl ID="ActionsTabControl" runat="server" Width="100%" TabAlign="Justify" Paddings-Padding="0">
            <Tabs>
                <dx:Tab Text="Créer une nouvelle demande" NavigateUrl="/Default.aspx"></dx:Tab>
                <dx:Tab Text="Suivre le statut des demandes en cours" NavigateUrl="/Track.aspx"></dx:Tab>
            </Tabs>
        </dx:ASPxTabControl>

        <dx:ASPxFormLayout runat="server" ID="FormLayout" ClientInstanceName="formLayout" CssClass="formLayout" UseDefaultPaddings="false" AlignItemCaptionsInAllGroups="True">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" />
            <SettingsItemCaptions Location="Top" />
            <Styles LayoutGroup-Cell-Paddings-Padding="0" LayoutItem-Paddings-PaddingBottom="8" />
            <Items>
                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16" ColumnCount="3">
                    <Items>
                        <dx:LayoutItem ShowCaption="False" RequiredMarkDisplayMode="Hidden" Paddings-Padding="10">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="NumField" runat="server" Width="100%" Caption="N° de la demande">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Left" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="N° de la demande est obligatoire" />
                                        </ValidationSettings>
                                        <ClientSideEvents Init="function(s, e){ s.Focus(); }" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" RequiredMarkDisplayMode="Hidden" Paddings-Padding="10">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxDateEdit ID="DateDemandField" runat="server" Width="100%" Caption="Date de la demande">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Left" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="Date de la demande est obligatoire" />
                                        </ValidationSettings>
                                    </dx:ASPxDateEdit>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                
                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16">
                    <Items>
                        <dx:LayoutItem ShowCaption="False" RequiredMarkDisplayMode="Hidden" Paddings-Padding="10">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxGridView ID="GridView" ClientInstanceName="GridView" runat="server" Width="100%" OnRowInserting="GridView_OnRowInserting">
                                        <SettingsPager Mode="ShowAllRecords"/>
                                        <SettingsBehavior AllowEllipsisInText="True" SortMode="Value" EnableCustomizationWindow="True"/>
                                        <SettingsDataSecurity AllowDelete="True" AllowEdit="True" AllowInsert="True"/>
                                        <Styles AlternatingRow-Enabled="True"/>
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowEditButton="true" ShowDeleteButton="True" ShowNewButton="True" Width="100" />
                                            <dx:GridViewDataTextColumn FieldName="Subject" Caption="Objet de la demande" />
                                            <dx:GridViewDataDateColumn FieldName="DeliveryDate" Caption="Date de liv. souaitée" Width="150" />
                                        </Columns>
                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                

                <dx:LayoutGroup GroupBoxDecoration="HeadingLine" ShowCaption="False">
                    <Paddings PaddingTop="13" PaddingBottom="13" />
                    <GroupBoxStyle CssClass="formLayout-groupBox" />
                    <Items>
                        <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right" Paddings-Padding="0">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxButton ID="ValidateButton" runat="server" Width="200" Text="Valider"></dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
    </div>
</asp:Content>