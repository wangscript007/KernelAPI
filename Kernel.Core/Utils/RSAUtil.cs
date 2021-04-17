using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kernel.Core.Utils
{
    public class RSAUtil
    {
        /// <summary>
        /// 生成PEM格式的公钥和密钥
        /// </summary>
        /// <param name="strength">长度</param>
        /// <returns>Item1:公钥；Item2:私钥；</returns>
        public static List<string> CreateKeyPair(int strength = 1024)
        {
            RsaKeyPairGenerator r = new RsaKeyPairGenerator();
            r.Init(new KeyGenerationParameters(new SecureRandom(), strength));
            AsymmetricCipherKeyPair keys = r.GenerateKeyPair();

            TextWriter privateTextWriter = new StringWriter();
            PemWriter privatePemWriter = new PemWriter(privateTextWriter);
            privatePemWriter.WriteObject(keys.Private);
            privatePemWriter.Writer.Flush();

            TextWriter publicTextWriter = new StringWriter();
            PemWriter publicPemWriter = new PemWriter(publicTextWriter);
            publicPemWriter.WriteObject(keys.Public);
            publicPemWriter.Writer.Flush();

            return new List<string> { publicTextWriter.ToString(), privateTextWriter.ToString() };
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="publicKey">公钥</param>
        /// <param name="encryptstring">待加密的字符串</param>
        /// <returns>加密后的Base64</returns>
        public static string Encrypt(string publicKey, string encryptstring)
        {
            using (TextReader reader = new StringReader(publicKey))
            {
                AsymmetricKeyParameter key = new PemReader(reader).ReadObject() as AsymmetricKeyParameter;
                Pkcs1Encoding pkcs1 = new Pkcs1Encoding(new RsaEngine());
                pkcs1.Init(true, key);//加密是true；解密是false;
                byte[] entData = Encoding.UTF8.GetBytes(encryptstring);
                entData = pkcs1.ProcessBlock(entData, 0, entData.Length);
                return Convert.ToBase64String(entData);
            }
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="privateKey">私钥</param>
        /// <param name="decryptstring">待解密的字符串(Base64)</param>
        /// <returns>解密后的字符串</returns>
        public static string Decrypt(string privateKey, string decryptstring)
        {
            using (TextReader reader = new StringReader(privateKey))
            {
                dynamic key = new PemReader(reader).ReadObject();
                var rsaDecrypt = new Pkcs1Encoding(new RsaEngine());
                if (key is AsymmetricKeyParameter)
                {
                    key = (AsymmetricKeyParameter)key;
                }
                else if (key is AsymmetricCipherKeyPair)
                {
                    key = ((AsymmetricCipherKeyPair)key).Private;
                }
                rsaDecrypt.Init(false, key);  //这里加密是true；解密是false  

                byte[] entData = Convert.FromBase64String(decryptstring);
                entData = rsaDecrypt.ProcessBlock(entData, 0, entData.Length);
                return Encoding.UTF8.GetString(entData);
            }
        }
    }
}
