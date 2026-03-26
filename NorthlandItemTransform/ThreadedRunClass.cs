using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class ThreadedRunClass
	{
		public void JobRun(RunMtParms rmp)
		{
			// Create SQL Connection
			String myConnStr = string.Format(@"server={0};database={1};Integrated Security=SSPI", rmp.ServerName, rmp.DbName);
			SqlConnection myCon;

			// Get Foreign Rate Codes
			myCon = new SqlConnection(myConnStr);
			List<trn_tmp_foreign_rate_codes> frcList = trn_tmp_foreign_rate_codes.LoadTable(myCon, rmp);

			// Get Breadcrumbs from RE
			myCon = new SqlConnection(myConnStr);
			List<ruleng_breadcrumb> brdcrmbList = ruleng_breadcrumb.LoadTable(myCon, rmp);

			// Get Subscriber Base Field Extract
			myCon = new SqlConnection(myConnStr);
			List<trn_sub_base_extract> subs = trn_sub_base_extract.LoadTable(myCon, rmp);

			// Get the Item Contract Codes Table
			myCon = new SqlConnection(myConnStr);
			List<trn_item_contract_codes> contracts = trn_item_contract_codes.LoadTable(myCon, rmp);

			// Get the Item A/O Codes Table
			myCon = new SqlConnection(myConnStr);
			List<trn_item_ao_codes_to_add> aoCodes = trn_item_ao_codes_to_add.LoadTable(myCon, rmp, 1);

			// Get the Item Directory Listing Svc Table
			myCon = new SqlConnection(myConnStr);
			List<trn_item_directory_listing_codes_to_add> dirListCodes = trn_item_directory_listing_codes_to_add.LoadTable(myCon, rmp);

			// Get the other Item A/O Codes Table using the same routines
			myCon = new SqlConnection(myConnStr);
			List<trn_item_ao_codes_to_add> aoCodes_t2 = trn_item_ao_codes_to_add.LoadTable(myCon, rmp, 2);

			// Create Item Interim Table from RE Matches
			List<trn_tmp_item_interim_table> itmIs = GetInfoForBreadcrumb(frcList, brdcrmbList, rmp.ruleTos, rmp.svc9xx, rmp.cdtPt, rmp.CcsPkgs, subs);

			// Update for the Bulk Codes
			itmIs = BulksUpdate(itmIs, brdcrmbList, subs);

			// Update the Psids
			UpdatePsids(itmIs, rmp);

			// Create the High Sid Bid List to use for Misc Code Adds
			List<customer_high_sid_bids> cHsb = customer_high_sid_bids.GenerateHighSidBidList(itmIs);

			// Updates for specific Customers and Subs goes here
			MiscItemDirListingCodes(dirListCodes, subs, itmIs, rmp.svc9xx, rmp.cdtPt, rmp.CcsPkgs, cHsb);

			MiscScrunch(itmIs, rmp.SpecItemScrunchCodes);

			MiscAddContractCodes(contracts, subs, itmIs, rmp.svc9xx, rmp.cdtPt, rmp.CcsPkgs, cHsb);

			AddVg001Svc(itmIs, subs, rmp.svc9xx, rmp.cdtPt, rmp.CcsPkgs, cHsb);

			MiscAddAoCodes(aoCodes, subs, itmIs, rmp.svc9xx, rmp.cdtPt, rmp.CcsPkgs, cHsb);
			MiscAddAoCodes_t2(aoCodes_t2, subs, itmIs, rmp.svc9xx, rmp.cdtPt, rmp.CcsPkgs, cHsb);

			DiscountAllItemsForSelectVips(itmIs, subs);

			// Fix the Bids, Sids, and Psids before writing the records out
			FixBidsAndSids(itmIs);

			// Write out trn.tmp_item_interim_table
			WriteTempItemInterimRecords(itmIs, myConnStr, rmp);

			// Write oht the trm.item table
			WriteItemsFromItemInterimTable(itmIs, subs, myConnStr, rmp);
		}
		private List<trn_tmp_item_interim_table> GetInfoForBreadcrumb(List<trn_tmp_foreign_rate_codes> frcList, List<ruleng_breadcrumb> brdcrmbList,
																	List<ruleng_rule_toterms> ruleTos, List<ccsr_services> svc9xx, List<dbo_vw_udct_tbl_pt> cdtPt,
																	List<ccsr_package_full> ccsPkgs, List<trn_sub_base_extract> subExtract)
		{
			// Combine Facts with Breadcrumbs
			var comboRec = (from fact in frcList
											join brdcrumb in brdcrmbList on fact.id equals brdcrumb.FactId
											join subs in subExtract on fact.ccs_subscriber equals subs.account_number
											select new { fact, brdcrumb, subs }).ToList();

			// Group by Match Ids
			var matchIds = (from cr in comboRec
											group cr by new
											{
												cr.brdcrumb.MatchId,
												cr.brdcrumb.RuleBaseId,
												cr.fact.ccs_customer,
												cr.fact.ccs_house,
												cr.fact.ccs_subscriber,
												cr.fact.spa_id,
												cr.fact.spaSysLvl,
												cr.fact.spaPrinLvl,
												cr.subs.vip_flag
											} into g
											select new
											{
												matchId = g.Key.MatchId,
												ruleBaseId = g.Key.RuleBaseId,
												spaId = g.Key.spa_id,
												spaSysId = g.Key.spaSysLvl,
												spaPrinId = g.Key.spaPrinLvl,
												ccs_customer = g.Key.ccs_customer,
												ccs_house = g.Key.ccs_house,
												ccs_subscriber = g.Key.ccs_subscriber,
												connectDate = g.Max(cr => cr.fact.SPASTARTDATE),
												discountStartDate = g.Min(cr => (cr.fact.discount_start_date != null) ? cr.fact.discount_start_date : Convert.ToDateTime("9999-12-31")),
												chargeAmount = g.Sum(cr => cr.fact.bulk_rate / cr.fact.QTY),
												phoneNo = g.Max(cr => cr.fact.PHONENUMBER),
												svc_qty = 1,
												vip_flag = g.Key.vip_flag
											}).ToList();

			// inner join
			var csgSide = (from mi in matchIds
										 join rt in ruleTos on mi.ruleBaseId equals rt.RuleId
										 select new { mi, rt }).ToList();

			// left join the 9xx
			var add9xx = (from c in csgSide
										join x9s in svc9xx on new { a1 = Convert.ToInt64(c.mi.spaId), a2 = c.rt.csg_svc } equals new { a1 = x9s.spaId, a2 = x9s.service_code } into j
										from j2 in j.DefaultIfEmpty()
										select new { c, pi = 0, j2 }).ToList();

			// join and expand from 9xx packages, remember: It's not possible to have both a service_code defined both as a package and as a service
			var add9xxP = (from xs in add9xx
										 join x9p in ccsPkgs on new { a1 = Convert.ToInt64(xs.c.mi.spaId), a2 = xs.c.rt.csg_svc } equals new { a1 = x9p.Package.spaId, a2 = x9p.Package.service_code } into jp
										 from j2p in jp.DefaultIfEmpty()
										 select new { c = xs.c, pi = j2p != null ? j2p.PackageItem.pkg_item_no : 0, j2 = j2p != null ? j2p.Service : xs.j2 }).ToList();

			// left join the Code Table PT
			var addPt = (from x in add9xxP
									 orderby x.c.mi.ccs_customer, x.c.mi.ccs_subscriber, x.c.mi.matchId, x.c.rt.Id, x.pi
									 join pt in cdtPt on new { a1 = x.c.mi.spaSysId, a2 = x.j2 == null ? "" : x.j2.product_type } equals new { a1 = pt.spaId, a2 = pt.code_table_value } into j
									 from j2 in j.DefaultIfEmpty()
									 select new { itemWrk = x.c.mi, ruleTo = x.c.rt, pkgItemNo = x.pi, s9xx = x.j2, LOB = j2 == null ? "" : j2.line_o_bus }).ToList();

			// Item Interim Work Fields
			List<trn_tmp_item_interim_table> itmIs = new List<trn_tmp_item_interim_table>();
			trn_tmp_item_interim_table newItmI;
			String HoldCustomer = "";
			Int64 HoldSid = 0;
			Int64 HoldBid = 0;
			Int32 HoldPackageItemNo = 0;
			// Use this for putting the bulk amount on just the first bulk code
			Int32 HoldMatchId = 0;
			bool FirstBulkInMatchDone = false;

			// Populate the Item Interim Table
			foreach (var i in addPt)
			{
				if (i.itemWrk.matchId != HoldMatchId)
				{
					HoldMatchId = i.itemWrk.matchId;
					FirstBulkInMatchDone = false;
				}
				if (HoldCustomer == "")
				{
					HoldCustomer = i.itemWrk.ccs_customer;
					HoldSid = 22000;
					HoldBid = 22000;
					HoldPackageItemNo = 0;
				}
				if (HoldCustomer == i.itemWrk.ccs_customer)
				{
					HoldSid++;
					if (HoldPackageItemNo == 0 ||
							i.pkgItemNo <= HoldPackageItemNo)
						HoldBid++;
					HoldPackageItemNo = i.pkgItemNo;
				}
				else
				{
					HoldCustomer = i.itemWrk.ccs_customer;
					HoldSid = 22001;
					HoldBid = 22001;
					HoldPackageItemNo = i.pkgItemNo;
				}
				newItmI = new trn_tmp_item_interim_table();
				newItmI.spa_id = Convert.ToInt64(i.itemWrk.spaId);
				newItmI.csg_sys = i.itemWrk.spaId.ToString().Substring(0, 4);
				newItmI.csg_prin = i.itemWrk.spaId.ToString().Substring(4, 4);
				newItmI.csg_agent = i.itemWrk.spaId.ToString().Substring(8, 4);
				newItmI.csg_account_number = i.itemWrk.ccs_subscriber;
				newItmI.bill_code = i.ruleTo.csg_svc;
				if (i.pkgItemNo != 0)
					newItmI.service_code = i.s9xx.service_code.Trim();
				else
					newItmI.service_code = i.ruleTo.csg_svc;
				//newItmI.discount_code = i.ruleTo.csg_disc;
				newItmI.discount_code = AssignDiscountCode(i.itemWrk.vip_flag, i.ruleTo.csg_disc, i.itemWrk.chargeAmount);
				newItmI.class_code = "";
				newItmI.customer_discount_code = i.ruleTo.csg_cust_disc;
				newItmI.pkg_item_no = i.pkgItemNo;
				newItmI.csg_campaign = "";
				newItmI.bid = HoldBid;
				newItmI.sid = HoldSid;
				newItmI.psid = HoldSid;
				newItmI.svc_qty = i.itemWrk.svc_qty; // this should always be a 1 anyway
				if (i.s9xx != null)
				{
					newItmI.nature_of_service_flag = i.s9xx.nature_of_service_flag;
					newItmI.charge_type = i.s9xx.charge_type;
					newItmI.charge_method = i.s9xx.charge_method;
					newItmI.product_type = i.s9xx.product_type;
					newItmI.lob = i.LOB;
					newItmI.converter_required_flag = i.s9xx.conv_rqd_flag;
					newItmI.provisionable_flag = i.s9xx.provisionable;
					newItmI.credit_charge = i.s9xx.credit_charge;
					newItmI.subscript_period = i.s9xx.subscript_period;
					newItmI.unit_of_measure = i.s9xx.unit_of_measure;
				}
				newItmI.reference = i.itemWrk.phoneNo;
				newItmI.is_active_subscriber = "Y";
				newItmI.connect_date = i.itemWrk.connectDate;
				if (newItmI.discount_code != "")
					if (i.itemWrk.discountStartDate != Convert.ToDateTime("9999-12-31"))
						newItmI.discount_start_date = i.itemWrk.discountStartDate;
					else
						newItmI.discount_start_date = i.itemWrk.connectDate;
				else
					newItmI.discount_start_date = Convert.ToDateTime("0001-01-01");
				// Charge Amount on Item Interim Table, the first ala-carte bulk per Match Id gets the amount, all others are 0
				if (i.s9xx != null && i.s9xx.charge_type.Trim() == "B" &&
						newItmI.bill_code == newItmI.service_code)
				{
					if (!FirstBulkInMatchDone)
					{
						//newItmI.charge_rate = i.itemWrk.chargeAmount;
						newItmI.charge_rate = CalculateBulkCharge(i.itemWrk.chargeAmount, i.ruleTo.point_code);
						FirstBulkInMatchDone = true;
					}
					else
						//newItmI.charge_rate = 0.00M;
						newItmI.charge_rate = CalculateBulkCharge(0.00M, i.ruleTo.point_code);
				}
				else
					newItmI.charge_rate = 0.00M;
				newItmI.rules_engine_pass = 1;
				newItmI.rule_id = i.itemWrk.ruleBaseId;
				newItmI.match_id = i.itemWrk.matchId;
				newItmI.csg_customer = i.itemWrk.ccs_customer;
				newItmI.csg_house = i.itemWrk.ccs_house;
				itmIs.Add(newItmI);
			}

			return itmIs;
		}

		private String AssignDiscountCode(String vipFlag, String discountCode, Decimal? ChargeAmount)
		{
			String discCode = discountCode;

			if (vipFlag == "E" && (ChargeAmount == null || ChargeAmount == 0.00M))
			{
				discountCode = "EMP00";
			}

			return discountCode;
		}

		private List<trn_tmp_item_interim_table> BulksUpdate(List<trn_tmp_item_interim_table> itmIs,
																										 List<ruleng_breadcrumb> brdcrmbList,
																										 List<trn_sub_base_extract> xrefs)
		{
			List<trn_tmp_item_interim_table> newI = itmIs;

			List<trn_tmp_item_interim_table> bulkList = (from it in itmIs where (it.charge_type != null && it.charge_type == "B") select it).ToList();

			var getMatchIds = (from b in bulkList
												 join bc in brdcrmbList on b.match_id equals bc.MatchId
												 select new { b.match_id, bc.FactId }).ToList();

			var groupedMatches = getMatchIds.OrderBy(o => o.match_id).ThenBy(o => o.FactId).GroupBy(mi => mi.match_id);

			List<BulkFactIdsByMatchIds> bulkFactIdsByMatchIds = new List<BulkFactIdsByMatchIds>();
			BulkFactIdsByMatchIds newFm;
			Int32 i = 0;
			foreach (var m in groupedMatches)
			{
				newFm = new BulkFactIdsByMatchIds();
				newFm.Matchid = m.Key.Value;
				i = 0;
				foreach (var f in m)
				{
					if (i == 0)
						newFm.FactIdList = f.FactId.ToString();
					else
						newFm.FactIdList = string.Format("{0},{1}", newFm.FactIdList, f.FactId.ToString());
					i++;
				}
				bulkFactIdsByMatchIds.Add(newFm);
			}

			// Make a list of the bulk updates by the "min_sid" and account where the FactIdList is the same
			var bulkUpdateList = (from b in bulkList
														join bfm in bulkFactIdsByMatchIds on b.match_id equals bfm.Matchid
														select new
														{
															b.csg_account_number,
															b.sid,
															b.charge_rate,
															reference = (b.reference != null) ? b.reference : "",
															b.service_code,
															bfm.FactIdList
														} into j
														group j by new { j.csg_account_number, j.service_code, j.FactIdList, j.reference } into j2
														select new
														{
															j2.Key.csg_account_number,
															j2.Key.service_code,
															j2.Key.FactIdList,
															bulk_charge_flat = j2.Sum(s => s.charge_rate),
															bulk_charge_unit = j2.Sum(s => s.charge_rate) / j2.Count(),
															bulk_qty_flat = 1,
															bulk_qty_unit = j2.Count(),
															min_sid = j2.Min(s => s.sid)
														}).ToList();

			// You can't do this in a "foreach" loop for some reason
			for (Int32 k = newI.Count() - 1; k >= 0; k--)
			{
				var itm = newI[k];
				var xAcctType = (from x in xrefs where x.account_number == itm.csg_account_number select x).FirstOrDefault();

				// Charge Type = B, Charge Method is F or U, Not provisionable or provisionable but not Telephony (may have to change)
				//   Update: I changed this to allow for Bulk Provisionables as long as the references were the same.
				if (itm.charge_type != null && itm.charge_type.Trim() == "B" && itm.bill_code == itm.service_code) // &&
					 //(itm.provisionable_flag == "N" || (itm.provisionable_flag == "Y" && itm.lob != "T")))
				{
					if (itm.charge_method.Trim() == "F" &&
						 (itm.nature_of_service_flag.Trim() == "01" || itm.nature_of_service_flag.Trim() == "03") &&
						 (xAcctType.account_type.Trim().ToUpper() == "RES") &&
						 (itm.service_code.Trim().Length >= 3) &&
						 (itm.service_code.Trim().Substring(0, 3).ToUpper() == "VGE" ||
							itm.service_code.Trim().Substring(0, 3).ToUpper() == "HGE" ||
							itm.service_code.Trim().Substring(0, 3).ToUpper() == "TGE"))
					{
						// don't include these codes in the roll up
					}
					else
					{
						var getUpdate = (from ul in bulkUpdateList where (ul.csg_account_number == itm.csg_account_number && ul.min_sid == itm.sid) select ul).FirstOrDefault();
						if (getUpdate == null)
						{
							// delete this item from the list
							newI.RemoveAt(k);
						}
						else
						{
							if (itm.charge_method.Trim() == "F")
							{
								itm.charge_rate = getUpdate.bulk_charge_flat;
								itm.svc_qty = getUpdate.bulk_qty_flat;
							}
							else if (itm.charge_method.Trim() == "U")
							{
								itm.charge_rate = getUpdate.bulk_charge_unit;
								itm.svc_qty = getUpdate.bulk_qty_unit;
							}
						}
					}
				}
			}

			return newI;
		}

		private void UpdatePsids(List<trn_tmp_item_interim_table> itmIs, RunMtParms rmp)
		{
			var rootCodes = (from i in itmIs
											 where i.provisionable_flag != null && i.provisionable_flag.ToUpper().Trim() == "Y"
											 join r in rmp.PdbCatalogRoots on new { j1 = i.spa_id, j2 = i.service_code.Trim() } equals new { j1 = r.spa_id, j2 = r.child_svc_code.Trim() }
											 select new
											 {
												 i.csg_account_number,
												 i.sid,
												 service_code = i.service_code.Trim(),
												 reference = (i.reference == null) ? "" : i.reference.Trim()
											 }).ToList();

			var branchCodes = (from i in itmIs
												 where i.provisionable_flag != null && i.provisionable_flag.ToUpper().Trim() == "Y" &&
															 i.reference != null &&
															 i.reference.Trim() != ""
												 join b in rmp.PdbCatalogBranches on new { j1 = i.spa_id, j2 = i.service_code.Trim() } equals new { j1 = b.spa_id, j2 = b.child_svc_code.Trim() }
												 select new
												 {
													 i.csg_account_number,
													 i.sid,
													 service_code = i.service_code.Trim(),
													 reference = (i.reference == null) ? "" : i.reference.Trim(),
													 parent_svc_code = b.parent_svc_code.Trim()
												 }).ToList();

			var leafCodes = (from i in itmIs
											 where i.provisionable_flag != null && i.provisionable_flag.ToUpper().Trim() == "Y" &&
														 i.reference != null &&
														 i.reference.Trim() != ""
											 join l in rmp.PdbCatalogLeaves on new { j1 = i.spa_id, j2 = i.service_code.Trim() } equals new { j1 = l.spa_id, j2 = l.child_svc_code.Trim() }
											 where l.parent_cast_type.Trim().ToUpper() == "ROOT"
											 select new
											 {
												 i.csg_account_number,
												 i.sid,
												 service_code = i.service_code.Trim(),
												 reference = (i.reference == null) ? "" : i.reference.Trim(),
												 parent_svc_code = l.parent_svc_code.Trim()
											 }).ToList();

			var aloneCodes = (from i in itmIs
												where i.provisionable_flag != null && i.provisionable_flag.ToUpper().Trim() == "Y"
												join a in rmp.PdbCatalogAlone on new { j1 = i.spa_id, j2 = i.service_code.Trim() } equals new { j1 = a.spa_id, j2 = a.child_svc_code.Trim() }
												select new
												{
													i.csg_account_number,
													i.sid,
													service_code = i.service_code.Trim(),
													reference = (i.reference == null) ? "" : i.reference.Trim()
												}).ToList();

			var joinBranches = (from b in branchCodes
													join r in rootCodes on new { j1 = b.csg_account_number, j2 = b.parent_svc_code } equals
																								 new { j1 = r.csg_account_number, j2 = r.service_code }
													select new
													{
														b.csg_account_number,
														b.sid,
														b.service_code,
														b.reference,
														root_sid = r.sid,
														root_svc = r.service_code,
														root_reference = r.reference
													}).ToList();
			var groupedBranches = (from j in joinBranches
														 orderby j.csg_account_number ascending, j.sid ascending, j.reference descending
														 group j by new { j.csg_account_number, j.sid } into g
														 select g.First()).ToList();

			var joinLeaves = (from l in leafCodes
												join r in rootCodes on new { j1 = l.csg_account_number, j2 = l.parent_svc_code, j3 = l.reference } equals
																							 new { j1 = r.csg_account_number, j2 = r.service_code, j3 = r.reference }
												select new
												{
													l.csg_account_number,
													l.sid,
													l.service_code,
													l.reference,
													root_sid = r.sid,
													root_svc = r.service_code,
													root_reference = r.reference
												}).ToList();
			var groupedLeaves = (from j in joinLeaves
													 orderby j.csg_account_number ascending, j.sid ascending, j.reference descending
													 group j by new { j.csg_account_number, j.sid } into g
													 select g.First()).ToList();

			var leafUpdateList = (from i in itmIs
														join l in groupedLeaves on new
														{ j1 = i.csg_account_number, j2 = i.sid } equals new
														{ j1 = l.csg_account_number, j2 = l.sid }
														select new { i, l }).ToList();
			foreach (var leaf in leafUpdateList)
			{
				leaf.i.psid = leaf.l.root_sid;
			}

			var branchUpdateList = (from i in itmIs
															join l in groupedBranches on new
															{ j1 = i.csg_account_number, j2 = i.sid } equals new
															{ j1 = l.csg_account_number, j2 = l.sid }
															select new { i, l }).ToList();
			foreach (var branch in branchUpdateList)
			{
				branch.i.psid = branch.l.root_sid;
			}

			var aloneUpdateList = (from i in itmIs
														 join a in aloneCodes on new
														 { j1 = i.csg_account_number, j2 = i.sid } equals new
														 { j1 = a.csg_account_number, j2 = a.sid }
														 select new { i, a }).ToList();
			foreach (var alone in aloneUpdateList)
			{
				alone.i.reference = "";
			}
		}

		private void MiscScrunch(List<trn_tmp_item_interim_table> itmIs,
												 List<spec_item_scrunch_codes> specs)
		{
			// Codes to Keep
			var csgCodeList = (from i in itmIs
												 join s in specs on i.service_code.Trim() equals s.scrunch_svc.Trim()
												 where i.bill_code.Trim() == i.service_code.Trim()
												 select new
												 {
													 i.csg_account_number,
													 service_code = i.service_code.Trim(),
													 i.sid
												 } into j
												 group j by new
												 {
													 j.csg_account_number,
													 j.service_code
												 } into j2
												 select new
												 {
													 csg_account_number = j2.Key.csg_account_number,
													 service_code = j2.Key.service_code,
													 keep_sid = j2.Min(s => s.sid)
												 }).ToList();

			// Delete Codes List
			var dList1 = (from i in itmIs
										join c in csgCodeList on new { j1 = i.csg_account_number, j2 = i.service_code.Trim() } equals new { j1 = c.csg_account_number, j2 = c.service_code }
										select new { i, c }).ToList().
									 OrderBy(csgl => csgl.i.csg_account_number);

			List<DeleteItemList> dList = (from d in dList1
																		where d.i.sid != d.c.keep_sid
																		select new DeleteItemList
																		{
																			csgAccountNo = d.i.csg_account_number,
																			sid = d.i.sid
																		}).ToList();

			Int32 k = -1;
			foreach (var d in dList)
			{
				k = itmIs.FindIndex(a => a.csg_account_number == d.csgAccountNo && a.sid == d.sid);
				itmIs.RemoveAt(k);
			}
		}

		private void AddVg001Svc(List<trn_tmp_item_interim_table> itmIs,
												 List<trn_sub_base_extract> subs,
												 List<ccsr_services> svc9xx,
												 List<dbo_vw_udct_tbl_pt> cdtPt,
												 List<ccsr_package_full> ccsPkgs,
												 List<customer_high_sid_bids> cHsb)
		{
			var hasList = new List<String>
			{
				"VS002", "VS003", "VS004", "VG002", "VG003", "VG004"
			};

			var hasList2 = new List<String>
			{
				"VS001", "VG001"
			};

			var potentialHasList = (from i in itmIs
															join r in hasList on
																i.service_code.Trim() equals r
															group i by new
															{
																i.csg_account_number,
																i.csg_customer,
																i.csg_house,
																i.csg_sys,
																i.csg_prin,
																i.csg_agent
															} into g
															select new
															{
																csg_account_number = g.Key.csg_account_number,
																csg_customer = g.Key.csg_customer,
																csg_house = g.Key.csg_house,
																csg_sys = g.Key.csg_sys,
																csg_prin = g.Key.csg_prin,
																csg_agent = g.Key.csg_agent,
																connect_date = g.Min(c => c.connect_date)
															}).ToList();

			if (potentialHasList.Count() == 0)
				return;

			var potentialHasList2 = (from i in itmIs
															 join l in hasList2 on
																 i.service_code.Trim() equals l
															 group i by new
															 {
																 i.csg_account_number
															 } into g
															 select new
															 {
																 csg_account_number = g.Key.csg_account_number
															 }).ToList();

			var missingCodes = potentialHasList.Where(k1 => !potentialHasList2.Any(
				k2 => k2.csg_account_number == k1.csg_account_number)).ToList();

			var addSubInfoToList = (from i in missingCodes
															join s in subs on
																i.csg_account_number equals s.account_number
															select new { i, s }).ToList();

			List<item_add_codes> iAdds = new List<item_add_codes>();
			item_add_codes newIa = new item_add_codes();

			foreach (var u in addSubInfoToList)
			{
				newIa = new item_add_codes();
				newIa.csg_sys = u.i.csg_sys;
				newIa.csg_prin = u.i.csg_prin;
				newIa.csg_agent = u.i.csg_agent;
				newIa.spaId = Convert.ToInt64(string.Format("{0}{1}{2}", newIa.csg_sys, newIa.csg_prin, newIa.csg_agent));
				newIa.spaSysLvl = Convert.ToInt64(string.Format("{0}{1}{2}", newIa.csg_sys, "0000", "0000"));
				newIa.spaPrinLvl = Convert.ToInt64(string.Format("{0}{1}{2}", newIa.csg_sys, newIa.csg_prin, "0000"));
				newIa.csg_customer_number = u.i.csg_customer;
				newIa.csg_account_number = u.i.csg_account_number;
				newIa.csg_house_number = u.i.csg_house;
				newIa.bill_code = "VG001";
				newIa.discount_code = "";
				newIa.customer_discount_code = "";
				newIa.campaign_code = "";
				newIa.psid = 0;
				newIa.reference = "";
				newIa.connect_date = u.i.connect_date;
				newIa.discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.customer_discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.bill_to_date = u.s.bill_to_date;
				newIa.first_billing_date = u.s.first_billing_date;
				newIa.vip_flag = u.s.vip_flag;
				newIa.bulk_qty = 1;
				newIa.bulk_charge = 0.00M;
				newIa.rules_engine_pass = 2;
				newIa.rule_id = -1007;
				newIa.match_id = -1007;
				newIa.code_cnt = 1;
				iAdds.Add(newIa);
			}

			AddNewItemInterimRecords(itmIs, iAdds, svc9xx, cdtPt, ccsPkgs, cHsb);

		}

		private void MiscAddContractCodes(List<trn_item_contract_codes> contracts,
																	List<trn_sub_base_extract> subs,
																	List<trn_tmp_item_interim_table> itmIs,
																	List<ccsr_services> svc9xx,
																	List<dbo_vw_udct_tbl_pt> cdtPt,
																	List<ccsr_package_full> ccsPkgs,
																	List<customer_high_sid_bids> cHsb)
		{
			var joinContractsToSubs = (from c in contracts
																 join s in subs on
																	 c.ccs_subscriber equals s.account_number
																 select new { c, s }).ToList();

			List<item_add_codes> iAdds = new List<item_add_codes>();
			item_add_codes newIa = new item_add_codes();
			foreach (var i in joinContractsToSubs)
			{
				newIa = new item_add_codes();
				newIa.csg_sys = i.c.ccs_subscriber.Substring(0, 4);
				newIa.csg_prin = i.c.ccs_subscriber.Substring(4, 2) + "00";
				newIa.csg_agent = i.c.ccs_subscriber.Substring(6, 3) + "0";
				newIa.spaId = Convert.ToInt64(string.Format("{0}{1}{2}", newIa.csg_sys, newIa.csg_prin, newIa.csg_agent));
				newIa.spaSysLvl = i.c.spaSysLvl;
				newIa.spaPrinLvl = i.c.spaPrinLvl;
				newIa.csg_customer_number = i.c.xrf_customer_ccs_id;
				newIa.csg_account_number = i.c.ccs_subscriber;
				newIa.csg_house_number = i.c.xrf_house_ccs_id;
				newIa.bill_code = i.c.contract_bill_code;
				newIa.discount_code = "";
				newIa.customer_discount_code = "";
				newIa.campaign_code = "";
				newIa.psid = 0;
				newIa.reference = "";
				newIa.connect_date = i.c.STARTDATE;
				newIa.discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.customer_discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.bill_to_date = i.s.bill_to_date;
				newIa.first_billing_date = i.s.first_billing_date;
				newIa.vip_flag = i.s.vip_flag;
				newIa.bulk_qty = 1;
				newIa.bulk_charge = 0.00M;
				newIa.rules_engine_pass = 2;
				newIa.rule_id = -1001;
				newIa.match_id = -1001;
				newIa.code_cnt = 1;
				iAdds.Add(newIa);
			}

			AddNewItemInterimRecords(itmIs, iAdds, svc9xx, cdtPt, ccsPkgs, cHsb);
		}

		private void MiscAddAoCodes(List<trn_item_ao_codes_to_add> aos,
															List<trn_sub_base_extract> subs,
															List<trn_tmp_item_interim_table> itmIs,
															List<ccsr_services> svc9xx,
															List<dbo_vw_udct_tbl_pt> cdtPt,
															List<ccsr_package_full> ccsPkgs,
															List<customer_high_sid_bids> cHsb)
		{
			var Num01and03s = (from i in itmIs
												 where i.nature_of_service_flag != null &&
												      (i.nature_of_service_flag.Trim() == "01" ||
															 i.nature_of_service_flag.Trim() == "03") &&
															i.lob != null &&
															i.lob.Trim() == "C"
												 group i by new
												 {
													 i.csg_account_number
												 } into g
												 select new
												 {
													 account_number = g.Key.csg_account_number,
													 num_01_03 = g.Count()
												 }).ToList();

			var joinAosToSubs1 = (from c in aos
													 join s in subs on
														 c.ccs_subscriber equals s.account_number
													 select new { c, s }).ToList();

			// Left join to Item NOS 01 & 03's
			var JoinAosToNum01and03s = (from a in joinAosToSubs1
																	join i in Num01and03s on
																		a.s.account_number equals i.account_number
																	into j
																	from j2 in j.DefaultIfEmpty()
																	select new { a.c, a.s, j2 }).ToList();

			List < item_add_codes > iAdds = new List<item_add_codes>();
			item_add_codes newIa = new item_add_codes();
			Int32 num_01_03 = 0;
			foreach (var i in JoinAosToNum01and03s)
			{
				if (i.j2 == null)
					num_01_03 = 0;
				else
					num_01_03 = i.j2.num_01_03;

				if (i.c.number_of_aos_to_add - num_01_03 >= 1)
				{
					newIa = new item_add_codes();
					newIa.csg_sys = i.c.ccs_subscriber.Substring(0, 4);
					newIa.csg_prin = i.c.ccs_subscriber.Substring(4, 2) + "00";
					newIa.csg_agent = i.c.ccs_subscriber.Substring(6, 3) + "0";
					newIa.spaId = Convert.ToInt64(string.Format("{0}{1}{2}", newIa.csg_sys, newIa.csg_prin, newIa.csg_agent));
					newIa.spaSysLvl = i.c.spaSysLvl;
					newIa.spaPrinLvl = i.c.spaPrinLvl;
					newIa.csg_customer_number = i.c.xrf_customer_ccs_id;
					newIa.csg_account_number = i.c.ccs_subscriber;
					newIa.csg_house_number = i.c.xrf_house_ccs_id;
					newIa.bill_code = i.c.bill_code;
					newIa.discount_code = "";
					newIa.customer_discount_code = "";
					newIa.campaign_code = "";
					newIa.psid = 0;
					newIa.reference = "";
					newIa.connect_date = i.c.connect_date;
					newIa.discount_start_date = Convert.ToDateTime("0001-01-01");
					newIa.customer_discount_start_date = Convert.ToDateTime("0001-01-01");
					newIa.bill_to_date = i.s.bill_to_date;
					newIa.first_billing_date = i.s.first_billing_date;
					newIa.vip_flag = i.s.vip_flag;
					newIa.bulk_qty = 1;
					newIa.bulk_charge = 0.00M;
					newIa.rules_engine_pass = 2;
					newIa.rule_id = -1003;
					newIa.match_id = -1003;
					newIa.code_cnt = i.c.number_of_aos_to_add - num_01_03;
					iAdds.Add(newIa);
				}
			}

			AddNewItemInterimRecords(itmIs, iAdds, svc9xx, cdtPt, ccsPkgs, cHsb);
		}

		private void MiscAddAoCodes_t2(List<trn_item_ao_codes_to_add> aos,
													List<trn_sub_base_extract> subs,
													List<trn_tmp_item_interim_table> itmIs,
													List<ccsr_services> svc9xx,
													List<dbo_vw_udct_tbl_pt> cdtPt,
													List<ccsr_package_full> ccsPkgs,
													List<customer_high_sid_bids> cHsb)
		{
			var joinAosToSubs = (from c in aos
													 join s in subs on
														 c.ccs_subscriber equals s.account_number
													 select new { c, s }).ToList();

			List<item_add_codes> iAdds = new List<item_add_codes>();
			item_add_codes newIa = new item_add_codes();
			foreach (var i in joinAosToSubs)
			{
				newIa = new item_add_codes();
				newIa.csg_sys = i.c.ccs_subscriber.Substring(0, 4);
				newIa.csg_prin = i.c.ccs_subscriber.Substring(4, 2) + "00";
				newIa.csg_agent = i.c.ccs_subscriber.Substring(6, 3) + "0";
				newIa.spaId = Convert.ToInt64(string.Format("{0}{1}{2}", newIa.csg_sys, newIa.csg_prin, newIa.csg_agent));
				newIa.spaSysLvl = i.c.spaSysLvl;
				newIa.spaPrinLvl = i.c.spaPrinLvl;
				newIa.csg_customer_number = i.c.xrf_customer_ccs_id;
				newIa.csg_account_number = i.c.ccs_subscriber;
				newIa.csg_house_number = i.c.xrf_house_ccs_id;
				newIa.bill_code = i.c.bill_code;
				newIa.discount_code = "";
				newIa.customer_discount_code = "";
				newIa.campaign_code = "";
				newIa.psid = 0;
				newIa.reference = "";
				newIa.connect_date = i.c.connect_date;
				newIa.discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.customer_discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.bill_to_date = i.s.bill_to_date;
				newIa.first_billing_date = i.s.first_billing_date;
				newIa.vip_flag = i.s.vip_flag;
				newIa.bulk_qty = 1;
				newIa.bulk_charge = 0.00M;
				newIa.rules_engine_pass = 2;
				newIa.rule_id = -1003;
				newIa.match_id = -1003;
				newIa.code_cnt = i.c.number_of_aos_to_add;
				iAdds.Add(newIa);
			}

			AddNewItemInterimRecords(itmIs, iAdds, svc9xx, cdtPt, ccsPkgs, cHsb);
		}

		private void MiscItemDirListingCodes(List<trn_item_directory_listing_codes_to_add> dirCodes,
													List<trn_sub_base_extract> subs,
													List<trn_tmp_item_interim_table> itmIs,
													List<ccsr_services> svc9xx,
													List<dbo_vw_udct_tbl_pt> cdtPt,
													List<ccsr_package_full> ccsPkgs,
													List<customer_high_sid_bids> cHsb)
		{
			var joinAosToSubs = (from c in dirCodes
													 join s in subs on
														 c.ccs_subscriber equals s.account_number
													 select new { c, s }).ToList();

			List<item_add_codes> iAdds = new List<item_add_codes>();
			item_add_codes newIa = new item_add_codes();
			foreach (var i in joinAosToSubs)
			{
				newIa = new item_add_codes();
				newIa.csg_sys = i.c.ccs_subscriber.Substring(0, 4);
				newIa.csg_prin = i.c.ccs_subscriber.Substring(4, 2) + "00";
				newIa.csg_agent = i.c.ccs_subscriber.Substring(6, 3) + "0";
				newIa.spaId = Convert.ToInt64(string.Format("{0}{1}{2}", newIa.csg_sys, newIa.csg_prin, newIa.csg_agent));
				newIa.spaSysLvl = i.c.spaSysLvl;
				newIa.spaPrinLvl = i.c.spaPrinLvl;
				newIa.csg_customer_number = i.c.xrf_customer_ccs_id;
				newIa.csg_account_number = i.c.ccs_subscriber;
				newIa.csg_house_number = i.c.xrf_house_ccs_id;
				newIa.bill_code = i.c.bill_code;
				newIa.discount_code = "";
				newIa.customer_discount_code = "";
				newIa.campaign_code = "";
				newIa.psid = 0;
				newIa.reference = "";
				newIa.connect_date = i.c.connect_date;
				newIa.discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.customer_discount_start_date = Convert.ToDateTime("0001-01-01");
				newIa.bill_to_date = i.s.bill_to_date;
				newIa.first_billing_date = i.s.first_billing_date;
				newIa.vip_flag = i.s.vip_flag;
				newIa.bulk_qty = 1;
				newIa.bulk_charge = 0.00M;
				newIa.rules_engine_pass = 2;
				newIa.rule_id = -1010;
				newIa.match_id = -1010;
				newIa.code_cnt = 1;
				iAdds.Add(newIa);
			}

			AddNewItemInterimRecords(itmIs, iAdds, svc9xx, cdtPt, ccsPkgs, cHsb);
		}

		private void DiscountAllItemsForSelectVips(List<trn_tmp_item_interim_table> tmpItemsList, List<trn_sub_base_extract> subs)
		{
			List<trn_tmp_item_interim_table> eList = (from i in tmpItemsList
																								join s in subs on i.csg_account_number equals s.account_number
																								where s.vip_flag.Trim() == "*"
																								select i).ToList();

			foreach (trn_tmp_item_interim_table e in eList)
			{
				e.discount_code = "HOUS1";
				e.discount_start_date = e.connect_date;
			}
		}

		private void FixBidsAndSids(List<trn_tmp_item_interim_table> itmIs)
		{
			List<customer_fix_sids> sidTable = (from i in itmIs
																					group i by new { i.csg_customer, i.sid } into g
																					orderby g.Key.csg_customer, g.Key.sid
																					select new customer_fix_sids { Customer = g.Key.csg_customer, OldSid = g.Key.sid, NewSid = 0 }).ToList();

			List<customer_fix_bids> bidTable = (from i in itmIs
																					group i by new { i.csg_customer, i.bid } into g
																					orderby g.Key.csg_customer, g.Key.bid
																					select new customer_fix_bids { Customer = g.Key.csg_customer, OldBid = g.Key.bid, NewBid = 0 }).ToList();

			string holdCust = "";
			Int64 newVal = 22001;
			foreach (customer_fix_sids s in sidTable)
			{
				if (holdCust != s.Customer)
				{
					holdCust = s.Customer;
					newVal = 22001;
				}
				s.NewSid = newVal;
				newVal++;
			}

			holdCust = "";
			foreach (customer_fix_bids b in bidTable)
			{
				if (holdCust != b.Customer)
				{
					holdCust = b.Customer;
					newVal = 22001;
				}
				b.NewBid = newVal;
				newVal++;
			}

			var fixTable = (from i in itmIs
											join s in sidTable on new { k1 = i.csg_customer, k2 = i.sid } equals new { k1 = s.Customer, k2 = s.OldSid }
											join p in sidTable on new { k1 = i.csg_customer, k2 = (Int64)i.psid } equals new { k1 = p.Customer, k2 = p.OldSid }
											join b in bidTable on new { k1 = i.csg_customer, k2 = i.bid } equals new { k1 = b.Customer, k2 = b.OldBid }
											select new { i, s, p, b }).ToList();

			foreach (var f in fixTable)
			{
				f.i.sid = f.s.NewSid;
				f.i.bid = f.b.NewBid;
				f.i.psid = f.p.NewSid;
			}
		}

		private void AddNewItemInterimRecords(List<trn_tmp_item_interim_table> ti, List<item_add_codes> newCds,
	                                        List<ccsr_services> svc9xx, List<dbo_vw_udct_tbl_pt> cdtPt,
	                                        List<ccsr_package_full> ccsPkgs, List<customer_high_sid_bids> cHsb)
		{
			// left join the 9xx
			var add9xx = (from c in newCds
										join x9s in svc9xx on new { a1 = c.spaId, a2 = c.bill_code.Trim() } equals new { a1 = x9s.spaId, a2 = x9s.service_code.Trim() } into j
										from j2 in j.DefaultIfEmpty()
										select new { c, pi = 0, j2 }).ToList();

			// join and expand from 9xx packages, remember: It's not possible to have both a service_code defined both as a package and as a service
			var add9xxP = (from xs in add9xx
										 join x9p in ccsPkgs on new { a1 = Convert.ToInt64(xs.c.spaId), a2 = xs.c.bill_code } equals new { a1 = x9p.Package.spaId, a2 = x9p.Package.service_code } into jp
										 from j2p in jp.DefaultIfEmpty()
										 select new { c = xs.c, pi = j2p != null ? j2p.PackageItem.pkg_item_no : 0, j2 = j2p != null ? j2p.Service : xs.j2 }).ToList();

			// left join the Code Table PT
			var addPt = (from x in add9xxP
									 orderby x.c.csg_customer_number, x.c.csg_account_number, x.c.match_id, x.pi
									 join pt in cdtPt on new { a1 = x.c.spaSysLvl, a2 = x.j2 == null ? "" : x.j2.product_type } equals new { a1 = pt.spaId, a2 = pt.code_table_value } into j
									 from j2 in j.DefaultIfEmpty()
									 select new { itemWrk = x.c, pkgItemNo = x.pi, s9xx = x.j2, LOB = j2 == null ? "" : j2.line_o_bus }).ToList();


			trn_tmp_item_interim_table newItmI;
			String HoldCustomer = "";
			Int64 HoldSid = 0;
			Int64 HoldBid = 0;
			Int32 HoldPackageItemNo = 0;
			customer_high_sid_bids cSidBid = new customer_high_sid_bids();

			foreach (var i in addPt)
			{
				for (Int32 j = 0; j < (Int32)i.itemWrk.code_cnt; j++)
				{
					if (HoldCustomer == "")
					{
						HoldCustomer = i.itemWrk.csg_customer_number;
						HoldPackageItemNo = 0;
						cSidBid = customer_high_sid_bids.GetCustomer(cHsb, i.itemWrk.csg_customer_number);
						HoldBid = cSidBid.Bid;
						HoldSid = cSidBid.Sid;
					}

					if (HoldCustomer == i.itemWrk.csg_customer_number)
					{
						HoldSid++;
						if (HoldPackageItemNo == 0 ||
								i.pkgItemNo <= HoldPackageItemNo)
							HoldBid++;
						HoldPackageItemNo = i.pkgItemNo;
					}
					else
					{
						customer_high_sid_bids.UpdateHighSidBidList(cHsb, HoldCustomer, HoldBid, HoldSid);
						HoldCustomer = i.itemWrk.csg_customer_number;
						HoldPackageItemNo = i.pkgItemNo;
						cSidBid = customer_high_sid_bids.GetCustomer(cHsb, i.itemWrk.csg_customer_number);
						HoldBid = cSidBid.Bid + 1;
						HoldSid = cSidBid.Sid + 1;
					}

					newItmI = new trn_tmp_item_interim_table();
					newItmI.spa_id = i.itemWrk.spaId;
					newItmI.csg_sys = i.itemWrk.csg_sys;
					newItmI.csg_prin = i.itemWrk.csg_prin;
					newItmI.csg_agent = i.itemWrk.csg_agent;
					newItmI.csg_account_number = i.itemWrk.csg_account_number;
					newItmI.bill_code = i.itemWrk.bill_code;
					if (i.pkgItemNo != 0)
						newItmI.service_code = i.s9xx.service_code.Trim();
					else
						newItmI.service_code = i.itemWrk.bill_code;
					newItmI.discount_code = i.itemWrk.discount_code;
					newItmI.class_code = "";
					newItmI.customer_discount_code = i.itemWrk.customer_discount_code;
					newItmI.pkg_item_no = i.pkgItemNo;
					newItmI.csg_campaign = "";
					newItmI.bid = HoldBid;
					newItmI.sid = HoldSid;
					if (i.itemWrk.psid == 0)
						newItmI.psid = HoldSid;
					else
						newItmI.psid = i.itemWrk.psid;

					if (i.s9xx != null)
					{
						newItmI.nature_of_service_flag = i.s9xx.nature_of_service_flag;
						newItmI.charge_type = i.s9xx.charge_type;
						newItmI.charge_method = i.s9xx.charge_method;
						newItmI.product_type = i.s9xx.product_type;
						newItmI.lob = i.LOB;
						newItmI.converter_required_flag = i.s9xx.conv_rqd_flag;
						newItmI.provisionable_flag = i.s9xx.provisionable;
						newItmI.credit_charge = i.s9xx.credit_charge;
						newItmI.subscript_period = i.s9xx.subscript_period;
						newItmI.unit_of_measure = i.s9xx.unit_of_measure;
					}

					newItmI.reference = i.itemWrk.reference;
					newItmI.is_active_subscriber = "Y";
					newItmI.connect_date = i.itemWrk.connect_date;

					if (i.itemWrk.discount_code != "")
					{
						if (i.itemWrk.discount_start_date != null)
							newItmI.discount_start_date = i.itemWrk.discount_start_date;
						else
							newItmI.discount_start_date = i.itemWrk.connect_date;
					}
					else
						newItmI.discount_start_date = Convert.ToDateTime("0001-01-01");

					if (i.s9xx != null && i.s9xx.charge_type.Trim() == "B" &&
							newItmI.bill_code == newItmI.service_code)
					{
						newItmI.svc_qty = (Int32)i.itemWrk.bulk_qty;
						newItmI.charge_rate = (Decimal)i.itemWrk.bulk_charge;
					}
					else
					{
						newItmI.svc_qty = 1;
						newItmI.charge_rate = 0.00M;
					}

					newItmI.rules_engine_pass = i.itemWrk.rules_engine_pass;
					newItmI.rule_id = i.itemWrk.rule_id;
					newItmI.match_id = i.itemWrk.match_id;
					newItmI.csg_customer = i.itemWrk.csg_customer_number;
					newItmI.csg_house = i.itemWrk.csg_house_number;
					ti.Add(newItmI);
				}
			}
			customer_high_sid_bids.UpdateHighSidBidList(cHsb, HoldCustomer, HoldBid, HoldSid);

		}

		private void WriteItemsFromItemInterimTable(List<trn_tmp_item_interim_table> itmI, List<trn_sub_base_extract> subs,
																								String myConnStr, RunMtParms rmp)
		{
			List<trn_item> items = new List<trn_item>();
			trn_item tni;

			// Add Sub Extract Fields to the Items
			var itemsPlusSubs = (from i in itmI
													 join s in subs on i.csg_account_number equals s.account_number into j
													 from j2 in j.DefaultIfEmpty()
													 select new { i, sub = j2 }).ToList();

			// Create the Item Records
			foreach (var r in itemsPlusSubs)
			{
				tni = new trn_item();

				tni.connect_date = GetConnectDate(r.i, r.sub, rmp.siteDates[0].merge_date);
				tni.system = r.i.csg_sys;
				tni.prin = r.i.csg_prin;
				tni.agent = r.i.csg_agent;
				tni.customer = r.i.csg_customer;
				tni.seq = r.i.sid;
				tni.rec_type = "F";
				tni.item_count = 1;
				tni.sid = r.i.sid;
				tni.file_location = "C";
				tni.item_status = "C";
				tni.item_type = "S";
				tni.lob = r.i.lob == null ? "N" : r.i.lob;
				tni.acct_sub = r.i.csg_account_number;
				tni.location = r.i.csg_house;
				if (r.i.provisionable_flag != null &&
						r.i.provisionable_flag == "Y" &&
						r.i.reference != null)
					tni.reference = r.i.reference;
				else
					tni.reference = "";
				tni.service_code = r.i.service_code;
				tni.client_init_srv_chg = "0";
				tni.bef_charge = GetChargeRates(r.i);
				tni.bid = r.i.bid;
				tni.bill_code = r.i.bill_code;
				tni.discount_code = r.i.discount_code;
				tni.class_serv_code = r.i.class_code;
				tni.disc_start_date = GetDiscountDate(r.i, rmp.siteDates[0].merge_date, tni);
				tni.bef_quantity = r.i.svc_qty;
				tni.aft_quantity = r.i.svc_qty;
				tni.installments = 0;
				tni.charge = tni.bef_charge;
				tni.bill_start_date = tni.connect_date;
				tni.bill_thru_date = GetBillToDate(r.i, r.sub);
				tni.cust_disc_code = r.i.customer_discount_code;
				tni.cust_disc_start_date = GetCustDiscStartDate(r.i, rmp.siteDates[0].merge_date);
				tni.sales_rep = "00000   ";
				tni.campaign = r.i.csg_campaign;
				tni.psid = Convert.ToDecimal(r.i.psid);
				tni.logical_delete_flag = "N";
				tni.create_date = tni.connect_date;
				tni.change_date = tni.connect_date;
				tni.change_time = new TimeSpan(0, 0, 0);
				tni.batch_add_date = tni.connect_date;
				tni.batch_chg_date = tni.connect_date;
				tni.entry_time = new TimeSpan(0, 0, 0);
				tni.susp_start_date = Convert.ToDateTime("0001-01-01");
				tni.susp_stop_date = Convert.ToDateTime("0001-01-01");
				if (r.i.charge_type != null && r.i.charge_type == "B")
					tni.default_chrg_ind = "N";
				else
					tni.default_chrg_ind = " ";
				if (r.i.charge_type != null && (r.i.charge_type == "E" || r.i.charge_type == "F"))
					tni.internal_bill_flag = r.i.charge_type;
				else
					tni.internal_bill_flag = "";

				items.Add(tni);
			}

			// BCP the Item Table
			SqlConnection myCon = new SqlConnection(myConnStr);
			myCon.Open();
			SqlBatchWriter bw = new SqlBatchWriter();
			bw.RowsToCopy = items.Count();
			bw.BcpTable(rmp.ItemTable, bw.ToDataTable(items), myCon);
			myCon.Close();
		}

		private void WriteTempItemInterimRecords(List<trn_tmp_item_interim_table> itmIs, String myConnStr, RunMtParms rmp)
		{
			SqlConnection myCon = new SqlConnection(myConnStr);
			myCon.Open();
			SqlBatchWriter bw = new SqlBatchWriter();
			bw.RowsToCopy = itmIs.Count();
			bw.BcpTable(rmp.TempItemInterimTable, bw.ToDataTable(itmIs), myCon);
			myCon.Close();
		}

		private DateTime GetBillToDate(trn_tmp_item_interim_table i, trn_sub_base_extract? s)
		{
			DateTime billDate = Convert.ToDateTime("0001-01-01");
			String chargeType = "";
			if (i.charge_type != null)
				chargeType = i.charge_type.Trim();
			String unitOfMeasure = "";
			if (i.unit_of_measure != null)
				unitOfMeasure = i.unit_of_measure.Trim();
			Int32 subscriptPeriod;
			bool subPeriodParsed = Int32.TryParse(i.subscript_period, out subscriptPeriod);
			if (!subPeriodParsed)
				subscriptPeriod = 0;
			DateTime connectDate = Convert.ToDateTime(i.connect_date);
			DateTime subBillToDate;
			if (s != null)
				subBillToDate = s.bill_to_date;
			else
				subBillToDate = Convert.ToDateTime("0001-01-01");

			if (chargeType == "E" || chargeType == "F")
			{
				if (unitOfMeasure == "M")
					billDate = connectDate.AddMonths(subscriptPeriod);
				else if (unitOfMeasure == "B")
					billDate = subBillToDate;
				else if (unitOfMeasure == "D")
					billDate = connectDate.AddDays(subscriptPeriod);
				else if (unitOfMeasure == "W")
					billDate = connectDate.AddDays(subscriptPeriod * 7);
				else
					billDate = Convert.ToDateTime("0001-01-01");
			}
			else if (chargeType == "S")
				billDate = Convert.ToDateTime("0001-01-01");
			else
				billDate = subBillToDate;

			return billDate;
		}

		private Decimal GetChargeRates(trn_tmp_item_interim_table i)
		{
			Decimal chargeRate = 0.00M;

			if (i.charge_type != null &&
					i.charge_type == "B")
				chargeRate = Convert.ToDecimal(i.charge_rate) * 100M;

			return chargeRate;
		}

		private DateTime GetCustDiscStartDate(trn_tmp_item_interim_table i, DateTime mergeDate)
		{
			DateTime custDiscStartDate = Convert.ToDateTime(i.discount_start_date);

			if (i.customer_discount_code.Trim() == "")
				custDiscStartDate = Convert.ToDateTime("0001-01-01");
			else if (i.discount_start_date == Convert.ToDateTime("0001-01-01"))
			{
				if (i.connect_date == Convert.ToDateTime("0001-01-01"))
					custDiscStartDate = mergeDate;
				else
					custDiscStartDate = Convert.ToDateTime(i.connect_date);
			}

			return custDiscStartDate;
		}

		private DateTime GetDiscountDate(trn_tmp_item_interim_table i, DateTime mergeDate, trn_item it)
		{
			DateTime discStartDate = Convert.ToDateTime(i.discount_start_date);

			if (i.discount_code.Trim() == "")
				discStartDate = Convert.ToDateTime("0001-01-01");
			else if (i.discount_start_date == Convert.ToDateTime("0001-01-01"))
			{
				if (it.connect_date == Convert.ToDateTime("0001-01-01"))
					discStartDate = mergeDate;
				else
					discStartDate = Convert.ToDateTime(it.connect_date);
			}

			// If the discount start date is within 30 days of the connect date, make it the connect date
			DateTime minConnDate = it.connect_date.AddDays(-30);
			DateTime maxConnDate = it.connect_date.AddDays(-1);
			if (discStartDate >= minConnDate && discStartDate <= maxConnDate)
				discStartDate = it.connect_date;

			return discStartDate;
		}

		private DateTime GetConnectDate(trn_tmp_item_interim_table i, trn_sub_base_extract? s, DateTime mergeDate)
		{
			DateTime connectDate;

			if (i.connect_date == null)
				connectDate = Convert.ToDateTime("0001-01-01");
			else
				connectDate = Convert.ToDateTime(i.connect_date);

			if (s != null && s.first_billing_date != null && connectDate < s.first_billing_date)
				connectDate = s.first_billing_date;
			else if (connectDate == Convert.ToDateTime("0001-01-01"))
				connectDate = mergeDate;

			if (connectDate > mergeDate)
				connectDate = mergeDate;

			return connectDate;
		}

		private Decimal CalculateBulkCharge(Decimal? chargeAmount, String? pointCodeAmount)
		{
			Decimal outCharge = 0.00M;

			try
			{
				outCharge = Convert.ToDecimal(pointCodeAmount);
			}
			catch
			{
				if (chargeAmount != null)
					outCharge = Convert.ToDecimal(chargeAmount);
				else
					outCharge = 0.00M;
			}

			return outCharge;
		}
	}
}
