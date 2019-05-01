using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Member
{
    class PasswordChangeFun
    {
        private static string sqlconn = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;

        //寄送新密碼給使用者
        public static bool CheckUser(string Username, string Email)
        {
            string sqlcmd = @"SELECT * FROM Users WHERE Username = @Username AND Email = @Email";
            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlParameter pUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUsername.Direction = ParameterDirection.Input;
                    pUsername.Value = Username;
                    cmd.Parameters.Add(pUsername);


                    SqlParameter pEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 64);
                    pEmail.Direction = ParameterDirection.Input;
                    pEmail.Value = Email;
                    cmd.Parameters.Add(pEmail);

                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        string NP = IDo.GetNewPW();//創造新密碼
                        UpdatePW(Username, Email, NP);//更新密碼
                        IDo.SendEmail(Email, Username, NP);//寄email
                        return true;
                    }
                    else
                        return false;

                }
            }

        }

        //更新密碼跟雜湊用亂碼
        /// <summary>
        /// Username為註冊的帳號 ,   Email為註冊的Email,     np 新密碼
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Email"></param>
        /// <param name="np"></param>
        public static void UpdatePW(string Username, string Email, string np)
        {
            string sqlcmd = @"UPDATE Users SET Password=@NewPW,RanNum=@NewRanNum WHERE Username = @Username AND Email = @Email";
            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {
                    cmd.CommandType = CommandType.Text;


                    SqlParameter pUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUsername.Direction = ParameterDirection.Input;
                    pUsername.Value = Username;
                    cmd.Parameters.Add(pUsername);

                    SqlParameter pEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 64);
                    pEmail.Direction = ParameterDirection.Input;
                    pEmail.Value = Email;
                    cmd.Parameters.Add(pEmail);

                    string RanNum = Guid.NewGuid().ToString("N");
                    SqlParameter pRannum = new SqlParameter("@NewRanNum", SqlDbType.NVarChar, 32);
                    pRannum.Direction = ParameterDirection.Input;
                    pRannum.Value = RanNum;
                    cmd.Parameters.Add(pRannum);

                    SqlParameter pNewPW = new SqlParameter("@NewPW", SqlDbType.VarBinary, 32);
                    pNewPW.Direction = ParameterDirection.Input;
                    pNewPW.Value = IDo.HashPw(np, RanNum);//新雜湊
                    cmd.Parameters.Add(pNewPW);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //使用者更改密碼 清除資料庫驗證碼
        public static bool UpdateUser(string Username,string nPassword)
        {
            string sqlcmd = @"UPDATE Users SET Password=@NewPW,RanNum=@NewRanNum,AuthCode=@AuthCode WHERE Username = @Username";
            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlParameter pUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUsername.Direction = ParameterDirection.Input;
                    pUsername.Value = Username;
                    cmd.Parameters.Add(pUsername);


                    SqlParameter pAuthCode = new SqlParameter("@AuthCode", SqlDbType.NVarChar, 8);
                    pAuthCode.Direction = ParameterDirection.Input;
                    pAuthCode.Value = String.Empty;
                    cmd.Parameters.Add(pAuthCode);

                    string RanNum = Guid.NewGuid().ToString("N");

                    SqlParameter pRannum = new SqlParameter("@NewRanNum", SqlDbType.NVarChar, 32);
                    pRannum.Direction = ParameterDirection.Input;
                    pRannum.Value = RanNum;
                    cmd.Parameters.Add(pRannum);

                    SqlParameter pNewPW = new SqlParameter("@NewPW", SqlDbType.VarBinary, 32);
                    pNewPW.Direction = ParameterDirection.Input;
                    pNewPW.Value = IDo.HashPw(nPassword, RanNum);//新雜湊
                    cmd.Parameters.Add(pNewPW);

                    conn.Open();
                    int n = (int)cmd.ExecuteNonQuery();
                    if (n != 0) return true;
                    else return false;
                }
            }
        }

        //寄送驗證碼
        public static void SendAuthCode(string Username)
        {
            string authcode = IDo.GetNewPW();
            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                string upacode = @"UPDATE Users SET AuthCode=@AuthCode WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(upacode, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlParameter pUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUsername.Direction = ParameterDirection.Input;
                    pUsername.Value = Username;
                    cmd.Parameters.Add(pUsername);

                    SqlParameter pAuthCode = new SqlParameter("@AuthCode", SqlDbType.NVarChar, 8);
                    pAuthCode.Direction = ParameterDirection.Input;
                    pAuthCode.Value = authcode;
                    cmd.Parameters.Add(pAuthCode);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                string sqlcmd = @"SELECT @Email = Email FROM Users WHERE Username = @Username";
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {
                    cmd.CommandType = CommandType.Text;

                    SqlParameter pUser = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUser.Direction = ParameterDirection.Input;
                    pUser.Value = Username;
                    cmd.Parameters.Add(pUser);

                    SqlParameter getEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 64);
                    getEmail.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(getEmail);          
                    cmd.ExecuteScalar();
                    string myEmail = getEmail.Value.ToString();
                    IDo.SendEmail(myEmail, Username, authcode);
                }

            }


        }
        //檢查驗證碼
        public static bool CheckAuthCode(string Username,string AuthCode)
        {
            string sqlcmd = @"SELECT * FROM Users WHERE Username = @Username AND AuthCode = @AuthCode";

            using (SqlConnection conn = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(sqlcmd, conn))
                {

                    SqlParameter pUsername = new SqlParameter("@Username", SqlDbType.NVarChar, 16);
                    pUsername.Direction = ParameterDirection.Input;
                    pUsername.Value = Username;
                    cmd.Parameters.Add(pUsername);

                    SqlParameter pAuthCode = new SqlParameter("@AuthCode", SqlDbType.NVarChar, 8);
                    pAuthCode.Direction = ParameterDirection.Input;
                    pAuthCode.Value = AuthCode;
                    cmd.Parameters.Add(pAuthCode);
                    conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                        return true;
                    else
                        return false;
                }
            }
        }




    }
}
