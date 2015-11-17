//===============================================================================
// Microsoft patterns & practices Enterprise Library Contribution
// Data Access Application Block
//===============================================================================



namespace WYYY.HDGM.DataContext
{
	/// <summary>
	/// Represents the description of an oracle package mapping.
	/// </summary>
	/// <remarks>
	/// <see cref="IOraclePackage"/> is used to specify how to transform stored procedure names
	/// into package qualified Oracle stored procedure names.
	/// </remarks>
	/// <seealso cref="OracleDatabase"/>
	public interface IOraclePackage
	{
		#region Properties
		/// <summary>
		/// Gets the name of the package.
		/// </summary>
		/// <value>The name of the package.</value>
		string Name
		{ get; }

		/// <summary>
		/// Gets the prefix for the package.
		/// </summary>
		/// <value>The prefix for the package.</value>
		string Prefix
		{ get; }
		#endregion
	}
}
