<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Default.aspx.cs" Inherits="DSTM.Default" Title="Accueil" %>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <div class="container p-5">
        <dx:ASPxTabControl ID="ActionsTabControl" runat="server" Width="100%" TabAlign="Justify" Paddings-Padding="0">
            <Tabs>
                <dx:Tab Text="Créer une nouvelle demande" NavigateUrl="/Default.aspx"></dx:Tab>
                <dx:Tab Text="Suivre le statut des demandes en cours" NavigateUrl="/Track.aspx"></dx:Tab>
            </Tabs>
        </dx:ASPxTabControl>

        <dx:ASPxFormLayout runat="server" ID="FormLayout" ClientInstanceName="FormLayout">
            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" />
            <Items>
                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16" ColumnCount="3">
                    <Items>
                        <dx:LayoutItem ShowCaption="False" RequiredMarkDisplayMode="Hidden">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxTextBox ID="NumField" ClientInstanceName="NumField" runat="server" Width="100%" Caption="N° de la demande" Font-Bold="True">
                                        <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Left" ErrorDisplayMode="ImageWithTooltip">
                                            <RequiredField IsRequired="true" ErrorText="N° de la demande est obligatoire" />
                                        </ValidationSettings>
                                        <ClientSideEvents Init="function(s, e){ s.Focus(); }" />
                                    </dx:ASPxTextBox>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" RequiredMarkDisplayMode="Hidden">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxDateEdit ID="DateDemandField" ClientInstanceName="DateDemandField" runat="server" Width="100%" Caption="Date de la demande" Font-Bold="True">
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
                        <dx:LayoutItem ShowCaption="False" RequiredMarkDisplayMode="Hidden">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxGridView ID="GridView" ClientInstanceName="GridView" KeyFieldName="Id" runat="server" Width="100%" 
                                                     OnRowInserting="GridView_OnRowInserting" OnRowUpdating="GridView_OnRowUpdating" OnRowDeleting="GridView_OnRowDeleting">
                                        <SettingsPager Mode="ShowAllRecords"/>
                                        <SettingsBehavior AllowEllipsisInText="True" AllowSort="False" AllowDragDrop="False"/>
                                        <Styles AlternatingRow-Enabled="True"/>
                                        <SettingsEditing Mode="Inline"></SettingsEditing>
                                        <SettingsCommandButton>
                                            <NewButton ButtonType="Image" Image-Width="16px" Image-Height="16px" Image-Url="/Content/Images/add.svg"/>
                                            <EditButton ButtonType="Image" Image-Width="16px" Image-Height="16px" Image-Url="/Content/Images/edit.svg"/>
                                            <DeleteButton ButtonType="Image" Image-Width="16px" Image-Height="16px" Image-Url="/Content/Images/delete.svg"/>
                                            <UpdateButton ButtonType="Image" Image-Width="16px" Image-Height="16px" Image-Url="/Content/Images/check.svg"/>
                                            <CancelButton ButtonType="Image" Image-Width="16px" Image-Height="16px" Image-Url="/Content/Images/close.svg"/>
                                        </SettingsCommandButton>
                                        <Columns>
                                            <dx:GridViewCommandColumn ShowEditButton="true" ShowDeleteButton="True" Width="100" ShowNewButtonInHeader="True" />
                                            <dx:GridViewDataTextColumn FieldName="Id" Visible="False" />
                                            <dx:GridViewDataTextColumn FieldName="Subject" Caption="Objet de la demande" />
                                            <dx:GridViewDataDateColumn FieldName="DeliveryDate" Caption="Date de liv. souaitée" Width="150" />
                                        </Columns>
                                    </dx:ASPxGridView>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                

                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16" ColumnCount="3">
                    <Items>
                        <dx:LayoutItem Caption="Commentaire" CaptionStyle-Font-Bold="True" CaptionSettings-Location="Top" ColumnSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxMemo ID="CommentField" runat="server" Width="100%" Rows="5"/>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        
                        <dx:LayoutItem Caption="Pièces jointes" CaptionStyle-Font-Bold="True" CaptionSettings-Location="Top">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxUploadControl ID="UploadControl" runat="server" ClientInstanceName="UploadControl" Width="100%"
                                                          NullText="Sélectionner multiple fichier..." UploadMode="Advanced" ShowUploadButton="True"
                                                          OnFilesUploadComplete="UploadControl_OnFilesUploadComplete">
                                        <AdvancedModeSettings EnableMultiSelect="True" EnableFileList="True"  EnableDragAndDrop="True" />
                                    </dx:ASPxUploadControl>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
                

                <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16" ColumnCount="3">
                    <Items>
                        <dx:LayoutItem ShowCaption="False" ColumnSpan="2">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxLabel runat="server" Text="* Veuillez télécharger le pièces jointes avant de valider *"/>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                        <dx:LayoutItem ShowCaption="False" HorizontalAlign="Right">
                            <LayoutItemNestedControlCollection>
                                <dx:LayoutItemNestedControlContainer>
                                    <dx:ASPxButton ID="ValidateButton" runat="server" Width="200" Text="Valider" OnClick="ValidateButton_OnClick">
                                    </dx:ASPxButton>
                                </dx:LayoutItemNestedControlContainer>
                            </LayoutItemNestedControlCollection>
                        </dx:LayoutItem>
                    </Items>
                </dx:LayoutGroup>
            </Items>
        </dx:ASPxFormLayout>
    </div>
</asp:Content>