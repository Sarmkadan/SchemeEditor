using System;
using LinqToDB.Mapping;

namespace SchemeEditor.Domain.Models
{
    [Table(TableNames.Schemes)]
    public class Scheme : Entity
    {
        [Column, NotNull]
        public string Name { get; set; }
        [Column]
        public string Body { get; set; }

        [Association(ThisKey = "CreatedBy", OtherKey = "Id", CanBeNull = true)]
        public User Author { get; set; }
    }
}