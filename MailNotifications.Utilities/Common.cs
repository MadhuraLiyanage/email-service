using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MailNotifications.Utilities
{
    public static class Common
    {
        private static readonly string _EmailValidation_Regex = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        private static readonly Regex _EmailValidation_Regex_Compiled = new Regex(_EmailValidation_Regex, RegexOptions.IgnoreCase);

        public static bool ValidateToken(string token)
        {
            //Session token is hardcorded for current use
            if (token.Equals("2r5u7x!A%D*G-KaPdSgVkYp3s6v9y/B?"))
            {
                return true;
            }
            return false;
        }

        public static string GetFileName(string orgFileName)
        {
            //get file extention
            string[] fileName = orgFileName.Split('.');
            int lstIndex = fileName.Length - 1;
            string fileExtention = fileName[lstIndex];


            //get new hex file name
            DateTime dt = DateTime.Now;
            string hexFileName = Convert.ToInt64(dt.ToString("yyyyMMddhhmmss")).ToString("X") + "." + fileExtention;
            return hexFileName;
        }

        public static string GetDirectory()
        {
            Random rnd = new Random();
            int num = rnd.Next();

            string hexDireName = Convert.ToInt64(num).ToString("X");
            return hexDireName;
        }

        public static void SaveAttachmentToHardisk(string filePathAndName, string base64String)
        {
            File.WriteAllBytes(filePathAndName, Convert.FromBase64String(base64String));
        }

        public static string ReadAttachmentFromHardisk(string filePathAndName)
        {
            Byte[] bytes = File.ReadAllBytes(filePathAndName);
            String file = Convert.ToBase64String(bytes);
            return file;
        }

        public static string Encrypt(string encryptString)
        {
            try
            {
                string textToEncrypt = encryptString;
                string ToReturn = "";
                string publickey = "ZJ3K4M6P";
                string secretkey = "M5N6P8R9";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public static string Decrypt(string encyptedKey)
        {
            try
            {
                string textToDecrypt = encyptedKey;
                string ToReturn = "";
                string publickey = "ZJ3K4M6P";
                string secretkey = "M5N6P8R9";
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }

        public static bool ValidateEmailAddress(string[] emailAddresses)
        {
            if (emailAddresses.Length > 0)
            {
                foreach (string email in emailAddresses)
                {
                    if (!_EmailValidation_Regex_Compiled.IsMatch(email))
                    {
                        return false;
                    }
                }
            }
 
            return true;
        }
    }
}