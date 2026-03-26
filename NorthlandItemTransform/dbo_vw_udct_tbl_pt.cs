using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class dbo_vw_udct_tbl_pt : Generated_Abstract_Classes.dbo_vw_udct_tbl_pt_base
	{
		public dbo_vw_udct_tbl_pt Copy2(dbo_vw_udct_tbl_pt source)
		{
			dbo_vw_udct_tbl_pt destRet = new dbo_vw_udct_tbl_pt();

			foreach (PropertyInfo property in typeof(dbo_vw_udct_tbl_pt).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}

		public static List<dbo_vw_udct_tbl_pt> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<dbo_vw_udct_tbl_pt> rt = new List<dbo_vw_udct_tbl_pt>();
			dbo_vw_udct_tbl_pt saHandler = new dbo_vw_udct_tbl_pt();

			String Query = string.Format(@"
select *
from {0} as a
order by a.spaId;", rmp.CodeTablePt);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new dbo_vw_udct_tbl_pt().CreateBaseRec(rdr);
						saHandler.code_table_value = saHandler.code_table_value.Trim();
						saHandler.line_o_bus = saHandler.line_o_bus.Trim();
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
