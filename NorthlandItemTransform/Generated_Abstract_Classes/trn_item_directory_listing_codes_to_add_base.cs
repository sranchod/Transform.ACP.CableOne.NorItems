using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class trn_item_directory_listing_codes_to_add_base
	{
		public Int64 tn_take_order { get; set; }
		public String ccs_subscriber { get; set; }
		public String xrf_customer_ccs_id { get; set; }
		public String xrf_house_ccs_id { get; set; }
		public Decimal service_acct_nbr { get; set; }
		public String? new_reference { get; set; }
		public Decimal? dld_sort_sequence { get; set; }
		public Int32 dld_csg_id { get; set; }
		public String bill_code { get; set; }
		public DateTime? connect_date { get; set; }

		public trn_item_directory_listing_codes_to_add CreateBaseRec(SqlDataReader r)
		{
			trn_item_directory_listing_codes_to_add n = new trn_item_directory_listing_codes_to_add();

			if (!r.IsDBNull(0)) n.tn_take_order = r.GetInt64(0);
			if (!r.IsDBNull(1)) n.ccs_subscriber = r.GetString(1);
			if (!r.IsDBNull(2)) n.xrf_customer_ccs_id = r.GetString(2);
			if (!r.IsDBNull(3)) n.xrf_house_ccs_id = r.GetString(3);
			if (!r.IsDBNull(4)) n.service_acct_nbr = r.GetDecimal(4);
			if (!r.IsDBNull(5)) n.new_reference = r.GetString(5);
			if (!r.IsDBNull(6)) n.dld_sort_sequence = r.GetDecimal(6);
			if (!r.IsDBNull(7)) n.dld_csg_id = r.GetInt32(7);
			if (!r.IsDBNull(8)) n.bill_code = r.GetString(8);
			if (!r.IsDBNull(9)) n.connect_date = r.GetDateTime(9);

			return n;
		}
	}
}
