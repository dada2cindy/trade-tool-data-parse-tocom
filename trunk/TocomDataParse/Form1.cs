using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace TocomDataParse
{
    public partial class Form1 : Form
    {
        private static readonly string DATA_PATH = "Data";
        private static readonly string OUTPUT_PATH = "output";
        private static readonly string DATA_URL = "http://www.tocom.or.jp/data/yakujou/";

        public Form1()
        {
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            //商品清單
            cbProduct.DataSource = GetProductList();
            cbProduct.DisplayMember = "Key";
            cbProduct.ValueMember = "Value";

            //回補月分清單
            cbDataCovertDate.DataSource = GetDataCoverDateList(6);
            cbDataCovertDate.DisplayMember = "Key";
            cbDataCovertDate.ValueMember = "Value";
        }

        private IList<KeyValuePair<string, string>> GetProductList()
        {
            Dictionary<string, string> dicProductList = new Dictionary<string, string>();

            //橡膠
            dicProductList.Add("JRU-橡膠", "81");
            //黃金
            dicProductList.Add("JAU-黃金", "11");
            //小黃金
            dicProductList.Add("JAM-小黃金", "16");
            //白銀
            dicProductList.Add("JSV-白銀", "12");
            //白金
            dicProductList.Add("JPL-//白金", "13");
            //小白金
            dicProductList.Add("JPM-小白金", "17");
            //鈀金
            dicProductList.Add("JPA-鈀金", "14");
            //汽油
            dicProductList.Add("JGL-汽油", "31");
            //燃油
            dicProductList.Add("JKE-燃油", "32");
            //原油
            dicProductList.Add("JCO-原油", "33");

            return dicProductList.ToList();
        }

        private static IList<KeyValuePair<string, string>> GetDataCoverDateList(int backMonths)
        {
            Dictionary<string, string> dicDataCoverDate = new Dictionary<string, string>();
            DateTime dateNow = DateTime.Today;

            for (int i = 0; i < backMonths; i++)
            {
                DateTime dateBack = dateNow.AddMonths((-1) * i);
                string key = string.Format("{0}-{1}", dateBack.Year, dateBack.Month.ToString().PadLeft(2, '0'));
                string value = string.Format("{0}-{1}.csv", dateBack.Year, dateBack.Month.ToString().PadLeft(2, '0'));
                dicDataCoverDate.Add(key, value);
            }

            return dicDataCoverDate.ToList();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";

            string code = cbProduct.SelectedValue.ToString();
            string fileName = cbProduct.Text.Split('-')[0];

            DoDataTransform(code, fileName);

            lblStatus.Text = "轉檔成功";
        }

        private void DoDataTransform(string code, string name)
        {
            string dataPath = txtPath.Text.Trim().ToUpper();
            string dataCode = code.Trim();
            string outFileName = name.Trim().ToUpper();
            string[] tradingMethod = new string[] { "11", "1" };
            //string path = Path.Combine(OUTPUT_PATH, outFileName + ".csv");
            FileStream fs = File.Create(Path.Combine(OUTPUT_PATH, outFileName + ".csv"));
            fs.Close();
            fs.Dispose();

            string[] directories = Directory.GetDirectories(Path.Combine(DATA_PATH, dataPath));
            foreach (string subPath in directories)
            {
                string[] dataFiles = Directory.GetFiles(subPath);
                foreach (string fileName in dataFiles)
                {
                    //FileInfo fileInfo = new FileInfo(fileName); 
                    StreamReader stmRdr = new StreamReader(fileName);
                    StreamWriter stmWtr = new StreamWriter(Path.Combine(OUTPUT_PATH, outFileName + ".csv"), true);
                    KData kData = null;
                    string data = stmRdr.ReadLine();
                    while (data != null)
                    {
                        string[] dataArray = data.Split(',');

                        if (dataCode.Equals(dataArray[2]) && tradingMethod.Contains((dataArray[1])))
                        {
                            KData currentKData = KDataConverter.ToKDataObject(dataArray);
                            if (kData != null)
                            {
                                if (!currentKData.Date.Equals(kData.Date))
                                {
                                    //寫入檔案到.csv
                                    stmWtr.WriteLine(KDataConverter.ToCSVObject(kData));
                                }
                            }
                            kData = currentKData;
                        }
                        data = stmRdr.ReadLine();
                    }
                    if (kData != null)
                    {
                        //寫入最後一筆
                        stmWtr.WriteLine(KDataConverter.ToCSVObject(kData));
                    }
                    stmRdr.Close();
                    stmRdr.Dispose();
                    stmWtr.Close();
                    stmWtr.Dispose();
                }
            }
        }

        private void btnDataCovert_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "開使回補";

            string fileDate = cbDataCovertDate.SelectedValue.ToString();
            string year = fileDate.Substring(0, 4);
            string url = DATA_URL + fileDate;

            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            Stream dataStream = httpResponse.GetResponseStream();
            byte[] buffer = new byte[81920];

            string dataPath = txtPath.Text.Trim().ToUpper();
            string fileFullName = Path.Combine(DATA_PATH, dataPath, year, fileDate);
            FileStream fs = File.Create(fileFullName);
            fs.Close();
            fs.Dispose();

            fs = new FileStream(fileFullName, FileMode.Open, FileAccess.Write);

            int size = 0;
            do
            {
                size = dataStream.Read(buffer, 0, buffer.Length);

                if (size > 0)

                    fs.Write(buffer, 0, size);

            } while (size > 0);

            fs.Close();
            fs.Dispose();

            dataStream.Close();
            httpResponse.Close();

            lblStatus.Text = "回補成功";
        }
    }
}
