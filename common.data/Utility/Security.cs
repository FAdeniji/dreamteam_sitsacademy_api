using System.Security.Cryptography;
using System.Text;

namespace common.data
{
    public static class Security
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZavcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Md5Hash(string input)
        {
            var hash = new StringBuilder();
            var md5Provider = new MD5CryptoServiceProvider();
            var bytes = md5Provider.ComputeHash(new UTF8Encoding().GetBytes(input));



            foreach (var t in bytes)
            {
                hash.Append(t.ToString("x2"));
            }
            return hash.ToString();
        }

        public static string Encrypt(string plainText, string passCode)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Ensure a valid key size (128, 192, or 256 bits)
                aesAlg.KeySize = 256;

                // Generate a random IV (Initialization Vector)
                aesAlg.GenerateIV();

                // Derive a valid key from the passCode using a key derivation function (e.g., PBKDF2)
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(passCode, aesAlg.IV, 10000);
                aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    // Prepend IV to the ciphertext for later use during decryption
                    byte[] ivAndCiphertext = aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray();
                    return Convert.ToBase64String(ivAndCiphertext);
                }
            }
        }

        public static string Decrypt(string cipherText, string passCode)
        {
            using (Aes aesAlg = Aes.Create())
            {
                // Ensure a valid key size (128, 192, or 256 bits)
                aesAlg.KeySize = 256;

                // Extract IV from the beginning of the ciphertext
                byte[] iv = Convert.FromBase64String(cipherText).Take(aesAlg.BlockSize / 8).ToArray();

                // Derive a valid key from the passCode using a key derivation function (e.g., PBKDF2)
                Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(passCode, iv, 10000);
                aesAlg.Key = keyDerivation.GetBytes(aesAlg.KeySize / 8);

                // Decrypt the remaining ciphertext
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText).Skip(aesAlg.BlockSize / 8).ToArray()))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static string Anonymise(string email)
        {
            // Mask email address
            string[] parts = email.Split('@');
            if (parts.Length == 2)
            {
                string maskedLocalPart = $"{parts[0].Substring(0, 2)}{RandomString(4)}";
                return maskedLocalPart + "@" + parts[1];
            }
            return email;
        }

        public static string AnonymiseNumber(string number)
        {
            return $"{number.Substring(0, 2)}-***-*****"; ;
        }

        public static string AnonymiseData(string data)
        {
            return $"{data.Substring(0, 1)}{RandomString(4)}"; ;
        }
    }
}