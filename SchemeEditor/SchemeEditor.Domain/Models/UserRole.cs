using LinqToDB.Mapping;

namespace SchemeEditor.Domain.Models
{
	
	[Table(TableNames.UserRoles)]
	public class UserRole: Entity
	{
		[Column, NotNull]
		public long UserId  { get; set; }
		[Association(ThisKey="UserId", OtherKey="Id", CanBeNull=false)]
		public User User { get; set; }
		[Column, NotNull]
		public long RoleId  { get; set; }
		[Association(ThisKey="RoleId", OtherKey="Id", CanBeNull=false)]
		public Role Role { get; set; }
	}
}