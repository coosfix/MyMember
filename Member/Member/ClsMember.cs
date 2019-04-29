using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member
{
    class ClsMember
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public void CreateUser()
        {
            string sqlconn = @"Data Source =(localdb)\MSSQLLocalDB;Initial Catalog =Northwind;Integrated Security = true;";
            string sqlcmd = @"Insert into Users(Username,Password,RanNum) Values(@Username,@Password,@RanNum) ";

            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    SqlParameter pUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUsername.Direction = ParameterDirection.Input;
                    pUsername.Value = Username;
                    cmd.Parameters.Add(pUsername);


                    string RanNum = Guid.NewGuid().ToString("N");

                    SqlParameter pRannum = new SqlParameter("@RanNum", SqlDbType.NVarChar, 32);
                    pRannum.Direction = ParameterDirection.Input;
                    pRannum.Value = RanNum;
                    cmd.Parameters.Add(pRannum);


                    SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarBinary, 32);
                    pPassword.Direction = ParameterDirection.Input;
                    string addranPassword = String.Concat(Password, RanNum);
                    byte[] bytePassword = Encoding.Unicode.GetBytes(addranPassword);
                    SHA256Managed Algorithm = new SHA256Managed();
                    pPassword.Value = Algorithm.ComputeHash(bytePassword);
                    cmd.Parameters.Add(pPassword);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void ValidateUser()
        {
            string sqlconn = @"Data Source =(localdb)\MSSQLLocalDB;Initial Catalog =Northwind;Integrated Security = true;";
            string sqlcmd = @"SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlParameter pUser = new SqlParameter("@Username", SqlDbType.NVarChar, 16);

                    pUser = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUser.Direction = ParameterDirection.Input;
                    pUser.Value = Username;
                    cmd.Parameters.Add(pUser);

                    SqlParameter pPassword = new SqlParameter("@Password", SqlDbType.VarBinary, 32);
                    pPassword.Direction = ParameterDirection.Input;
                    string r = String.Concat(Password, GetRanNum(Username));
                    byte[] bytepassword = Encoding.Unicode.GetBytes(r);
                    SHA256Managed Algorithm = new SHA256Managed();
                    pPassword.Value = Algorithm.ComputeHash(bytepassword);
                    cmd.Parameters.Add(pPassword);
                    conn.Open();
                   SqlDataReader dr =  cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        FrmMain go = new FrmMain(Username);
                        go.Show();
                    }
                    else
                        MessageBox.Show("帳號錯誤");
                }
            }
        }
        public void PasswordChange()
        {
            //😂
        }

        public string GetRanNum(string Username)
        {
            string sqlconn = @"Data Source =(localdb)\MSSQLLocalDB;Initial Catalog =Northwind;Integrated Security = true;";
            string sqlcmd = @"SELECT @RanNum = RanNum FROM Users WHERE Username = @Username";
            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlParameter pUser = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUser.Direction = ParameterDirection.Input;
                    pUser.Value = Username;
                    cmd.Parameters.Add(pUser);

                    SqlParameter getRannum = new SqlParameter("@RanNum", SqlDbType.NVarChar, 32);
                    getRannum.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(getRannum);


                    conn.Open();//拿亂數       
                    cmd.ExecuteScalar();
                    string getranNum = getRannum.Value.ToString();

                    return getranNum;
                }
            }
        }
    }
}
