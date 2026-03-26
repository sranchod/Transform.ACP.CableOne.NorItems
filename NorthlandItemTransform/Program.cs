using System;
using System.Threading;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Xml.Serialization;
using MtShellProgram;
using NorthlandItemTransform.Generated_Abstract_Classes;

namespace NorthlandItemTransform
{
	class program
	{
		private static Semaphore _pool;
		static void Main(string[] args)
		{
			DateTime startTime = DateTime.Now;
			Console.WriteLine("Program Start Time: {0}", startTime);
			if (args.Length == 0)
				throw new InvalidOperationException("Config File Missing");

			// Config File Example provided, change as needed
			String cfName = args[0].ToString();   // Config File is a Json File
			ConfigFile cf;
			try
			{
				cf = new ConfigFile().ReadConfigFile(cfName);
			}
			catch
			{
				throw new InvalidOperationException("Config File Missing or not found");
			}

			// Get Database from the argument if provided and override the config file.
			if (args.Length > 1)
			{
				cf.DatabaseName = args[1].ToString();
			}

			_pool = new Semaphore(initialCount: 0, maximumCount: cf.NumberofThreads);

			SqlConnection myCon;
			String myConnStr = string.Format(@"server={0};database={1};Integrated Security=SSPI", cf.ServerName, cf.DatabaseName);

      Console.WriteLine("Config File: {0}", cf.ToJson());

      // Truncate output tables
      TruncateTable(cf, cf.TempItemInterimTable);
			TruncateTable(cf, cf.ItemTable);

			RunMtParms rmpTemplate = new RunMtParms();
			rmpTemplate.ServerName = cf.ServerName;
			rmpTemplate.DbName = cf.DatabaseName;
			rmpTemplate.Splits = cf.NumberofSplits;
			rmpTemplate.CcsrServicesTable = cf.CcsrServicesTable;
			rmpTemplate.CcsrPackagesTable = cf.CcsrPackagesTable;
			rmpTemplate.CcsrPackageItemsTable = cf.CcsrPackageItemsTable;
			rmpTemplate.CodeTablePt = cf.CodeTablePt;
			rmpTemplate.BreadCrumbTable = cf.BreadCrumbTable;
			rmpTemplate.RuleToTermsTable = cf.RuleToTermsTable;
			rmpTemplate.SiteDatesTable = cf.SiteDatesTable;
			rmpTemplate.ItemTable = cf.ItemTable;
			rmpTemplate.ForeignRateCodesTable = cf.ForeignRateCodesTable;
			rmpTemplate.TempItemInterimTable = cf.TempItemInterimTable;
			rmpTemplate.SubscriberTable = cf.SubscriberTable;
			rmpTemplate.XrefTable = cf.XrefTable;
			rmpTemplate.ItemContractCodesTable = cf.ItemContractCodesTable;
			rmpTemplate.ItemDirectoryListingSvcsTable = cf.ItemDirectoryListingSvcsTable;
			rmpTemplate.ItemAoCodesTable = cf.ItemAoCodesTable;
			rmpTemplate.ItemAoCodesTable_t2 = cf.ItemAoCodesTable_t2;
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.siteDates = site_spc_site_dates.LoadTable(myCon, rmpTemplate);
			Console.WriteLine("Num records in site dates: {0}", rmpTemplate.siteDates.Count);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.ruleTos = ruleng_rule_toterms.LoadTable(myCon, rmpTemplate);
			Console.WriteLine("Num records in ruleTos: {0}", rmpTemplate.ruleTos.Count);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.svc9xx = ccsr_services.LoadTable(myCon, rmpTemplate);
			Console.WriteLine("Num records in 9xx services: {0}", rmpTemplate.svc9xx.Count);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.cdtPt = dbo_vw_udct_tbl_pt.LoadTable(myCon, rmpTemplate);
			Console.WriteLine("Num records in Code Table PT: {0}", rmpTemplate.cdtPt.Count);
			myCon = new SqlConnection(myConnStr);
			List<ccsr_packages> pkgs = ccsr_packages.LoadTable(myCon, rmpTemplate);
			myCon = new SqlConnection(myConnStr);
			List<ccsr_package_item> pkgsItm = ccsr_package_item.LoadTable(myCon, rmpTemplate);
			rmpTemplate.CcsPkgs = ccsr_package_full.CombineCcsrPackages(pkgs, pkgsItm, rmpTemplate.svc9xx);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.PdbCatalogRoots = trn_item_pdb_catalog_roots.LoadTable(myCon, cf.ItemPdbRootsTable);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.PdbCatalogBranches = trn_item_pdb_catalog_branches.LoadTable(myCon, cf.ItemPdbBranchesTable);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.PdbCatalogLeaves = trn_item_pdb_catalog_leaves.LoadTable(myCon, cf.ItemPdbLeavesTable);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.PdbCatalogAlone = trn_item_pdb_catalog_alone.LoadTable(myCon, cf.ItemPdbAloneTable);
			myCon = new SqlConnection(myConnStr);
			rmpTemplate.SpecItemScrunchCodes = spec_item_scrunch_codes.LoadTable(myCon, cf.SpecScrunchTable);

			rmpTemplate.Thread = 0;

			RunMtParms rmp;
			for (Int32 i = 0; i < cf.NumberofSplits; i++)
			{ 
				rmp = new RunMtParms();
				rmp = rmp.Copy(rmpTemplate, rmp);
				rmp.Thread = i + 1;
				Thread t = new Thread(new ParameterizedThreadStart(RunMt));
				t.Start(rmp);
			}

			Thread.Sleep(1000);  // Sleep for 1 second to give the threads a chance to all get queued up

			Console.WriteLine("Releasing {0} Threads.", cf.NumberofThreads);
			_pool.Release(releaseCount: cf.NumberofThreads);

			Console.WriteLine("Program done.");
		}

		private static void RunMt(object rmp)
		{
			ThreadedRunClass tr = new ThreadedRunClass();
			RunMtParms rmp2 = new RunMtParms();
			rmp2 = (RunMtParms)rmp;

			Console.WriteLine("Thread {0} begins " +
					"and waits for the semaphore.", rmp2.Thread);

			_pool.WaitOne();

			// Execute the new multi-threaded class here
			Console.WriteLine("Thread {0} enters the semaphore.", rmp2.Thread);
			tr.JobRun(rmp2);
			Console.WriteLine("{2}: Thread {0} finished.  Previous semaphore count: {1}", rmp2.Thread, _pool.Release(), DateTime.Now);
		}

		private static void TruncateTable(ConfigFile cf, String tableName)
		{
			string myConnStr = string.Format(@"server={0};database={1};Integrated Security=SSPI", cf.ServerName, cf.DatabaseName);
			SqlConnection myConn = new SqlConnection(myConnStr);
			myConn.Open();

			String myQ = string.Format(@"
truncate table {0};
", tableName);

			SqlCommand cmd = myConn.CreateCommand();
			cmd.CommandText = myQ;
			cmd.ExecuteNonQuery();

			myConn.Close();
		}
	}
}