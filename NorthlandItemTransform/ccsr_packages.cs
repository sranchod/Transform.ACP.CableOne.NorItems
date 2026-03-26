using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class ccsr_packages : Generated_Abstract_Classes.ccsr_packages_base
	{
		public ccsr_packages Copy2(ccsr_packages source)
		{
			ccsr_packages destRet = new ccsr_packages();

			foreach (PropertyInfo property in typeof(ccsr_packages).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}

		public static List<ccsr_packages> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<ccsr_packages> rt = new List<ccsr_packages>();
			ccsr_packages saHandler = new ccsr_packages();

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
order by a.spaId, a.service_code;", rmp.CcsrPackagesTable, rmp.ForeignRateCodesTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new ccsr_packages().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
