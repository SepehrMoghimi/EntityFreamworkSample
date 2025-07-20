using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.DTOs.Account
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Emial { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}