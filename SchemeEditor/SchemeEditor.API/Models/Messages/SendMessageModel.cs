using System.Collections.Generic;

namespace SchemeEditor.API.Models.Messages
{
	public class SendMessageModel
	{
		public string Title { get; set; }
		public string Text { get; set; }
		public IEnumerable<Addresse> Addresses { get; set; }
	}
}