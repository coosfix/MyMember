using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Member
{
    class ClsMember
    {
        private string sqlconn = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;

        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }


        //新建使用者
        public void CreateUser()
        {
            string sqlcmd = @"Insert into Users(Username,Password,Email,RanNum) Values(@Username,@Password,@Email,@RanNum) ";

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
                    pPassword.Value = IDo.HashPw(Password, RanNum);
                    cmd.Parameters.Add(pPassword);

                    SqlParameter pEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 64);
                    pEmail.Direction = ParameterDirection.Input;
                    pEmail.Value = Email;
                    cmd.Parameters.Add(pEmail);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
            }
        }

        //驗證使用者
        public bool ValidateUser()
        {
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
                  
                    pPassword.Value = IDo.HashPw(Password, GetRanNum(Username));
                    cmd.Parameters.Add(pPassword);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                        return true;
                    else
                        return false;
                }
            }
        }

        //取得資料庫雜湊用亂碼
        public string GetRanNum(string Username)
        {
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
