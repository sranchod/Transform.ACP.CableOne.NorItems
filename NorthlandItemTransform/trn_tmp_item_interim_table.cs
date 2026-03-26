using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NorthlandItemTransform
{
	public class trn_tmp_item_interim_table : Generated_Abstract_Classes.trn_tmp_item_interim_table_base
	{
		public trn_tmp_item_interim_table Copy(trn_tmp_item_interim_table source, trn_tmp_item_interim_table dest)
		{
			trn_tmp_item_interim_table destRet = dest;
			foreach (PropertyInfo property in typeof(trn_tmp_item_interim_table).GetProperties().Where(p => p.CanWrite))
			{
				property.SetValue(destRet, property.GetValue(source, null), null);
			}
			return destRet;
		}
	}
}
