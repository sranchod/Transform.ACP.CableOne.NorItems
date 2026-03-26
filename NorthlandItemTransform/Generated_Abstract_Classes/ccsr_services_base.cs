using NorthlandItemTransform;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{

	public abstract class ccsr_services_base
	{
		public Int32 Id { get; set; }
		public Int64 spaId { get; set; }
		public String service_code { get; set; }
		public String rcd_type { get; set; }
		public String? top_twenty_flag { get; set; }
		public String? rcd_desc { get; set; }
		public String? time_estimate { get; set; }
		public String? amt_on_wo_flag { get; set; }
		public String? category_code { get; set; }
		public String? prereq_cat_code { get; set; }
		public String? multiple_quantity { get; set; }
		public String? nature_of_service_flag { get; set; }
		public String? rcd_chg { get; set; }
		public String? stop_date { get; set; }
		public String? rcd_new_chg { get; set; }
		public String? rcd_prorate_flag { get; set; }
		public String? discount_period { get; set; }
		public String? item_tax_method { get; set; }
		public String? conv_rqd_flag { get; set; }
		public String? serv_sum_code { get; set; }
		public String? charge_type { get; set; }
		public String? charge_method { get; set; }
		public String? prnt_zero_chg { get; set; }
		public String? adjsvc_switch { get; set; }
		public String? start_entry_date { get; set; }
		public String? end_entry_date { get; set; }
		public String? security_func { get; set; }
		public String? svc_classification { get; set; }
		public String? line_ind { get; set; }
		public String? trunk_chan { get; set; }
		public String? svp_delete { get; set; }
		public String? placement_cd { get; set; }
		public String? aiu_delete { get; set; }
		public String? nine_as_wash_group { get; set; }
		public String? omt_price_plan_id { get; set; }
		public String? credit_charge { get; set; }
		public String? tx_pre_dscnt_rt { get; set; }
		public String? sec_resource { get; set; }
		public String? solo_ind { get; set; }
		public String? item_continue_with_cat { get; set; }
		public String? rcm { get; set; }
		public String? statement_desc { get; set; }
		public String? product_type { get; set; }
		public String? min_install_chg { get; set; }
		public String? max_install_chg { get; set; }
		public String? unit_of_measure { get; set; }
		public String? class_sc { get; set; }
		public String? provisionable { get; set; }
		public String? cat_component { get; set; }
		public String? upsell_serv_flag { get; set; }
		public String? prorate_desc { get; set; }
		public String? wac_flag { get; set; }
		public String? subscript_period { get; set; }
		public String? cust_prem_wk { get; set; }
		public String? lead_family { get; set; }
		public String? min_days_to_sched { get; set; }
		public String? ear_dg_rest_svc { get; set; }
		public String? match_equip_id { get; set; }
		public String? time_est_by_blk_qty { get; set; }
		public String? blk_svc_split_method { get; set; }
		public String? dma_flag { get; set; }
		public String? vertex_tax_override { get; set; }
		public String? def_price_plan { get; set; }
		public String? extrnl_provision { get; set; }
		public String? extrnl_prvsion_id { get; set; }
		public String? de_exempt { get; set; }
		public String? svc_paired_dscnt { get; set; }
		public String? multi_profile { get; set; }
		public String? dup_assign { get; set; }
		public String? dscnt_track_cd { get; set; }
		public String? discnt_rank { get; set; }
		public String? fully_tax_flag { get; set; }
		public String? fully_tax_eff_date { get; set; }
		public String? statement_desc_01 { get; set; }
		public String? statement_desc_02 { get; set; }
		public String? statement_desc_03 { get; set; }
		public String? statement_desc_04 { get; set; }
		public String? statement_desc_05 { get; set; }

		public ccsr_services CreateBaseRec(SqlDataReader r)
		{
			ccsr_services n = new ccsr_services();

			if (!r.IsDBNull(0)) n.Id = r.GetInt32(0);
			if (!r.IsDBNull(1)) n.spaId = r.GetInt64(1);
			if (!r.IsDBNull(2)) n.service_code = r.GetString(2);
			if (!r.IsDBNull(3)) n.rcd_type = r.GetString(3);
			if (!r.IsDBNull(4)) n.top_twenty_flag = r.GetString(4);
			if (!r.IsDBNull(5)) n.rcd_desc = r.GetString(5);
			if (!r.IsDBNull(6)) n.time_estimate = r.GetString(6);
			if (!r.IsDBNull(7)) n.amt_on_wo_flag = r.GetString(7);
			if (!r.IsDBNull(8)) n.category_code = r.GetString(8);
			if (!r.IsDBNull(9)) n.prereq_cat_code = r.GetString(9);
			if (!r.IsDBNull(10)) n.multiple_quantity = r.GetString(10);
			if (!r.IsDBNull(11)) n.nature_of_service_flag = r.GetString(11);
			if (!r.IsDBNull(12)) n.rcd_chg = r.GetString(12);
			if (!r.IsDBNull(13)) n.stop_date = r.GetString(13);
			if (!r.IsDBNull(14)) n.rcd_new_chg = r.GetString(14);
			if (!r.IsDBNull(15)) n.rcd_prorate_flag = r.GetString(15);
			if (!r.IsDBNull(16)) n.discount_period = r.GetString(16);
			if (!r.IsDBNull(17)) n.item_tax_method = r.GetString(17);
			if (!r.IsDBNull(18)) n.conv_rqd_flag = r.GetString(18);
			if (!r.IsDBNull(19)) n.serv_sum_code = r.GetString(19);
			if (!r.IsDBNull(20)) n.charge_type = r.GetString(20);
			if (!r.IsDBNull(21)) n.charge_method = r.GetString(21);
			if (!r.IsDBNull(22)) n.prnt_zero_chg = r.GetString(22);
			if (!r.IsDBNull(23)) n.adjsvc_switch = r.GetString(23);
			if (!r.IsDBNull(24)) n.start_entry_date = r.GetString(24);
			if (!r.IsDBNull(25)) n.end_entry_date = r.GetString(25);
			if (!r.IsDBNull(26)) n.security_func = r.GetString(26);
			if (!r.IsDBNull(27)) n.svc_classification = r.GetString(27);
			if (!r.IsDBNull(28)) n.line_ind = r.GetString(28);
			if (!r.IsDBNull(29)) n.trunk_chan = r.GetString(29);
			if (!r.IsDBNull(30)) n.svp_delete = r.GetString(30);
			if (!r.IsDBNull(31)) n.placement_cd = r.GetString(31);
			if (!r.IsDBNull(32)) n.aiu_delete = r.GetString(32);
			if (!r.IsDBNull(33)) n.nine_as_wash_group = r.GetString(33);
			if (!r.IsDBNull(34)) n.omt_price_plan_id = r.GetString(34);
			if (!r.IsDBNull(35)) n.credit_charge = r.GetString(35);
			if (!r.IsDBNull(36)) n.tx_pre_dscnt_rt = r.GetString(36);
			if (!r.IsDBNull(37)) n.sec_resource = r.GetString(37);
			if (!r.IsDBNull(38)) n.solo_ind = r.GetString(38);
			if (!r.IsDBNull(39)) n.item_continue_with_cat = r.GetString(39);
			if (!r.IsDBNull(40)) n.rcm = r.GetString(40);
			if (!r.IsDBNull(41)) n.statement_desc = r.GetString(41);
			if (!r.IsDBNull(42)) n.product_type = r.GetString(42);
			if (!r.IsDBNull(43)) n.min_install_chg = r.GetString(43);
			if (!r.IsDBNull(44)) n.max_install_chg = r.GetString(44);
			if (!r.IsDBNull(45)) n.unit_of_measure = r.GetString(45);
			if (!r.IsDBNull(46)) n.class_sc = r.GetString(46);
			if (!r.IsDBNull(47)) n.provisionable = r.GetString(47);
			if (!r.IsDBNull(48)) n.cat_component = r.GetString(48);
			if (!r.IsDBNull(49)) n.upsell_serv_flag = r.GetString(49);
			if (!r.IsDBNull(50)) n.prorate_desc = r.GetString(50);
			if (!r.IsDBNull(51)) n.wac_flag = r.GetString(51);
			if (!r.IsDBNull(52)) n.subscript_period = r.GetString(52);
			if (!r.IsDBNull(53)) n.cust_prem_wk = r.GetString(53);
			if (!r.IsDBNull(54)) n.lead_family = r.GetString(54);
			if (!r.IsDBNull(55)) n.min_days_to_sched = r.GetString(55);
			if (!r.IsDBNull(56)) n.ear_dg_rest_svc = r.GetString(56);
			if (!r.IsDBNull(57)) n.match_equip_id = r.GetString(57);
			if (!r.IsDBNull(58)) n.time_est_by_blk_qty = r.GetString(58);
			if (!r.IsDBNull(59)) n.blk_svc_split_method = r.GetString(59);
			if (!r.IsDBNull(60)) n.dma_flag = r.GetString(60);
			if (!r.IsDBNull(61)) n.vertex_tax_override = r.GetString(61);
			if (!r.IsDBNull(62)) n.def_price_plan = r.GetString(62);
			if (!r.IsDBNull(63)) n.extrnl_provision = r.GetString(63);
			if (!r.IsDBNull(64)) n.extrnl_prvsion_id = r.GetString(64);
			if (!r.IsDBNull(65)) n.de_exempt = r.GetString(65);
			if (!r.IsDBNull(66)) n.svc_paired_dscnt = r.GetString(66);
			if (!r.IsDBNull(67)) n.multi_profile = r.GetString(67);
			if (!r.IsDBNull(68)) n.dup_assign = r.GetString(68);
			if (!r.IsDBNull(69)) n.dscnt_track_cd = r.GetString(69);
			if (!r.IsDBNull(70)) n.discnt_rank = r.GetString(70);
			if (!r.IsDBNull(71)) n.fully_tax_flag = r.GetString(71);
			if (!r.IsDBNull(72)) n.fully_tax_eff_date = r.GetString(72);
			if (!r.IsDBNull(73)) n.statement_desc_01 = r.GetString(73);
			if (!r.IsDBNull(74)) n.statement_desc_02 = r.GetString(74);
			if (!r.IsDBNull(75)) n.statement_desc_03 = r.GetString(75);
			if (!r.IsDBNull(76)) n.statement_desc_04 = r.GetString(76);
			if (!r.IsDBNull(77)) n.statement_desc_05 = r.GetString(77);

			return n;
		}
	}
}