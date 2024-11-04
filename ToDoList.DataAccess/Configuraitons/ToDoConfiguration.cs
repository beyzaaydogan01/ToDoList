using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Models.Entities;

namespace ToDoList.DataAccess.Configuraitons;

public class ToDoConfiguration : IEntityTypeConfiguration<ToDo>
{
    public void Configure(EntityTypeBuilder<ToDo> builder)
    {
        builder.ToTable("Todos").HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("ToDoId");
        builder.Property(x => x.Title).HasColumnName("Title");
        builder.Property(x => x.Description).HasColumnName("Description");
        builder.Property(x => x.StartDate).HasColumnName("StartDate");
        builder.Property(x => x.EndDate).HasColumnName("EndDate");
        builder.Property(x => x.Priority).HasColumnName("Priority");
        builder.Property(x => x.CategoryId).HasColumnName("Category_Id");
        builder.Property(x => x.Completed).HasColumnName("IsCompleted");
        builder.Property(x => x.UserId).HasColumnName("UserId");

        builder.HasOne(x => x.User)
            .WithMany(x => x.ToDos)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.ToDos)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}

