using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WinformsMVCAsyncExampleNetStandard.ServicioPrueba;

namespace WinformsMVCAsyncExampleNetStandard.Controllers.Main
{
    public class Controller
    {
        #region Singleton
        private static Controller instance;
        public static Controller Instance
        {
            get
            {
                if (instance == null) instance = new Controller();

                return instance;
            }
        }
        #endregion

        public delegate void ModelChangeDelegate(object sender, ModelChangeEventArgs e);
        public event ModelChangeDelegate ModelChangeEvent;

        RijndaelManaged cripto = new RijndaelManaged();

        public void RegisterView(IView view)
        {
            this.ModelChangeEvent += new ModelChangeDelegate(view.ModelChange);
        }
        public void UnregisterView(IView view)
        {
            this.ModelChangeEvent -= new ModelChangeDelegate(view.ModelChange);
        }

        public void RaiseModelChange(object sender, ModelChangeEventArgs e, ref BackgroundWorker worker)
        {
            if (ModelChangeEvent != null)
            {
                e.ElapsedMilliseconds = CalculateSaldos(worker);
                ModelChangeEvent(sender, e);

            }
        }

        public long CalculateSaldos(BackgroundWorker worker, bool useService = false)
        {

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            string file = AppDomain.CurrentDomain.BaseDirectory + "\\saldo.bin";


            Transaccion[] resp = null;
            ServiceClient client = new ServicioPrueba.ServiceClient();

            if (useService)
            {
                // the certificate is invalid for the domain: https://localhost:6300/Service.svc
                // only for test purpoises... 
                System.Net.ServicePointManager.ServerCertificateValidationCallback +=
                    (se, cert, chain, sslerror) =>
                    {
                        return true;
                    };

                //TODO: Cache data, check for changes first.
                resp = client.GetData("user", "password");
                SerializationToFile(file, resp);
            }
            else
            {

                resp = (Transaccion[])DeserializeFile(file);

            }

            int percentComplete = 0, n = 0;
            int cntTrans = (int)resp.Count();
                       
            var parallelOptions = new ParallelOptions() {
                MaxDegreeOfParallelism = -1
            };


            Parallel.ForEach(resp, parallelOptions, transaccion =>
                           {

                               foreach (var r in resp)
                               {
                                   Task.Delay(3000);
                               }

                               n++;
                               percentComplete = (n *100 / cntTrans);
                               worker.ReportProgress(percentComplete);

                           });

  
            sw.Stop();

            return sw.ElapsedMilliseconds;

        }

        public long CalculateCommision(long n)
        {            

            int count = 0, prime;
            long a = 0, b;

            while (count < n)
            {
                a = 2;
                b = 2;
                prime = 1;

                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }

                if (prime > 0)
                {
                    count++;
                }
                a++;
            }

            return --a;
        }
        

        public string Desencrypt(string ClaveCifrado, string Cadena)
        {
     
            byte[] Clave = Encoding.ASCII.GetBytes(ClaveCifrado);
            byte[] IV = Encoding.ASCII.GetBytes("1234567812345678");

            byte[] inputBytes = Convert.FromBase64String(Cadena);

            string result = string.Empty;

            using (MemoryStream ms = new MemoryStream(inputBytes))
            {
                using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(objCryptoStream, true))
                    {
                        result = sr.ReadToEnd();
                    }
                }
            }
            return result;
        }

        void SerializationToFile(string serializationFile, Object obj) {

            using (Stream stream = File.Open(serializationFile, FileMode.Create))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                bformatter.Serialize(stream, obj);
            }

        }

        Object DeserializeFile(string deSerializationFile) {

            using (Stream stream = File.Open(deSerializationFile, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                return bformatter.Deserialize(stream);
            }

        }




    }
}
