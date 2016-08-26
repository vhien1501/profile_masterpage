using System;
using DIT;
using System.Text.RegularExpressions;

namespace changeprofile_masterpage
{
    public partial class Profile : System.Web.UI.Page
    {
        private const int SALTSIZE = 64;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DITUsers.LoadDB("minhphat1893");
                lblUsers.Text= DITUsers.Login;
                txtFirstName.Text= DITUsers.FirstName;
                txtLastName.Text = DITUsers.LastName;
                txtPhone.Text = DITUsers.Phone;
                txtEmail.Text = DITUsers.Email ;
                if (DITUsers.Gender == 1)
                {
                    ddlGender.SelectedIndex = 1;

                }
                else if (DITUsers.Gender == 2)
                {
                    ddlGender.SelectedIndex = 2;
                }
                txtDOB.Text = DITUsers.DOB;
                if (DITUsers.Language == "0")
                {
                    ddlLanguage.SelectedIndex = 0;
                }
                else if (DITUsers.Language == "1")
                {
                    ddlLanguage.SelectedIndex = 1;
                }
                else if (DITUsers.Language == "2")
                {
                    ddlLanguage.SelectedIndex = 2;
                }
            }
        }
       

        protected void btnSave_Click(object sender, EventArgs e)
        {

            if (DITUsers.SetData(lblUsers.Text, txtFirstName.Text, txtLastName.Text, txtPhone.Text, txtEmail.Text, Convert.ToInt32(ddlGender.Text), txtDOB.Text, ddlLanguage.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Success, "Update successfull!");
            }
            else
            {
                if (DITUsers.CheckEmail(txtEmail.Text, lblUsers.Text))
                {
                    DITHelper.ShowMessage(this, MessageType.Error, txtEmail.Text + " was used.");
                }
                else
                {
                    DITHelper.ShowMessage(this, MessageType.Error, "Fail To Update");
                }
            }
        }


        private void ChangeCurrentPassword()
        {

            if (DITUsers.CheckCurrentPassword(lblUsers.Text,txtCurrentPassword.Text))
            {
                if (txtNewPassword.Text == txtConfirmPassword.Text)
                {

                    if (txtConfirmPassword.Text.Length > 6)
                    {
                        string salt = DITUsers.GenerateSalt(SALTSIZE);
                        DITUsers.SetNewSalt(salt, lblUsers.Text);

                        string hash = DITUsers.GenerateHash(txtConfirmPassword.Text, salt);
                        DITUsers.SetNewPassword(hash, lblUsers.Text);

                        DITHelper.ShowMessage(this, MessageType.Error, "Success");
                    }
                    else
                    {
                        DITHelper.ShowMessage(this, MessageType.Error, "at least 6 letters");
                    }
                }
                else
                {
                    DITHelper.ShowMessage(this, MessageType.Error, "Confirm Password Wrong");
                    
                }
            }
            else
            {
               
                DITHelper.ShowMessage(this, MessageType.Error, "Current Password Wrong");
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangeCurrentPassword();
        }

    }
}