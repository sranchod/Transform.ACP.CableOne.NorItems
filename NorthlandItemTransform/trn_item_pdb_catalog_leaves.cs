using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class trn_item_pdb_catalog_leaves : NorthlandItemTransform.Generated_Abstract_Classes.trn_item_pdb_catalog_leaves_base
	{
		public trn_item_pdb_catalog_leaves Copy2(trn_item_pdb_catalog_leaves source)
		{
			trn_item_pdb_catalog_leaves destRet = new trn_item_pdb_catalog_leaves();

			foreach (PropertyInfo property in typeof(trn_item_pdb_catalog_leaves).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}
		public static trn_item_pdb_catalog_leaves Copy(trn_item_pdb_catalog_leaves source, trn_item_pdb_catalog_leaves dest)
		{
			trn_item_pdb_catalog_leaves destRet = dest;
			foreach (PropertyInfo property in typeof(trn_item_pdb_catalog_leaves).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}

		public static List<trn_item_pdb_catalog_leaves> LoadTable(SqlConnection myCon, String tableName)
		{
			myCon.Open();

			List<trn_item_pdb_catalog_leaves> rt = new List<trn_item_pdb_catalog_leaves>();
			trn_item_pdb_catalog_leaves saHandler = new trn_item_pdb_catalog_leaves();

			String Query = string.Format(@"
select a.*
from {0} as a;", tableName);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new trn_item_pdb_catalog_leaves().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
