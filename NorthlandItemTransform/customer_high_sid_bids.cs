using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthlandItemTransform
{
	public class customer_high_sid_bids
	{
		public String Customer { get; set; }
		public Int64 Bid { get; set; }
		public Int64 Sid { get; set; }

		public static List<customer_high_sid_bids> GenerateHighSidBidList(List<trn_tmp_item_interim_table> it)
		{
			List<customer_high_sid_bids> newHsb = (from i in it
																						 group i by new
																						 {
																							 i.csg_customer
																						 } into g
																						 orderby g.Key.csg_customer
																						 select new customer_high_sid_bids
																						 {
																							 Customer = g.Key.csg_customer,
																							 Bid = g.Max(i => i.bid),
																							 Sid = g.Max(i => i.sid)
																						 }).ToList();

			return newHsb;
		}

		public static void UpdateHighSidBidList(List<customer_high_sid_bids> hsbList, String cust, Int64 newBid, Int64 newSid)
		{
			customer_high_sid_bids? upd = (from h in hsbList where h.Customer == cust select h).FirstOrDefault();
			if (upd != null)
			{
				upd.Sid = newSid;
				upd.Bid = newBid;
			}
			else
			{
				customer_high_sid_bids add = new customer_high_sid_bids();
				add.Customer = cust;
				add.Bid = newBid;
				add.Sid = newSid;
				hsbList.Add(add);
			}
		}

		public static customer_high_sid_bids GetCustomer(List<customer_high_sid_bids> hsbList, String cust)
		{
			customer_high_sid_bids retSidBid = new customer_high_sid_bids();
			retSidBid.Customer = cust;
			retSidBid.Bid = 22000;
			retSidBid.Sid = 22000;

			customer_high_sid_bids? lookupSidBid = (from h in hsbList where h.Customer == cust select h).FirstOrDefault();
			if (lookupSidBid != null)
			{
				retSidBid.Bid = lookupSidBid.Bid;
				retSidBid.Sid = lookupSidBid.Sid;
			}

			return retSidBid;
		}
	}
}
