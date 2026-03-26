using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class ruleng_breadcrumb : Generated_Abstract_Classes.ruleng_breadcrumb_base
	{
		public ruleng_breadcrumb Copy2(ruleng_breadcrumb source)
		{
			ruleng_breadcrumb destRet = new ruleng_breadcrumb();

			foreach (PropertyInfo property in typeof(ruleng_breadcrumb).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}

		public static ruleng_breadcrumb Copy(ruleng_breadcrumb source, ruleng_breadcrumb dest)
		{
			ruleng_breadcrumb destRet = dest;
			foreach (PropertyInfo property in typeof(ruleng_breadcrumb).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}

		public static List<ruleng_breadcrumb> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<ruleng_breadcrumb> rt = new List<ruleng_breadcrumb>();
			ruleng_breadcrumb saHandler = new ruleng_breadcrumb();

			String Query = string.Format(@"
select b.*
from {2} as b
inner join {3} as a on
  b.FactId = a.id
where convert(bigint, a.ccs_customer) % {0} = ({1} - 1)
order by a.ccs_customer, a.id;", rmp.Splits, rmp.Thread, rmp.BreadCrumbTable, rmp.ForeignRateCodesTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new ruleng_breadcrumb().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
