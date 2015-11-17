//===============================================================================
// Microsoft patterns & practices Enterprise Library Contribution
// Data Access Application Block
//===============================================================================

using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace MedicalInsuranceService.Data.Configuration
{
    /// <summary>
	/// Oracle-specific configuration section.
	/// </summary>
    [ResourceDescription(typeof(DesignResources), "OracleConnectionSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "OracleConnectionSettingsDisplayName")]
    public class OracleConnectionSettings : SerializableConfigurationSection
	{
		#region Constants
		private const string oracleConnectionDataCollectionProperty = "";

		/// <summary>
		/// The section name for the <see cref="OracleConnectionSettings"/>.
		/// </summary>
		public const string SectionName = "oracleConnectionSettings";
		#endregion

		#region Properties
		/// <summary>
		/// Collection of Oracle-specific connection information.
		/// </summary>
		/// <value>The oracle connections data.</value>
		[ConfigurationProperty(oracleConnectionDataCollectionProperty, IsRequired = false, IsDefaultCollection = true)]
		public NamedElementCollection<OracleConnectionData> OracleConnectionsData
		{
			get { return (NamedElementCollection<OracleConnectionData>)base[oracleConnectionDataCollectionProperty]; }
		}
		#endregion

		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="OracleConnectionSettings"/> class with default values.
		/// </summary>
		public OracleConnectionSettings()
		{
		}
		#endregion

		#region Public Static Methods
		/// <summary>
		/// Retrieves the <see cref="OracleConnectionSettings"/> from the configuration source.
		/// </summary>
		/// <param name="configurationSource">The configuration source to retrieve the configuration from.</param>
		/// <returns>
		/// The configuration section, or <see langword="null"/> (<b>Nothing</b> in Visual Basic)
		/// if not present in the configuration source.
		/// </returns>
		public static OracleConnectionSettings GetSettings(IConfigurationSource configurationSource)
		{
			return configurationSource.GetSection(SectionName) as OracleConnectionSettings;
		}
		#endregion
	}
}
