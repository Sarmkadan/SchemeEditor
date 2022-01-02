using System;
using LinqToDB.Mapping;

namespace SchemeEditor.Domain.Models
{
    public abstract class Entity
    {
        [Column, PrimaryKey, Identity] public long Id { get; set; }
        [Column, NotNull] public DateTime CreatedAt { get; set; }
        [Column, NotNull] public long CreatedBy { get; set; }
        [Column] public DateTime ModifiedAt { get; set; }
        [Column] public long ModifiedBy { get; set; }
        [Column] public DateTime? DeletedAt { get; set; }
        [Column] public long? DeletedBy { get; set; }
    }
}