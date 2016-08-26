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
            UpdateData();
            if (!IsPostBack)
            {
                
                DITUsers.GetUsersByLogin("minhphat1893");
                lblUsers.Text= DITUsers.Login;
                txtFirstName.Text= DITUsers.Firstname;
                txtLastName.Text = DITUsers.Lastname;
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
                txtDOB.Text = DITUsers.Dob;
                if (DITUsers.Language == "en")
                {
                    ddlLanguage.SelectedIndex = 0;
                }
                else if (DITUsers.Language == "ko")
                {
                    ddlLanguage.SelectedIndex = 1;
                }
                else if (DITUsers.Language == "vi")
                {
                    ddlLanguage.SelectedIndex = 2;
                }
            }
        }
       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (DITHelper.isEmptyString(txtFirstName.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please insert your first name.");
                return;
            }

            if (DITHelper.isEmptyString(txtLastName.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please insert your last name.");
                return;
            }

            if (DITHelper.isEmptyString(txtEmail.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please insert your email.");
                return;
            }

            if (DITHelper.isEmptyString(txtPhone.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please insert your phone.");
                return;
            }

            if (ddlGender.SelectedIndex==0)
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please choose your gender.");
                return;
            }


            if (DITUsers.SetData(DITUsers.Login, DITUsers.Firstname, DITUsers.Lastname, DITUsers.Phone, DITUsers.Email, DITUsers.Gender, DITUsers.Dob, DITUsers.Language))
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
            if (DITHelper.isEmptyString(txtCurrentPassword.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please insert your current password.");
                return;
            }

            if (DITHelper.isEmptyString(txtNewPassword.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please insert your new password.");
                return;
            }

            if (DITHelper.isEmptyString(txtConfirmPassword.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Please insert your confirm password.");
                return;
            }

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

        private void UpdateData()
        {
            DITUsers.Login = lblUsers.Text;
            DITUsers.Firstname = txtFirstName.Text;
            DITUsers.Lastname = txtLastName.Text;
            DITUsers.Email = txtEmail.Text;
            DITUsers.Dob = txtDOB.Text;
            DITUsers.Phone = txtPhone.Text;
            DITUsers.Gender = Convert.ToInt32(ddlGender.Text);
            DITUsers.Language = ddlLanguage.Text;
        }

    }
}