using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class trn_item_ao_codes_to_add_base
	{
		public String ccs_subscriber { get; set; }
		public String xrf_customer_ccs_id { get; set; }
		public String xrf_house_ccs_id { get; set; }
		public String is_active_subscriber { get; set; }
		public Int32 video_box_count { get; set; }
		public Int32 number_of_aos_to_add { get; set; }
		public String bill_code { get; set; }
		public DateTime connect_date { get; set; }

		public trn_item_ao_codes_to_add CreateBaseRec(SqlDataReader r)
		{
			trn_item_ao_codes_to_add n = new trn_item_ao_codes_to_add();

			if (!r.IsDBNull(0)) n.ccs_subscriber = r.GetString(0);
			if (!r.IsDBNull(1)) n.xrf_customer_ccs_id = r.GetString(1);
			if (!r.IsDBNull(2)) n.xrf_house_ccs_id = r.GetString(2);
			if (!r.IsDBNull(3)) n.is_active_subscriber = r.GetString(3);
			if (!r.IsDBNull(4)) n.video_box_count = r.GetInt32(4);
			if (!r.IsDBNull(5)) n.number_of_aos_to_add = r.GetInt32(5);
			if (!r.IsDBNull(6)) n.bill_code = r.GetString(6);
			if (!r.IsDBNull(7)) n.connect_date = r.GetDateTime(7);

			return n;
		}
	}
}
