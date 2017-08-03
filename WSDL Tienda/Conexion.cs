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
            return "Server=10.10.10.208;Database=BdWebService;User ID=sa;Password=Bata2013;Trusted_Connection=False;";
        }
        public static string myconexion_tda()
        {
            return "Server=10.10.10.208;Database=BdTienda;User ID=sa;Password=Bata2013;Trusted_Connection=False;";
        }
        public static String myconexion_almacen()
        {
            return "Server=10.10.10.208;Database=BdAlmacen;User ID=sa;Password=Bata2013;Trusted_Connection=False;";
        }
    }
}