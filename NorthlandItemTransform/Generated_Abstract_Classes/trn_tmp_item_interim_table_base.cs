using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
  public abstract class trn_tmp_item_interim_table_base
  {
    public Int64 spa_id { get; set; }
    public String csg_sys { get; set; }
    public String csg_prin { get; set; }
    public String csg_agent { get; set; }
    public String csg_account_number { get; set; }
    public String bill_code { get; set; }
    public String service_code { get; set; }
    public String discount_code { get; set; }
    public String class_code { get; set; }
    public String customer_discount_code { get; set; }
    public Int32? pkg_item_no { get; set; }
    public Int64 bid { get; set; }
    public Int64 sid { get; set; }
    public Int64? psid { get; set; }
    public Int32 svc_qty { get; set; }
    public String? nature_of_service_flag { get; set; }
    public String? charge_type { get; set; }
    public String? charge_method { get; set; }
    public String? product_type { get; set; }
    public String? converter_required_flag { get; set; }
    public String? provisionable_flag { get; set; }
    public String? reference { get; set; }
    public String is_active_subscriber { get; set; }
    public DateTime? connect_date { get; set; }
    public DateTime? discount_start_date { get; set; }
    public Decimal? charge_rate { get; set; }
    public Int32? rules_engine_pass { get; set; }
    public Int32? rule_id { get; set; }
    public Int32? match_id { get; set; }
    public String? lob { get; set; }
    public String? csg_campaign { get; set; }
    public String? credit_charge { get; set; }
    public String csg_customer { get; set; }
    public String csg_house { get; set; }
    public String? unit_of_measure { get; set; }
    public String? subscript_period { get; set; }

    public trn_tmp_item_interim_table CreateBaseRec(SqlDataReader r)
    {
      trn_tmp_item_interim_table n = new trn_tmp_item_interim_table();
  
      if (!r.IsDBNull(0)) n.spa_id = r.GetInt64(0);
      if (!r.IsDBNull(1)) n.csg_sys = r.GetString(1);
      if (!r.IsDBNull(2)) n.csg_prin = r.GetString(2);
      if (!r.IsDBNull(3)) n.csg_agent = r.GetString(3);
      if (!r.IsDBNull(4)) n.csg_account_number = r.GetString(4);
      if (!r.IsDBNull(5)) n.bill_code = r.GetString(5);
      if (!r.IsDBNull(6)) n.service_code = r.GetString(6);
      if (!r.IsDBNull(7)) n.discount_code = r.GetString(7);
      if (!r.IsDBNull(8)) n.class_code = r.GetString(8);
      if (!r.IsDBNull(9)) n.customer_discount_code = r.GetString(9);
      if (!r.IsDBNull(10)) n.pkg_item_no = r.GetInt32(10);
      if (!r.IsDBNull(11)) n.bid = r.GetInt64(11);
      if (!r.IsDBNull(12)) n.sid = r.GetInt64(12);
      if (!r.IsDBNull(13)) n.psid = r.GetInt64(13);
      if (!r.IsDBNull(14)) n.svc_qty = r.GetInt32(14);
      if (!r.IsDBNull(15)) n.nature_of_service_flag = r.GetString(15);
      if (!r.IsDBNull(16)) n.charge_type = r.GetString(16);
      if (!r.IsDBNull(17)) n.charge_method = r.GetString(17);
      if (!r.IsDBNull(18)) n.product_type = r.GetString(18);
      if (!r.IsDBNull(19)) n.converter_required_flag = r.GetString(19);
      if (!r.IsDBNull(20)) n.provisionable_flag = r.GetString(20);
      if (!r.IsDBNull(21)) n.reference = r.GetString(21);
      if (!r.IsDBNull(22)) n.is_active_subscriber = r.GetString(22);
      if (!r.IsDBNull(23)) n.connect_date = r.GetDateTime(23);
      if (!r.IsDBNull(24)) n.discount_start_date = r.GetDateTime(24);
      if (!r.IsDBNull(25)) n.charge_rate = r.GetDecimal(25);
      if (!r.IsDBNull(26)) n.rules_engine_pass = r.GetInt32(26);
      if (!r.IsDBNull(27)) n.rule_id = r.GetInt32(27);
      if (!r.IsDBNull(28)) n.match_id = r.GetInt32(28);
      if (!r.IsDBNull(29)) n.lob = r.GetString(29);
      if (!r.IsDBNull(30)) n.csg_campaign = r.GetString(30);
      if (!r.IsDBNull(31)) n.credit_charge = r.GetString(31);
      if (!r.IsDBNull(32)) n.csg_customer = r.GetString(32);
      if (!r.IsDBNull(33)) n.csg_house = r.GetString(33);
      if (!r.IsDBNull(34)) n.unit_of_measure = r.GetString(34);
      if (!r.IsDBNull(35)) n.subscript_period = r.GetString(35);

      return n;
    }
  }
}