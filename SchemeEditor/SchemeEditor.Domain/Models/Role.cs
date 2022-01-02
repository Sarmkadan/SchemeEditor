using System.Collections.Generic;
using LinqToDB.Mapping;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Domain.Models
{
	[Table(TableNames.Roles)]
	public class Role: Entity, IApplicationRole
	{
		[Column, NotNull]
		public string Name { get; set; }
		[Column]
		public string NormalizedName { get; set; }

		[Association(ThisKey="Id", OtherKey="RoleId", CanBeNull=true, Relationship = Relationship.OneToMany)]
		public virtual IEnumerable<UserRole> UserRoles { get; set; }
	}
}