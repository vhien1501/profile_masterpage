using System;
using DIT;

namespace changeprofile_masterpage
{
    public partial class Profile : System.Web.UI.Page
    {
        private const int SALTSIZE = 64;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DITUser.LoadDB("minhphat1893");
                lblUsers.Text= DITUser.Login;
                txtFirstName.Text= DITUser.FirstName;
                txtLastName.Text = DITUser.LastName;
                txtPhone.Text = DITUser.Phone;
                txtEmail.Text = DITUser.Email ;
                if (DITUser.Gender == 1)
                {
                    ddlGender.SelectedIndex = 1;

                }
                else if (DITUser.Gender == 2)
                {
                    ddlGender.SelectedIndex = 2;
                }
                txtDOB.Text = DITUser.DOB;
                if (DITUser.Language == "0")
                {
                    ddlLanguage.SelectedIndex = 0;
                }
                else if (DITUser.Language == "1")
                {
                    ddlLanguage.SelectedIndex = 1;
                }
                else if (DITUser.Language == "2")
                {
                    ddlLanguage.SelectedIndex = 2;
                }
            }
        }
       

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (DITUser.SetData(lblUsers.Text, txtFirstName.Text, txtLastName.Text, txtPhone.Text, txtEmail.Text, Convert.ToInt32(ddlGender.Text), txtDOB.Text, ddlLanguage.Text))
            {
                DITHelper.ShowMessage(this, MessageType.Success, "Congrat!");
            }
            else
            {
                DITHelper.ShowMessage(this, MessageType.Error, "Fail To Update");
            }

        }

        private void ChangeCurrentPassword()
        {

            if (DITUser.CheckCurrentPassword(lblUsers.Text,txtCurrentPassword.Text))
            {
                if (txtNewPassword.Text == txtConfirmPassword.Text)
                {

                    if (txtConfirmPassword.Text.Length > 6)
                    {
                        string salt = DITUser.GenerateSalt(SALTSIZE);
                        DITUser.SetNewSalt(salt, lblUsers.Text);

                        string hash = DITUser.GenerateHash(txtConfirmPassword.Text, salt);
                        DITUser.SetNewPassword(hash, lblUsers.Text);

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
                    //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Confirm Password Wrong');", true);
                }
            }
            else
            {
                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Current Password Wrong');", true);
                DITHelper.ShowMessage(this, MessageType.Error, "Current Password Wrong");
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangeCurrentPassword();
        }

    }
}