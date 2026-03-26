using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace NorthlandItemTransform
{
	public class SqlBatchWriter
	{
		public Int32 RowsToCopy { get; set; }

		public DataTable ToDataTable<T>(List<T> items)
		{
			DataTable dataTable = new DataTable(typeof(T).Name);
			//Get all the properties by using reflection   
			PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo prop in Props)
			{
				//Setting column names as Property names  
				dataTable.Columns.Add(prop.Name);
			}
			foreach (T item in items)
			{
				var values = new object[Props.Length];
				for (int i = 0; i < Props.Length; i++)
				{
					values[i] = Props[i].GetValue(item, null);
				}
				dataTable.Rows.Add(values);
			}
			return dataTable;
		}

		public bool BcpTable(string TableName, System.Data.DataTable dt, SqlConnection bcpConn)
		{
			bool Pass = false;

			using (SqlTransaction Trans = bcpConn.BeginTransaction())
			{
				Trans.Save("Beginning");

				using (SqlBulkCopy bc = new SqlBulkCopy(bcpConn, SqlBulkCopyOptions.Default, Trans))
				{
					//foreach (DataColumn col in dt.Columns)
					//{
					//	bc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
					//}
					bc.DestinationTableName = TableName;
					bc.SqlRowsCopied += new SqlRowsCopiedEventHandler(BcpRowsCopied);
					bc.NotifyAfter = 1000;
					bc.BulkCopyTimeout = 0;
					//bc.Dump();
					try
					{
						bc.WriteToServer(dt);
						bc.Close();
						Trans.Commit();
						Pass = true;
					}
					catch (Exception err)
					{
						//dt.Dump();
						Console.WriteLine(string.Format("BCP Failed!\nError = {0}", err.Message));
						Trans.Rollback();
						Pass = false;
						throw new Exception(err.Message);
					}
				}
			}

			return Pass;
		}

		private void BcpRowsCopied(object sender, SqlRowsCopiedEventArgs e)
		{
			Console.WriteLine(string.Format("BCP Progress: {0} rows of {1} rows copied.", e.RowsCopied, RowsToCopy));
		}

	}
}
