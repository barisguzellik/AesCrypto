using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AesCrypto.DTOs.Request;
using AesCrypto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AesCrypto.Controllers
{
    [Route("api/[controller]")]
    public class CryptoController : Controller
    {

        private static IConfiguration _configuration;
        private string cryptoKey = "";
        public RijndaelManaged _aes = new RijndaelManaged();

        public CryptoController(IConfiguration configuration)
        {
            _configuration = configuration;
            cryptoKey = _configuration["CryptoKey"];


            var keyArray = Convert.FromBase64String(cryptoKey);
            var keyArrayBytes32Value = new byte[24];
            Array.Copy(keyArray, keyArrayBytes32Value, 24);

            byte[] ivArray = { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1, 7, 7, 7, 7 };
            var ivBytes16Value = new byte[16];
            Array.Copy(ivArray, ivBytes16Value, 16);

            var aes = new RijndaelManaged();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = keyArrayBytes32Value;
            aes.IV = ivBytes16Value;

            _aes = aes;
        }

        [HttpPost]
        [Route("encrypt")]
        public ApiResponse Encrypt([FromBody] EncryptRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse
                {
                    status = false,
                    error_code = "OBJECT_IS_NULL",
                    error_message = "value cannot be null",
                    request = request
                };
            }

            try
            {

                var crypt = _aes.CreateEncryptor();
                var plainTextByte = ASCIIEncoding.UTF8.GetBytes(request.value);
                var cipherText = crypt.TransformFinalBlock(plainTextByte, 0, plainTextByte.Length);


                return new ApiResponse
                {
                    status = true,
                    response = Convert.ToBase64String(cipherText),
                    request = request
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    status = false,
                    error_code = "CRYPTO_ERROR",
                    error_message = ex.Message,
                    request = request
                };
            }


        }

        [HttpPost]
        [Route("decrypt")]
        public ApiResponse Decrypt([FromBody] DecryptRequest request)
        {
            if (!ModelState.IsValid)
            {
                return new ApiResponse
                {
                    status = false,
                    error_code = "VALUE_IS_NULL",
                    error_message = "Value cannot be null",
                    request = request
                };
            }

            try
            {

                var decrypt = _aes.CreateDecryptor();
                var encryptedBytes = Convert.FromBase64CharArray(request.value.ToCharArray(), 0, request.value.Length);
                var decryptedData = decrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

                return new ApiResponse
                {
                    status = true,
                    response = ASCIIEncoding.UTF8.GetString(decryptedData),
                    request = request
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    status = false,
                    error_code = "CRYPTO_ERROR",
                    error_message = ex.Message,
                    request = request
                };
            }
        }
    }
}
