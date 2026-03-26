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
	public class spec_item_scrunch_codes : spec_item_scrunch_codes_base
	{
		public spec_item_scrunch_codes Copy2(spec_item_scrunch_codes source)
		{
			spec_item_scrunch_codes destRet = new spec_item_scrunch_codes();

			foreach (PropertyInfo property in typeof(spec_item_scrunch_codes).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}
		public static spec_item_scrunch_codes Copy(spec_item_scrunch_codes source, spec_item_scrunch_codes dest)
		{
			spec_item_scrunch_codes destRet = dest;
			foreach (PropertyInfo property in typeof(spec_item_scrunch_codes).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}

		public static List<spec_item_scrunch_codes> LoadTable(SqlConnection myCon, String tableName)
		{
			myCon.Open();

			List<spec_item_scrunch_codes> rt = new List<spec_item_scrunch_codes>();
			spec_item_scrunch_codes saHandler = new spec_item_scrunch_codes();

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
						saHandler = new spec_item_scrunch_codes().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
