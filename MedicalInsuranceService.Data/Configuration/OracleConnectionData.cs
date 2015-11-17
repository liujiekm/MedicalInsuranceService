//===============================================================================
// Microsoft patterns & practices Enterprise Library Contribution
// Data Access Application Block
//===============================================================================

using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace MedicalInsuranceService.Data.Configuration
{
    /// <summary>
	/// Oracle-specific connection information.
	/// </summary>
	public class OracleConnectionData : NamedConfigurationElement
	{
		#region Constants
		private const string packagesProperty = "packages";
		#endregion

		#region Properties
		/// <summary>
		/// Gets a collection of <see cref="OraclePackageData"/> objects.
		/// </summary>
		/// <value>A collection of <see cref="OraclePackageData"/> objects.</value>
		[ConfigurationProperty(packagesProperty, IsRequired = true)]
		public NamedElementCollection<OraclePackageData> Packages
		{
			get
			{
				return (NamedElementCollection<OraclePackageData>)base[packagesProperty];
			}
		}
		#endregion

		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="OracleConnectionData"/> class with default values.
		/// </summary>
		public OracleConnectionData()
		{
		}
		#endregion
	}
}
