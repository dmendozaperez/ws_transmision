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

           String pathFileIn = @"D:\TD170505.107";

            byte[] _archivo_bytes = File.ReadAllBytes(pathFileIn);



            bataconexion.bata_transaccionSoapClient trans = new bataconexion.bata_transaccionSoapClient();

            //string _error = trans.ws_error_mov_transac(conexion, "5034", "xxxxxx");
            //byte[] _archivo_bytes = trans.ws_transmision_salida(conexion, "50797");

            //string[] _archivo = { "16154501.cen" };

            //string[] _valor = trans.ws_borrar_archivo_cen(conexion, "00048", _archivo);

            String[] _mensaje = trans.ws_transmision_ingreso(conexion, _archivo_bytes, "TD170505.107");

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
    }
}
