using System;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Model.Helpers
{
    public static class Helper
    {
        /// <summary>
        /// вычисление хэша для паролей
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string ComputeHash(this string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            HashAlgorithm hash = new MD5CryptoServiceProvider();

            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            byte[] hashWithSaltBytes = new byte[hashBytes.Length +
                                                10240];

            for (int i = 0; i < hashBytes.Length; i++)
                hashWithSaltBytes[i] = hashBytes[i];

            for (int i = 0; i < 10240; i++)
                hashWithSaltBytes[hashBytes.Length + i] = (byte) i;

            byte[] secondHashBytes = hash.ComputeHash(hashWithSaltBytes);

            string hashValue = Convert.ToBase64String(secondHashBytes);

            return hash.ComputeHash(Encoding.UTF8.GetBytes(hashValue)).ToString();
        }

        /// <summary>
        /// отправка сообщений с кодом подтверждения
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        public static void SendMailWithConfirmCode(String email, String code)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("NewsBlogTeam@mail.ru");
            mail.Subject = "Confirm Registration";
            String body = String.Format(@"Бодрый день! Ваш электронный адрес был указан при попытке зарегистрироваться на новостном блоге.
                                        Пожалуйста введи код, присланный в сообщении, в форму на сайте. Спасибо.
                                        
                                        Код : {0}", code);

            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = body;

            SmtpClient smtp = new SmtpClient {Host = "smtp.gmail.com"};
            try
            {
                smtp.Send(mail);
            }
            catch (Exception) { return; }
        }

        /// <summary>
        /// отправка сообщений с новым паролем
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static void SendMailWithNewPassword(String email, String password)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress("NewsBlogTeam@mail.ru");
            mail.Subject = "Confirm Registration";
            String body = String.Format(@"Бодрый день! Ваш электронный адрес был указан при попытке восстановить пароль.
                                        Ваш пароль изменен, но настоятельно рекомендуем сменить его в будущем. Спасибо.
                                        
                                        Пароль : {0}", password);

            mail.BodyEncoding = Encoding.UTF8;
            mail.Body = body;

            SmtpClient smtp = new SmtpClient { Host = "smtp.gmail.com" };
            try
            {
                smtp.Send(mail);
            }
            catch (Exception) { return; }
        }
    }
}