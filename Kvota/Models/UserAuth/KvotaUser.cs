using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kvota.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Kvota.Models.UserAuth;

// Add profile data for application users by adding properties to the KvotaUser class
public class KvotaUser : IdentityUser,ISoftDelete
{
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? LastName { get; set; }
    public string? Position { get; set; }

    public string? CompanyName { get; set; }
    public string? CompanyInn { get; set; }
    public virtual Address? Address { get; set; }


    public bool IsDeleted { get; set; }
    public bool IsFullDeleted { get; set; }
    public DateTime? DateTimeUpdate { get; set; }
    public string? UpdatedUser { get; set; }
}

