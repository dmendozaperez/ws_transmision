using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Capa_Dato;
namespace Form_Cliente
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            bataconexion.Autenticacion conexion = new bataconexion.Autenticacion();
            conexion.user_name = "emcomer";
            conexion.user_password = "Bata2013";

           String pathFileIn = @"D:\TD171025.104";

            //byte[] _archivo_bytes = trans.ws_transmision_salida(conexion, "50805");

            //byte[] _archivo_bytes = File.ReadAllBytes(pathFileIn);



            bataconexion.bata_transaccionSoapClient trans = new bataconexion.bata_transaccionSoapClient();

            byte[] _archivo_bytes = trans.ws_transmision_salida(conexion, "00048");

            File.WriteAllBytes("D://pruebacen.zip", _archivo_bytes);

            //string[] _archivo = { "24174628.cen", "24175733.cen", "24190415.cen" };
            //string[] _valor = trans.ws_borrar_archivo_cen(conexion, "50003", _archivo);



            //string _existe = trans.ws_existe_tienda(conexion, "50147");


            //string _error = trans.ws_error_mov_transac(conexion, "5034", "xxxxxx");
            //byte[] _archivo_bytes = trans.ws_transmision_salida(conexion, "50805");

            //File.WriteAllBytes("D://pruebacen.zip", _archivo_bytes);

            //string[] _archivo = { "16154501.cen" };

            //string[] _valor = trans.ws_borrar_archivo_cen(conexion, "00048", _archivo);

            String[] _mensaje = trans.ws_transmision_ingreso(conexion, _archivo_bytes, "TD171025.104");

            string _va;
            _va = "";

            //string[] _archivo = { "16221149", "16230703", "17001258", "17080754", "17090807", "17100335", "17110208", "17120147", "17130851", "17140139" };
            //string[] _valor = trans.ws_borrar_archivo_cen(conexion, "50143", _archivo);

            //DateTime fecha_ini =Convert.ToDateTime("23/02/2015");
            //DateTime fecha_fin = Convert.ToDateTime("24/02/2015");

            //string[] inser = trans.ws_inserta_asistencia(conexion, "83056", "50174", fecha_ini);

            //DataSet inser = trans.ws_consulta_asistencia(conexion, "50143", fecha_ini, fecha_fin);

            //Byte[] _archivo = trans.ws_actualiza_archivo(conexion, "POSVFP.exe", "1.1.1");

            //string _existe = trans.ws_existe_tienda(conexion, "50147");

            //MessageBox.Show("exito");

            //Cursor.Current = Cursors.Default;
            //return;

            //byte[] _archivo_bytes = trans.ws_transmision_salida(conexion, "50401");
            
            

            //string[] _archivo = { "05131911.CEN", "05140706.CEN", "05141606.CEN", "05143902.CEN", "06220230.CEN", "06230051.CEN"};
            //string[] _valor= trans.ws_borrar_archivo_cen(conexion, "50401", _archivo);

            //string va = "";

            //Cursor.Current = Cursors.Default;
            //return;

            //string[] _mensaje = trans.ws_tienda_lista(conexion);



            //Byte[] _archivo = trans.ws_update_app_tx(conexion, "1.1.23");

            ////converti array archivo

            //if (_archivo!=null)
            //{ 
            //    File.WriteAllBytes(@"C:/TRANSMI.exe", _archivo);
            //    MessageBox.Show("Se genero el archivo");
            //    return;
            //}


            //

            //string _existe = trans.ws_existe_tienda(conexion, "50147");

            //String[] _mensaje = trans.ws_transmision_ingreso(conexion, _archivo_bytes, "TD140118.147");

            //string[] _mensaje = trans.ws_tienda_lista(conexion);



            //trans.ws_tienda_lista(  trans.ws_tienda_lista(conexion);



            //bataconexion.Resultado  res = trans.web_service_ingreso(conexion, _archivo_bytes, "TD140118.147");

            //if (_mensaje[0].ToString() == "0")
            //{
            //    MessageBox.Show("El archivo fue enviado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
            //    MessageBox.Show(_mensaje[1].ToString(), "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

            //MessageBox.Show(_mensaje);

            Cursor.Current = Cursors.Default;
        }

        private void btnupload_Click(object sender, EventArgs e)
        {
            metodo_upload();
        }
        private void metodo_upload()
        {
            try
            {
                string _tienda = "XXXX";

                bataconexion.Autenticacion conexion = new bataconexion.Autenticacion();
                conexion.user_name = "emcomer";
                conexion.user_password = "Bata2013";

                bataconexion.bata_transaccionSoapClient trans = new bataconexion.bata_transaccionSoapClient();
                var _lista_file = trans.ws_get_file_upload(conexion);
                if (_lista_file!= null)
                {
                    foreach(var itemcab in _lista_file)
                    {
                        string _carpeta_local = itemcab.tda_act_carpetalocal;
                        string _carpeta_server = itemcab.tda_act_rutaws + "\\" + itemcab.tda_act_carpetanom;
                        foreach (var itemdet in itemcab.tda_act_file)
                        {
                            string _fecha_file_server = itemdet.fecha_file_server;
                            string _nombre_filer_server = itemdet.name_file_server;
                            decimal _longitud_file_server = itemdet.longitud_file_server;

                            string _ruta_local_file = _carpeta_local + "\\" + _nombre_filer_server;
                            /*si el archivo existe entonces verificamos que este con la ultima version por la fecha de modificacion*/
                            if (File.Exists(@_ruta_local_file))
                            {

                                FileInfo info = new FileInfo(@_ruta_local_file);
                                string _fecha_file_local= info.LastWriteTime.ToString("dd/MM/yyyy H:mm:ss");
                                decimal _longitud_file_local = info.Length;

                                /*si la fecha es diferente entonces modificamos*/
                                if (_longitud_file_server != _longitud_file_local)
                                {
                                    string file_ruta_server = _carpeta_server + "\\" + _nombre_filer_server;

                                    byte[] file_upload = trans.ws_bytes_file_server(conexion,file_ruta_server);

                                    if (file_upload!=null)
                                    {
                                        File.WriteAllBytes(@_ruta_local_file, file_upload);

                                        string[] _existe_ws_urldata = trans.ws_existe_fepe_dll_data(conexion, _tienda, _nombre_filer_server, _longitud_file_server);
                                        if (_existe_ws_urldata[0].ToString() == "0")
                                        {
                                            string _act = trans.ws_update_fepe_dll(conexion, _tienda, _nombre_filer_server, _longitud_file_server);
                                        }

                                    }
                                }
                                else
                                {
                                    //_dbftienda();
                                    string[] _existe_ws_urldata = trans.ws_existe_fepe_dll_data(conexion, _tienda, _nombre_filer_server, _longitud_file_server);
                                    if (_existe_ws_urldata[0].ToString() == "0")
                                    {
                                        string _act = trans.ws_update_fepe_dll(conexion, _tienda, _nombre_filer_server, _longitud_file_server);
                                    }
                                }

                            } 

                        }
                    }
                 }
            }
            catch
            {

            }
        }

        private void btnguias_Click(object sender, EventArgs e)
        {
            bataconexion.Autenticacion conexion = new bataconexion.Autenticacion();
            conexion.user_name = "emcomer";
            conexion.user_password = "Bata2013";
            bataconexion.bata_transaccionSoapClient trans = new bataconexion.bata_transaccionSoapClient();
            var lista= trans.ws_get_guias_tienda_almacen(conexion, "50336");

            foreach(var guia in lista)
            {
                //guia.
            }
        }
    }
}
