//===============================================================================
// Microsoft patterns & practices Enterprise Library Contribution
// Data Access Application Block
//===============================================================================

using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;

namespace MedicalInsuranceService.Data.Configuration
{
    /// <summary>
	/// <para>Represents the package information to use when calling a stored procedure for Oracle.</para>
	/// </summary>
	/// <remarks>
	/// <para>
	/// A package name can be appended to the stored procedure name of a command if the prefix of the stored procedure
	/// matchs the prefix defined. This allows the caller of the stored procedure to use stored procedures
	/// in a more database independent fashion.
	/// </para>
	/// </remarks>
	public class OraclePackageData : NamedConfigurationElement, IOraclePackage
	{
		#region Constants
		private const string prefixProperty = "prefix";
		#endregion

		#region Properties
		/// <summary>
		/// Gets or sets the prefix of the stored procedures that are in the package in Oracle.
		/// </summary>
		/// <value>
		/// The prefix of the stored procedures that are in the package in Oracle.
		/// </value>
		[ConfigurationProperty(prefixProperty, IsRequired = true)]
		public string Prefix
		{
			get
			{
				return (string)this[prefixProperty];
			}
			set
			{
				this[prefixProperty] = value;
			}
		}
		#endregion

		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="OraclePackageData"/> class.
		/// </summary>
		public OraclePackageData()
			: base()
		{
			this.Prefix = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OraclePackageData"/> class, given the prefix to search for and the name of the package.
		/// </summary>
		/// <param name="name">The name of the package to append to any found procedure that has the <paramref name="prefix"/>.</param>
		/// <param name="prefix">The prefix of the stored procedures used in this package.</param>
		public OraclePackageData(string name, string prefix)
			: base(name)
		{
			this.Prefix = prefix;
		}
		#endregion
	}
}