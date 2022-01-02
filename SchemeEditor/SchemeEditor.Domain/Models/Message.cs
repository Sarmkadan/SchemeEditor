using System.Collections.Generic;
using LinqToDB.Mapping;

namespace SchemeEditor.Domain.Models
{
	
	[Table(TableNames.Messages)]
	public class Message: Entity
	{
		[Column, NotNull]
		public string Title { get; set; }
		[Column, NotNull]
		public string Body { get; set; }
		[Association(ThisKey = "Id", OtherKey = "UserId", CanBeNull = true, Relationship = Relationship.OneToMany)]
		public virtual IEnumerable<MessageUser> MessageUsers { get; set; }
		[Column]
		public string Addresses { get; set; }
		[Association(ThisKey = "CreatedBy", OtherKey = "Id", CanBeNull = true)]
		public User Author { get; set; }
	}
}