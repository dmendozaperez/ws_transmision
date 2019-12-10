using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace WSDL_Tienda
{
    public class Conexion
    {
        //static string _myconexion = ConfigurationManager.ConnectionStrings["SQL_PROD_WB"].ConnectionString;
        //static string _myconexion_tda = ConfigurationManager.ConnectionStrings["SQL_PROD_POS"].ConnectionString;
        //static string _myconexion_posperu = ConfigurationManager.ConnectionStrings["SQL_PROD_POS"].ConnectionString;
        //static string _myconexion_ws = ConfigurationManager.ConnectionStrings["SQL_PROD_WS"].ConnectionString;

        static string _myconexion = Encripta.encryption.RijndaelDecryptString(ConfigurationManager.ConnectionStrings["SQL_PROD_WB"].ConnectionString);
        static string _myconexion_tda = Encripta.encryption.RijndaelDecryptString(ConfigurationManager.ConnectionStrings["SQL_PROD_POS"].ConnectionString);
        static string _myconexion_posperu = Encripta.encryption.RijndaelDecryptString(ConfigurationManager.ConnectionStrings["SQL_PROD_POS"].ConnectionString);
        static string _myconexion_ws = Encripta.encryption.RijndaelDecryptString(ConfigurationManager.ConnectionStrings["SQL_PROD_WS"].ConnectionString);

        public static string myconexion()
        {
            return _myconexion;
            //return "Server=posperu.bgr.pe;Database=BdWebService;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
        public static string myconexion_tda()
        {
            return _myconexion_tda;
            //return "Server=posperu.bgr.pe;Database=BDPOS;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
        public static String myconexion_almacen()
        {
            return "Server=posperu.bgr.pe;Database=BdAlmacen;User ID=sa;Password=Bata2013;Trusted_Connection=False;";
        }
        public static String myconexion_posperu()
        {
            return _myconexion_posperu;
            //return "Server=posperu.bgr.pe;Database=BDPOS;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
        public static String myconexion_ws()
        {
            return _myconexion_ws;
            //return "Server=posperu.bgr.pe;Database=BDWSBATA;User ID=pos_oracle;Password=Bata2018**;Trusted_Connection=False;";
        }
    }
}