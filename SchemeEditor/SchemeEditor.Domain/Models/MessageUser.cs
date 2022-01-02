using LinqToDB.Mapping;

namespace SchemeEditor.Domain.Models
{
	
	[Table(TableNames.MessageUsers)]
	public class MessageUser: Entity
	{
		[Column, NotNull]
		public long UserId { get; set; }
		[Column, NotNull]
		public long MessageId { get; set; }
		[Association(ThisKey = "UserId", OtherKey = "Id", CanBeNull = false)]
		public virtual User User { get; set; }
		[Association(ThisKey = "MessageId", OtherKey = "Id", CanBeNull = false)]
		public virtual Message Message { get; set; }
		[Column, NotNull]
		public bool IsRead { get; set; }
	}
}