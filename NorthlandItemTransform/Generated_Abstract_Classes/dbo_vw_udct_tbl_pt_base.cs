using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
  public abstract class dbo_vw_udct_tbl_pt_base
  {
    public Int64 spaId { get; set; }
    public String code_table { get; set; }
    public String code_table_value { get; set; }
    public String? strt_date { get; set; }
    public String? stop_date { get; set; }
    public String? freq_use { get; set; }
    public String? spec_hndl { get; set; }
    public String? desc { get; set; }
    public String? assign { get; set; }
    public String? line_o_bus { get; set; }
    public String? addl_data { get; set; }
    public String? rated_use { get; set; }
    public String? analog { get; set; }
    public String? digital { get; set; }
    public String? trbl_id { get; set; }
    public String? tpt_late_chg_comtx_cat { get; set; }
    public String? tpt_late_chg_comtx_svc { get; set; }
    public String? tpt_event_commtax_cat { get; set; }
    public String? tpt_event_commtax_svc { get; set; }

    public dbo_vw_udct_tbl_pt CreateBaseRec(SqlDataReader r)
    {
      dbo_vw_udct_tbl_pt n = new dbo_vw_udct_tbl_pt();
  
      if (!r.IsDBNull(0)) n.spaId = r.GetInt64(0);
      if (!r.IsDBNull(1)) n.code_table = r.GetString(1);
      if (!r.IsDBNull(2)) n.code_table_value = r.GetString(2);
      if (!r.IsDBNull(3)) n.strt_date = r.GetString(3);
      if (!r.IsDBNull(4)) n.stop_date = r.GetString(4);
      if (!r.IsDBNull(5)) n.freq_use = r.GetString(5);
      if (!r.IsDBNull(6)) n.spec_hndl = r.GetString(6);
      if (!r.IsDBNull(7)) n.desc = r.GetString(7);
      if (!r.IsDBNull(8)) n.assign = r.GetString(8);
      if (!r.IsDBNull(9)) n.line_o_bus = r.GetString(9);
      if (!r.IsDBNull(10)) n.addl_data = r.GetString(10);
      if (!r.IsDBNull(11)) n.rated_use = r.GetString(11);
      if (!r.IsDBNull(12)) n.analog = r.GetString(12);
      if (!r.IsDBNull(13)) n.digital = r.GetString(13);
      if (!r.IsDBNull(14)) n.trbl_id = r.GetString(14);
      if (!r.IsDBNull(15)) n.tpt_late_chg_comtx_cat = r.GetString(15);
      if (!r.IsDBNull(16)) n.tpt_late_chg_comtx_svc = r.GetString(16);
      if (!r.IsDBNull(17)) n.tpt_event_commtax_cat = r.GetString(17);
      if (!r.IsDBNull(18)) n.tpt_event_commtax_svc = r.GetString(18);

      return n;
    }
  }
}