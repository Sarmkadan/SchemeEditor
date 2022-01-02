using System;

namespace SchemeEditor.Abstraction.Application.Models
{
	public class MessagesFilter
	{
		public long? UserId { get; set; }
		public DateTime? CreatedAt { get; set; }
		public bool? IsRead { get; set; }
		public int? Take { get; set; }
		public int? Page { get; set; }
	}
}