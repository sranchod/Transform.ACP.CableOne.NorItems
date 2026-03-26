using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
  public abstract class ruleng_rule_toterms_base
  {
    public Int32 Id { get; set; }
    public Int32 RuleId { get; set; }
    public String csg_svc { get; set; }
    public String csg_disc { get; set; }
    public String csg_cust_disc { get; set; }
		public String? point_code { get; set; }

		public ruleng_rule_toterms CreateBaseRec(SqlDataReader r)
    {
      ruleng_rule_toterms n = new ruleng_rule_toterms();
  
      if (!r.IsDBNull(0)) n.Id = r.GetInt32(0);
      if (!r.IsDBNull(1)) n.RuleId = r.GetInt32(1);
      if (!r.IsDBNull(2)) n.csg_svc = r.GetString(2);
      if (!r.IsDBNull(3)) n.csg_disc = r.GetString(3);
      if (!r.IsDBNull(4)) n.csg_cust_disc = r.GetString(4);
			if (!r.IsDBNull(5)) n.point_code = r.GetString(5);

			return n;
    }
  }
}