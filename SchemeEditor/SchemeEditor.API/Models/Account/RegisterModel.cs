using System.ComponentModel.DataAnnotations;

namespace SchemeEditor.API.Models.Account
{
	public class RegisterModel
	{
		public bool TermsAcceptedTemplate { get; private set; } = true;
		[Required(AllowEmptyStrings = false, ErrorMessage = "Имя обязательно для заполнения")]
		public string Name { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string Position { get; set; }
		public string City { get; set; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Организация обязательна для заполнения")]
		public string Company  { get; set; }
		public string PasswordHash { get; set; }

		[EmailAddress]
		[Required(AllowEmptyStrings = false, ErrorMessage = "Почта обязательна для заполнения")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = false, ErrorMessage = "Телефон обязателен для заполнения")]
		[Phone]
		public string Phone { get; set; }
		[Compare("TermsAcceptedTemplate", ErrorMessage = "Нужно принять лицензионное соглашение")]
		public bool TermsAccepted { get; set; }
	}
}