using System;
using System.Text;
using PCLCrypto;

namespace FriendLoc.Common.Statistics
{
    public static class Crypto
    {
        private const int Iterations = 5000;
        private const int EncryptionKeyLength = 16;
        public const string ConfigKey = "5gDjAQAAyQACAAAAAQDKAAIAAAAAAMsABAAAAAAIAADhAMMBAAAtLS0tLUJFR0lOIFBVQkxJQyBLRVktLS0tLQpNSUlCSWpBTkJna3Foa2lHOXcwQkFRRUZBQU9DQVE4QU1JSUJDZ0tDQVFFQWxsbDR0R0tQZnBTc21EL3lxZWh1Cm5zZDNSYUxWc3E5bDFTU21oYW42UUY4REo0dWVOVGQwMGZlZ2krYzduM25UWVVYRTdMRmxoTnhJUEs0T1VBOFIKYUJqYWFydHNuREVrNFh4WC9sSFpxcy82b0RNL2lHUWlGaEZTWFJUV2g2aUdmdW5WeEdvWG01ZWg3enBQWTJLaQpOc0VGZ3c5eEloVG9iVjJoOUZaZ004aTIrTXNrTWRBdG1jZ0h3djN6U2F5S0g1S3dWMkE5WWgzeml5SkhlKzVWCjNwNkZsRDN5ZEV6MFAyRDUxOHp0WVJISFFnOTNTTHhyT05jMHAwVDRFVmZHbHUvbndmbnM2YnZ3VHE1TVo4eloKbjNlUFNrWEhERVBoUVJQWld2S2lpWlRySlRXaWROU2xOb3FhTUZzUVMrY2hXcm5ZdjJtZU45ZEc2SXp2UXd6VgpGd0lEQVFBQgotLS0tLUVORCBQVUJMSUMgS0VZLS0tLS0K";
        private const string SALT = "5gDjAQAAyQACAAAAAQ";

        public static string ComputeHash(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var hasher = PCLCrypto.WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = hasher.HashData(inputBytes);
            var hashedAsString = Convert.ToBase64String(hash);

            return hashedAsString;
        }
        public static string ComputeHash(byte[] input)
        {
            if (input == null || input.Length == 0) return string.Empty;

            var hasher = PCLCrypto.WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
            byte[] hash = hasher.HashData(input);
            var hashedAsString = Convert.ToBase64String(hash);

            return hashedAsString;
        }

        public static string GenerateRandomKey(int length)
        {
            byte[] buffer = WinRTCrypto.CryptographicBuffer.GenerateRandom(length);
            return Convert.ToBase64String(buffer);
        }

        public static string Encrypt(string saltText, string dataText, string keyText, string ivText)
        {
            string encryptedText = null;

            if (string.IsNullOrEmpty(keyText))
                throw new ArgumentException("KeyText parameter cannot be null or empty string");

            if (string.IsNullOrEmpty(dataText))
                return string.Empty;

            byte[] data = Encoding.UTF8.GetBytes(dataText);
            byte[] iv = string.IsNullOrEmpty(ivText) ? null : Encoding.UTF8.GetBytes(ivText);

            byte[] cipherText = Encrypt(saltText, data, keyText, ivText);

            if (cipherText != null)

                encryptedText = Convert.ToBase64String(cipherText);

            return encryptedText;
        }

        public static byte[] Encrypt(string saltText, byte[] data, string keyText, string ivText)
        {
            if (string.IsNullOrEmpty(keyText))
                throw new ArgumentException("KeyText parameter cannot be null or empty string");

            if (data == null || data.Length == 0)

                return null;

            byte[] iv = string.IsNullOrEmpty(ivText) ? null : Encoding.UTF8.GetBytes(ivText);

            var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);

            var keyMaterial = CreateKeyMaterial(keyText, saltText, EncryptionKeyLength, Iterations);
            var key = provider.CreateSymmetricKey(keyMaterial);

            byte[] cipherText = WinRTCrypto.CryptographicEngine.Encrypt(key, data, iv);

            return cipherText;
        }

        public static string Decrypt(string saltText, string dataText, string keyText, string ivText)
        {
            string decrypted = null;

            if (string.IsNullOrEmpty(keyText))
                throw new ArgumentException("KeyText parameter cannot be null or empty string");

            if (string.IsNullOrEmpty(dataText))
                return string.Empty;

            byte[] data = Convert.FromBase64String(dataText);
            byte[] iv = string.IsNullOrEmpty(ivText) ? null : Encoding.UTF8.GetBytes(ivText);

            byte[] plainText = Decrypt(saltText, data, keyText, ivText);

            if (plainText != null)

                decrypted = Encoding.UTF8.GetString(plainText, 0, plainText.Length);

            return decrypted;
        }

        public static byte[] Decrypt(string saltText, byte[] data, string keyText, string ivText)
        {
            if (string.IsNullOrEmpty(keyText))
                throw new ArgumentException("KeyText parameter cannot be null or empty string");

            if (data == null || data.Length == 0)

                return null;

            byte[] iv = string.IsNullOrEmpty(ivText) ? null : Encoding.UTF8.GetBytes(ivText);

            var provider = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);

            var keyMaterial = CreateKeyMaterial(keyText, saltText, EncryptionKeyLength, Iterations);
            var key = provider.CreateSymmetricKey(keyMaterial);

            byte[] decrypted = WinRTCrypto.CryptographicEngine.Decrypt(key, data, iv);

            return decrypted;
        }

        static byte[] CreateKeyMaterial(string keySeed, string saltText, int keyLengthInBytes = 16, int iterations = 5000)
        {
            byte[] salt = Encoding.UTF8.GetBytes(saltText);
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(keySeed, salt, iterations, keyLengthInBytes);
            return key;
        }

        public static byte[] CreateDerivedKey(byte[] password, byte[] salt, int keyLengthInBytes = 32, int iterations = 1000)
        {
            byte[] key = NetFxCrypto.DeriveBytes.GetBytes(password, salt, iterations, keyLengthInBytes);
            return key;
        }

        public static byte[] AESEncrypt(byte[] data, byte[] password)
        {

            byte[] salt = Encoding.UTF8.GetBytes(SALT);

            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, data);
            return bytes;
        }

        public static byte[] AESEncrypt(byte[] data, byte[] password, byte[] salt)
        {

            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, data);
            return bytes;
        }

        public static string AESEncrypt(string data, string password)
        {

            byte[] salt = Encoding.UTF8.GetBytes(SALT);

            byte[] key = CreateDerivedKey(Encoding.UTF8.GetBytes(password), salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Encrypt(symetricKey, Encoding.UTF8.GetBytes(data));

            return Convert.ToBase64String(bytes);// Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }


        public static string AESDecrypt(byte[] data, byte[] password, byte[] salt)
        {
            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, data);
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        public static byte[] AESDecryptToBytes(byte[] data, byte[] password)
        {
            byte[] salt = Encoding.UTF8.GetBytes(SALT);

            byte[] key = CreateDerivedKey(password, salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            return WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, data);
        }

        public static string AESDecrypt(string data, string password)
        {
            byte[] salt = Encoding.UTF8.GetBytes(SALT);

            byte[] key = CreateDerivedKey(Encoding.UTF8.GetBytes(password), salt);

            ISymmetricKeyAlgorithmProvider aes = WinRTCrypto.SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithm.AesCbcPkcs7);
            ICryptographicKey symetricKey = aes.CreateSymmetricKey(key);
            var bytes = WinRTCrypto.CryptographicEngine.Decrypt(symetricKey, Convert.FromBase64String(data));
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
