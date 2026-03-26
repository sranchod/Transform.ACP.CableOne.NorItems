using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class spec_item_scrunch_codes_base
	{
		public Int32 excel_row { get; set; }
		public String? record_description { get; set; }
		public String? scrunch_svc { get; set; }

		public spec_item_scrunch_codes CreateBaseRec(SqlDataReader r)
		{
			spec_item_scrunch_codes n = new spec_item_scrunch_codes();

			if (!r.IsDBNull(0)) n.excel_row = r.GetInt32(0);
			if (!r.IsDBNull(1)) n.record_description = r.GetString(1);
			if (!r.IsDBNull(2)) n.scrunch_svc = r.GetString(2);

			return n;
		}
	}
}