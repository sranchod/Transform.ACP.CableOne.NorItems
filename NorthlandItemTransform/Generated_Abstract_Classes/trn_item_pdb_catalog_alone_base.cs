using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class trn_item_pdb_catalog_alone_base
	{
		public Int64 spa_id { get; set; }
		public String child_svc_code { get; set; }

		public trn_item_pdb_catalog_alone CreateBaseRec(SqlDataReader r)
		{
			trn_item_pdb_catalog_alone n = new trn_item_pdb_catalog_alone();

			if (!r.IsDBNull(0)) n.spa_id = r.GetInt64(0);
			if (!r.IsDBNull(1)) n.child_svc_code = r.GetString(1);

			return n;
		}
	}
}