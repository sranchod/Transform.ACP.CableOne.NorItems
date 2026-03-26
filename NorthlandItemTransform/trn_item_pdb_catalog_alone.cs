using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NorthlandItemTransform.Generated_Abstract_Classes;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class trn_item_pdb_catalog_alone : trn_item_pdb_catalog_alone_base
	{
		public trn_item_pdb_catalog_alone Copy2(trn_item_pdb_catalog_alone source)
		{
			trn_item_pdb_catalog_alone destRet = new trn_item_pdb_catalog_alone();

			foreach (PropertyInfo property in typeof(trn_item_pdb_catalog_alone).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}
		public static trn_item_pdb_catalog_alone Copy(trn_item_pdb_catalog_alone source, trn_item_pdb_catalog_alone dest)
		{
			trn_item_pdb_catalog_alone destRet = dest;
			foreach (PropertyInfo property in typeof(trn_item_pdb_catalog_alone).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}

		public static List<trn_item_pdb_catalog_alone> LoadTable(SqlConnection myCon, String tableName)
		{
			myCon.Open();

			List<trn_item_pdb_catalog_alone> rt = new List<trn_item_pdb_catalog_alone>();
			trn_item_pdb_catalog_alone saHandler = new trn_item_pdb_catalog_alone();

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
						saHandler = new trn_item_pdb_catalog_alone().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
