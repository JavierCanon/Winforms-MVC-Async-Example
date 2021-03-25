//PRUEBA OLIMPIA
//La función de la aplicación actual es calcular el saldo final de las cuentas de un "banco", para esto se consume un servicio que devuelve 
//las transacciones realizas a la cuentas.

//Paso 1: Hacer funcionar la aplicación. Debido al aumento de transacciones y al  colocar al servicio con SSL la aplicación actual esta fallando.
//Paso 2: Estructurar mejor el codigo. Uso de patrones, buenas practicas, etc.
//Paso 3: Optimizar el codigo, como se menciono en el paso 1 el aumento de transacciones ha causado que el calculo de los saldos se demore demasiado.
//Paso 4: Adicionar una barra de progreso al formulario. Actualizar la barra con el progreso del proceso, evitando bloqueos del GUI.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinformsMVCAsyncExampleNetStandard.Controllers.Main;

namespace WinformsMVCAsyncExampleNetStandard.Views.Main
{
    public partial class FormMain : Form, IView
    {
        BackgroundWorker Bw = new BackgroundWorker();
        bool isFormClosing = false;

        public FormMain()
        {
            InitializeComponent();
            Bw.WorkerReportsProgress = true;
            Bw.WorkerSupportsCancellation = true;
            Bw.DoWork += Bw_DoWork;
            Bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            Bw.ProgressChanged += Bw_ProgressChanged;

        }


        #region IView Members

   
        public void ModelChange(object sender, ModelChangeEventArgs e)
        {
            string newText = (e.ElapsedMilliseconds/1000).ToString("n") + " seg.";
            lblTiempoTotal.Invoke((MethodInvoker)delegate {
                lblTiempoTotal.Text = newText;
            });

            marqueeProgressBarControl1.Invoke((MethodInvoker)delegate {
                marqueeProgressBarControl1.Visible = false;
            });


        }

        #endregion


        private void FormMain_Load(object sender, EventArgs e)
        {

            Controller.Instance.RegisterView(this);
        }
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Bw.CancelAsync();
            isFormClosing = true;
        }


        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Controller.Instance.UnregisterView(this);
        }


        private void BtnCalcular_Click(object sender, EventArgs e)
        {

            if (!Bw.IsBusy)
            {
                Bw.RunWorkerAsync();
                marqueeProgressBarControl1.Visible = true;
                progressBar1.Value = 0;
                progressBar1.Visible = true;
                lblTiempoTotal.Text = string.Empty;
            }
        }

        void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Controller.Instance.RaiseModelChange(this, new ModelChangeEventArgs(new long()), ref Bw);
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int p = e.ProgressPercentage;
            if (!isFormClosing)
            {
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value = p;

                });
            }
        }

        void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            progressBar1.Invoke((MethodInvoker)delegate {
                progressBar1.Value = 100;
            });

        }


    }
}
