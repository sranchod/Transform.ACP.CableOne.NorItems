using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class trn_tmp_foreign_rate_codes : Generated_Abstract_Classes.trn_tmp_foreign_rate_codes_base
	{
		public Int64 spaSysLvl { get; set; }
		public Int64 spaPrinLvl { get; set; }

		public trn_tmp_foreign_rate_codes Copy2(trn_tmp_foreign_rate_codes source)
		{
			trn_tmp_foreign_rate_codes destRet = new trn_tmp_foreign_rate_codes();

			foreach (PropertyInfo property in typeof(trn_tmp_foreign_rate_codes).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}
		public static trn_tmp_foreign_rate_codes Copy(trn_tmp_foreign_rate_codes source, trn_tmp_foreign_rate_codes dest)
		{
			trn_tmp_foreign_rate_codes destRet = dest;
			foreach (PropertyInfo property in typeof(trn_tmp_foreign_rate_codes).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}

		public static List<trn_tmp_foreign_rate_codes> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<trn_tmp_foreign_rate_codes> rt = new List<trn_tmp_foreign_rate_codes>();
			trn_tmp_foreign_rate_codes saHandler = new trn_tmp_foreign_rate_codes();

			String Query = string.Format(@"
select a.*
from {2} as a
where convert(bigint, a.ccs_customer) % {0} = ({1} - 1)
order by a.ccs_customer, a.id;", rmp.Splits, rmp.Thread, rmp.ForeignRateCodesTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new trn_tmp_foreign_rate_codes().CreateBaseRec(rdr);
						saHandler.spaSysLvl = Convert.ToInt64(saHandler.spa_id.ToString().Substring(0, 4) + "00000000");
						saHandler.spaPrinLvl = Convert.ToInt64(saHandler.spa_id.ToString().Substring(0, 8) + "0000");
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
