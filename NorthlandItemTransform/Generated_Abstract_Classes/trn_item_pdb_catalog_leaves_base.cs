using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class trn_item_pdb_catalog_leaves_base
	{
		public Int64 spa_id { get; set; }
		public String child_svc_code { get; set; }
		public String parent_cast_type { get; set; }
		public String parent_svc_code { get; set; }

		public trn_item_pdb_catalog_leaves CreateBaseRec(SqlDataReader r)
		{
			trn_item_pdb_catalog_leaves n = new trn_item_pdb_catalog_leaves();

			if (!r.IsDBNull(0)) n.spa_id = r.GetInt64(0);
			if (!r.IsDBNull(1)) n.child_svc_code = r.GetString(1);
			if (!r.IsDBNull(2)) n.parent_cast_type = r.GetString(2);
			if (!r.IsDBNull(3)) n.parent_svc_code = r.GetString(3);

			return n;
		}
	}

}
