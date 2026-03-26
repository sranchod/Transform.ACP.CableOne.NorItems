using Microsoft.Data.SqlClient;
using Newtonsoft.Json;

namespace NorthlandItemTransform.Generated_Abstract_Classes
{
	public abstract class trn_tmp_foreign_rate_codes_base
	{
		public Int64 id { get; set; }
		public String ccs_subscriber { get; set; }
		public String ccs_customer { get; set; }
		public String ccs_house { get; set; }
		public Int32? SUBS { get; set; }
		public String? PKGCODE { get; set; }
		public String? FRANCHISE { get; set; }
		public Int32? TMP_SUBPKGINFO { get; set; }
		public String? PKGDESCRIP { get; set; }
		public Int32? QTY { get; set; }
		public DateTime? SPASTARTDATE { get; set; }
		public DateTime? SPAENDDATE { get; set; }
		public Int32? SUBPKG_IS_ACTIVE { get; set; }
		public Int32? ISCUSTOMRATE { get; set; }
		public Decimal? AMTFIRST { get; set; }
		public Decimal? AMTEXTRA { get; set; }
		public Decimal? total_charge { get; set; }
		public Int64? pkg_code_order { get; set; }
		public String? fgn_com_res { get; set; }
		public Int64? spa_id { get; set; }
		public String? PHONENUMBER { get; set; }
		public String? tn_CONTACT_DESCRIP { get; set; }
		public DateTime? discount_start_date { get; set; }
		public Decimal? bulk_rate { get; set; }
		public String? source_table { get; set; }

		public trn_tmp_foreign_rate_codes CreateBaseRec(SqlDataReader r)
		{
			trn_tmp_foreign_rate_codes n = new trn_tmp_foreign_rate_codes();

			if (!r.IsDBNull(0)) n.id = r.GetInt64(0);
			if (!r.IsDBNull(1)) n.ccs_subscriber = r.GetString(1);
			if (!r.IsDBNull(2)) n.ccs_customer = r.GetString(2);
			if (!r.IsDBNull(3)) n.ccs_house = r.GetString(3);
			if (!r.IsDBNull(4)) n.SUBS = r.GetInt32(4);
			if (!r.IsDBNull(5)) n.PKGCODE = r.GetString(5);
			if (!r.IsDBNull(6)) n.FRANCHISE = r.GetString(6);
			if (!r.IsDBNull(7)) n.TMP_SUBPKGINFO = r.GetInt32(7);
			if (!r.IsDBNull(8)) n.PKGDESCRIP = r.GetString(8);
			if (!r.IsDBNull(9)) n.QTY = r.GetInt32(9);
			if (!r.IsDBNull(10)) n.SPASTARTDATE = r.GetDateTime(10);
			if (!r.IsDBNull(11)) n.SPAENDDATE = r.GetDateTime(11);
			if (!r.IsDBNull(12)) n.SUBPKG_IS_ACTIVE = r.GetInt32(12);
			if (!r.IsDBNull(13)) n.ISCUSTOMRATE = r.GetInt32(13);
			if (!r.IsDBNull(14)) n.AMTFIRST = r.GetDecimal(14);
			if (!r.IsDBNull(15)) n.AMTEXTRA = r.GetDecimal(15);
			if (!r.IsDBNull(16)) n.total_charge = r.GetDecimal(16);
			if (!r.IsDBNull(17)) n.pkg_code_order = r.GetInt64(17);
			if (!r.IsDBNull(18)) n.fgn_com_res = r.GetString(18);
			if (!r.IsDBNull(19)) n.spa_id = r.GetInt64(19);
			if (!r.IsDBNull(20)) n.PHONENUMBER = r.GetString(20);
			if (!r.IsDBNull(21)) n.tn_CONTACT_DESCRIP = r.GetString(21);
			if (!r.IsDBNull(22)) n.discount_start_date = r.GetDateTime(22);
			if (!r.IsDBNull(23)) n.bulk_rate = r.GetDecimal(23);
			if (!r.IsDBNull(24)) n.source_table = r.GetString(24);

			return n;
		}
	}
}