using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace InterviewTest_Clicknext.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [PersonalData]
    [DisplayName("First Name")]
    [Column(TypeName ="nvarchar(100)")]
    public string FirstName { get; set; }

    [PersonalData]
    [DisplayName("Last Name")]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    [PersonalData]
    [DisplayName("Account Number")]
    [Column(TypeName = "nvarchar(100)")]
    public string AccountNumberGenerated { get; set; }
    [PersonalData]
    [DisplayName("Current Balance")]
    public double CurrentAccountBalance { get; set; }
    Random rand = new Random();
    public ApplicationUser()
    {
        //generrate accountNumber for customer
        AccountNumberGenerated = Convert.ToString((long)Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
        CurrentAccountBalance = 100000; // default CurrentAccountBalance
    }
}

