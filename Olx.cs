using PlistFileReadingConsole;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Claunia.PropertyList;
namespace DarkDemo
{
    public partial class Olx : Form
    {
        public string OLX_PATH = "";
        string button_selected = "";
        DataSet data = new DataSet();
        
        string s, t,u,v;
        public Olx()
        {
            InitializeComponent();
        }

        public Olx(string name)
        {
            InitializeComponent();
            char[] delims = new[] { '\r', '\n' };
            string text = File.ReadAllText("Configurations.txt");
            string[] values = text.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            string[] path = values[0].Split('=');
            OLX_PATH = path[1];
            button_selected = name;
            LoadData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Olx_Menu menu = new Olx_Menu();
            this.Hide();
            menu.Show();
        }

        void StartProgressBar()
        {
            if (dgv_category.Visible)
                dgv_category.Visible = false;
            
           pictureBox2.Visible = false; pictureBox3.Visible = false; pictureBox4.Visible = false;
            pictureBox2.ImageLocation = OLX_PATH + "\\databases\\LetGoImage\\2021020102021033.jpg";
            pictureBox3.ImageLocation = OLX_PATH + "\\databases\\LetGoImage\\2021020102021083.jpg";
            pictureBox4.ImageLocation = OLX_PATH + "\\databases\\LetGoImage\\2021020102021131.jpg";
            label1.Visible = false;
            label3.Visible = false;
            label4.Visible = false;

            lblFetch.Visible = true;
            progressBar1.Visible = true;
            progressBar1.Value = 0;
            progressBar1.Update();
            backgroundWorker1.RunWorkerAsync();
        }

        [Obsolete]
        void LoadData()
        {
            string query = "";
            DataTable dt = new DataTable();
            StartProgressBar();
            //lblinfo.Visible = true;
            SQLiteConnection conn = new SQLiteConnection();
            try
            {
                if (button_selected == "Images")
                {
                    lblName.Text = button_selected;
                    pictureBox2.Visible = true;
                    pictureBox3.Visible = true;
                    pictureBox4.Visible = true;
                    label1.Visible = true;
                    label3.Visible = true;
                    label4.Visible = true;
                }
                    if (button_selected == "User Login Accounts")
                {

                    btnSummary.Visible = true;
                    lblName.Text = button_selected;
                    
                    FileStream fs = new FileStream(OLX_PATH + "\\databases\\panamera.olx.pk.plist", FileMode.Open, FileAccess.Read);
                    var dict = DataExtractor.GetDataFronPList(fs);
                    dgv_category.Rows.Clear();
                    dgv_category.Refresh();
                    
                    dgv_category.ColumnCount = 2;
                    dgv_category.Columns[0].Name = "Name";
                    dgv_category.Columns[1].Name = "Value";
                    foreach (var group in dict)
                    {
                        s = group.Key.ToString();
                        t= group.Value.ToString();
                        dgv_category.Rows.Add(s.ToString().Trim('\n'), t.ToString().Trim('\n'));

                    }
                    
                        /*  XDocument docs = XDocument.Load(OLX_PATH + "\\databases\\panamera.olx.pk.xml");
                          var elements = docs.Descendants("dict");
                          Dictionary<string, string> keyValues = new Dictionary<string, string>();



                          foreach (var a in elements)
                          {

                         keyValues = docs.Descendants("dict")
          .SelectMany(d => d.Elements("key").Zip(d.Elements().Where(e => e.Name != "key"), (k, v) => new { Key = k, Value = v }))
          .ToDictionary(i => i.Key.Value, i => i.Value.Value);

                          }
                          dgv_category.DataSource = (from d in keyValues orderby d.Value select new { d.Key, d.Value }).ToList();
                          dgv_category.DataSource = data.Tables[0];*/


                        /* lblName.Text = button_selected;
                         btnSummary.Visible = true;
                         conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                         conn.Open();
                         query = "Select * From ZUSER";
                         SQLiteCommand cmd = new SQLiteCommand(query, conn);
                         SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                         da.Fill(dt);
                         da.Fill(data);
                         dgv_category.DataSource = dt;*/
                    }
                if (button_selected == "App Activity")
                {


                    lblName.Text = button_selected;

                    FileStream fs = new FileStream(OLX_PATH + "\\databases\\asia.olx.pk.plist", FileMode.Open, FileAccess.Read);
                    var dict = DataExtractor.GetDataFronPList(fs);
                    dgv_category.Rows.Clear();
                    dgv_category.Refresh();

                    dgv_category.ColumnCount = 2;
                    dgv_category.Columns[0].Name = "Key";
                    dgv_category.Columns[1].Name = "Value";
                    foreach (var group in dict)
                    {
                        s = group.Key.ToString();
                        t = group.Value.ToString();
                        dgv_category.Rows.Add(s.ToString().Trim('\n'), t.ToString().Trim('\n'));

                    }
                }
                    if (button_selected == "Posted Ad's")
                {
                    lblName.Text = button_selected;
                    btnSummary.Visible = true;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                    conn.Open();
                    query = "Select Z_PK AS 'ID',ZADTITLE AS 'AD Title',ZLATITUDE as 'Latitude',ZLONGITUDE as 'Longitude',ZDISPLAYPRICE as 'Price',ZADSTATUS as 'AD Status',ZCURRENCY as 'Currency',ZIMAGEURL as 'Image URL' From ZAD";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    da.Fill(data);
                    dgv_category.DataSource = dt;
                    conn.Close();
                }
           
                if (button_selected == "Conversations")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                    conn.Open();
                    query = "SELECT ZUSERID FROM ZUSER";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    getConverstationData(dt);
                 
                   
               
                }
                if (button_selected == "Ad's Details")
                {
                  lblName.Text = button_selected;
                    btnSummary.Visible = true;

                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                    conn.Open();
                    query = "Select Z_PK AS 'ID',ZADCITYID AS 'City ID',ZITEMID AS 'Item ID',ZITEMCATEGORYID AS 'Category ID', ZADTITLE AS 'AD Title',ZLATITUDE as 'Latitude',ZLONGITUDE as 'Longitude',ZDISPLAYPRICE as 'Price',ZADSTATUS as 'AD Status',ZCURRENCY as 'Currency',ZIMAGEURL as 'Image URL' From ZAD";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    da.Fill(data);
                    dgv_category.Rows.Clear();
                    dgv_category.Refresh();
/*
                    dgv_category.ColumnCount = 12;
                    dgv_category.Columns[0].Name = "ID";
                    dgv_category.Columns[1].Name = "City ID";
                    dgv_category.Columns[2].Name = "Item ID";
                    dgv_category.Columns[3].Name = "Category ID";
                    dgv_category.Columns[4].Name = "AD Title";
                    dgv_category.Columns[5].Name = "Latitude";
                    dgv_category.Columns[6].Name = "Longitude";
                    dgv_category.Columns[7].Name = "Price";
                    dgv_category.Columns[8].Name = "AD Status";
                    dgv_category.Columns[9].Name = "Currency";
                    dgv_category.Columns[10].Name = "Image URL";
               
               
                    
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        dgv_category.Rows.Add(dt.Rows[i].ItemArray[0].ToString(),  dt.Rows[i].ItemArray[1].ToString(),dt.Rows[i].ItemArray[2].ToString(),
                                              dt.Rows[i].ItemArray[3].ToString(), dt.Rows[i].ItemArray[4].ToString(), dt.Rows[i].ItemArray[5].ToString(), dt.Rows[i].ItemArray[6].ToString(), dt.Rows[i].ItemArray[7].ToString(), dt.Rows[i].ItemArray[8].ToString(), dt.Rows[i].ItemArray[9].ToString(), dt.Rows[i].ItemArray[10].ToString()
                                          );
                    }*/
               dgv_category.DataSource = dt;
                }
                if (button_selected == "Buyer's info")
                {
                    lblName.Text = button_selected;
                 
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                    conn.Open();
                    query = "Select ZUSERID as 'ID',ZNAME as 'Name',ZUSERSTATUS as 'Status' from ZUSER";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    da.Fill(data);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "Olx Attributes")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ncninjatrackingtable.sqlite";
                    conn.Open();
                    query = "Select Z_PK AS 'ID',ZEVENT AS 'Category ID',ZKEY as 'Version',ZVALUE as 'Value' From ZTRACK";
                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }
                if (button_selected == "App Security")
                {
                    lblName.Text = button_selected;
                    conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                    conn.Open();
                    query = "Select Z_PK AS 'ID',ZTITLE AS 'Title',ZSUBTITLE AS 'Subbtitle' ,ZSUBTYPE AS 'Category',ZID AS 'Name',ZBODY AS 'Message',ZICONURL AS 'Icon URL' From ZSYSTEMMESSAGEDATA";


                    SQLiteCommand cmd = new SQLiteCommand(query, conn);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                    da.Fill(dt);
                    dgv_category.DataSource = dt;
                }

                dgv_category.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgv_category.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i=0;i<100;i++)
            {
                if (backgroundWorker1.CancellationPending)
                    e.Cancel = true;
                else
                {
                    Thread.Sleep(30);
                    backgroundWorker1.ReportProgress(i);
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //ProgressBar.Visible = false;
            progressBar1.Value = 0;
            progressBar1.Visible = false;
            lblFetch.Visible = false;
            dgv_category.Visible = true;
            if(lblName.Text == "Conversations")
            { 
                lblList.Visible = true;
                cmbBuyerList.Visible = true;
                dgv_category.Location = new Point(dgv_category.Location.X, 38);
                dgv_category.Height = 607;
            }
        }

        private void btnSummary_Click(object sender, EventArgs e)
        {
            if (button_selected == "User Login Accounts")
            {
                LoginBtnSummanry();   
            }
            if (button_selected == "Posted Ad's")
            {
                PostedAdsBtnSummary();
            }
            if (button_selected == "Ad's Details")
            {
                DetailAdsBtnSummary();
            }
            if (button_selected == "Buyer's info")
            {
                BuyerInfoBtnSummary();
            }
        }

        void LoginBtnSummanry()
        {


            lblName.Text = button_selected;

            FileStream fs = new FileStream(OLX_PATH + "\\databases\\panamera.olx.pk.plist", FileMode.Open, FileAccess.Read);
            var dict = DataExtractor.GetDataFronPList(fs);
            dgv_category.Rows.Clear();
            dgv_category.Refresh();

            dgv_category.ColumnCount = 2;
            dgv_category.Columns[0].Name = "Artifacts";
            dgv_category.Columns[1].Name = "Information";
            foreach (var group in dict)
            {
                s = group.Key.ToString();
                t = group.Value.ToString();
                if (s.ToString().Trim('\n') == "AppsFlyerUserId")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "currentLanguage")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "AKLastCheckInAttemptDate")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "AKLastCheckInSuccessDate")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "baseurl")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "userPhoneNumber")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
              
                if (s.ToString().Trim('\n') == "AppleLocale")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                
                if (s.ToString().Trim('\n') == "login_method")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "version_number")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "AKLastEmailListRequestDateKey")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                if (s.ToString().Trim('\n') == "userEmail")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
               
               
                if (s.ToString().Trim('\n') == "last_time_update_notification_updated")
                {
                    dgv_category.Rows.Add(s.ToString(), t.ToString());
                }
                // dgv_category.Rows.Add(s.ToString().Trim('\n').Trim('\n'), t.ToString().Trim('\n').Trim('\n'));

            }


        } //end of function
        string query = "";
        DataTable dt = new DataTable();
       
        //lblinfo.Visible = true;
        SQLiteConnection conn = new SQLiteConnection();
        void PostedAdsBtnSummary()
        {
            
            dgv_category.DataSource = null;
            dgv_category.Refresh();

         
            conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
            conn.Open();
            query = "Select Z_PK AS 'AD ID', ZADSTATUS as 'AD Status', ZDISPLAYPRICE as 'Price',ZADTITLE AS 'Description' From ZAD";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dt);
            da.Fill(data);
            dgv_category.DataSource = dt;
            conn.Close();

        } //end of function

        void DetailAdsBtnSummary()
        {
            dgv_category.DataSource = null;
            lblName.Text = button_selected;
            btnSummary.Visible = true;
            conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
            conn.Open();
            query = "Select Z_PK AS 'ID',ZITEMID AS 'Item ID',ZITEMCATEGORYID AS 'Category ID',ZLATITUDE as 'Latitude',ZLONGITUDE as 'Longitude' From ZAD";
            SQLiteCommand cmd = new SQLiteCommand(query, conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            da.Fill(dt);
            da.Fill(data);
            dgv_category.DataSource = dt;
            conn.Close();
        } //end of function

        void BuyerInfoBtnSummary()
        {
            string id = "", name = "", number = "";
            dgv_category.DataSource = null;
            dgv_category.Refresh();
            dgv_category.ColumnCount = 3;
            dgv_category.Columns[0].Name = "Buyer's Id";
            dgv_category.Columns[1].Name = "Name";
            dgv_category.Columns[2].Name = "Caller No's";
            for (int i = 0; i < data.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < data.Tables[0].Rows[i].ItemArray.Length; j++)
                {
                    string[] values = data.Tables[0].Rows[i].ItemArray[j].ToString().Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(',');
                    if (j == 1)
                        number = data.Tables[0].Rows[i].ItemArray[j].ToString();
                    else
                    {
                        for(int k=0;k<values.Length;k++)
                        {
                            string[] row_text = values[k].Trim(new Char[] { '{', '/', '\'', '}', '[', ']' }).Split(':');
                            if (row_text[0] == "\"id\"")
                            {
                                id = row_text[1];
                            }
                            else if(row_text[0] == "\"name\"")
                            {
                                name = row_text[1];
                            }
                        }
                    }
                }
                dgv_category.Rows.Add(id,name,number);
            }
        } //end of function
        void getConverstationData(DataTable conversation_list)
        {
            string[] curr_id, name;
            for (int i = 0; i < conversation_list.Rows.Count; i++)
            {
                curr_id = conversation_list.Rows[i].ItemArray[0].ToString().Split('\n');
                name = getBuyerDetail(curr_id[0]).Split('\n');
                cmbBuyerList.Items.Add(name[0].Trim('\n'));
            }
        } //end of function

        string getBuyerDetail(string id)
        {
            string buyer_detail = "";
            try
            {
                string query;
                DataTable buyerdata = new DataTable();
                SQLiteConnection conn = new SQLiteConnection();
                conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                conn.Open();
                query = "Select ZUSERID as 'ID' , ZNAME as 'Name' From ZUSER Where ZUSERID =" + id;
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(buyerdata);
                 buyer_detail = buyer_detail + buyerdata.Rows[0].ItemArray[1].ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return buyer_detail;
        } //end of function

        

        private void cmbBuyerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string sender_name="";
            string sender_type = "";
            string sender_from = "";
            string sender_1= "107664021";
            string sender_2 = "102605566";
            try
            {
                lblList.Visible = false;
                cmbBuyerList.Visible = false;
                string query,sender_detail,sender_id;
                DataTable conv_detail = new DataTable();
                DataTable conv_detail2 = new DataTable();
                conv_detail.Clear();
                SQLiteConnection conn = new SQLiteConnection();
                conn.ConnectionString = "Data Source =" + OLX_PATH + "\\databases\\ChatDataModel.sqlite";
                conn.Open();
                
                //get Conversation Id
                query = "Select ZUSERID From ZUSER Where ZNAME like"+"\"%" + cmbBuyerList.SelectedItem.ToString().Trim('\n') + "%\"";
                SQLiteCommand cmd = new SQLiteCommand(query, conn);
                SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
                da.Fill(conv_detail);
               sender_id = conv_detail.Rows[0].ItemArray[0].ToString();
              sender_name = cmbBuyerList.SelectedItem.ToString().Trim('\n');
                conv_detail.Clear();
                
                //get Conversation

              //  query = "Select ZMESSAGEID AS 'Message ID',ZBODY AS 'Text',ZDATESTRING as 'Date',ZSTATUS AS 'Status' From ZMESSAGE Where ZTO =" + sender_id;
                
                //


                //get Conversation
                if (sender_id == sender_1)
                {
                    query = "Select ZMESSAGEID AS 'Message ID',ZBODY AS 'Text',ZDATESTRING as 'Date',ZSTATUS AS 'Status',ZTO,ZFROM From ZMESSAGE Where ZCONVERSATIONDATA =1";
                }
                if (sender_id == sender_2)
                {
                    query = "Select ZMESSAGEID AS 'Message ID',ZBODY AS 'Text',ZDATESTRING as 'Date',ZSTATUS AS 'Status',ZTO,ZFROM From ZMESSAGE Where ZCONVERSATIONDATA =2";
                }
                cmd = new SQLiteCommand(query, conn);
                da = new SQLiteDataAdapter(cmd);
                da.Fill(conv_detail);
                //get buyer details
                sender_detail = getBuyerDetail(sender_id);
                string[] row_text = sender_detail.Split('\n');
                
                //Adding data to GridView
                dgv_category.Rows.Clear();
                dgv_category.Refresh();
               
                dgv_category.ColumnCount = 6;
                dgv_category.Columns[0].Name = "Buyer Name";
                dgv_category.Columns[1].Name = "Message ID";
                dgv_category.Columns[2].Name = "Sender Type";
                dgv_category.Columns[3].Name = "Text";
                dgv_category.Columns[4].Name = "Created Time";
                dgv_category.Columns[5].Name = "Status";
                for (int i=0;i<conv_detail.Rows.Count;i++)
                {
                    if (sender_id == conv_detail.Rows[i].ItemArray[5].ToString() )
                    {
                        if (sender_1 == conv_detail.Rows[i].ItemArray[5].ToString())
                        {
                            sender_type = "buyer";
                        }
                        else
                        { sender_type = "seller"; }
                    }
                    if (sender_id == conv_detail.Rows[i].ItemArray[6].ToString() )
                    {
                        if (sender_2 == conv_detail.Rows[i].ItemArray[6].ToString())
                        {
                            sender_type = "buyer";
                        }
                        else
                        { sender_type = "seller"; }
                    }

                    dgv_category.Rows.Add(sender_name, conv_detail.Rows[i].ItemArray[1].ToString(), sender_type , conv_detail.Rows[i].ItemArray[2].ToString(), conv_detail.Rows[i].ItemArray[3].ToString(),
                                          conv_detail.Rows[i].ItemArray[4].ToString()
                                          );
                        }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } //end of function

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public string getSenderType(string value)
        {
            string type = "";
            string[] rows = value.Split('\n');
            for(int i=0;i<rows.Length;i++)
            {
                if(rows[i].Contains("senderType"))
                {
                    string[] values = rows[i].Split('=');
                    type = values[1].ToString();
                    break;
                }
            }
            return type;
        }

        private void lblName_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgv_category_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}
