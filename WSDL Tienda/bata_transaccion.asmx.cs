using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Capa_Dato;
using System.Data;
using System.IO;
namespace WSDL_Tienda
{
    /// <summary>
    /// Descripción breve de bata_transaccion
    /// </summary>
    /// 
   
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]       
    public class bata_transaccion : Seguridad
    {
        private static string _mensaje_conexion_sql = "";

        [WebMethod, SoapHeader("CredencialAutenticacion")]     
        public DataSet ws_consulta_asistencia(string _cod_tda,DateTime _fecha_ini,DateTime _fecha_fin)
        {
            DataSet dt = null;
            try
            {

                dt=Basico._consulta_asistencia(_cod_tda, _fecha_ini, _fecha_fin);
                //dt = null;
            }
            catch
            {
                dt = null;
            }
            return dt;

        }

        #region<REGION PARA LA ACTUALIZACION DE LA URL FACTURACION ELECTRONICA>
        //ACTUALIZAR WEB SERVICE FACTURACION ELECTRONICA

        //*************************
        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public string ws_tdaupdate_wsurl(string _cod_tda, string _ws_url)
        {
            string _error = "";
            try
            {

               Basico._update_wsurl_FE(_cod_tda, _ws_url);
               // _error = "xxx";
            }
            catch
            {

            }
            return _error;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_tdaupdate_wsurl_win(string _cod_tda, string _ws_url)
        {
            string _error = "";
            try
            {
                Basico._update_wsurl_FE_Win(_cod_tda, _ws_url);
                //_error = "xxx";
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string[] ws_existe_fepe_dll_data(string _cod_tda, string _fepe_nombre, decimal fepe_peso)
        {
            string _error = "";
            string _estado = "0";
            try
            {
                _error = Basico._verifica_data_fepe_dll(_cod_tda, _fepe_nombre, fepe_peso, ref _estado);
                //_error = "aaa";
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            String[] _respuesta = new String[] { "existe", "url_ws" };
            string _error_codigo = "";
            string _mensaje = "";
            if (_error.Length == 0)
            {
                _error_codigo = _estado;
                _mensaje = "ok";
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            else
            {
                _error_codigo = "1";
                _mensaje = _error;
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            return _respuesta;

        }


        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string[] ws_existe_certificado_WS(string _cod_tda, string _cer_nombre,decimal cer_peso)
        {
            string _error = "";
            string _estado = "0";
            try
            {
                _error = Basico._verifica_data_certificado(_cod_tda, _cer_nombre, cer_peso, ref _estado);
                //_error = "aa";
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            String[] _respuesta = new String[] { "existe", "url_ws" };
            string _error_codigo = "";
            string _mensaje = "";
            if (_error.Length == 0)
            {
                _error_codigo = _estado;
                _mensaje = "ok";
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            else
            {
                _error_codigo = "1";
                _mensaje = _error;
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            return _respuesta;

        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string[] ws_existe_url_WS_Win(string _cod_tda, string _ws_url)
        {
            string _error = "";
            string _estado = "0";
            try
            {
                _error = Basico._verifica_data_url_win(_cod_tda, _ws_url, ref _estado);
                //_error = "zzz";
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            String[] _respuesta = new String[] { "existe", "url_ws" };
            string _error_codigo = "";
            string _mensaje = "";
            if (_error.Length == 0)
            {
                _error_codigo = _estado;
                _mensaje = "ok";
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            else
            {
                _error_codigo = "1";
                _mensaje = _error;
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            return _respuesta;

        }

        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public string[] ws_existe_url_WS(string _cod_tda,string _ws_url)
        {
            string _error = "";
            string _estado = "0";
            try
            {
                _error = Basico._verifica_data_url(_cod_tda, _ws_url,ref _estado);
                //_error = "xxx";
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            String[] _respuesta = new String[] { "existe", "url_ws" };
            string _error_codigo = "";
            string _mensaje = "";
            if (_error.Length == 0)
            {
                _error_codigo = _estado;
                _mensaje = "ok";
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            else
            {
                _error_codigo = "1";
                _mensaje = _error;
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            return _respuesta;

        }

        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public string[] ws_update_url_fe(string _tipo,string _url_in)
        {
            DataTable dt;
            string _estado = "0";
            string _error = "";
            string _url = "";
            try
            {

                //_error = "aaa";

                dt = Basico._dt_ws_url_return(_tipo, _url_in);
                if (dt != null)
                {
                    _estado = dt.Rows[0]["existe"].ToString();
                    _url = dt.Rows[0]["url_ws"].ToString();
                }
            }
            catch(Exception exc)
            {
                _error = exc.Message;
                dt = null;
            }
            String[] _respuesta = new String[] { "existe", "url_ws" };
            string _error_codigo = "";
            string _mensaje = "";
            if (_error.Length==0)
            {
                _error_codigo = _estado;
                _mensaje = _url;
                _respuesta[0] = _error_codigo;
                _respuesta[1] = _mensaje;
            }
            else
            {
                _error_codigo = "0";
                _mensaje =  _error;
                _respuesta[0]=_error_codigo;
                _respuesta[1] = _mensaje;
            }
            return _respuesta;
        }
        #endregion
        [WebMethod, SoapHeader("CredencialAutenticacion")]        
        public string[] ws_inserta_asistencia(string _cod_sup,string _cod_tda,DateTime _fecha_hora,string _codusu="")
        {
            string _error = "";            
            try
            {

                //_error = "xxx";

                string _valida = Basico._verifica_supervisor(_cod_sup, ref _error);

                if (_error.Length == 0)
                {
                    if (_valida == "1")
                    {
                        _error = Basico._insertar_asistencia(_cod_sup, _cod_tda, _fecha_hora, _codusu);
                    }
                    else
                    {
                        _error = "0";
                    }
                }
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            String[] _respuesta = new String[] { "codigo", "descripcion" };
            string _error_codigo = "";
            string _mensaje = "";
            //Resultado res;
            if (_error.Length == 0)
            {
                _error_codigo = "1";
                _mensaje = "Los datos se grabaron exitosamente";
                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);

                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }
            else
            {

                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);
                _error_codigo = "0";
                _mensaje = (_error == "0") ? "El codigo del supervisor no existe en la base de datos" : _error;
                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }

            //return res;
            return _respuesta;
            
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Byte[] ws_transmision_salida(String _tienda)
        {
            Byte[] ws=null;
            try
            {
                ws= Basico.archivos_cen_tx(_tienda);               
            }
            catch(Exception exc)
            {
                string _log = "C:/inetpub/wwwroot/web_site_tienda/log";
                string _archivo_log_fecha = _log + "\\log_" + DateTime.Today.ToString("dd-MM-yy") + ".log";
                //string strPathLog = @"C:\log_error_efact.txt";
                TextWriter tw = new StreamWriter(_archivo_log_fecha, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + _tienda + " " + " (ws_transmision_salida)" + "===>>" + exc.Message.ToString());
                tw.Close();
                ws = null;
            }

            if (ws == null)
            {
                //string array2 = Convert.ToBase64String("MA").ToString();
                //byte[] array = System.Text.Encoding.ASCII.GetBytes("MA");
                //byte[] numbers = Convert.ToBase64String("MA").ToString();
                String abc = "0";
                byte[] numbers = System.Text.Encoding.ASCII.GetBytes(abc);
                ws = numbers;

            }

            return ws;         
        }
        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string[] ws_borrar_archivo_cen(String _tienda, String[] _archivos)
        {
            
            string _error = "";
            try
            {
                _error=Basico._borrar_cen(_tienda, _archivos);
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }

            String[] _respuesta = new String[] { "codigo", "descripcion" };
            string _error_codigo = "";
            string _mensaje = "";
            //Resultado res;
            if (_error.Length == 0)
            {
                _error_codigo = "1";
                _mensaje = "se eliminaron los archivos exitosamente";
                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);

                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }
            else
            {

                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);
                _error_codigo = "0";
                _mensaje = _error;
                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }

            //return res;
            return _respuesta;
            
        }       

        [WebMethod, SoapHeader("CredencialAutenticacion")]        
        public List<String> ws_tienda_lista()
        {
            //string[]  tiendas=null;
            List<string> lista =null;
            string _error = "";
            try
            {
                lista=new List<string>();
                DataTable dt = Basico.dt_tiendas(ref _error);
                if (dt!=null)
                {
                    if (dt.Rows.Count>0)
                    {
                        for (Int32 i=0;i<dt.Rows.Count;++i)
                        {
                            string _tienda = dt.Rows[i]["Tienda"].ToString();                           
                            lista.Add(_tienda);
                        }
                       
                    }
                    
                }

                if (_error.Length>0)
                {
                    lista.Add(_error);
                }

               
            }
            catch(Exception exc)
            {
                lista.Add(exc.Message);
            }
                return lista;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public byte[] ws_actualiza_archivo(string _name_archivo, string _version_tienda)
        {
            byte[] _archivo = null;
            try
            {
                //_archivo = null;
                Basico._compara_version(_version_tienda, ref _archivo,_name_archivo);
            }
            catch
            {
                _archivo = null;
            }
            if (_archivo == null)
            {
                //string array2 = Convert.ToBase64String("MA").ToString();
                //byte[] array = System.Text.Encoding.ASCII.GetBytes("MA");
                //byte[] numbers = Convert.ToBase64String("MA").ToString();
                String abc = "0";
                byte[] numbers = System.Text.Encoding.ASCII.GetBytes(abc);
                _archivo = numbers;

            }
            return _archivo;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Byte[] ws_update_app_tx(String _version_tienda)
        {
            Byte[] _archivo = null; 
            try
            {
                Basico._compara_version(_version_tienda, ref _archivo);
                //_archivo = null;
            }
            catch
            {
                _archivo=null;
            }
            if (_archivo == null)
            {
                //string array2 = Convert.ToBase64String("MA").ToString();
                //byte[] array = System.Text.Encoding.ASCII.GetBytes("MA");
                //byte[] numbers = Convert.ToBase64String("MA").ToString();
                String abc="0";
                byte[] numbers =  System.Text.Encoding.ASCII.GetBytes(abc);
                _archivo = numbers;

            }
            return _archivo;
        }


        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_update_dll_com(string _tienda, string _archivo)
        {
            string _error = "";
            try
            {
                _error = Basico.actualizar_dll_com(_tienda, _archivo);
               // _error = "xxx";
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }


        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_update_fepe_dll(string _tienda, string _archivo, decimal _tanaño)
        {
            string _error = "";
            try
            {
                _error = Basico._actualizar_fepe_dll(_tienda, _archivo, _tanaño);
                //_error = "xxx";

            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_update_certificado_pfx(string _tienda, string _archivo, decimal _tanaño)
        {
            string _error = "";
            try
            {
                _error = Basico._actualizar_certificado_pfx(_tienda, _archivo, _tanaño);
               // _error = "xx";
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_update_versiondll_net(string _tienda, string _archivo, string _version)
        {
            string _error="";
            try
            {
                _error=Basico.actualizar_version_dllbasico(_tienda, _archivo, _version);
                //_error = "xxx";
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_existe_tienda(string _codigo_tienda)
        {
            string _existe = "0";
            try
            {
                _existe = Basico._existe_tienda(_codigo_tienda);
            }
            catch(Exception exc)
            {
                string _log = "C:/inetpub/wwwroot/web_site_tienda/log";
                string _archivo_log_fecha = _log + "\\log_" + DateTime.Today.ToString("dd-MM-yy") + ".log";
                //string strPathLog = @"C:\log_error_efact.txt";
                TextWriter tw = new StreamWriter(_archivo_log_fecha, true);
                tw.WriteLine(DateTime.Now.ToString() + " " + _codigo_tienda + " " + " (ws_existe_tienda)" + "===>>" + exc.Message.ToString());
                //tw.WriteLine(DateTime.Now.ToString() + "===>>" + exc.Message.ToString());
                tw.Close();
                _existe = "-1";
            }
            return _existe;
        }

        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public string[] ws_transmision_ingreso_transminet(Byte[] _archivo_zip, string _name)
        {
            string valida = "";
            try
            {
                valida = "xxx";
                //if (VerificarPermisos(CredencialAutenticacion))
                //{

                    //valida = Basico.copiar_archivo_Tienda(_archivo_zip, _name,true);

                //}
                //else
                //{
                //    valida = "usuario y/o contraseña no valida";
                //}
            }
            catch (Exception exc)
            {
                valida = exc.Message;
            }

            String[] _respuesta = new String[] { "codigo", "descripcion" };
            string _error_codigo = "";
            string _mensaje = "";
            //Resultado res;
            if (valida.Length == 0)
            {
                _error_codigo = "1";
                _mensaje = "transmision exitosa";
                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);

                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }
            else
            {

                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);
                _error_codigo = "0";
                _mensaje = valida;
                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }

            //return res;
            return _respuesta;
        }


        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string[] ws_transmision_ingreso(Byte[] _archivo_zip, string _name)
        {
            string valida = "";
            try
            {
                if (VerificarPermisos(CredencialAutenticacion))
                {

                    valida = Basico.copiar_archivo_Tienda(_archivo_zip,_name);                    
                  
                }
                else
                {
                    valida = "usuario y/o contraseña no valida";
                }
            }
            catch (Exception exc)
            {
                valida = exc.Message;
            }

            String[] _respuesta = new String[] { "codigo","descripcion" };
            string _error_codigo = "";
            string _mensaje="";
            //Resultado res;
            if (valida.Length == 0)
            {                
                _error_codigo = "1";
                _mensaje = "transmision exitosa";
                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);

                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString(); 
            }
            else
            {

                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);
                _error_codigo = "0";
                _mensaje = valida;
                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString(); 
            }
            
            //return res;
            return _respuesta;
        }
        public static Boolean VerificarPermisos(Autenticacion value)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                //Verifica los permiso Ej. Consulta a BD 

                //invocar de la base de datos las credenciales  para la web service

                Basico.credenciales_service(value.user_name, ref _mensaje_conexion_sql);

                if (_mensaje_conexion_sql.Length==0)
                {
                        if (value.user_name == Basico._user_service && value.user_password == Basico._password_service)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                }
                else
                {
                    return false;

                }
            }
        }

        #region<REGION DE VERSION DEL SERVICIO WINDOWS>
        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Byte[] ws_dll_service_tda(string _name_archivo)
        {
            Byte[] _archivo = null;
            try
            {
                _archivo = Basico._extrae_archivo_dlltda(_name_archivo);
               // _archivo = null;
            }
            catch
            {
                _archivo = null;
            }
            return _archivo;
        }

        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public Boolean ws_existe_exewinupdate_version(string _version)
        {
            Boolean _valida = false;
            try
            {
                //_valida = Basico._verifica_version_exeupdate(_version);
                _valida = false;
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Boolean ws_existe_fepe_dll(Decimal _tamaño)
        {
            Boolean _valida = false;
            try
            {
                _valida = Basico._verifica_dll_fepe(_tamaño);
               // _valida = false;
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Boolean ws_existe_certificado(Decimal _tamaño)
        {
            Boolean _valida = false;
            try
            {
                _valida = Basico._verifica_certificado(_tamaño);
               // _valida = false;
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Boolean ws_existe_genera_hash_version(string _version)
        {
            Boolean _valida = false;
            try
            {
                _valida = Basico._verifica_version_dllgenerahash(_version);
               // _valida = false;
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Boolean ws_existe_serviciowin_version(string _version)
        {
            Boolean _valida = false;
            try
            {
               _valida=Basico._verifica_version_windll(_version);
               // _valida = false;
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }
        #endregion

        #region<REGION PARA GENERAR EL MOVIMIENTO WEB SERVICE>
        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public string ws_envia_stock_inv(string _cod_tda,DataTable dtstk_plan)
        {
            string _error = "";
            try
            {

                //_error = Basico._envia_stock_pla(_cod_tda, dtstk_plan);
                _error = "xxx";
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_error_mov_transac(string _cod_tda,string _error_des)
        {
            string _error = "";
            try
            {
                //_error = Basico._update_error(_cod_tda, _error_des);
                _error = "xx";
            }
            catch
            {

            }
            return _error;
        }

        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public string ws_genera_mov(string _tipo_origen,string _cod_tda, DataTable _dt_cab, DataTable _dt_det)
        {
            string _error = "";
            try
            {
                //_error= Basico._generar_movimiento(_tipo_origen,_cod_tda, _dt_cab, _dt_det);
                _error = "xxx";
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }
        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public DataSet ws_recepcion_transac(string _cod_tda)
        {
            DataSet ds = null;
            try
            {
                //ds = Basico._transac_pendientes(_cod_tda);
                ds = null;
            }
            catch
            {
                ds = null;
            }
            return ds;
        }
        [WebMethod,SoapHeader("CredencialAutenticacion")]
        public String ws_transaccion_flag(Decimal _nro_transa)
        {
            string _error = "";
            try
            {
                //_error = Basico._transac_flag_update(_nro_transa);
                _error = "sss";
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        #endregion



        #region<REGION DE PRUEBA ENVIO DATA SQL>


        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public Boolean ws_tienda_inv(string _cod_tda)
        {
            Boolean _valida = false;
            try
            {
                //_valida = Basico.getTienda_inv(_cod_tda);
                _valida = false;
            }
            catch
            {
                _valida = false;
            }
            return _valida;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string ws_existe_archivo_SQL(string _nombre)
        {
            string _error = "";
            try
            {
                _error = Basico._existe_archivo(_nombre);
                //_error = "xxxx";
            }
            catch(Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }

        [WebMethod, SoapHeader("CredencialAutenticacion")]
        public string[] ws_transmision_ingreso_SQL(Byte[] _archivo_zip, string _name)
        {
            string valida = "";
            try
            {
                //valida = "xxxx";
                if (VerificarPermisos(CredencialAutenticacion))
                {

                    valida = Basico.copiar_archivo_Tienda_SQL(_archivo_zip, _name);

                }
                else
                {
                    valida = "usuario y/o contraseña no valida";
                }
            }
            catch (Exception exc)
            {
                valida = exc.Message;
            }

            String[] _respuesta = new String[] { "codigo", "descripcion" };
            string _error_codigo = "";
            string _mensaje = "";
            //Resultado res;
            if (valida.Length == 0)
            {
                _error_codigo = "1";
                _mensaje = "transmision exitosa";
                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);

                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }
            else
            {

                //res.codresul = _error_codigo.ToString();
                //res.desresul = _mensaje.ToString();
                //res = new Resultado(_error_codigo, _mensaje);
                _error_codigo = "0";
                _mensaje = valida;
                _respuesta[0] = _error_codigo.ToString();
                _respuesta[1] = _mensaje.ToString();
            }

            //return res;
            return _respuesta;
        }
        #endregion

        #region<UPDATE DE CUPONES>
        [WebMethod]
        public string ws_update_vales(string _serie, string _correlativo, string _cod_tda_venta, string _dni_venta,
            string _nombres_venta, string _fecha_doc, string _tipo_doc, string _serie_doc, string _numero_doc,
            string _estado_doc, string _fc_nint, string _email_venta, string _telefono_venta)
        {
            string _error = "";
            try
            {
                //_error = "xxxx";
                _error = Basico.update_vales(_serie, _correlativo, _cod_tda_venta, _dni_venta,
                _nombres_venta, _fecha_doc, _tipo_doc, _serie_doc, _numero_doc, _estado_doc, _fc_nint, _email_venta, _telefono_venta);

            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }
        [WebMethod]
        public string ws_update_venta_empl(string _Tip_Id_Ven, string _Nro_Dni_Ven, string _Cod_Tda_Ven,
                                                  string _Nro_Doc_Ven, string _Tip_Doc_Ven, string _Ser_Doc_Ven,
                                                  string _Num_Doc_Ven, string _Fec_Doc_Ven, string _Est_Doc_Ven,
                                                  string _Fc_Nin_Ven)
        {
            string _error = "";
            try
            {
                //_error = "ssss";

                _error = Basico.update_venta_empleado(_Tip_Id_Ven, _Nro_Dni_Ven, _Cod_Tda_Ven,
                                                    _Nro_Doc_Ven, _Tip_Doc_Ven, _Ser_Doc_Ven,
                                                    _Num_Doc_Ven, _Fec_Doc_Ven, _Est_Doc_Ven,
                                                    _Fc_Nin_Ven);
            }
            catch (Exception exc)
            {
                _error = exc.Message;
            }
            return _error;
        }
        #endregion
    }
}
