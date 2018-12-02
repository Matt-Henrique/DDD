﻿using Invoisys.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace Invoisys.Infrastructure.Data.EntityConfig
{
    public class UserConfig: EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            HasKey(u => u.Id);
            Property(u => u.Id)
                .IsRequired()
                .HasMaxLength(128);
            Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(256);
            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(256);
            ToTable("AspNetUsers");
        }
    }
}