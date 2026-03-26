using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class trn_sub_base_extract
	{
		public String account_number { get; set; }
		public DateTime bill_to_date { get; set; }
		public DateTime first_billing_date { get; set; }
		public String vip_flag { get; set; }
		public String account_type { get; set; }

		public static List<trn_sub_base_extract> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<trn_sub_base_extract> rt = new List<trn_sub_base_extract>();
			trn_sub_base_extract saHandler = new trn_sub_base_extract();

			String Query = string.Format(@"
select a.account_number, a.bill_to_date, a.first_billing_date, a.vip_flag, b.account_type
from {2} as a
inner join {3} as b on
  a.account_number = b.ccs_id
where convert(bigint, b.xrf_customer_ccs_id) % {0} = {1} - 1
  and b.is_active_subscriber = 'Y'
order by a.account_number;", rmp.Splits, rmp.Thread, rmp.SubscriberTable, rmp.XrefTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new trn_sub_base_extract().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
		public trn_sub_base_extract CreateBaseRec(SqlDataReader r)
		{
			trn_sub_base_extract n = new trn_sub_base_extract();

			if (!r.IsDBNull(0)) n.account_number = r.GetString(0);
			if (!r.IsDBNull(1)) n.bill_to_date = r.GetDateTime(1);
			if (!r.IsDBNull(2)) n.first_billing_date = r.GetDateTime(2);
			if (!r.IsDBNull(3)) n.vip_flag = r.GetString(3);
			if (!r.IsDBNull(4)) n.account_type = r.GetString(4);

			return n;
		}
	}
}
