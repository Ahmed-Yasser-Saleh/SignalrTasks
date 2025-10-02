using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace signalrTask.Models
{
    public class Employee: IdentityUser
    {
        public string? name { get; set; }
        public string? address { get; set; }
        public int? age { get; set; }
    }
}
