using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace Wardship
{
    public static class genImport
    {
        public static DataRow GetDataRow(string fileName, string strConn, string SQL)
        {
            using (OleDbDataAdapter xlsHDR = new OleDbDataAdapter(SQL, strConn))
            {
                using (DataSet xlsDSHdr = new DataSet())
                {
                    try
                    {
                        xlsHDR.Fill(xlsDSHdr, "HeaderRow");
                        return xlsDSHdr.Tables["HeaderRow"].Rows[0];
                    }
                    catch
                    {
                        throw new NotUploaded("Error reading header row data");
                    }
                }
            }
        }
        public static DataTable GetDataTable(string strConn, string SQL, string desc)
        {
            using (OleDbDataAdapter xls = new OleDbDataAdapter(SQL, strConn))
            {
                using (DataSet xlsDS = new DataSet())
                {
                    try
                    {
                        xls.Fill(xlsDS, "import");
                        return xlsDS.Tables["import"];
                    }
                    catch
                    {
                        throw new NotUploaded(string.Format("Error while reading {0} data", desc));
                    }
                }
            }
        }

    }
}