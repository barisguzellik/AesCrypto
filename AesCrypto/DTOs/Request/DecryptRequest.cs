using System;
using System.ComponentModel.DataAnnotations;

namespace AesCrypto.DTOs.Request
{
    public class DecryptRequest
    {
        [Required]
        public string value { get; set; }
    }
}
