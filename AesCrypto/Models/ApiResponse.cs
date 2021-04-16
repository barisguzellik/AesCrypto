using System;
namespace AesCrypto.Models
{
    public class ApiResponse
    {
        public bool status { get; set; }
        public string error_code { get; set; }
        public string error_message { get; set; }
        public dynamic response { get; set; }
        public dynamic request { get; set; }

    }
}
