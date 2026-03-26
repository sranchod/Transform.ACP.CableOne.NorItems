using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class trn_item_contract_codes_base
	{
		public Int32? SUBS { get; set; }
		public String? CONTRACT_NUMBER { get; set; }
		public DateTime? ENDDATE { get; set; }
		public DateTime? STARTDATE { get; set; }
		public String ccs_subscriber { get; set; }
		public String xrf_customer_ccs_id { get; set; }
		public String xrf_house_ccs_id { get; set; }
		public String is_active_subscriber { get; set; }
		public String? contract_bill_code { get; set; }

		public trn_item_contract_codes CreateBaseRec(SqlDataReader r)
		{
			trn_item_contract_codes n = new trn_item_contract_codes();

			if (!r.IsDBNull(0)) n.SUBS = r.GetInt32(0);
			if (!r.IsDBNull(1)) n.CONTRACT_NUMBER = r.GetString(1);
			if (!r.IsDBNull(2)) n.ENDDATE = r.GetDateTime(2);
			if (!r.IsDBNull(3)) n.STARTDATE = r.GetDateTime(3);
			if (!r.IsDBNull(4)) n.ccs_subscriber = r.GetString(4);
			if (!r.IsDBNull(5)) n.xrf_customer_ccs_id = r.GetString(5);
			if (!r.IsDBNull(6)) n.xrf_house_ccs_id = r.GetString(6);
			if (!r.IsDBNull(7)) n.is_active_subscriber = r.GetString(7);
			if (!r.IsDBNull(8)) n.contract_bill_code = r.GetString(8);

			return n;
		}
	}
}