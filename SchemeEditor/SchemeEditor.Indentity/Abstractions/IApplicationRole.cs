namespace SchemeEditor.Identity.Abstractions
{
	public interface IApplicationRole
	{
		long Id { get; set; }
		string Name { get; set; }
		string NormalizedName { get; set; }
	}
}