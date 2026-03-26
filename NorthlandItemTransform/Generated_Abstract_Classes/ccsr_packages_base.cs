using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class ccsr_packages_base
	{
		public Int32 Id { get; set; }
		public Int64 spaId { get; set; }
		public String? service_code { get; set; }
		public String? top_twenty_flag { get; set; }
		public String? rcd_desc { get; set; }
		public String? time_estimate { get; set; }
		public String? amt_on_wo_flag { get; set; }
		public String? category_code { get; set; }
		public String? prereq_cat_code { get; set; }
		public String? multiple_quantity { get; set; }
		public String? nature_of_service_flag { get; set; }
		public String? pkg_stmt_desc { get; set; }
		public String? class_services { get; set; }
		public String? pkg_tax_method { get; set; }
		public String? pkg_conv_rqd_flag { get; set; }
		public String? pkg_serv_sum_code { get; set; }
		public String? auto_pkg_incl_flag { get; set; }
		public String? indiv_pkg_breakdn_flag { get; set; }
		public String? pkg_prnt_zero_chg { get; set; }
		public String? pkg_savings { get; set; }
		public String? pkg_adjsvc_switch { get; set; }
		public String? pkg_start_entry_date { get; set; }
		public String? pkg_end_entry_date { get; set; }
		public String? pkg_security_func { get; set; }
		public String? pkg_svc_classification { get; set; }
		public String? pkg_force_autopkg { get; set; }
		public String? pkg_tx_pre_dscn_rt { get; set; }
		public String? nine_ap_wash_group { get; set; }
		public String? pkg_paired_dscnt { get; set; }
		public String? pkg_sec_resource { get; set; }
		public String? pkg_prorate_flag { get; set; }
		public String? pkg_rcm { get; set; }
		public String? pkg_stop_date { get; set; }
		public String? pkg_comp_redisplay { get; set; }
		public String? pkg_product_type { get; set; }
		public String? subscript_period_bp { get; set; }
		public String? charge_type_bp { get; set; }
		public String? pkg_vertex_tax_overide { get; set; }
		public String? pkg_def_price_plan { get; set; }
		public String? pkg_statement_desc_01 { get; set; }
		public String? pkg_statement_desc_02 { get; set; }
		public String? pkg_statement_desc_03 { get; set; }
		public String? pkg_statement_desc_04 { get; set; }
		public String? pkg_statement_desc_05 { get; set; }
		public String? pkg_statement_desc_06 { get; set; }
		public String? pkg_statement_desc_07 { get; set; }
		public String? pkg_statement_desc_08 { get; set; }
		public String? pkg_statement_desc_09 { get; set; }
		public String? pkg_statement_desc_10 { get; set; }
		public String? pkg_statement_desc_11 { get; set; }

		public ccsr_packages CreateBaseRec(SqlDataReader r)
		{
			ccsr_packages n = new ccsr_packages();

			if (!r.IsDBNull(0)) n.Id = r.GetInt32(0);
			if (!r.IsDBNull(1)) n.spaId = r.GetInt64(1);
			if (!r.IsDBNull(2)) n.service_code = r.GetString(2);
			if (!r.IsDBNull(3)) n.top_twenty_flag = r.GetString(3);
			if (!r.IsDBNull(4)) n.rcd_desc = r.GetString(4);
			if (!r.IsDBNull(5)) n.time_estimate = r.GetString(5);
			if (!r.IsDBNull(6)) n.amt_on_wo_flag = r.GetString(6);
			if (!r.IsDBNull(7)) n.category_code = r.GetString(7);
			if (!r.IsDBNull(8)) n.prereq_cat_code = r.GetString(8);
			if (!r.IsDBNull(9)) n.multiple_quantity = r.GetString(9);
			if (!r.IsDBNull(10)) n.nature_of_service_flag = r.GetString(10);
			if (!r.IsDBNull(11)) n.pkg_stmt_desc = r.GetString(11);
			if (!r.IsDBNull(12)) n.class_services = r.GetString(12);
			if (!r.IsDBNull(13)) n.pkg_tax_method = r.GetString(13);
			if (!r.IsDBNull(14)) n.pkg_conv_rqd_flag = r.GetString(14);
			if (!r.IsDBNull(15)) n.pkg_serv_sum_code = r.GetString(15);
			if (!r.IsDBNull(16)) n.auto_pkg_incl_flag = r.GetString(16);
			if (!r.IsDBNull(17)) n.indiv_pkg_breakdn_flag = r.GetString(17);
			if (!r.IsDBNull(18)) n.pkg_prnt_zero_chg = r.GetString(18);
			if (!r.IsDBNull(19)) n.pkg_savings = r.GetString(19);
			if (!r.IsDBNull(20)) n.pkg_adjsvc_switch = r.GetString(20);
			if (!r.IsDBNull(21)) n.pkg_start_entry_date = r.GetString(21);
			if (!r.IsDBNull(22)) n.pkg_end_entry_date = r.GetString(22);
			if (!r.IsDBNull(23)) n.pkg_security_func = r.GetString(23);
			if (!r.IsDBNull(24)) n.pkg_svc_classification = r.GetString(24);
			if (!r.IsDBNull(25)) n.pkg_force_autopkg = r.GetString(25);
			if (!r.IsDBNull(26)) n.pkg_tx_pre_dscn_rt = r.GetString(26);
			if (!r.IsDBNull(27)) n.nine_ap_wash_group = r.GetString(27);
			if (!r.IsDBNull(28)) n.pkg_paired_dscnt = r.GetString(28);
			if (!r.IsDBNull(29)) n.pkg_sec_resource = r.GetString(29);
			if (!r.IsDBNull(30)) n.pkg_prorate_flag = r.GetString(30);
			if (!r.IsDBNull(31)) n.pkg_rcm = r.GetString(31);
			if (!r.IsDBNull(32)) n.pkg_stop_date = r.GetString(32);
			if (!r.IsDBNull(33)) n.pkg_comp_redisplay = r.GetString(33);
			if (!r.IsDBNull(34)) n.pkg_product_type = r.GetString(34);
			if (!r.IsDBNull(35)) n.subscript_period_bp = r.GetString(35);
			if (!r.IsDBNull(36)) n.charge_type_bp = r.GetString(36);
			if (!r.IsDBNull(37)) n.pkg_vertex_tax_overide = r.GetString(37);
			if (!r.IsDBNull(38)) n.pkg_def_price_plan = r.GetString(38);
			if (!r.IsDBNull(39)) n.pkg_statement_desc_01 = r.GetString(39);
			if (!r.IsDBNull(40)) n.pkg_statement_desc_02 = r.GetString(40);
			if (!r.IsDBNull(41)) n.pkg_statement_desc_03 = r.GetString(41);
			if (!r.IsDBNull(42)) n.pkg_statement_desc_04 = r.GetString(42);
			if (!r.IsDBNull(43)) n.pkg_statement_desc_05 = r.GetString(43);
			if (!r.IsDBNull(44)) n.pkg_statement_desc_06 = r.GetString(44);
			if (!r.IsDBNull(45)) n.pkg_statement_desc_07 = r.GetString(45);
			if (!r.IsDBNull(46)) n.pkg_statement_desc_08 = r.GetString(46);
			if (!r.IsDBNull(47)) n.pkg_statement_desc_09 = r.GetString(47);
			if (!r.IsDBNull(48)) n.pkg_statement_desc_10 = r.GetString(48);
			if (!r.IsDBNull(49)) n.pkg_statement_desc_11 = r.GetString(49);

			return n;
		}
	}
}
