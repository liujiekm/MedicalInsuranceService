//===============================================================================
// Microsoft patterns & practices Enterprise Library Contribution
// Data Access Application Block
//===============================================================================

using System.Collections.Generic;
using System.Data;

namespace MedicalInsuranceService.Data
{
	/// <summary>
	/// Registry of parameter types
	/// </summary>
	internal sealed class ParameterTypeRegistry
	{
		#region Fields
		private IDictionary<string, DbType> parameterTypes;
		#endregion

		#region Construction
		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterTypeRegistry"/> class.
		/// </summary>
		internal ParameterTypeRegistry()
		{
			this.parameterTypes = new Dictionary<string, DbType>();
		}
		#endregion

		#region Internal Methods
		/// <summary>
		/// Registers the type of the parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <param name="parameterType">Type of the parameter.</param>
		internal void RegisterParameterType(string parameterName, DbType parameterType)
		{
			this.parameterTypes[parameterName] = parameterType;
		}

		/// <summary>
		/// Determines whether [has registered parameter type] [the specified parameter name].
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <returns>
		/// 	<c>true</c> if [has registered parameter type] [the specified parameter name]; otherwise, <c>false</c>.
		/// </returns>
		internal bool HasRegisteredParameterType(string parameterName)
		{
			return this.parameterTypes.ContainsKey(parameterName);
		}

		/// <summary>
		/// Gets the type of the registered parameter.
		/// </summary>
		/// <param name="parameterName">Name of the parameter.</param>
		/// <returns></returns>
		internal DbType GetRegisteredParameterType(string parameterName)
		{
			return this.parameterTypes[parameterName];
		}
		#endregion
	}
}
