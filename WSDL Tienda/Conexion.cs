using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace WSDL_Tienda
{
    public class Conexion
    {
        //static string strconexion = ConfigurationManager.ConnectionStrings["MyConexionSql"].ConnectionString;
        public static string myconexion()
        {
            return "Server=posperu.bgr.pe;Database=BdWebService;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
        public static string myconexion_tda()
        {
            return "Server=posperu.bgr.pe;Database=BDPOS;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
        public static String myconexion_almacen()
        {
            return "Server=posperu.bgr.pe;Database=BdAlmacen;User ID=sa;Password=Bata2013;Trusted_Connection=False;";
        }
        public static String myconexion_posperu()
        {
            return "Server=posperu.bgr.pe;Database=BDPOS;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
        public static String myconexion_ws()
        {
            return "Server=posperu.bgr.pe;Database=BDWSBATA;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
    }
}