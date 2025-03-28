using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymTastic.Models.Models;

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


        }
    }
}
