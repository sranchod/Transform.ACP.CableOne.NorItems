using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthlandItemTransform
{
	public class ConfigFile
	{
		public String ServerName { get; set; }
		public String DatabaseName { get; set; }
		public Int32 NumberofSplits { get; set; }
		public Int32 NumberofThreads { get; set; }
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
		public String ItemPdbRootsTable { get; set; }
		public String ItemPdbBranchesTable { get; set; }
		public String ItemPdbLeavesTable { get; set; }
		public String ItemPdbAloneTable { get; set; }
		public String SpecScrunchTable { get; set; }
		public String ItemContractCodesTable { get; set; }
		public String ItemAoCodesTable { get; set; }
		public String ItemAoCodesTable_t2 { get; set; }
		public String ItemDirectoryListingSvcsTable { get; set; }

		public string ToJson()
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(this);
		}

		public ConfigFile FromJson(string data)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigFile>(data);
		}

		public ConfigFile ReadConfigFile(string file)
		{
			String nl = "";
			String cNl = "";

			using (StreamReader sr = File.OpenText(file))
			{
				while ((nl = sr.ReadLine()) != null)
				{
					cNl = cNl + nl;
				}
			}
			ConfigFile cf = new ConfigFile();
			return cf.FromJson(cNl);
		}
	}
}
