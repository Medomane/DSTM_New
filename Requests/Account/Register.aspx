<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Root.master" CodeBehind="Register.aspx.cs" Inherits="DSTM.Account.RegisterModule" Title="Inscription" %>

<asp:Content runat="server" ContentPlaceHolderID="Head">
    <link rel="stylesheet" type="text/css" href='/Content/SignInRegister.css'/>
    <script type="text/javascript" src='/Content/SignInRegister.js'></script>
</asp:Content>

<asp:Content ID="Content" ContentPlaceHolderID="PageContent" runat="server">
    <div class="formLayout-verticalAlign">
        <div class="formLayout-container">
            <dx:ASPxTabControl ID="SignInRegisterTabControl" runat="server" Width="100%" TabAlign="Justify" Paddings-Padding="0">
                <Tabs>
                    <dx:Tab Text="Se connecter" NavigateUrl="SignIn.aspx"></dx:Tab>
                    <%--<dx:Tab Text="S'inscrire" NavigateUrl="Register.aspx"></dx:Tab>--%>
                </Tabs>
            </dx:ASPxTabControl>

            <dx:ASPxFormLayout runat="server" ID="FormLayout" ClientInstanceName="formLayout" CssClass="formLayout" UseDefaultPaddings="false">
                <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" />
                <SettingsItemCaptions Location="Top" />
                <Styles LayoutGroup-Cell-Paddings-Padding="0" LayoutItem-Paddings-PaddingBottom="8" />
                <Items>
                    <dx:LayoutGroup ShowCaption="False" GroupBoxDecoration="None" Paddings-Padding="16">
                        <Items>
                            <dx:LayoutItem Caption="Prénom">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="FirstNameTextBox" runat="server" Width="100%">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Prénom est obligatoire" />
                                            </ValidationSettings>
                                            <ClientSideEvents Init="function(s, e){ s.Focus(); }" />
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Nom">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="LastNameTextBox" runat="server" Width="100%">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Nom est obligatoire" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Email">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxTextBox ID="EmailTextBox" runat="server" Width="100%">
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RegularExpression ErrorText="Invalid e-mail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" />
                                                <RequiredField IsRequired="true" ErrorText="Email est obligatoire" />
                                            </ValidationSettings>
                                        </dx:ASPxTextBox>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Password">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButtonEdit ID="PasswordButtonEdit" runat="server" ClientInstanceName="passwordButtonEdit" Width="100%" Password="true" ClearButton-DisplayMode="Never">
                                            <ButtonStyle Border-BorderWidth="0" Width="6" CssClass="eye-button" HoverStyle-BackColor="Transparent" PressedStyle-BackColor="Transparent">
                                            </ButtonStyle>
                                            <ButtonTemplate>
                                                <div></div>
                                            </ButtonTemplate>
                                            <Buttons>
                                                <dx:EditButton>
                                                </dx:EditButton>
                                            </Buttons>
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Mot de passe est obligatoire" />
                                            </ValidationSettings>
                                            <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" Validation="onPasswordValidation" />
                                        </dx:ASPxButtonEdit>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>

                            <dx:LayoutItem Caption="Confirm password">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButtonEdit ID="ConfirmPasswordButtonEdit" runat="server" ClientInstanceName="confirmPasswordButtonEdit" Width="100%" Password="true" ClearButton-DisplayMode="Never">
                                            <ButtonStyle Border-BorderWidth="0" Width="6" CssClass="eye-button" HoverStyle-BackColor="Transparent" PressedStyle-BackColor="Transparent">
                                            </ButtonStyle>
                                            <ButtonTemplate>
                                                <div></div>
                                            </ButtonTemplate>
                                            <Buttons>
                                                <dx:EditButton>
                                                </dx:EditButton>
                                            </Buttons>
                                            <ValidationSettings Display="Dynamic" SetFocusOnError="true" ErrorTextPosition="Bottom" ErrorDisplayMode="ImageWithText">
                                                <RequiredField IsRequired="true" ErrorText="Confirmer le mot de passe" />
                                            </ValidationSettings>
                                            <ClientSideEvents ButtonClick="onPasswordButtonEditButtonClick" Validation="onPasswordValidation" />
                                        </dx:ASPxButtonEdit>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                            <dx:LayoutItem ShowCaption="False" Name="GeneralError">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <div id="GeneralErrorDiv" runat="server" class="formLayout-generalErrorText"></div>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                    <dx:LayoutGroup GroupBoxDecoration="HeadingLine" ShowCaption="False">
                        <Paddings PaddingTop="13" PaddingBottom="13" />
                        <GroupBoxStyle CssClass="formLayout-groupBox" />
                        <Items>
                            <dx:LayoutItem ShowCaption="False" HorizontalAlign="Center" Paddings-Padding="0">
                                <LayoutItemNestedControlCollection>
                                    <dx:LayoutItemNestedControlContainer>
                                        <dx:ASPxButton ID="RegisterButton" runat="server" Width="200" Text="Inscription" OnClick="RegisterButton_Click"></dx:ASPxButton>
                                    </dx:LayoutItemNestedControlContainer>
                                </LayoutItemNestedControlCollection>
                            </dx:LayoutItem>
                        </Items>
                    </dx:LayoutGroup>
                </Items>
            </dx:ASPxFormLayout>
        </div>
    </div>

</asp:Content>