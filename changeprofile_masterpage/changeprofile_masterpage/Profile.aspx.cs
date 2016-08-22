using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using DIT;

namespace changeprofile_masterpage
{
    public partial class Profile : System.Web.UI.Page
    {
        private string ConnectionString = "Server=192.168.1.19;Initial Catalog=dev;user id=user;Password=1";
        private const int SALTSIZE = 64;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadDB();
            }
        }
        public void loadDB()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            string selectQuery = "SELECT login,first_name,last_name,email,dob,mobile,gender,lang FROM users WHERE login='minhphat1893'";
            SqlCommand cmd = new SqlCommand(selectQuery, connection);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    lblUsers.Text = reader["login"].ToString();
                    txtFirstName.Value = reader["first_name"].ToString();
                    txtLastName.Value = reader["last_name"].ToString();
                    txtPhone.Value = reader["mobile"].ToString();
                    txtEmail.Value = reader["email"].ToString();
                    int numGender = Convert.ToInt32(reader["gender"]);
                    if (numGender == 1)
                    {
                        ddlGender.SelectedIndex = 1;

                    }
                    else if (numGender == 2)
                    {
                        ddlGender.SelectedIndex = 2;
                    }

                    txtDOB.Value = Convert.ToDateTime(reader["dob"]).ToString("dd/MM/yyyy");
                    string language = reader["lang"].ToString().ToLower();
                    if (language == "0")
                    {
                        ddlLanguage.SelectedIndex = 0;
                    }
                    else if (language == "1")
                    {
                        ddlLanguage.SelectedIndex = 1;
                    }
                    else if (language == "2")
                    {
                        ddlLanguage.SelectedIndex = 2;
                    }

                }
            }
            connection.Close();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();
                string updateQuery = "UPDATE users SET first_name=@first_name,last_name=@last_name,email=@email,dob=@dob,mobile=@mobile,gender=@gender,lang=@lang where login=@login";
                SqlCommand cmd = new SqlCommand(updateQuery, connection);
                string dob = Convert.ToDateTime(txtDOB.Value).ToString("yyyy/MM/dd");
                cmd.Parameters.AddWithValue("@login", lblUsers.Text);
                cmd.Parameters.AddWithValue("@first_name", txtFirstName.Value);
                cmd.Parameters.AddWithValue("@last_name", txtLastName.Value);
                cmd.Parameters.AddWithValue("@email", txtEmail.Value);
                cmd.Parameters.AddWithValue("@dob", dob);
                cmd.Parameters.AddWithValue("@mobile", txtPhone.Value);
                cmd.Parameters.AddWithValue("@gender", ddlGender.Value);
                cmd.Parameters.AddWithValue("@lang", ddlLanguage.Value);
                cmd.ExecuteNonQuery();
                connection.Close();

                DITHelper.ShowMessage(this, MessageType.Success, "Congrat!");

                //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Update Successful');", true);
            }
            catch
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Fail To Update');", true);
            }


        }

        #region Security
        public static string GenerateSalt(int length)
        {
            byte[] randomArray = new byte[length];
            string randomString;

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(randomArray);

            randomString = Convert.ToBase64String(randomArray);

            return BitConverter.ToString(randomArray).Replace("-", "").ToLower(); ;

        }
        public static string GenerateHash(string str, string salt)
        {
            byte[] plainText = Encoding.UTF8.GetBytes(str);
            byte[] s = Encoding.UTF8.GetBytes(salt);

            HashAlgorithm algorithm = new SHA512Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }

            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = s[i];
            }

            byte[] hash = algorithm.ComputeHash(plainTextWithSaltBytes);

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
        #endregion

        private void SetNewSalt(string salt, string login)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "UPDATE users SET salt = @salt WHERE login = @login";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@login", login);

                cmd.ExecuteNonQuery();
            }
        }

        private void SetNewPassword(string password, string login)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "UPDATE users SET password = @password WHERE login = @login";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@login", login);

                cmd.ExecuteNonQuery();
            }
        }

        private bool CheckCurrentPassword(string login)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT password,salt FROM users WHERE login = @login";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@login", login);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            string salt = reader["salt"].ToString();
                            string CurrentPassword = GenerateHash(txtCurrentPassword.Text, salt);

                            if (reader["password"].ToString() == CurrentPassword)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private void ChangeCurrentPassword()
        {

            if (CheckCurrentPassword(lblUsers.Text))
            {
                if (txtNewPassword.Text == txtConfirmPassword.Text)
                {

                    string salt = GenerateSalt(SALTSIZE);
                    SetNewSalt(salt, lblUsers.Text);

                    string hash = GenerateHash(txtConfirmPassword.Text, salt);
                    SetNewPassword(hash, lblUsers.Text);

                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Success');", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Confirm Password Wrong');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Current Password Wrong');", true);
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            ChangeCurrentPassword();
        }

    }
}