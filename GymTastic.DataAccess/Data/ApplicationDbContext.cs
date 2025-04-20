using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTastic.Models.Models;
using System.Threading.RateLimiting;

namespace GymTastic.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Atlete> Atletes { get; set; } 
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<FileClassificationType> FileClassificationTypes { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<ClassFeedback> ClassFeedbacks { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<PersonalizedTraining> PersonalizedTraining { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FileClassificationType>().HasData(
                new FileClassificationType { Id = 1, Description = "Identificação"}
                );

            builder.Entity<Gender>().HasData(
                new Gender { Id = 1, GenderDescription = "Feminino" },
                new Gender { Id = 2, GenderDescription = "Masculino" }
                );

            //builder.Entity<Atlete>().HasData(
            //    new Atlete
            //    {
            //        Id = 1,
            //        FirstName = "Tomás",
            //        LastName = "Santos Fernandes",
            //        BirthDate = DateTime.Parse("24/09/2007"),
            //        GenderId = 2,
            //        FIN = 2000000,
            //        CC = "12113131",
            //        InscriptionDate = DateTime.Parse("20/12/2022"),
            //        Email = "tomasantifernandes@gmail.com",
            //        PhoneNumber = "999999999",
            //        PhotoUrl = "",
            //        Address = "Estrada da Póvoa",
            //        ZipCode = "1750-224",
            //        City = "Lisbon",
            //        EmergencyContact = "Tomás",
            //        EmergencyPhone = "1212121",
            //        EmergencyEmail = "tomas@gmail.com",
            //        Height = 1.80f,
            //        Weight = 62.4f
            //    });
        }
    }
}
