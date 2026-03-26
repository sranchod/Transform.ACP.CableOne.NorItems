using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class site_spc_site_dates : Generated_Abstract_Classes.site_spc_site_dates_base
	{
		public site_spc_site_dates Copy2(site_spc_site_dates source)
		{
			site_spc_site_dates destRet = new site_spc_site_dates();

			foreach (PropertyInfo property in typeof(site_spc_site_dates).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}

		public static List<site_spc_site_dates> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<site_spc_site_dates> rt = new List<site_spc_site_dates>();
			site_spc_site_dates saHandler = new site_spc_site_dates();

			String Query = string.Format(@"
select a.*
from {0} as a;", rmp.SiteDatesTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new site_spc_site_dates().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
