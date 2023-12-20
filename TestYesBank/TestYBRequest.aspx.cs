using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestYesBank
{
    public partial class TestYBRequest : System.Web.UI.Page
    {

        private static readonly Encoding encoding = Encoding.UTF8;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            //string YesBankURL = "https://invoicexpressnewuat.yesbank.in/pay/web/pushapi/index";
            string YesBankURL = "https://invoicexpressnewuat.yesbank.in/pay/web/pushapi/index";
            string mercid = ConfigurationManager.AppSettings["Merchant Code"].ToString();
            string secretKey = ConfigurationManager.AppSettings["Secret Key"].ToString();



            //parameters.Add("student_id", "1023"); 
            //parameters.Add("pay_mode", "online");
            Random rn = new Random();
            int uniq = rn.Next();
            parameters.Add("request_id", uniq.ToString());
            parameters.Add("template_id", "4345");
            parameters.Add("mobile", "9876543211");
            parameters.Add("email", "abcd123@gmail.com");
            parameters.Add("amount", "1");
            //parameters.Add("additional_info_1", "8673");
            parameters.Add("student_id", "8673");
            //Order id should be unique in every transaction





            //parameters.Add("privatekey", privatekey.Trim());
            //parameters.Add("mercid", mercid.Trim());
            //parameters.Add("return_url", "http://52.172.189.132/SURAJ_TestYesBank/TestYBResponse.aspx");
            parameters.Add("return_url", "https://yesbankpay.iweb.ltd/TestYBResponse.aspx");


            ErrorLog(uniq.ToString());
            List<Dictionary<String, object>> listObj = new List<Dictionary<string, object>>();
            listObj.Add(parameters);


            //string sslData = JsonConvert.SerializeObject(listObj);
            string sslData = MyDictionaryToJson(parameters);


            string message = sslData;
            //string secretKey1 = secretKey;

            // Create sha256 hash
            SHA256 mySHA256 = SHA256Managed.Create();
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(secretKey));

            // Create secret IV
            byte[] iv = new byte[16] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            string encrypted = this.EncryptString(message, key, iv);

            string decryt = this.DecryptString(encrypted, key, iv);

            string outputHTML = "<html>";

            outputHTML += "<head>";
            outputHTML += "<title>Merchant Check Out Page</title>";
            outputHTML += "</head>";
            outputHTML += "<body>";
            outputHTML += "<center><h1>Please do not refresh this page...</h1></center>";
            outputHTML += "<form method='post' action='" + YesBankURL + "' name='f1'>";
            outputHTML += "<table border='1'>";
            outputHTML += "<tbody>";

            //foreach (string key in parameters.Keys)
            //{
            //outputHTML += "<input type='hidden' name='" + key + "' value='" + parameters[key] + "'>'";
            outputHTML += "<input type='hidden' name='request' value='" + encrypted + "'>'";

            outputHTML += "<input type='hidden' name='merchant_code' value='" + mercid + "'>'";//}

            //String paymode = "200"; 
            //outputHTML += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
            outputHTML += "</tbody>";
            outputHTML += "</table>";
            outputHTML += "<script type='text/javascript'>";
            outputHTML += "document.f1.submit();";
            outputHTML += "</script>";
            outputHTML += "</form>";
            outputHTML += "</body>";
            outputHTML += "</html>";



            Response.Write(outputHTML);

        }


        string MyDictionaryToJson(Dictionary<string, object> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }




        public string EncryptString(string plainText, byte[] key, byte[] iv)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            //encryptor.Padding = PaddingMode.Zeros;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            //encryptor.Key = key;
            //encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;
        }

        public string DecryptString(string cipherText, byte[] key, byte[] iv)
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
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            catch (Exception ex)
            {
                throw;
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

    }
}