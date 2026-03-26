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
	public class trn_item_contract_codes : trn_item_contract_codes_base
	{
		public Int64 spaSysLvl { get; set; }
		public Int64 spaPrinLvl { get; set; }

		public trn_item_contract_codes Copy2(trn_item_contract_codes source)
		{
			trn_item_contract_codes destRet = new trn_item_contract_codes();

			foreach (PropertyInfo property in typeof(trn_item_contract_codes).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}
		public static trn_item_contract_codes Copy(trn_item_contract_codes source, trn_item_contract_codes dest)
		{
			trn_item_contract_codes destRet = dest;
			foreach (PropertyInfo property in typeof(trn_item_contract_codes).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}

		public static List<trn_item_contract_codes> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<trn_item_contract_codes> rt = new List<trn_item_contract_codes>();
			trn_item_contract_codes saHandler = new trn_item_contract_codes();

			String Query = string.Format(@"
select a.*
from {2} as a
where convert(bigint, a.xrf_customer_ccs_id) % {0} = ({1} - 1)
order by a.xrf_customer_ccs_id;", rmp.Splits, rmp.Thread, rmp.ItemContractCodesTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new trn_item_contract_codes().CreateBaseRec(rdr);
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
