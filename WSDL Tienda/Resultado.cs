﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSDL_Tienda
{
    public class Resultado
    {
        private string _codresul;
        private String _desresul;
        public String codresul
        {
            get { return _codresul; }
            set { _codresul = value; }
        }
        public String desresul
        {
            get { return _desresul; }
            set { _desresul = value; }
        }
        public Resultado()
        {
        }
        public Resultado(String vcodresul,String vdesresul):this()
        {
            this.codresul = vcodresul;
            this.desresul = vdesresul;
        }
    }

    public class Resultado_Tda_Ecu
    {
        public Boolean existe { get; set; }
        public string descripcion { get; set; }
    }
}