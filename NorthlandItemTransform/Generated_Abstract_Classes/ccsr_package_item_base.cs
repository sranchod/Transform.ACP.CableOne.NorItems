using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class ccsr_package_item_base
	{
		public Int32 Id { get; set; }
		public Int32 packageId { get; set; }
		public Int32 servicesId { get; set; }
		public Int32 pkg_item_no { get; set; }
		public Int32 serv_chg { get; set; }
		public Int32 new_chg { get; set; }

		public ccsr_package_item CreateBaseRec(SqlDataReader r)
		{
			ccsr_package_item n = new ccsr_package_item();

			if (!r.IsDBNull(0)) n.Id = r.GetInt32(0);
			if (!r.IsDBNull(1)) n.packageId = r.GetInt32(1);
			if (!r.IsDBNull(2)) n.servicesId = r.GetInt32(2);
			if (!r.IsDBNull(3)) n.pkg_item_no = r.GetInt32(3);
			if (!r.IsDBNull(4)) n.serv_chg = r.GetInt32(4);
			if (!r.IsDBNull(5)) n.new_chg = r.GetInt32(5);

			return n;
		}
	}
}
