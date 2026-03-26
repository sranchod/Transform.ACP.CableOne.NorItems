using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NorthlandItemTransform;

namespace NorthlandItemTransform
{
	public class RunMtParms
	{
		public String ServerName { get; set; }
		public String DbName { get; set; }
		public Int32 Splits { get; set; }
		public Int32 Thread { get; set; }
		public String SubscriberTable { get; set; }
		public String BreadCrumbTable { get; set; }	
		public String ForeignRateCodesTable { get; set; }
		public String RuleToTermsTable { get; set; }	
		public String CcsrServicesTable { get; set; }
		public String CcsrPackagesTable { get; set; }
		public String CcsrPackageItemsTable { get; set; }
		public String CodeTablePt { get; set; }
		public String SiteDatesTable { get; set; }
		public String TempItemInterimTable { get; set; }
		public String ItemTable { get; set; }
		public String XrefTable { get; set; }
		public String ItemContractCodesTable { get; set; }
		public String ItemDirectoryListingSvcsTable { get; set; }
		public String ItemAoCodesTable { get; set; }
		public String ItemAoCodesTable_t2 { get; set; }
		public List<site_spc_site_dates> siteDates { get; set; }
		public List<ruleng_rule_toterms> ruleTos { get; set; }
		public List<ccsr_services> svc9xx { get; set; }
		public List<dbo_vw_udct_tbl_pt> cdtPt { get; set; }
		public List<ccsr_package_full> CcsPkgs { get; set; }
		public List<trn_item_pdb_catalog_roots> PdbCatalogRoots { get; set; }
		public List<trn_item_pdb_catalog_branches> PdbCatalogBranches { get; set; }
		public List<trn_item_pdb_catalog_leaves> PdbCatalogLeaves { get; set; }
		public List<trn_item_pdb_catalog_alone> PdbCatalogAlone { get; set; }
		public List<spec_item_scrunch_codes> SpecItemScrunchCodes { get; set; }


		public RunMtParms Copy(RunMtParms source, RunMtParms dest)
		{
			RunMtParms destRet = dest;
			foreach (PropertyInfo property in typeof(RunMtParms).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}
	}
}
