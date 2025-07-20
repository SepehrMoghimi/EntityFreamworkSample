using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.stock;
using api.Helpers;
using api.models;

namespace api.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
} 