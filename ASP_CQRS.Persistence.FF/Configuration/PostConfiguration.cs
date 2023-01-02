using System;
using System.Collections.Generic;
using System.Text;
using ASP_CQRS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASP_CQRS.Persistence.FF.Configuration
{
    class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(80);
        }
    }


}
