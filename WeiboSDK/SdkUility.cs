using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace WeiboSDKForWinRT
{
    internal class SdkUility
    {
        /// <summary>
        /// Sha1加密
        /// </summary>
        /// <param name="baseString">需要加密的字符串</param>
        /// <param name="keyString">密钥</param>
        /// <returns></returns>
        internal static string Sha1Encrypt(string baseString, string keyString)
        {
            var crypt = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
            var buffer = CryptographicBuffer.ConvertStringToBinary(baseString, BinaryStringEncoding.Utf8);
            var keyBuffer = CryptographicBuffer.ConvertStringToBinary(keyString, BinaryStringEncoding.Utf8);
            var key = crypt.CreateKey(keyBuffer);

            var sigBuffer = CryptographicEngine.Sign(key, buffer);
            string signature = CryptographicBuffer.EncodeToBase64String(sigBuffer);
            return signature;
        }


        internal static string GetQueryParameter(string input, string parameterName)
        {
            char[] splitChars = new char[] { '&', '?' };
            foreach (string item in input.Split(splitChars))
            {
                var parts = item.Split('=');
                if (parts[0] == parameterName)
                {
                    return parts[1];
                }
            }
            return String.Empty;
        }
    }
}
