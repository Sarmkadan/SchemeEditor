using System;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Abstraction.Application.Models
{
    public class SchemeView
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public long? DeletedBy { get; set; }
        public User Author { get; set; }
    }
}