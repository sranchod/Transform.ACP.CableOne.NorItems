using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class ccsr_package_item : Generated_Abstract_Classes.ccsr_package_item_base
	{
		public ccsr_package_item Copy2(ccsr_package_item source)
		{
			ccsr_package_item destRet = new ccsr_package_item();

			foreach (PropertyInfo property in typeof(ccsr_package_item).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}

		public static List<ccsr_package_item> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<ccsr_package_item> rt = new List<ccsr_package_item>();
			ccsr_package_item saHandler = new ccsr_package_item();

			String Query = string.Format(@"
with cteForeignRateCodeSpas as
(
	select distinct a.spa_id
	from {1} as a

), cteOnlyUsedSpas as
(
	select c.*, a.spaId
	from {2} as a
	inner join cteForeignRateCodeSpas as b on
	  a.spaId = b.spa_id
	inner join {0} as c on
	  a.Id = c.packageId		
	where a.spaId != 0

)
select *
from cteOnlyUsedSpas as a
order by a.spaId, a.packageId, a.Id;", rmp.CcsrPackageItemsTable, rmp.ForeignRateCodesTable, rmp.CcsrPackagesTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new ccsr_package_item().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
