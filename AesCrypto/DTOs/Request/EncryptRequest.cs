using System;
using System.ComponentModel.DataAnnotations;

namespace AesCrypto.DTOs.Request
{
    public class EncryptRequest
    {
        [Required]
        public string value { get; set; }
    }
}
