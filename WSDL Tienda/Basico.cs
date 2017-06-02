using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.AccessControl;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Zip;
namespace WSDL_Tienda
{
    public class Basico
    {
        #region<prime>
        #endregion
        public static string _user_service { set; get; }
        public static string _password_service { set; get; }

        private static String _ruta_dll_serviciowin { get { return "C:\\inetpub\\wwwroot\\service_windows_tda\\Transmision.Net.Basico.dll"; } }

        private static String _ruta_dll_modulo_hash { get { return "C:\\inetpub\\wwwroot\\service_windows_tda\\Modulo_Hash.dll"; } }

        private static String _ruta_certificado { get { return "C:\\inetpub\\wwwroot\\service_windows_tda\\CDBATA.pfx"; } }

        private static String _ruta_fepe_dll { get { return "C:\\inetpub\\wwwroot\\service_windows_tda\\Carvajal.FEPE.PreSC.dll"; } }

        private static String _ruta_exe_updatewin { get { return "C:\\inetpub\\wwwroot\\service_windows_tda\\Transmision.NetWin.Update.exe"; } }

        //ACA VERIFICAMOS SI LA VERSION DEL EXE DEL UPDATE ESTA ACTUALIZADO
        public static Boolean _verifica_version_exeupdate(string _version)
        {
            Boolean _valida = false;
            try
            {
                var fvi = FileVersionInfo.GetVersionInfo(_ruta_exe_updatewin);
                var version_server = fvi.FileVersion;
                if (version_server != _version)
                {
                    _valida = true;
                }
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }

        //verificamos el servicio de la facturacion modulo hash
        public static Boolean _verifica_version_dllgenerahash(string _version)
        {
            Boolean _valida = false;
            try
            {
                var fvi = FileVersionInfo.GetVersionInfo(_ruta_dll_modulo_hash);
                var version_server = fvi.FileVersion;
                if (version_server != _version)
                {
                    _valida = true;
                }
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }

        //ACA VERIFICAMOS LA VERSION DE LA DLL DEL SERVICIO WINDOWS
        public static Boolean _verifica_version_windll(string _version)
        {
            Boolean _valida = false;
            try
            {
                var fvi = FileVersionInfo.GetVersionInfo(_ruta_dll_serviciowin);
                var version_server = fvi.FileVersion;
                if (version_server != _version)
                {
                    _valida = true;
                }
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }
        /*TAMAÑO DE LA DLL FEPE*/
        public static Boolean _verifica_dll_fepe(decimal _tamaño_dll_fepe)
        {
            Boolean _valida = false;
            try
            {
                //DateTime fvi = File.GetCreationTime(_ruta_certificado);
                FileInfo info = new FileInfo(_ruta_fepe_dll);
                decimal tamaño = info.Length;
                //var fvi = FileVersionInfo.GetVersionInfo(_ruta_dll_serviciowin);
                //var version_server = fvi.FileVersion;
                if (tamaño != _tamaño_dll_fepe)
                {
                    _valida = true;
                }
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }
        public static Boolean _verifica_certificado(decimal _tamaño_certificado)
        {
            Boolean _valida = false;
            try
            {
                //DateTime fvi = File.GetCreationTime(_ruta_certificado);
                FileInfo info = new FileInfo(_ruta_certificado);
                decimal tamaño = info.Length;
                //var fvi = FileVersionInfo.GetVersionInfo(_ruta_dll_serviciowin);
                //var version_server = fvi.FileVersion;
                if (tamaño != _tamaño_certificado)
                {
                    _valida = true;
                }
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }
        //VALIDA CERTIFICADO


        //verificamos si existe supervisor en la intranet
        public static string _verifica_supervisor(string _cod_sup,ref string _error)
        {
            string _existe = "0";            
            string sqlquery = "USP_Verificar_Supervisor";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_sup", _cod_sup);
                cmd.Parameters.Add("@existe", SqlDbType.VarChar, 1);
                cmd.Parameters["@existe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _existe = cmd.Parameters["@existe"].Value.ToString();
                _error = "";

            }
            catch(Exception exc)
            {
                _error = exc.Message;
                 _existe = "0"; 

            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _existe;
        }

        //con este grabamos el control de asistencia
        public static string _insertar_asistencia(string _cod_sup,string _cod_tda,DateTime _fecha_hora,string _codusu="")
        {
            string _error = "";
            string sqlquery="USP_Insertar_Asistencia";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_sup", _cod_sup);
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@fecha_hora",_fecha_hora);
                cmd.Parameters.AddWithValue("@codusu", _codusu);
                cmd.ExecuteNonQuery();
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }

            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        //consultamos los datos de la asistencia
        public static DataSet _consulta_asistencia(string _cod_tda,DateTime _fecha_ini,DateTime _fecha_fin)
        {
            string sqlquery = "USP_Consulta_Asistencia";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet dt = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@fec_ini", _fecha_ini);
                cmd.Parameters.AddWithValue("@fec_fin", _fecha_fin);
                da = new SqlDataAdapter(cmd);
                dt = new DataSet();
                da.Fill(dt);
            }
            catch
            {
                dt = null;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return dt;
        }
        public static Boolean credenciales_service(string _usuario,ref string _mensaje)
        {
            string sqlquery = "USP_Credenciales_Web_Service";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            Boolean acceso = true;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user", _usuario);
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 30);
                cmd.Parameters["@user"].Direction = ParameterDirection.InputOutput;
                cmd.Parameters["@password"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _user_service = cmd.Parameters["@user"].Value.ToString();
                _password_service = cmd.Parameters["@password"].Value.ToString();

            }
            catch(Exception exc)
            {
                _mensaje = exc.Message;
                acceso = false;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return acceso;
        }
        private static string Right(string param, int length)
        {
            //start at the index based on the lenght of the sting minus
            //the specified lenght and assign it a variable
            string result = param.Substring(param.Length - length, length);
            //return the result of the operation
            return result;
        }
        private static string Left(string param, int length)
        {
            //we start at 0 since we want to get the characters starting from the
            //left and with the specified lenght and assign it to a variable
            string result = param.Substring(0, length);
            //return the result of the operation
            return result;
        }

        //METODO PARA GRABAR EL MOVIMIENTO
        #region<REGION PARA GENERAR EL MOVIMIENTO>
        
        public static string _transac_flag_update(Decimal _nro_transa)
        {
            String sqlquery = "USP_Actualiza_TransacFlag";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _error = "";
            try
            {
                cn = new SqlConnection(Conexion.myconexion_almacen());
                if (cn.State ==0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nro_transa", _nro_transa);
                cmd.ExecuteNonQuery();
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        public static DataSet _transac_pendientes(string _cod_tda)
        {
            String sqlquery = "USP_Transac_Pendientes";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion_almacen());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_entid", _cod_tda);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
            }
            catch
            {
                ds = null;
            }
            return ds;
        }

        public static string _envia_stock_pla(string _cod_tda,DataTable dt)
        {
            string _error = "";
            string sqlquery = "USP_Insertar_Inventario";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion_almacen());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_ENTID", _cod_tda);
                cmd.Parameters.AddWithValue("@Tbl_Inventario",dt);
                cmd.ExecuteNonQuery();
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        public static string _update_error(string _cod_tda,string _error_mov)
        {
            string _error = "";
            string sqlquery = "USP_Errores_Trans_Mov";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion_almacen());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@error_des", _error_mov);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                if (cn != null)
                   if (cn.State == ConnectionState.Open) cn.Close();
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        public static string _generar_movimiento(string _tipo_origen, string _cod_tda,DataTable _dt_cab,DataTable _dt_det)
        {
            string _error = "";
            string sqlquery = "[USP_Generar_Transaccion]";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion_almacen());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@COD_ENTID", _cod_tda);
                cmd.Parameters.AddWithValue("@TIP_ORIGEN", _tipo_origen);
                cmd.Parameters.AddWithValue("@MOV_CAB", _dt_cab);
                cmd.Parameters.AddWithValue("@MOV_DET", _dt_det);
                cmd.ExecuteNonQuery();
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        #endregion

        //en este metodo vamos a copiar el archivo
        public static string _existe_tienda(string _tienda)
        {
            string sqlquery = "USP_Consulta_Tienda_SW";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _existe = "0";
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tienda", _tienda);
                cmd.Parameters.Add("@existe", SqlDbType.VarChar, 1);
                cmd.Parameters["@existe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _existe = cmd.Parameters["@existe"].Value.ToString();
            }
            catch(Exception exc)
            {
                string _log="C:/inetpub/wwwroot/web_site_tienda/log";
                string _archivo_log_fecha = _log + "\\log_" + DateTime.Today.ToString("dd-MM-yy") + ".log";
                //string strPathLog = @"C:\log_error_efact.txt";
                TextWriter tw = new StreamWriter(_archivo_log_fecha, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + _tienda + " " + " (_existe_tienda)" + "===>>" + exc.Message.ToString());
                tw.Close();

                _existe = "-1";// "0";
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _existe;
        }
        public static string _existe_archivo(string _tienda_archivo)
        {
            string _error = "";
            try
            {
                string _verifica_tienda = Left(_tienda_archivo, 2);
                string _tienda = "";
                if (_verifica_tienda == "TD")
                {
                    _tienda = "TD" + Right(_tienda_archivo, 3);
                }
                DataSet dsruta = ds_ruta_carpeta();
                if (dsruta != null)
                {
                    DataTable dt_ruta = dsruta.Tables[0];
                    for (Int32 i = 2; i < dt_ruta.Rows.Count; ++i)
                    {
                        string _ruta_carpeta = dt_ruta.Rows[i]["ruta_server"].ToString() + "/" + _tienda;
                        if (!(Directory.Exists(@_ruta_carpeta)))
                        {
                            Directory.CreateDirectory(@_ruta_carpeta);
                        }

                        DataTable dt_tipo = dsruta.Tables[1];

                        if (dt_tipo.Rows.Count > 0)
                        {
                            for (Int32 a = 0; a < dt_tipo.Rows.Count; ++a)
                            {
                                string _carpeta_ing = _ruta_carpeta + "/" + dt_tipo.Rows[a]["carpeta"].ToString();
                                //_archivo_ruta = _carpeta_ing + "/" + _tienda_archivo;
                                if (!(Directory.Exists(_carpeta_ing)))
                                {
                                    Directory.CreateDirectory(@_carpeta_ing);
                                }
                                else
                                {
                                    if (File.Exists(@_carpeta_ing + "\\" + _tienda_archivo))
                                    {
                                        _error = "1";
                                        break;
                                    }
                                    else
                                    {
                                        _error = "0";
                                        break;
                                    }
                                }

                                
                            }
                        }

                    }
                }

            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        public static byte[] _extrae_archivo_dlltda(string _nom_archivo)
        {
            Byte[] _archivo = null;
            string _ruta_archivo = @"C:\inetpub\wwwroot\service_windows_tda\" + _nom_archivo;
            try
            {
                _archivo = File.ReadAllBytes(_ruta_archivo);
                //File.WriteAllBytes(_ruta_archivo, _archivo);
                //switch (_nom_archivo)
                //{
                //    case "DBF.NET.dll":
                //        File.WriteAllBytes(_ruta_archivo, _archivo);
                //        break;
                //    case "Genera_Transmision.exe":
                //        File.WriteAllBytes(_ruta_archivo, _archivo);
                //        break;
                //    case "Genera_Transmision.exe.config":
                //        File.WriteAllBytes(_ruta_archivo, _archivo);
                //        break;
                //    case "GlobalSolucion.dll":
                //        File.WriteAllBytes(_ruta_archivo, _archivo);
                //        break;
                //    case "Transmision.Net.Basico.dll":
                //        File.WriteAllBytes(_ruta_archivo, _archivo);
                //        break;
                //}
            }
            catch
            {
                _archivo = null;
            }
            return _archivo;
        }
        public static string copiar_archivo_Tienda(byte[] _archivo_zip, string _tienda_archivo,Boolean _transmi=false)
        {

            string _verifica_tienda = Left(_tienda_archivo, 2);
            string _tienda = "";
            if (_verifica_tienda == "TD")
            {
                _tienda = "TD" + Right(_tienda_archivo, 3);
            }
            else
            { 
                //verificar si es archivo de almacen
                string _cod_alm=Right(_tienda_archivo, 3);
                if (_cod_alm == "001")
                {
                    _tienda = "TD" + Right(_tienda_archivo, 3);
                }
                else
                { 
                     _tienda = _cod_archivo(_cod_alm);
                     if (_tienda.Length == 0)
                     { 
                         return "Verifique el archivo " +  _tienda_archivo + " , que existe el codigo en la bd";
                     }
                }

            }

            //string _tienda = "TD" + Right(_tienda_archivo,3);

            //string _tienda = "TD" + Right(_tienda_archivo, 3);

            string _archivo_ruta = "";
            string _error = "";
            try
            {
                DataSet dsruta = ds_ruta_carpeta();

                if (dsruta!=null)
                {
                    DataTable dt_ruta = dsruta.Tables[0];

                    for (Int32 i = 0; i < dt_ruta.Rows.Count;++i )
                    {
                        string _ruta_carpeta=dt_ruta.Rows[i]["ruta_server"].ToString() + "/" + _tienda;
                        string _ruta_tda = dt_ruta.Rows[i]["ruta_server"].ToString();

                        //creando la carpeta de la tienda

                        //_error = _ruta_carpeta;
                        //DirectorySecurity securityRules = new DirectorySecurity();
                        //securityRules.AddAccessRule(new FileSystemAccessRule(@_ruta_carpeta, FileSystemRights.Write, AccessControlType.Allow));

                        //DirectorySecurity dSecurity = new DirectorySecurity();
                        // dSecurity.AddAccessRule(new FileSystemAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                        //NetworkShare.ConnectToShare(@dt_ruta.Rows[i]["ruta_server"].ToString(), "interfa", "interfa");

                        if (!(Directory.Exists(@_ruta_carpeta)))
                        {
                            Directory.CreateDirectory(@_ruta_carpeta);
                        }

                        DataTable dt_tipo = dsruta.Tables[1];

                        if (dt_tipo.Rows.Count>0)
                        {
                            for (Int32 a=0;a<dt_tipo.Rows.Count;++a)
                            {
                                string _carpeta_ing = _ruta_carpeta + "/" + dt_tipo.Rows[a]["carpeta"].ToString();
                                _archivo_ruta = _carpeta_ing + "/" + _tienda_archivo;
                                if  (!(Directory.Exists(_carpeta_ing)))
                                {
                                    Directory.CreateDirectory(@_carpeta_ing);
                                }
                                //verificar si el archivo existe y esta en proceso entonces vamos a poner en un temporal para que vuelva a procesar
                                //y no interfiera en un proceso del procedure sql y service

                                /*en este caso la variabe _transmi si es que es false ingresa aca porque no es un archivo de transmision.net*/
                                /*de lo contrario si es un archivo de tramsmision.net entonces lo almacena en temp*/

                                if (!_transmi)
                                { 

                                    //if (_ruta_tda == @"D:\BASE_TIENDA")
                                    //{

                                        //if (File.Exists(_archivo_ruta))
                                        //{
                                        //    string _temporal = _carpeta_ing + "\\Temp";

                                        //    if (!Directory.Exists(@_temporal))
                                        //    {
                                        //        Directory.CreateDirectory(@_temporal);
                                        //    }
                                        //    string _archivo_ruta_temp = _temporal + "/" + _tienda_archivo;

                                        //    File.WriteAllBytes(_archivo_ruta_temp, _archivo_zip);
                                        //}
                                        //else
                                        //{
                                            File.WriteAllBytes(_archivo_ruta, _archivo_zip);
                                        //}
                                    //}
                                    //else
                                    //{
                                    //    File.WriteAllBytes(_archivo_ruta, _archivo_zip);
                                    //}
                                }
                                else
                                {
                                    /*caso este sea archivo de trasmision.net entonces lo ponemos en temp*/
                                    if (_ruta_tda == @"D:\BASE_TIENDA")
                                    {                                       
                                        string _temporal = _carpeta_ing + "\\Temp";

                                        if (!Directory.Exists(@_temporal))
                                        {
                                            Directory.CreateDirectory(@_temporal);
                                        }
                                        string _archivo_ruta_temp = _temporal + "/" + _tienda_archivo;

                                        File.WriteAllBytes(_archivo_ruta_temp, _archivo_zip);
                                    }
                                }
                            }
                        }


                        

                    }

                                                                                                         

                }

            }
            catch(Exception exc)
            {
                
                _error = exc.Message;
                throw;
            }
            return _error;
        }

        public static Boolean getTienda_inv(String _cod_tda)
        {
            Boolean _valida = false;
            string sqlquery = "USP_Get_Tda_Inv";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {

                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tienda", _cod_tda);
                cmd.Parameters.Add("@inv", SqlDbType.Bit);
                cmd.Parameters["@inv"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _valida =Convert.ToBoolean(cmd.Parameters["@inv"].Value);
            }
            catch
            {
                if (cn!=null)
                if (cn.State == ConnectionState.Open) cn.Close();
                _valida = false;
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _valida;
        }

        private static DataSet ds_ruta_carpeta(decimal _tx=0)
        {
            string sqlquery = "USP_Leer_Ruta_Carpeta";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataSet ds = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tx", _tx);
                da = new SqlDataAdapter(cmd);
                ds = new DataSet();
                da.Fill(ds);
                
            }
            catch
            {
                ds = null;
                throw;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return ds;
        }

        public static DataTable dt_tiendas(ref string _error)
        {
            string sqlquery = "USP_Lista_Tiendas";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch(Exception exc)
            {
                _error = exc.Message;
                dt = null;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return dt;
        }

        private static string _cod_archivo(string _cod_tra)
        {
            string sqlquery = "USP_Consulta_CodReg";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tra", _cod_tra);
                cmd.Parameters.Add("@cod_retorno", SqlDbType.VarChar, 10);
                cmd.Parameters["@cod_retorno"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                return cmd.Parameters["@cod_retorno"].Value.ToString();
            }
            catch
            {
                if (cn.State==ConnectionState.Open) cn.Close();
                throw;
            }
            if (cn.State==ConnectionState.Open) cn.Close();
        }

        public static void _compara_version(string _version_tienda,ref byte[] _archivo)
        {
            string _ruta_exe = "D:/Transmision/TRANSMI.exe";
            string _version_exe_server;
            try
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(_ruta_exe);
                _version_exe_server = fvi.FileVersion;

                if (_version_exe_server!=_version_tienda)
                {
                    _archivo = convertir_archivo_a_byte(_ruta_exe);
                }
                
            }
            catch
            {
                _archivo = null;
            }
          
        }

        public static void _compara_version(string _version_tienda, ref byte[] _archivo,string _name_Archivo)
        {
            string _ruta_exe = "D:/Transmision/" + _name_Archivo;
            string _version_exe_server;
            try
            {
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(_ruta_exe);
                _version_exe_server = fvi.FileVersion;

                if (_version_exe_server != _version_tienda)
                {
                    _archivo = convertir_archivo_a_byte(_ruta_exe);
                }

            }
            catch
            {
                _archivo = null;
            }

        }

        private static byte[] convertir_archivo_a_byte(string ruta)
        {

            FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read);
            /*Create a byte array of file stream length*/
            byte[] b = new byte[fs.Length];
            /*Read block of bytes from stream into the byte array*/
            fs.Read(b, 0, System.Convert.ToInt32(fs.Length));
            /*Close the File Stream*/
            fs.Close();

            return b;
        }

        public static string _borrar_cen(string _tienda_carpeta,String[] _archivo)
        {
            //string _tienda = "TD" + Right(_tienda_carpeta, 3);

            string _tienda = "";// "TD" + Right(_tienda_archivo, 3);
            if (Left(_tienda_carpeta, 1) == "5")
            {
                _tienda = "TD" + Right(_tienda_carpeta, 3);
            }
            else
            {
                _tienda = _tienda_carpeta;
            }


            string _error = "";
            try
            {
                 DataSet dsruta = ds_ruta_carpeta(1);

                 if (dsruta != null)
                 {
                     DataTable dt_ruta = dsruta.Tables[0];
                     DataTable dt_tipo = dsruta.Tables[2];

                     string _ruta_carpeta = dt_ruta.Rows[0]["ruta_server"].ToString() + "/" + _tienda;
                     string _carpeta_sal = _ruta_carpeta + "/" + dt_tipo.Rows[0]["carpeta"].ToString();


                     for (Int32 i = 0; i < _archivo.Length;++i )
                     {
                         string _ruta_archivo = _carpeta_sal + "/" + _archivo[i].ToString() ;

                         if (File.Exists(_ruta_archivo))
                         {
                             string _ruta_archivo_copia = _carpeta_sal + "/bk";
                             if (!Directory.Exists(_ruta_archivo_copia))
                             {
                                 Directory.CreateDirectory(_ruta_archivo_copia);
                             }
                             FileInfo infofile = new FileInfo(_ruta_archivo);
                             string _archivo_copiar = infofile.Name;
                             string _ruta_copiar_error = _ruta_archivo_copia + "\\" + _archivo_copiar;
                             File.Copy(_ruta_archivo, _ruta_copiar_error, true);
                             File.Delete(_ruta_archivo);
                         }

                     }
                 }
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        public static byte[] archivos_cen_tx(string _tienda)
        {
            byte[] _archivo_comp =null;
            try
            {
                string _ruta = "";
                String[] filenames = _ruta_archivos_cen(_tienda,ref _ruta);

                string ruta_zip = _ruta + "/" + _tienda +  ".zip";

                if (File.Exists(ruta_zip))
                {
                    File.Delete(ruta_zip);
                }

                //crear archivo zip
                ZipOutputStream zipOut = new ZipOutputStream(File.Create(@ruta_zip));

                //*********************               

                for (Int32 i = 0; i < filenames.Length; ++i)
                {
                    string _archivo_xml = filenames[i].ToString();
                    FileInfo fi = new FileInfo(_archivo_xml);
                    ICSharpCode.SharpZipLib.Zip.ZipEntry entry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fi.Name);
                    FileStream sReader = File.OpenRead(_archivo_xml);
                    byte[] buff = new byte[Convert.ToInt32(sReader.Length)];
                    sReader.Read(buff, 0, (int)sReader.Length);
                    entry.DateTime = fi.LastWriteTime;
                    entry.Size = sReader.Length;
                    sReader.Close();
                    zipOut.PutNextEntry(entry);
                    zipOut.Write(buff, 0, buff.Length);
                }

                zipOut.Finish();
                zipOut.Close();

                byte[] file = File.ReadAllBytes(ruta_zip);

                if (filenames.Length==0)
                {
                    _archivo_comp = null;
                }
                else
                { 
                    _archivo_comp = file;
                }
                


                //si el archivo existe entonces lo eliminamos porque ya se transformo en bytes
                if (File.Exists(ruta_zip))
                {
                    File.Delete(ruta_zip);
                }
                                                        
            }
            catch (Exception exc)
            {
                string _log = "C:/inetpub/wwwroot/web_site_tienda/log";
                string _archivo_log_fecha = _log + "\\log_" + DateTime.Today.ToString("dd-MM-yy") + ".log";
                //string strPathLog = @"C:\log_error_efact.txt";
                TextWriter tw = new StreamWriter(_archivo_log_fecha, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + _tienda + " " + " (archivos_cen_tx)" + "===>>" + exc.Message.ToString());
                tw.Close();
                _archivo_comp =null;
            }
            return _archivo_comp;
        }

        //public static string[] 
        //direccion de facturacion electronica web service
        #region<ACTUALIZACION DE LA FACTURACION ELECTRONICA TDA>
        public static void _update_wsurl_FE_Win(string _cod_tda, string _ws_url)
        {
            string sqlquery = "USP_TdaUpdate_WSUrl_Win";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@ws_url", _ws_url);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();
                throw;
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
        }

        public static void _update_wsurl_FE(string _cod_tda,string _ws_url)
        {
            string sqlquery = "USP_TdaUpdate_WSUrl";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@ws_url", _ws_url);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();
            }
            if (cn!=null)
            if (cn.State == ConnectionState.Open) cn.Close();
        }


        public static string _verifica_data_fepe_dll(string _cod_tda, string _fepe_nom, decimal _fepe_peso, ref string _existe)
        {
            string sqlquery = "[USP_Verifica_Data_FEPE_DLL]";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _valida = "";
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@carvajal_fepe_dll", _fepe_nom);
                cmd.Parameters.AddWithValue("@carvajal_fepe_peso", _fepe_peso);
                cmd.Parameters.Add("@existe", SqlDbType.VarChar, 1);
                cmd.Parameters["@existe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _existe = cmd.Parameters["@existe"].Value.ToString();
            }
            catch (Exception exc)
            {
                _valida = exc.Message; ;
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();

            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _valida;
        }

        public static string _verifica_data_certificado(string _cod_tda, string _cer_nom,decimal _cer_peso, ref string _existe)
        {
            string sqlquery = "[USP_Verifica_Data_Certificado]";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _valida = "";
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@certificado_nombre",_cer_nom);
                cmd.Parameters.AddWithValue("@certificado_peso", _cer_peso);
                cmd.Parameters.Add("@existe", SqlDbType.VarChar, 1);
                cmd.Parameters["@existe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _existe = cmd.Parameters["@existe"].Value.ToString();
            }
            catch (Exception exc)
            {
                _valida = exc.Message; ;
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();

            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _valida;
        }
        public static string _verifica_data_url_win(string _cod_tda, string _ws_url, ref string _existe)
        {
            string sqlquery = "USP_Verifica_Data_Url_Win";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _valida = "";
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@ws_url", _ws_url);
                cmd.Parameters.Add("@existe", SqlDbType.VarChar, 1);
                cmd.Parameters["@existe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _existe = cmd.Parameters["@existe"].Value.ToString();
            }
            catch (Exception exc)
            {
                _valida = exc.Message; ;
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();

            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _valida;
        }
        public static string _verifica_data_url(string _cod_tda,string _ws_url,ref string _existe)
        {
            string sqlquery = "USP_Verifica_Data_Url";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            string _valida = "";
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _cod_tda);
                cmd.Parameters.AddWithValue("@ws_url", _ws_url);
                cmd.Parameters.Add("@existe", SqlDbType.VarChar, 1);
                cmd.Parameters["@existe"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                _existe = cmd.Parameters["@existe"].Value.ToString();
            }
            catch(Exception exc)
            {
                _valida = exc.Message; ;
                if (cn != null)
                    if (cn.State == ConnectionState.Open) cn.Close();
                
            }
            if (cn != null)
                if (cn.State == ConnectionState.Open) cn.Close();
            return _valida;
        }

        public static DataTable _dt_ws_url_return(string _tipo, string _url_in)
        {
            string sqlquery = "USP_ConDev_Ws";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tipo", _tipo);
                cmd.Parameters.AddWithValue("@url_in",_url_in);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch
            {
                dt = null;
                throw;
            }
            return dt;
        }
        #endregion

        private static string[] _ruta_archivos_cen(string _tienda_archivo,ref string _ruta)
        {
            string[] _CEN = null;
            string _tienda = "";// "TD" + Right(_tienda_archivo, 3);
            if (Left(_tienda_archivo, 1) == "5")
            {
                _tienda = "TD" + Right(_tienda_archivo, 3);
            }
            else
            {
                _tienda = _tienda_archivo;
            }

            try
            {
                DataSet dsruta = ds_ruta_carpeta(1);

                if (dsruta != null)
                {
                    DataTable dt_ruta = dsruta.Tables[0];
                    DataTable dt_tipo = dsruta.Tables[2];



                    string _ruta_carpeta = dt_ruta.Rows[0]["ruta_server"].ToString() + "/" + _tienda;
                    string _carpeta_sal = _ruta_carpeta + "/" + dt_tipo.Rows[0]["carpeta"].ToString();
                    _ruta = _carpeta_sal;
                    _CEN = System.IO.Directory.GetFiles(@_carpeta_sal, "*.CEN");
                    //var xml_p = from xmlp in _xml where Basico.Right(xmlp, 10) != "_inter.xml" select xmlp;
                }
            }
            catch
            {
                _CEN = null;
            }
            return _CEN;
        }

        #region<REGION DE PRUEBA SQL>

        public static string actualizar_dll_com(string _tienda, string _archivo)
        {
            string _error = "";
            string sqlquery = "USP_Update_DllComTda";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _tienda);
                cmd.Parameters.AddWithValue("@tda_dll_com", _archivo);                
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }

        public static string _actualizar_fepe_dll(string _tienda, string _archivo, Decimal _tamaño)
        {
            string _error = "";
            string sqlquery = "USP_Update_FEPE";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _tienda);
                cmd.Parameters.AddWithValue("@carvajal_Fepe_Dll", _archivo);
                cmd.Parameters.AddWithValue("@carvajal_Fepe_peso", _tamaño);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }


        public static string _actualizar_certificado_pfx(string _tienda, string _archivo, Decimal _tamaño)
        {
            string _error = "";
            string sqlquery = "USP_Update_Certificado";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _tienda);
                cmd.Parameters.AddWithValue("@certificado_nombre", _archivo);
                cmd.Parameters.AddWithValue("@certificado_peso", _tamaño);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        public static string actualizar_version_dllbasico(string _tienda,string _archivo,string _version)
        {
            string _error = "";
            string sqlquery = "USP_Update_VersionDLLTda";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cod_tda", _tienda);
                cmd.Parameters.AddWithValue("@archivo", _archivo);
                cmd.Parameters.AddWithValue("@version", _version);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        public static string copiar_archivo_Tienda_SQL(byte[] _archivo_zip, string _tienda_archivo)
        {

            string _verifica_tienda = Left(_tienda_archivo, 2);
            string _tienda = "";
            if (_verifica_tienda == "TD")
            {
                _tienda = "TD" + Right(_tienda_archivo, 3);
            }
            else
            {
                //verificar si es archivo de almacen
                string _cod_alm = Right(_tienda_archivo, 3);
                if (_cod_alm == "001")
                {
                    _tienda = "TD" + Right(_tienda_archivo, 3);
                }
                else
                {
                    _tienda = _cod_archivo(_cod_alm);
                    if (_tienda.Length == 0)
                    {
                        return "Verifique el archivo " + _tienda_archivo + " , que existe el codigo en la bd";
                    }
                }

            }

            //string _tienda = "TD" + Right(_tienda_archivo,3);

            //string _tienda = "TD" + Right(_tienda_archivo, 3);

            string _archivo_ruta = "";
            string _error = "";
            try
            {
                DataSet dsruta = ds_ruta_carpeta();

                if (dsruta != null)
                {
                    DataTable dt_ruta = dsruta.Tables[0];

                    for (Int32 i = 2; i < dt_ruta.Rows.Count; ++i)
                    {
                        string _ruta_carpeta = dt_ruta.Rows[i]["ruta_server"].ToString() + "/" + _tienda;
                        string _ruta_tda = dt_ruta.Rows[i]["ruta_server"].ToString();
                        //creando la carpeta de la tienda

                        //_error = _ruta_carpeta;
                        //DirectorySecurity securityRules = new DirectorySecurity();
                        //securityRules.AddAccessRule(new FileSystemAccessRule(@_ruta_carpeta, FileSystemRights.Write, AccessControlType.Allow));

                        //DirectorySecurity dSecurity = new DirectorySecurity();
                        // dSecurity.AddAccessRule(new FileSystemAccessRule(new System.Security.Principal.SecurityIdentifier(System.Security.Principal.WellKnownSidType.WorldSid, null), FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));

                        //NetworkShare.ConnectToShare(@dt_ruta.Rows[i]["ruta_server"].ToString(), "interfa", "interfa");

                        if (_ruta_tda == @"E:\BASE_TIENDA")
                        {                       

                                if (!(Directory.Exists(@_ruta_carpeta)))
                                {
                                    Directory.CreateDirectory(@_ruta_carpeta);
                                }

                                DataTable dt_tipo = dsruta.Tables[1];

                                if (dt_tipo.Rows.Count > 0)
                                {
                                    for (Int32 a = 0; a < dt_tipo.Rows.Count; ++a)
                                    {
                                        string _carpeta_ing = _ruta_carpeta + "/" + dt_tipo.Rows[a]["carpeta"].ToString();
                                        _archivo_ruta = _carpeta_ing + "/" + _tienda_archivo;
                                        if (!(Directory.Exists(_carpeta_ing)))
                                        {
                                            Directory.CreateDirectory(@_carpeta_ing);
                                        }
                                        //System.IO.File.WriteAllBytes(_archivo_ruta, _archivo_zip);

                                        /*caso este sea archivo de trasmision.net entonces lo ponemos en temp*/


                                        string _temporal = _carpeta_ing + "\\Temp";

                                        if (!Directory.Exists(@_temporal))
                                        {
                                            Directory.CreateDirectory(@_temporal);
                                        }
                                        string _archivo_ruta_temp = _temporal + "/" + _tienda_archivo;

                                        File.WriteAllBytes(_archivo_ruta_temp, _archivo_zip);

                                    }
                                }


                        }

                    }



                }

            }
            catch (Exception exc)
            {

                _error = exc.Message;
                throw;
            }
            return _error;
        }
        #endregion

        #region<METODO PARA VALES>
        public static string update_venta_empleado(string _Tip_Id_Ven, string _Nro_Dni_Ven, string _Cod_Tda_Ven,
                                                   string _Nro_Doc_Ven, string _Tip_Doc_Ven, string _Ser_Doc_Ven,
                                                   string _Num_Doc_Ven, string _Fec_Doc_Ven, string _Est_Doc_Ven,
                                                   string _Fc_Nin_Ven)
        {
            string _error = "";
            string sqlquery = "USP_Update_venta_empleado";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion_tda());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tip_Id_Ven", _Tip_Id_Ven);
                cmd.Parameters.AddWithValue("@Nro_Dni_Ven", _Nro_Dni_Ven);
                cmd.Parameters.AddWithValue("@Cod_Tda_Ven", _Cod_Tda_Ven);
                cmd.Parameters.AddWithValue("@Nro_Doc_Ven", _Nro_Doc_Ven);
                cmd.Parameters.AddWithValue("@Tip_Doc_Ven", _Tip_Doc_Ven);
                cmd.Parameters.AddWithValue("@Ser_Doc_Ven", _Ser_Doc_Ven);
                cmd.Parameters.AddWithValue("@Num_Doc_Ven", _Num_Doc_Ven);
                cmd.Parameters.AddWithValue("@Fec_Doc_Ven", _Fec_Doc_Ven);
                cmd.Parameters.AddWithValue("@Est_Doc_Ven", _Est_Doc_Ven);
                cmd.Parameters.AddWithValue("@Fc_Nin_Ven", _Fc_Nin_Ven);
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
                if (cn != null) ;
                if (cn.State == ConnectionState.Open) cn.Close();
            }
            if (cn != null) ;
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        public static string update_vales(string _serie, string _correlativo, string _cod_tda_venta, string _dni_venta,
            string _nombres_venta, string _fecha_doc, string _tipo_doc, string _serie_doc, string _numero_doc,
            string _estado_doc, string _fc_nint, string _email_venta, string _telefono_venta)
        {
            string _error = "";
            string sqlquery = "USP_UPDATE_VALES";
            SqlConnection cn = null;
            SqlCommand cmd = null;
            try
            {
                cn = new SqlConnection(Conexion.myconexion_tda());
                if (cn.State == 0) cn.Open();
                cmd = new SqlCommand(sqlquery, cn);
                cmd.CommandTimeout = 0;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@serie", _serie);
                cmd.Parameters.AddWithValue("@correlativo", _correlativo);
                cmd.Parameters.AddWithValue("@cod_tda_venta", _cod_tda_venta);
                cmd.Parameters.AddWithValue("@dni_venta", _dni_venta);
                cmd.Parameters.AddWithValue("@nombres_venta", _nombres_venta);

                cmd.Parameters.AddWithValue("@fecha_doc", _fecha_doc);
                cmd.Parameters.AddWithValue("@tipo_doc", _tipo_doc);
                cmd.Parameters.AddWithValue("@serie_doc", _serie_doc);
                cmd.Parameters.AddWithValue("@numero_doc", _numero_doc);
                cmd.Parameters.AddWithValue("@estado_doc", _estado_doc);

                cmd.Parameters.AddWithValue("@fc_nint", _fc_nint);
                cmd.Parameters.AddWithValue("@emai_venta", _email_venta);
                cmd.Parameters.AddWithValue("@telefono_venta", _telefono_venta);

                cmd.ExecuteNonQuery();
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            if (cn.State == ConnectionState.Open) cn.Close();
            return _error;
        }
        #endregion
    }
}