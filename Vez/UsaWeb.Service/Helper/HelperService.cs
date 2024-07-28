using Newtonsoft.Json;
using System.Text;
using System.Net.Mail;
using System.Net;
//using SendGrid;
//using SendGrid.Helpers.Mail;
using UsaWeb.Service.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UsaWeb.Service.ViewModels;
using System.Globalization;
using System.Text.Json.Nodes;
using UsaWeb.Service.Controllers;
using UsaWeb.Service.Models;
using Amazon.Runtime.Internal.Transform;

namespace UsaWeb.Service.Helper
{
    public class HelperService
    {
        private static Environment setting = new Environment();
        private static Jwt settingJwt = new Jwt();
        private static S3 settingS3 = new S3();

        static HelperService()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            setting = configuration.GetSection("Environment").Get<Environment>();
            settingJwt = configuration.GetSection("Jwt").Get<Helper.Jwt>();
            settingS3 = configuration.GetSection("s3").Get<Helper.S3>();
        }

        public static S3 GetS3Setting()
        { 
            return settingS3;
        }

        public string GetScrambleString(string text)
        {
            Random rand = new Random();
            StringBuilder jumbleSB = new StringBuilder();
            jumbleSB.Append(text);
            int lengthSB = jumbleSB.Length;
            for (int i = 0; i < lengthSB; ++i)
            {
                int index1 = (rand.Next() % lengthSB);
                int index2 = (rand.Next() % lengthSB);

                Char temp = jumbleSB[index1];
                jumbleSB[index1] = jumbleSB[index2];
                jumbleSB[index2] = temp;

            }
            return jumbleSB.ToString();
        }

        public static string GetPatchString(string text)
        {
            var operationStrings = "";
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            foreach (var val in values)
            {
                operationStrings += "{ \"op\": \"replace\", \"path\": \"/" + val.Key + "\", \"value\": \"" + val.Value + "\" }, ";
            }

            operationStrings = "[ " + operationStrings + " ]";
            //var operationStrings = "[ { \"op\": \"replace\", \"path\": \"/text\", \"value\": \"NEW VALUE\" } ] ";


            return operationStrings;
        }

        public static async Task<bool> SendVerifyEmail(string toEmail, string toName, string code)
        {
            DBHelper.LogError("SendVerifyEmail - " + toEmail);
            string body = string.Empty;
            string path = System.IO.Directory.GetCurrentDirectory() + "\\confirm.html";
            string text = File.ReadAllText(path);
            text = text.Replace("{webAppUrl}", setting.webAppUrl);
            text = text.Replace("?id=", "?id=" + code);
            text = text.Replace("{app_Name}", setting.webApp);


            body = text;
            DBHelper.LogError("SendVerifyEmail - before sending");
            return await SendEmail(toEmail, toName, "Your " + setting.webApp + " Account", body);
        }

        public static async Task<bool> SendEmail(string toEmail, string toName, string subject,string body)
        {
            DBHelper.LogError("SendEmail");

            if (!setting.isEmailEnable) {
                DBHelper.LogError("isEmailEnable: false");
                return false;
            }

            var model = new EmailJsonApi
            { 
               fromAddress = setting.fromAddress,
               fromName = setting.fromName,
               toAddress = toEmail,
               toName = toName,
               subject = subject,
               body = body
            };
            var jsonObject = JsonConvert.SerializeObject(model);
            var url = setting.apiUrl; //<- your url here
            DBHelper.LogError("url:" + url);
            HttpClient client = new HttpClient();
            var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
            try
            {
                var res = await client.PostAsync(url, content);
                return res.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                DBHelper.LogError(ex.Message);
            }

            return false;
        }

        public static async Task<bool> SendEmail2(string toEmail, string toName, string subject, string body)
        {
            DBHelper.LogError("SendEmail2");
           
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                  {
                      { "fromAddress", setting.fromAddress },
                      { "fromName",  setting.fromName },
                      {  "toAddress" , toEmail },
                      { "toName" , toEmail },
                      { "subject" , subject },
                      { "body" , body }
                  };
                try
                {
                    DBHelper.LogError("url:" + setting.apiUrl);
                    var content = new FormUrlEncodedContent(values);
                    var response = await client.PostAsync(setting.apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                    DBHelper.LogError(responseString);
                }
                catch (Exception ex)
                {
                    DBHelper.LogError("email error:" + ex.Message);
                }
                
            }


            return false;
        }


        public static async Task<bool> SendForgottenEmail(string toEmail, string toName, string code)
        {
            DBHelper.LogError("SendForgottenEmail - " + toEmail);
            string body = string.Empty;
            body = "Reset your password\n\n";
            body = body + setting.webAppUrl + "change-password?code=" + code;
            //string path = System.IO.Directory.GetCurrentDirectory() + "\\confirm.html";
            //string text = File.ReadAllText(path);
            //text = text.Replace("{webAppUrl}", setting.webAppUrl);
            //text = text.Replace("?id=", "?id=" + code);

            //body = text;
            DBHelper.LogError("SendForgottenEmail - before sending");
            return await SendEmail(toEmail, toName, "Verify your email address", body);
        }

        public static string GetToken(string userName)
        {
            string issuer = settingJwt.Issuer;
            string audience = settingJwt.Audience;

            var key = Encoding.ASCII.GetBytes("This is a sample secret key - please don't use in production environment.'");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Email, userName),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }


        public static DateTime? ParseDate(string date)
        {
            if (DateTime.TryParse(date, out var dt))
            {
                return dt;
            }
            else
            {
                return null;
            }

        }

        public static async Task<string> GetEncryptedPassword(string password)
        {
            EncryptHelper eHlp = new EncryptHelper();
            var enc = await eHlp.EncryptAsync(password, setting.saltKey);

            string encryptedString = Convert.ToBase64String(enc);

            return encryptedString;
        }

        public static async Task<string> GetDecryptPassword(string encPass)
        {
            EncryptHelper eHlp = new EncryptHelper();

            byte[] bytes = Convert.FromBase64String(encPass);

            var pass = await eHlp.DecryptAsync(bytes, setting.saltKey);
            return pass;
        }
    }
}
