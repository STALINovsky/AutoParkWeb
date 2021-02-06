using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutoParkWeb.Models.ViewModels
{
    public class JsonFileViewModel
    {
        public IFormFile File { get; set; }
    }
}
