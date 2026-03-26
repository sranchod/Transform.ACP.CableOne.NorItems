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
	public class trn_item_ao_codes_to_add : trn_item_ao_codes_to_add_base
	{
		public Int64 spaSysLvl { get; set; }
		public Int64 spaPrinLvl { get; set; }

		public trn_item_ao_codes_to_add Copy2(trn_item_ao_codes_to_add source)
		{
			trn_item_ao_codes_to_add destRet = new trn_item_ao_codes_to_add();

			foreach (PropertyInfo property in typeof(trn_item_ao_codes_to_add).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}
		public static trn_item_ao_codes_to_add Copy(trn_item_ao_codes_to_add source, trn_item_ao_codes_to_add dest)
		{
			trn_item_ao_codes_to_add destRet = dest;
			foreach (PropertyInfo property in typeof(trn_item_ao_codes_to_add).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}

		public static List<trn_item_ao_codes_to_add> LoadTable(SqlConnection myCon, RunMtParms rmp, Int32 tableToUse)
		{
			myCon.Open();

			List<trn_item_ao_codes_to_add> rt = new List<trn_item_ao_codes_to_add>();
			trn_item_ao_codes_to_add saHandler = new trn_item_ao_codes_to_add();
			String aoTableName = rmp.ItemAoCodesTable;
			if (tableToUse == 2)
				aoTableName = rmp.ItemAoCodesTable_t2;

			String Query = string.Format(@"
select a.*
from {2} as a
where convert(bigint, a.xrf_customer_ccs_id) % {0} = ({1} - 1)
order by a.xrf_customer_ccs_id;", rmp.Splits, rmp.Thread, aoTableName);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new trn_item_ao_codes_to_add().CreateBaseRec(rdr);
						saHandler.spaSysLvl = Convert.ToInt64(saHandler.ccs_subscriber.Substring(0, 4) + "00000000");
						saHandler.spaPrinLvl = Convert.ToInt64(saHandler.ccs_subscriber.Substring(0, 6) + "000000");
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
