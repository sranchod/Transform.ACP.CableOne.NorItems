using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class ruleng_rule_toterms : Generated_Abstract_Classes.ruleng_rule_toterms_base
	{
		public ruleng_rule_toterms Copy2(ruleng_rule_toterms source)
		{
			ruleng_rule_toterms destRet = new ruleng_rule_toterms();

			foreach (PropertyInfo property in typeof(ruleng_rule_toterms).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}

			return destRet;
		}

		public static List<ruleng_rule_toterms> LoadTable(SqlConnection myCon, RunMtParms rmp)
		{
			myCon.Open();

			List<ruleng_rule_toterms> rt = new List<ruleng_rule_toterms>();
			ruleng_rule_toterms saHandler = new ruleng_rule_toterms();

			String Query = string.Format(@"
select a.*
from {0} as a
order by a.id;", rmp.RuleToTermsTable);

			using (myCon)
			using (var cmd = new SqlCommand(Query, myCon))
			{
				using (var rdr = cmd.ExecuteReader())
				{
					cmd.CommandTimeout = 0;
					while (rdr.Read())
					{
						saHandler = new ruleng_rule_toterms().CreateBaseRec(rdr);
						rt.Add(saHandler);
					}
				}
			}

			myCon.Close();

			return rt;
		}
	}
}
