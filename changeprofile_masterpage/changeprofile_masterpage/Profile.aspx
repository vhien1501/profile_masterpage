<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="changeprofile_masterpage.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="form-horizontal">
        <div class="form-group">
            <label style="font-size: 14px" class="col-sm-4 control-label">Welcome to HQ</label>
            <asp:Label ID="lblUsers" runat="server" Font-Size="14px" CssClass="control-label col-sm-1"></asp:Label>
        </div>
    </div>

    <div class="form-horizontal">
        <div class="form-group">
            <label id="lblFirstName" class="col-sm-4 control-label">First Name</label>
            <div class="col-sm-4">
                <asp:TextBox ID="txtFirstName" ClientIDMode="Static" runat="server" CssClass="form-control input-sm"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label id="lblLastName" class="col-sm-4 control-label">Last Name</label>
            <div class="col-sm-4">
                <asp:TextBox ID="txtLastName" ClientIDMode="Static" CssClass="form-control input-sm" placeholder="Your Last Name" runat="server"> </asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label id="lblEmail" class="col-sm-4 control-label">Email</label>
            <div class="col-sm-4">
                <asp:TextBox ID="txtEmail" class="email"  ClientIDMode="Static"   CssClass="form-control input-sm" placeholder="hqgaming@abc.com" runat="server"> </asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label id="lblDOB" class="col-sm-4 control-label">Date of Birth</label>
            <div class="col-sm-4">
                <asp:TextBox ID="txtDOB" ClientIDMode="Static" CssClass="form-control" placeholder="Date of Birth" runat="server"> </asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label id="lblPhone" class="col-sm-4 control-label">Phone Number</label>
            <div class="col-sm-4">
                <asp:TextBox ID="txtPhone" ClientIDMode="Static"  CssClass="form-control input-sm" placeholder="Your Tel-number" runat="server"> </asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label id="lblGender" class="col-sm-4 control-label">Gender</label>
            <div class="col-sm-4">
                 <asp:DropDownList ID="ddlGender"  ClientIDMode="Static"  CssClass="form-control input-sm" runat="server">
                    <asp:ListItem Value="0">Gender </asp:ListItem>
                    <asp:ListItem Value="1">Male </asp:ListItem>
                    <asp:ListItem Value="2">Female </asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <label id="lblLanguage" class="col-sm-4 control-label">Language</label>
            <div class="col-sm-4">
                <asp:DropDownList ID="ddlLanguage" CssClass="form-control input-sm" runat="server">
                    <asp:ListItem Value="0">English </asp:ListItem>
                    <asp:ListItem Value="1">Korean </asp:ListItem>
                    <asp:ListItem Value="2">Vietnamese </asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-5 col-sm-4">
                <asp:Button ID="btnSave" runat="server" Text="Save" Width="150" CssClass="btn btn-info" OnClick="btnSave_Click" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-sm-4 control-label">Current Password</label>
            <div class="col-sm-4">
                <asp:TextBox type="password" class="form-control" ID="txtCurrentPassword" placeholder="input your current password" runat="server"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-sm-4 control-label">New Password</label>
            <div class="col-sm-4">
                <asp:TextBox type="password" class="form-control" ID="txtNewPassword" placeholder="input your new password" runat="server"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-sm-4 control-label">Confirm Password</label>
            <div class="col-sm-4">
                <asp:TextBox type="password" class="form-control" ID="txtConfirmPassword" placeholder="confirm your new password" runat="server"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-5 col-sm-4">
                <asp:Button ID="btnChangePassword" runat="server" CssClass="btn btn-info" Text="Change Password" OnClick="btnChangePassword_Click" />
            </div>
        </div>

    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Scripts" runat="server">
       <script type="text/javascript">
        $(function () {
            $('#txtDOB').datetimepicker({
                format: 'DD/MM/YYYY',
                showTodayButton: true,
                showClear: true,
                showClose: true,
                useCurrent: false
            });

            $.validator.addMethod('dropdown', function (value, element) {
                return (value != '0');
            }, 'Please select a option.');

            $("#MainForm").validate();
            $

            $('#txtPhone').rules('add', {
                required: true,
                number: true,
                messages: {
                    required: "Phone is required.",
                    number: "Please insert only number."
                }
            });
            $('#txtEmail').rules('add', {
                required: true,
                email: true,
                messages: {
                    required: "Email is required.",
                    email: "hqgaming@abc.com"
                }
            });

            $.validator.addMethod("regex", function (value, element, regexp) {
                var re = new RegExp(regexp);
                return this.optional(element) || re.test(value);
            }, "Only characters from A-Z");

            $('#txtFirstName').rules('add', {
                required: true,
                regex: "^[a-zA-Z]+$",
                messages: {
                    required: "First name is required."
                }
            });

            $('#txtLastName').rules('add', {
                required: true,
                regex: "^[a-zA-Z]+$",
                messages: {
                    required: "Last name is required."
                }
            });

            $('#ddlGender').rules('add', {
                required: true,
                dropdown: true,
                messages: {
                    required:"Please choose your gender."
                }
            })






        });

    </script>
</asp:Content>
