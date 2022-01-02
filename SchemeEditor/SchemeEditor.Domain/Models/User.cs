using System.Collections.Generic;
using System.Linq;
using LinqToDB.Mapping;
using Newtonsoft.Json;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Domain.Models
{
	[Table(TableNames.Users)]
	public class User: Entity, IApplicationUser
	{
		[Column, JsonIgnore]
		public string PasswordHash { get; set; }
		[Column, NotNull]
		public string Email { get; set; }
		[Column, NotNull]
		public bool EmailConfirmed { get; set; }
		[Column, JsonIgnore]
		public string NormalizedEmail { get; set; }
		[Column, NotNull]
		public string Phone { get; set; }
		[Column, NotNull]
		public bool PhoneConfirmed { get; set; }
		[Column, NotNull]
		public string Name { get; set; }
		[Column]
		public string LastName { get; set; }
		[Column]
		public string MiddleName { get; set; }
		[Column]
		public string City { get; set; }
		[Column]
		public string Company  { get; set; }
		[Column]
		public string Position { get; set; }
		[Column, NotNull]
		public bool IsBlocked { get; set; }
		[Association(ThisKey="Id", OtherKey="UserId", CanBeNull=true, Relationship = Relationship.OneToMany), JsonIgnore]
		public virtual IEnumerable<UserRole> UserRoles { get; set; }
		[NotColumn]
		public IApplicationRole Role => this.UserRoles?.FirstOrDefault()?.Role;
	}
}