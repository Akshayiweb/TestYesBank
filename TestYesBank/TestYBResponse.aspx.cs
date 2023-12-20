using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls; 
using System.Web.Script.Serialization;
using System.Net; 
using System.Data;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace TestYesBank
{
    public partial class TestYBResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string secretKey = ConfigurationManager.AppSettings["Secret Key"].ToString();

                SHA256 mySHA256 = SHA256Managed.Create();
                byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(secretKey));

                // Create secret IV
                byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

                //string res = "n8JJ4BIwkdfkpkBi3AQVzHPHVjvAH5HLqGtCePNUa%2Bp9jwzExR30NyevkkJ5xfcW2zJ%2F%2BmmhUXjZpCCCSPwoOOZO8PI6Oj1Dv1RZVfcYbUb2mdkN6SNDTsVgcikw3A%2FNuWSB3VAFpny1mYLrDA8uGGYlDjH1g1rSvAWbcgXVIE9%2FIHCPfMnULp0gpv9zCGWFhqe%2B1e9KnA0n6r%2B3rqxuU92IIFoMdb%2BH7U62USQhcZd09ak7oiHkObtRJZemYDRPt4WsRWJLfQjp%2FHW68QEaN%2FNA2CqrRT0%2FGhuR2%2FZw5o8VccZPSxfEGcYIIrMe7lJR3eZNUjbGl%2BTyMl2mzqwSYxS%2BUj3Nu%2BSZzIw7hOzB7GtlJO9vZqAc8jlahlvY4CT0avD6qRbf%2BtjSveWW%2FHmFCfoshZGCiF%2B%2FGJpURJIv85uymw64SKnPLuUeQ9YFFolwMoNJMBAPkUYGeanC0c1bMCeiZ161BDkkucXt7paf8KTaqlRksXVhlZ0Ey59gc4LSHwNK4TX1ZX19bjtkVh%2FdD7rSQUA5IEUmFebnjtNAe7ozbKS%2BWyPcjlWoBAyTeJW5j1YuFqOZYpDCEQJIkAg0my80AZBOPYg9%2FkPu8mZNefbqC%2B7TzE4xYGBhesN0iLOcHXUiGgNAqUxjiDmjhnuKEwT3uMyc0sDJPVaZA6p4tf6HW%2BwCLg2cuaj6xwMlvbmAnHiz6mY92oe9fyzxLC%2FvpsTzoKLJb40wIRJJn60eO7I%3D";

                //string res = "n8JJ4BIwkdfkpkBi3AQVzHPHVjvAH5HLqGtCePNUa%2boR5z1QMLcCE1bUZbCsoETGan7NT4Cx2GaRyMwXN2yP7eB2jZsMK%2fWLWtzXwWPXfvszRkZnZ2HnZ9aaKbzehc8T7zuzhp08jJduBJ9q4UTtpMq5BIscTAkRchHKAFWhwSbnTpzH80%2b3yQjolIpZk6ogu%2fFAa%2fp5QRk0fpp%2fu6ClzzpM1OAtwV5oI%2bUy2sAfF4wZ8%2bIpWrVbL%2fo4Zp%2b6wSzaJL%2fvqfiKxgXvIRD%2bBb2vE9AmbjgkH5Qq0sx5zCkmOdQkBI1DVwTVURrwm3ujCJrlWXYwxNDqpDlac84fyB2qUM6G9hoYgtifRx1i6znnYB50QZbCZIjQZpOBHkS2t7bDPi8SH1OFhIjnStDf9ZgYv%2bUOTe7pWsOtQcbSch8%2bc3aGfna2OOtcnduGZ%2bxmcBxMWUWXPOOV7T9s79NzAy4pTRe%2f0lvqLjrMuD%2fbU%2bUo7%2flU%2f5IdLJCmQLFIjLUFzok0BC%2fGtOlBlHliUo%2fvJd%2f7ZlSxCfc7xU%2f6o2JQId6%2b716ODSUynRAjb0XBW2vXyr4X4v202eEI6a2ihPFbVBpvlmYuLpbAgZGw%2b03x8pkd7qCNAqioLA01%2bjiG1vyoCatuFaSSnTm%2fDoy1JnfQrOGvZ9PHBlRLTxcb8VjbOPF3FO2DA9sl6V00MDLOUBL%2fobBwOmOtYkKoXoOqX%2b6%2bPATGbA6pyY%2fn0T2EnJAidjNfIbU%3d";

                string res = Request.Form.ToString().Replace("response=", "");

                //res = HttpUtility.UrlDecode(res);


                string res1 = DecryptString(res, key, iv);

                lblResp.Text = res1.ToString();
                ErrorLog("data:" + Request.Form);
                ErrorLog("data:" + res);

                //string[] keys = Request.Form.AllKeys;
                //ErrorLog("keys:" + keys);
                //Response.Write("<script>console.log('abc');</script>");
                //Response.Write("<script>console.log("+ Page.Response + ");</script>");
                //Response.Write("<script>console.log(" + Request + ");</script>");
                ////ErrorLog("header validation:" + Request.Headers["validation"].ToString());
                //ErrorLog("encrypted1:" + Page.Response);
                //ErrorLog("encrypted2:" + Request);
                //ErrorLog("Request.InputStream:" + Request.InputStream.ToString());
                //ErrorLog("encrypted:" + Request.Form["encrypted"]);
                //ErrorLog("validation:" + Request.Params.Get("validation"));
                //ErrorLog("request_id:" + Request.Params.Get("request_id"));
                //ErrorLog("transaction_details:" + Request.Params.Get("transaction_details"));
                //ErrorLog("encrypted:" + Request.Params.Get("encrypted"));

                // first
                // second
                // third
            }
            catch (Exception ex)
            {
                ErrorLog(ex.Message.ToString());
            }
          

        }


       
        public void ErrorLog(string sErrMsg)
        {
            string sPathName = Server.MapPath("Logs");
            if (!(System.IO.Directory.Exists(sPathName)))
                System.IO.Directory.CreateDirectory(sPathName);
            string FullPath = sPathName + "/ErrorLog.txt";
            if (!(System.IO.File.Exists(FullPath)))
                System.IO.File.Create(FullPath);
            string datetimeStr = DateTime.Now.ToString();
            StreamWriter sw = new StreamWriter(FullPath, true);
            sw.WriteLine("-------------------------");
            sw.WriteLine(datetimeStr + " : " + sErrMsg);
            sw.Flush();
            sw.Close();
        }

        static string DecryptString(string cipherText, byte[] key, byte[] iv)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try
            {
                // Convert the ciphertext string into a byte array
                string urldec = System.Web.HttpUtility.UrlDecode(cipherText);
                byte[] cipherBytes = Convert.FromBase64String(urldec);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainText;
        }
    }
}