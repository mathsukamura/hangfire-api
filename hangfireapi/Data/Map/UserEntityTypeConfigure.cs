using apiemail.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace hangfireapi.Data.Map;

public class UserEntityTypeConfigure: IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .HasColumnName("email")
            .IsRequired()
            .HasColumnType("varchar(100)");
        
        builder.Property(u => u.Password)
            .HasColumnName("Password")
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(u => u.EsqueciMinhaSenha)
            .IsRequired(false)
            .HasColumnType("varchar(100)")
            .HasColumnName("esquecisenha");
    }
}