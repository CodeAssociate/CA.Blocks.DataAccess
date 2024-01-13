namespace CA.Blocks.DataAccess.Generic
{
	/// <summary>
	/// Names for collectionNam parameters across DB providers, see https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/ole-db-schema-collections
	/// </summary>
	public static class CommonOleDbCollectionNames
	{

		/// <summary>
		/// Implemented in Microsoft SQL Server OLE DB Provider, Microsoft Oracle OLE DB Provider, Microsoft Jet OLE DB Provider
		/// </summary>
		public const string Tables = "Tables";
		/// <summary>
		/// Implemented in Microsoft SQL Server OLE DB Provider, Microsoft Oracle OLE DB Provider, Microsoft Jet OLE DB Provider
		/// </summary>
		public const string Columns = "Columns";
		/// <summary>
		/// Implemented in Microsoft SQL Server OLE DB Provider, Microsoft Oracle OLE DB Provider, Microsoft Jet OLE DB Provider
		/// </summary>
		public const string Procedures = "Procedures";

		/// <summary>
		/// Implemented in  Microsoft Oracle OLE DB Provider
		/// </summary>
		public const string ProcedureColumns = "ProcedureColumns";

		/// <summary>
		/// Implemented in Microsoft SQL Server OLE DB Provider, Microsoft Oracle OLE DB Provider
		/// </summary>
		public const string ProcedureParameters = "ProcedureParameters";

		/// <summary>
		/// Implemented in Microsoft SQL Server OLE DB Provider, 
		/// </summary>
		public const string Catalog = "Catalog";

		/// <summary>
		/// Implemented in Microsoft SQL Server OLE DB Provider, Microsoft Oracle OLE DB Provider, Microsoft Jet OLE DB Provider
		/// </summary>
		public const string Views = "Views";

		/// <summary>
		/// Implemented in Microsoft SQL Server OLE DB Provider, Microsoft Oracle OLE DB Provider, Microsoft Jet OLE DB Provider
		/// </summary>
		public const string Indexes = "Indexes";

	}

}