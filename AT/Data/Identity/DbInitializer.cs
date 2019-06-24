using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AT.Data.Identity
{
        public class DbInitializer : IDbInitializer
        {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public DbInitializer(
                ApplicationDbContext context,
                UserManager<ApplicationUser> userManager,
                RoleManager<IdentityRole> roleManager)
            {
                _context = context;
                _userManager = userManager;
                _roleManager = roleManager;
            }

            //This example just creates an Administrator role and one Admin users
            public async void Initialize()
            {
                //create database schema if none exists
                _context.Database.EnsureCreated();
          
            //If there is already an Administrator role, abort
            if (_context.Roles.Any(r => r.Name == "Administrator")) return;

                //Create the Administartor Role
                await _roleManager.CreateAsync(new IdentityRole("Administrator"));
                await _roleManager.CreateAsync(new IdentityRole("Member"));
              
            //Create the default Admin account and apply the Administrator role

            string username = "adam@yewtrade.com";
            string firstname = "Adam";
            string password = "Palmtr33!";
            string organisation = "Yewtrade";
            string division = "IT";
            string jobtitle = "CEO";
            string lastname = "Musson";
            string phone = "07592 654512";
            string email = "adam@yewtrade.com";
           
           

                await _userManager.CreateAsync(new ApplicationUser {UserName = username,
                                                                    FirstName = firstname,
                                                                    LastName = lastname,
                                                                    Company = organisation,
                                                                    Division = division,
                                                                    JobTitle = jobtitle,
                                                                    Mobile = phone,
                                                                  
                                                                    Email = email,
                                                                    EmailConfirmed = true }, 
                                                                    password);
                await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(username), "Administrator");

             username = "martin@amatistraining.com";
             firstname = "Martin";
             password = "amatis1234!";
             organisation = "Amatis Training";
             jobtitle = "CEO";
             division = "Head Office";

             lastname = "Maya";
             phone = "07710 569375";
             email = "martin@amatistraining.com";


            await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = username,
                FirstName = firstname,
                LastName = lastname,
                Company = organisation,
                Division = division,
                JobTitle = jobtitle,
                Mobile = phone,

                Email = email,
                EmailConfirmed = true
            },
              password);
            await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(username), "Administrator");

          
           

          

        }
    }
    }


