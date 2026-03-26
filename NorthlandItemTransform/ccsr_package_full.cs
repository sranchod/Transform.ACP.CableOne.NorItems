using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthlandItemTransform
{
	public class ccsr_package_full
	{
		public ccsr_packages Package { get; set; }
		public ccsr_package_item PackageItem { get; set; }
		public ccsr_services Service { get; set; }

		public static List<ccsr_package_full> CombineCcsrPackages(List<ccsr_packages> pkg9xx, List<ccsr_package_item> pkgi9xx, List<ccsr_services> svc9xx)
		{
			var comboRec = (from pkg in pkg9xx
											join pki in pkgi9xx on pkg.Id equals pki.packageId
											select new { pkg, pki } into pkgs
											join svc in svc9xx on pkgs.pki.servicesId equals svc.Id
											select new { pkgs.pkg, pkgs.pki, svc }
										).ToList();
			List<ccsr_package_full> ccsPkgs = new List<ccsr_package_full>();
			ccsr_package_full ccsPkg;
			foreach (var p in comboRec)
			{
				ccsPkg = new ccsr_package_full();
				ccsPkg.Package = p.pkg;
				ccsPkg.PackageItem = p.pki;
				ccsPkg.Service = p.svc;
				ccsPkg.Service.rcd_chg = ccsPkg.PackageItem.serv_chg.ToString();
				ccsPkg.Service.rcd_new_chg = ccsPkg.PackageItem.new_chg.ToString();
				ccsPkgs.Add(ccsPkg);
			}
			return ccsPkgs;
		}
	}
}
