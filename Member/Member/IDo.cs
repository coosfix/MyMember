using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Member
{
    class IDo
    {
        //產生新密碼
        public static string GetNewPW()
        {
            string[] Code =
                { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L",
                "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d",
                "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "p", "q", "r",
                "s", "t", "u", "v", "w", "x", "y", "z" };

            string newPW = "";
            Random r = new Random();
            for (int i = 0; i < 8; i++)
                newPW += Code[r.Next(Code.Length)];

            return newPW;
        }

        //雜湊密碼
        public static byte[] HashPw(string P, string R)
        {
            string r = String.Concat(P, R);
            byte[] bytepassword = Encoding.Unicode.GetBytes(r);
            SHA256Managed Algorithm = new SHA256Managed();
            return Algorithm.ComputeHash(bytepassword);
        }
        //判斷Email格式
        public static bool IsValidEmail(string email)
        {
            if (String.IsNullOrWhiteSpace(email)) return false;
            return Regex.IsMatch(email,
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            RegexOptions.IgnoreCase);
        }




        //寄送Email
        /// <summary>
        /// 寄送目標Email , 使用者Username , 新密碼
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Username"></param>
        /// <param name="NewPW"></param>
        public static void SendEmail(string Email, string Username, string Num)
        {
            NetworkCredential login;
            MailMessage msg = new MailMessage();
            login = new NetworkCredential("stepmania002", "F129455352");//登入寄件者
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Port = 587;
            SmtpServer.EnableSsl = true;
            SmtpServer.Credentials = login;
            msg.From = new MailAddress("stepmania002@gmail.com");//寄件者
            msg.To.Add(Email);//可多個
            msg.Subject = "會員註冊確認信"; //設定信件主旨
            msg.IsBodyHtml = true;//設定信件內容為HTML格式 
            string tempmail = System.IO.File.ReadAllText("../Email/mail.html");//讀取html
            string My_Mail = GetMailBody(tempmail, Username, Num);//取得新html
            msg.Body = My_Mail; //設定信件內容 
            SmtpServer.Send(msg);//送出
        }

        //製作Email內容
        /// <summary>
        /// Email樣式 html檔 , ...., ....
        /// </summary>
        /// <param name="TempString"></param>
        /// <param name="UserName"></param>
        /// <param name="NewPW"></param>
        /// <returns></returns>
        public static string GetMailBody(string TempString, string UserName, string NewPW)
        {
            //將使用者資料填入
            TempString = TempString.Replace("{{UserName}}", UserName);
            TempString = TempString.Replace("{{MyNewPW}}", NewPW);
            //回傳最後結果
            return TempString;
        }
    }
}
