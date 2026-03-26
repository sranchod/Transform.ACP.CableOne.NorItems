using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
  public abstract class site_spc_site_dates_base
  {
    public Int32 id { get; set; }
    public String site_id { get; set; }
    public String spa_option { get; set; }
    public String default_equip_spa_option { get; set; }
    public String xref_digits { get; set; }
    public DateTime merge_date { get; set; }
    public DateTime foreign_cutoff_date { get; set; }
    public DateTime cycle_from_date { get; set; }
    public DateTime cycle_to_date { get; set; }
    public DateTime? addressable_cutover_date { get; set; }
    public DateTime? true_cutoff_date { get; set; }

    public site_spc_site_dates CreateBaseRec(SqlDataReader r)
    {
      site_spc_site_dates n = new site_spc_site_dates();
  
      if (!r.IsDBNull(0)) n.id = r.GetInt32(0);
      if (!r.IsDBNull(1)) n.site_id = r.GetString(1);
      if (!r.IsDBNull(2)) n.spa_option = r.GetString(2);
      if (!r.IsDBNull(3)) n.default_equip_spa_option = r.GetString(3);
      if (!r.IsDBNull(4)) n.xref_digits = r.GetString(4);
      if (!r.IsDBNull(5)) n.merge_date = r.GetDateTime(5);
      if (!r.IsDBNull(6)) n.foreign_cutoff_date = r.GetDateTime(6);
      if (!r.IsDBNull(7)) n.cycle_from_date = r.GetDateTime(7);
      if (!r.IsDBNull(8)) n.cycle_to_date = r.GetDateTime(8);
      if (!r.IsDBNull(9)) n.addressable_cutover_date = r.GetDateTime(9);
      if (!r.IsDBNull(10)) n.true_cutoff_date = r.GetDateTime(10);

      return n;
    }
  }
}