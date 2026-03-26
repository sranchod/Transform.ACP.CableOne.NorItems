using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
  public abstract class ruleng_breadcrumb_base
  {
    public Int32 Id { get; set; }
    public Int32 MatchId { get; set; }
    public String Group0 { get; set; }
    public Int32 RuleTermid { get; set; }
    public Int32 RuleBaseId { get; set; }
    public Int32 Pass { get; set; }
    public Int32 TimesMatched { get; set; }
    public Int32 FactId { get; set; }

    public ruleng_breadcrumb CreateBaseRec(SqlDataReader r)
    {
      ruleng_breadcrumb n = new ruleng_breadcrumb();
  
      if (!r.IsDBNull(0)) n.Id = r.GetInt32(0);
      if (!r.IsDBNull(1)) n.MatchId = r.GetInt32(1);
      if (!r.IsDBNull(2)) n.Group0 = r.GetString(2);
      if (!r.IsDBNull(3)) n.RuleTermid = r.GetInt32(3);
      if (!r.IsDBNull(4)) n.RuleBaseId = r.GetInt32(4);
      if (!r.IsDBNull(5)) n.Pass = r.GetInt32(5);
      if (!r.IsDBNull(6)) n.TimesMatched = r.GetInt32(6);
      if (!r.IsDBNull(7)) n.FactId = r.GetInt32(7);

      return n;
    }
  }
}