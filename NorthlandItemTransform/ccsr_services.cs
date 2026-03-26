using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class ccsr_services : Generated_Abstract_Classes.ccsr_services_base
	{
		public ccsr_services Copy2(ccsr_services source)
		{
			ccsr_services destRet = new ccsr_services();

			foreach (PropertyInfo property in typeof(ccsr_services).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}

		public static List<ccsr_services> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<ccsr_services> rt = new List<ccsr_services>();
			ccsr_services saHandler = new ccsr_services();

			String Query = string.Format(@"
with cteForeignRateCodeSpas as
(
	select distinct a.spa_id
	from {1} as a

), cteOnlyUsedSpas as
(
	select a.*
	from {0} as a
	inner join cteForeignRateCodeSpas as b on
	  a.spaId = b.spa_id
	where a.spaId != 0

)
select *
from cteOnlyUsedSpas as a
order by a.spaId, a.service_code;", rmp.CcsrServicesTable, rmp.ForeignRateCodesTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new ccsr_services().CreateBaseRec(rdr);
						saHandler.product_type = saHandler.product_type.Trim();
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
