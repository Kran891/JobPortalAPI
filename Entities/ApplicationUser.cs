﻿using Microsoft.AspNetCore.Identity;

namespace JobPortal.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Resume { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public virtual List<UserSkills> Skills { get; set; }

        }
    }