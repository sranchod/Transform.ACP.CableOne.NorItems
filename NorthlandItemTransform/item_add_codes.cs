using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthlandItemTransform
{
	public class item_add_codes
	{
		public String csg_sys { get; set; }
		public String csg_prin { get; set; }
		public String csg_agent { get; set; }
		public Int64 spaId { get; set; }
		public Int64 spaSysLvl { get; set; }
		public Int64 spaPrinLvl { get; set; }
		public String csg_customer_number { get; set; }
		public String csg_account_number { get; set; }
		public String csg_house_number { get; set; }
		public String bill_code { get; set; }
		public String discount_code { get; set; }
		public String customer_discount_code { get; set; }
		public String campaign_code { get; set; }
		public Int64 psid { get; set; }
		public String reference { get; set; }
		public DateTime? connect_date { get; set; }
		public DateTime? discount_start_date { get; set; }
		public DateTime? customer_discount_start_date { get; set; }
		public DateTime bill_to_date { get; set; }
		public DateTime first_billing_date { get; set; }
		public String vip_flag { get; set; }
		public Int32? bulk_qty { get; set; }
		public Decimal? bulk_charge { get; set; }
		public Int32 rules_engine_pass { get; set; }
		public Int32 rule_id { get; set; }
		public Int32 match_id { get; set; }
		public Int32? code_cnt { get; set; }
	}
}
