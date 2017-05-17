using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace WSDL_Tienda
{
    public class Paquete
    {
        public static string enviar_zip(byte[] _archivo,string _nombre_archivo)
        {
            string resulado = "";
            string _ruta_carpeta = "d:/tdprueba.zip";
            try
            {
               
                if (System.IO.File.Exists(_ruta_carpeta))
                {
                    System.IO.File.Delete(_ruta_carpeta);
                }


                if (!(System.IO.File.Exists(_ruta_carpeta)))
                {
                    StreamWriter objlog = new StreamWriter(_ruta_carpeta);
                    objlog.Flush();
                    objlog.Close();
                    objlog.Dispose();
                    UnzipFile(_ruta_carpeta, _archivo);                   
                }
              
                                            
                resulado = "";
            }
            catch (Exception exc)
            {
                if (System.IO.File.Exists(_ruta_carpeta))
                {
                    System.IO.File.Delete(_ruta_carpeta);
                }
                resulado = exc.Message;
            }
            return resulado;
        }
        public static void UnzipFile(string sourcePath, byte[] gzip)
        {            
            FileStream streamWriter = null;
            try
            {
                Stream stream1 = new MemoryStream(gzip);
                using (ZipInputStream s = new ZipInputStream(stream1))
                {
                    ZipEntry theEntry;
                    while ((theEntry = s.GetNextEntry()) != null)
                    {

                        streamWriter = File.OpenWrite(sourcePath);

                        int size = 8192;
                        byte[] data = new byte[8192];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;

                            }
                        }              
                        streamWriter.Close();

                    }
                }
                              
            }
            catch (Exception)
            {
                streamWriter.Close();
                throw;
            }
        }
    }

}