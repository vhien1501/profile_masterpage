using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DIT;

namespace DIT
{
    public class DITUsers
    {
        private static string ConnectionString = "Server=192.168.1.19;Initial Catalog=dev;user id=user;Password=1";
        public static string Login;
        public static string FirstName;
        public static string LastName;
        public static string Phone;
        public static string Email;
        public static int Gender;
        public static string DOB;
        public static string Language;

        public static void LoadDB(string username)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            string selectQuery = "SELECT login,first_name,last_name,email,dob,mobile,gender,lang FROM users WHERE login=@username";
            SqlCommand cmd = new SqlCommand(selectQuery, connection);
            cmd.Parameters.AddWithValue("@username", username);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Login = reader["login"].ToString();
                    FirstName = reader["first_name"].ToString();
                    LastName = reader["last_name"].ToString();
                    Phone = reader["mobile"].ToString();
                    Email = reader["email"].ToString();

                    if (!reader.IsDBNull(reader.GetOrdinal("gender")))
                        Gender = Convert.ToInt32(reader["gender"]);
                    if (!reader.IsDBNull(reader.GetOrdinal("dob")))
                        DOB = Convert.ToDateTime(reader["dob"]).ToString("dd/MM/yyyy");

                    Language = reader["lang"].ToString().ToLower();
            
                }
            }
            connection.Close();
        }

        public static bool SetData(string login, string firstname, string lastname, string phone, string email, int gender, string dob, string language) {
            try
            {
                SqlConnection connection = new SqlConnection(ConnectionString);
                connection.Open();

                string updateQuery = "UPDATE users SET first_name=@first_name, last_name=@last_name, email=@email, dob=@dob, mobile=@mobile, gender=@gender, lang=@lang where login=@login";

                SqlCommand cmd = new SqlCommand(updateQuery, connection);


                if (login == "")
                {
                    cmd.Parameters.AddWithValue("@login", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@login", login);
                }
              

                if (firstname == "")
                {
                    cmd.Parameters.AddWithValue("@first_name", DBNull.Value);
                }
                
                else if (Regex.Match(firstname, @"^([a-zA-Z]{1,50})$").Success)
                {
                    cmd.Parameters.AddWithValue("@first_name", firstname);
                }
                

                if (lastname == "")
                {
                    cmd.Parameters.AddWithValue("@last_name", DBNull.Value);
                }
                else if (Regex.Match(firstname, @"^([a-zA-Z]{1,50})$").Success)
                {
                    cmd.Parameters.AddWithValue("@last_name", lastname);
                }

                if (email == "")
                {
                    cmd.Parameters.AddWithValue("@email", DBNull.Value);
                }
                else 
                {
                    if (CheckEmail(email, login))
                    {
                    }

                    else if (Regex.Match(email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$").Success)
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                    }
                }
                if (dob == "")
                {
                    cmd.Parameters.AddWithValue("@dob", DBNull.Value);
                }
                else 
                {
                    DateTime DOB = Convert.ToDateTime(dob);
                    cmd.Parameters.AddWithValue("@dob", dob);
                }

                if (phone == "")
                {
                    cmd.Parameters.AddWithValue("@mobile", DBNull.Value);
                }
                else if(Regex.Match(phone, @"^([0-9]{9,15})$").Success)
                {
                    cmd.Parameters.AddWithValue("@mobile", phone);
                }

                if (gender == 0)
                {
                    cmd.Parameters.AddWithValue("@gender", DBNull.Value);
                }
                else if (gender!=0)
                {
                    cmd.Parameters.AddWithValue("@gender", gender);
                }

                if (language == "")
                {
                    cmd.Parameters.AddWithValue("@lang", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@lang", language);
                }
                //cmd.Parameters.AddWithValue("@first_name", firstname);
                //cmd.Parameters.AddWithValue("@last_name", lastname);
                //cmd.Parameters.AddWithValue("@email", email);
                //cmd.Parameters.AddWithValue("@dob", DOB);
                //cmd.Parameters.AddWithValue("@mobile", phone);
                //cmd.Parameters.AddWithValue("@gender", gender);
                //cmd.Parameters.AddWithValue("@lang", language);

                cmd.ExecuteNonQuery();
                connection.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void SetNewSalt(string salt, string login)
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

        public static void SetNewPassword(string password, string login)
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

        public static bool CheckEmail(string myEmail, string userLogin)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT email FROM users WHERE login NOT IN (SELECT login FROM users WHERE login=@login) AND email=@email";
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@login", userLogin);
                cmd.Parameters.AddWithValue("@email", myEmail);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();                      
                        if (!DITHelper.isEmptyString(reader["email"].ToString()))
                        {
                            return true; // not empty = have email value.
                        }                    
                    }                 
                }            
            }
            return false;//empty= dont have email value.
        }
              
               
        public static bool CheckCurrentPassword(string login, string currentpassword)
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
                            string CurrentPassword = GenerateHash(currentpassword, salt);

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
    }
}