using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Meteroi;
using System.Threading;
using System.Threading.Tasks;
using Basic;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

//image joint
using Accord.Imaging;
using Accord.Imaging.Filters;
using Accord.Math;
using AForge;

//
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.ML;
using Emgu.CV.Util;
using Emgu.CV.GPU;
using Emgu.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.CV.Stitching;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using OpenTK.Graphics;

namespace Protein_Crystallization
{
    public partial class Detector : Form
    {
        // This delegate enables asynchronous calls for setting
        // the text property on a TextBox control.
        delegate void SetTextCallback(string text);
        BasicForm picture;
        private List<int> x = new List<int>();
        private List<int> y = new List<int>();


        public Detector()
        {
            InitializeComponent();
            picture = new BasicForm();
            picture.parent_window = this;
            picture.Visible = false;
            updateGrid(26);
            update_delta();

            //img1 = (Bitmap)Image.FromFile( Application.StartupPath + @"/photo/" + listBox1.Items[0].ToString());
            //img2 = (Bitmap)Image.FromFile(Application.StartupPath + @"/photo/" + listBox1.Items[1].ToString());
            //Concatenate concatenate = new Concatenate(img1);
            //pictureBox3.Image = concatenate.Apply(img2);
        }

        private void updateGrid(int grid_size)
        {
            if (dataGridView1.Rows.Count == (grid_size + 1))
                return;
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(grid_size);
            for (int i = 0; i < grid_size; i++)
                this.dataGridView1[0, i].Value = Convert.ToString(i + 1);
            for (int i = 0; i < grid_size; i++)
                this.dataGridView1[1, i].Value = "";
            for (int i = 0; i < grid_size; i++)
                this.dataGridView1[2, i].Value = "";
            for (int i = 0; i < grid_size; i++)
                this.dataGridView1[3, i].Value = "";
            for (int i = 0; i < grid_size; i++)
                this.dataGridView1[4, i].Value = "";
            for (int i = 0; i < grid_size; i++)
                this.dataGridView1[6, i].Value = "";
            for (int i = 0; i < grid_size; i++)
            {
                this.dataGridView1.Rows[i].Cells[5].Value = "注射";
            }
        }

        private int string_to_int(int default_value, string str, bool errror_message)
        {
            int i;
            try
            {
                i = int.Parse(str);
            }
            catch (SystemException)
            {
                if (errror_message == true)
                    MessageBox.Show("请输入正确的数值");
                i = default_value;
            }
            return i;
        }
        private int string_to_int(int default_value, string str)
        { return string_to_int(default_value, str, true); }

        private uint string_to_uint(uint default_value, string str, bool errror_message)
        {
            uint i;
            try
            {
                i = uint.Parse(str);
            }
            catch (SystemException)
            {
                if (errror_message == true)
                    MessageBox.Show("请输入正确的数值");
                i = default_value;
            }
            return i;
        }
        private uint string_to_uint(uint default_value, string str)
        { return string_to_uint(default_value, str, true); }

        private float string_to_float(float default_value, string str, bool errror_message)
        {
            float i;
            try
            {
                i = float.Parse(str);
            }
            catch (SystemException)
            {
                if (errror_message == true)
                    MessageBox.Show("请输入正确的数值");
                i = default_value;
            }
            return i;
        }
        private float string_to_float(float default_value, string str)
        { return string_to_float(default_value, str, true); }

        //sensor_x,sensor_y, sensor_z,在校准→激光位置校准
        private void update_delta()
        {
            delta_x = (int)(string_to_float(3, sensor_x.Text, false) * -100);
            delta_y = (int)(string_to_float(3, sensor_y.Text, false) * 100);
            delta_z = (int)(string_to_float(3, sensor_z.Text, false) * -333);
        }

        private void update_delta_s()
        {
            delta_x_s = (int)(string_to_float(3, syringe_x.Text, false) * -100);
            delta_y_s = (int)(string_to_float(3, syringe_y.Text, false) * 100);
            delta_z_s = (int)(string_to_float(3, syringe_z.Text, false) * -333);
        }

        private void updatebar()
        {
            float t1 = PCAS.get_box_temperature();
            float t2 = PCAS.get_chip_temperature();
            float m1 = PCAS.get_box_moisture();
            float m2 = PCAS.get_chip_moisture();
            if (!float.IsNaN(t1))
                temperature1.Text = t1.ToString("0.00");
            if (!float.IsNaN(t2))
                temperature0.Text = t2.ToString("0.00");
            if (!float.IsNaN(m1) && m1 > 0 && m1 < 100)
                moisture1.Text = m1.ToString("0.00");
            if (!float.IsNaN(m2) && m2 > 0 && m2 < 100)
                moisture0.Text = m2.ToString("0.00");
        }

        public class setting
        {
            [XmlElementAttribute("Target_temperature")]
            public float temperature;
            [XmlElementAttribute("Target_moisture")]
            public float moisture;

            [XmlElementAttribute("Samples")]
            public uint samples;
            [XmlElementAttribute("Sample_id")]
            public uint sampleid;
            [XmlElementAttribute("Sample_radius")]
            public float s_radius;
            [XmlElementAttribute("Sample_angle_offset")]
            public float s_angle;

            [XmlElementAttribute("Hole_id")]
            public uint holeid;
            [XmlElementAttribute("Hole_radius")]
            public float h_radius;
            [XmlElementAttribute("Hole_angle_offset")]
            public float h_angle;
            [XmlElementAttribute("Hole_distance_offset")]
            public float h_off;

            [XmlElementAttribute("LED_light")]
            public uint LED;
            [XmlElementAttribute("Liquid_uL")]
            public uint uL;

            [XmlElementAttribute("Testing_time")]
            public int test_time;
            [XmlElementAttribute("Testing_time_unit")]
            public int test_time_unit;

            [XmlElementAttribute("Interval_time")]
            public int time;
            [XmlElementAttribute("Interval_time_unit")]
            public int time_unit;

            [XmlElementAttribute("grid_size")]
            public int grid_size;

            [XmlElementAttribute("grid_name")]
            public List<string> grid_name;

            [XmlElementAttribute("grid_resualt")]
            public List<string> grid_resualt;

            [XmlElementAttribute("grid_ diameter")]
            public List<string> grid_diameter;

            [XmlElementAttribute("grid_uL")]
            public List<string> grid_uL;

            [XmlElementAttribute("grid_other")]
            public List<string> grid_other;

            [XmlElementAttribute("laser_x")]
            public float laser_delta_x;

            [XmlElementAttribute("laser_y")]
            public float laser_delta_y;

            [XmlElementAttribute("laser_z")]
            public float laser_delta_z;

            [XmlElementAttribute("syringe_x")]
            public float syringe_delta_x;

            [XmlElementAttribute("syringe_y")]
            public float syringe_delta_y;

            [XmlElementAttribute("syringe_z")]
            public float syringe_delta_z;

            public setting()
            {
                grid_name = new List<string>();
                grid_resualt = new List<string>();
                grid_diameter = new List<string>();
                grid_uL = new List<string>();
                grid_other = new List<string>();
            }
        }
        private void LoadSetting_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//Load setting file
            {
                int i;
                setting save = new setting();
                XmlSerializer serializer = new XmlSerializer(save.GetType());
                TextReader read = new StreamReader(openFileDialog1.FileName);
                save = (setting)serializer.Deserialize(read);
                read.Close();
                targettemp.Text = save.temperature.ToString();
                targetmoist.Text = save.moisture.ToString();

                sample_radius.Text = save.s_radius.ToString();
                Sample.Text = save.samples.ToString();
                sampleid.Text = save.sampleid.ToString();
                angle.Text = save.s_angle.ToString();

                radius.Text = save.h_radius.ToString();
                holeid.Text = save.holeid.ToString();
                //hole_off.Text      = save.h_off.ToString();
                holeangle.Text = save.h_angle.ToString();

                LED_light.Value = new decimal(save.LED);
                uL.Text = save.uL.ToString();

                textBox7.Text = save.test_time.ToString();
                comboBox1.SelectedIndex = save.test_time_unit;
                textBox1.Text = save.time.ToString();
                comboBox2.SelectedIndex = save.time_unit;

                sensor_x.Text = save.laser_delta_x.ToString();
                sensor_y.Text = save.laser_delta_y.ToString();
                sensor_z.Text = save.laser_delta_z.ToString();

                syringe_x.Text = save.syringe_delta_x.ToString();
                syringe_y.Text = save.syringe_delta_y.ToString();
                syringe_z.Text = save.syringe_delta_z.ToString();

                update_delta();
                update_delta_s();

                i = 0;
                foreach (string s in save.grid_name)
                {
                    dataGridView1[1, i].Value = s;
                    i++;
                }
                i = 0;
                foreach (string s in save.grid_resualt)
                {
                    dataGridView1[2, i].Value = s;
                    i++;
                }
                i = 0;
                foreach (string s in save.grid_diameter)
                {
                    dataGridView1[3, i].Value = s;
                    i++;
                }
                i = 0;
                foreach (string s in save.grid_uL)
                {
                    dataGridView1[4, i].Value = s;
                    i++;
                }
                i = 0;
                foreach (string s in save.grid_other)
                {
                    dataGridView1[6, i].Value = s;
                    i++;
                }
            }
        }
        private void SaveSetting_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)//Save setting file
            {
                setting save = new setting();
                save.temperature = float.Parse(targettemp.Text);
                save.moisture = float.Parse(targetmoist.Text);

                save.s_radius = float.Parse(sample_radius.Text);
                save.samples = uint.Parse(Sample.Text);
                save.sampleid = uint.Parse(sampleid.Text);
                save.s_angle = float.Parse(angle.Text);

                save.h_radius = float.Parse(radius.Text);
                save.holeid = uint.Parse(holeid.Text);
                //save.h_off    = float.Parse(hole_off.Text);
                save.h_angle = float.Parse(holeangle.Text);

                save.LED = decimal.ToUInt32(LED_light.Value);
                save.uL = uint.Parse(uL.Text);

                save.test_time = int.Parse(textBox7.Text);
                save.test_time_unit = comboBox1.SelectedIndex;
                save.time = int.Parse(textBox1.Text);
                save.time_unit = comboBox2.SelectedIndex;

                save.grid_size = dataGridView1.RowCount - 1;
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_name.Add(dataGridView1[1, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_resualt.Add(dataGridView1[2, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_diameter.Add(dataGridView1[3, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_uL.Add(dataGridView1[4, i].Value.ToString());
                }
                for (int i = 0; i < save.grid_size; i++)
                {
                    save.grid_other.Add(dataGridView1[6, i].Value.ToString());
                }

                save.laser_delta_x = float.Parse(sensor_x.Text);
                save.laser_delta_y = float.Parse(sensor_y.Text);
                save.laser_delta_z = float.Parse(sensor_z.Text);

                save.syringe_delta_x = float.Parse(syringe_x.Text);
                save.syringe_delta_y = float.Parse(syringe_y.Text);
                save.syringe_delta_z = float.Parse(syringe_z.Text);

                XmlSerializer serializer = new XmlSerializer(save.GetType());
                TextWriter writer = new StreamWriter(saveFileDialog1.FileName);
                serializer.Serialize(writer, save);
                writer.Close();
            }
        }

        private void LoadCoodinate_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)//Load the coordinate file
            {
                StreamReader file = new StreamReader(openFileDialog2.FileName);
                while (!file.EndOfStream)
                {
                    string strReadLine = file.ReadLine(); //读取每行数据
                    string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
                    MatchCollection mc = Regex.Matches(strReadLine, regexStr);
                    if (mc.Count < 2)
                        return;
                    x.Add(int.Parse(mc[0].Value));
                    y.Add(int.Parse(mc[1].Value));
                }
            }
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            string username = UserName.Text;
            string password = PassWords.Text;
            string ipaddress = IPAddress0.Text + '.' + IPAddress1.Text + '.' + IPAddress2.Text + '.' + IPAddress3.Text;
            if (PCAS.connect(ipaddress, username, password))
            {
                float t1 = PCAS.get_box_temperature();
                float t2 = PCAS.get_chip_temperature();
                float m1 = PCAS.get_box_moisture();
                float m2 = PCAS.get_chip_moisture();
                welcome.Visible = false;// Set the welcome page as invisible
                timer1.Enabled = true;
                updatebar();
            }
            else
            {
                MessageBox.Show("连接失败");
            }
        }

        //设置→检测
        private void button5_Click(object sender, EventArgs e)
        {
            int i = string_to_int(0, textBox2.Text);
            if (i == 0)
            {
                PCAS.microscopexy(0, 0);
                return;
            }
            i = i - 1;
            if (i > x.Count)
                return;
            PCAS.microscopexy(x[i], y[i]);
        }
        //校准→对准标记位置校准→左下
        private void AlignmentMark0_Click(object sender, EventArgs e)
        {
            checkBox0.Checked = true;
            PCAS.set_ref(0);
        }
        //左上
        private void AlignmentMark1_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            PCAS.set_ref(1);
        }
        //右上
        private void AlignmentMark2_Click(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            PCAS.set_ref(2);
        }
        //右下
        private void AlignmentMark3_Click(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
            PCAS.set_ref(3);
        }
        private void original_Click(object sender, EventArgs e)
        {
            checkBox0.Checked = false;
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
            PCAS.set_ref(4);
        }
        private void RemoteClose_Click(object sender, EventArgs e)
        {
            string username = UserName.Text;
            string password = PassWords.Text;
            string ipaddress = IPAddress0.Text + '.' + IPAddress1.Text + '.' + IPAddress2.Text + '.' + IPAddress3.Text;
            DialogResult r;
            r = MessageBox.Show("确认关机", "远程关机", MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (r == DialogResult.Cancel)
                return;
            if (PCAS.disconnect(ipaddress, username, password) == true)
            {
                welcome.Visible = true;// Set the welcome page as invisible
                timer1.Enabled = false;
                picture.Close();
            }
            else
            {
                MessageBox.Show("关机失败");
            }
        }
        private void xp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(1);
        }
        private void xpp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(100);
        }
        private void ymm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(100);
        }
        private void ym_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(1);
        }
        private void xm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(-1);
        }
        private void xmm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(-100);
        }
        private void yp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(-1);
        }
        private void ypp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_y(-100);
        }
        private void zmm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(-100);
        }
        private void zm_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(-10);
        }
        private void zp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(10);
        }
        private void zpp_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_z(100);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            updatebar();
            logtext.Text = PCAS.get_log();
            logtext.Select(logtext.TextLength, 0);//光标定位到文本最后
            logtext.ScrollToCaret();//滚动到光标处
        }

        private void LED_light_ValueChanged(object sender, EventArgs e)
        {
            uint i = decimal.ToUInt32(LED_light.Value);
            if (i > 100)
            {
                MessageBox.Show("请输入正确的亮度比列");
                return;
            }
            PCAS.set_led(i);
        }
        private void targettemp_TextChanged(object sender, EventArgs e)
        {
            //if (targettemp.Text.Length != 0)
            //{
            //    float i = string_to_float(25, targettemp.Text);
            //    targettemp.Text = i.ToString();
            //}
        }
        private void targetmoist_TextChanged(object sender, EventArgs e)
        {
            //if (targetmoist.Text.Length != 0)
            //{
            //    float i = string_to_float(80, targetmoist.Text);
            //    targetmoist.Text = i.ToString();
            //}
        }
        private void radius_TextChanged(object sender, EventArgs e)
        {
            //if (radius.Text.Length != 0)
            //{
            //    float i = string_to_float(22.02f, radius.Text);
            //    radius.Text = i.ToString();
            //}
        }
        private void Sample_TextChanged(object sender, EventArgs e)
        {
            //if (Sample.Text.Length != 0)
            //{
            //    uint i = string_to_uint(25, Sample.Text);
            //    Sample.Text = i.ToString();
            //    updateGrid((int)i);
            //}
        }

        private void sampleid_TextChanged(object sender, EventArgs e)
        {
            //if (sampleid.Text.Length != 0)
            //{
            //    uint i = string_to_uint(0, sampleid.Text);
            //    sampleid.Text = i.ToString();
            //}
        }

        private void angle_TextChanged(object sender, EventArgs e)
        {
            //if (angle.Text.Length != 0)
            //{
            //    float i = string_to_float(0, angle.Text);
            //    angle.Text = i.ToString();
            //}
        }

        private void sample_radius_TextChanged(object sender, EventArgs e)
        {
            //if (sample_radius.Text.Length != 0)
            //{
            //    float i = string_to_float(14.05f, sample_radius.Text);
            //    sample_radius.Text = i.ToString();
            //}
        }

        private void holeid_TextChanged(object sender, EventArgs e)
        {
            //if (holeid.Text.Length != 0)
            //{
            //    uint i = string_to_uint(0, holeid.Text);
            //    holeid.Text = i.ToString();
            //}
        }

        private void holeangle_TextChanged(object sender, EventArgs e)
        {
            //if (holeangle.Text.Length != 0)
            //{
            //    float i = string_to_float(0, holeangle.Text);
            //    holeangle.Text = i.ToString();
            //}
        }

        private void hole_off_TextChanged(object sender, EventArgs e)
        {
            //if (hole_off.Text.Length != 0)
            //{
            //    float i = string_to_float(25.00f, hole_off.Text);
            //    hole_off.Text = i.ToString();
            //}
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            //if (textBox7.Text.Length != 0)
            //{
            //    uint i = string_to_uint(5, textBox7.Text);
            //    textBox7.Text = i.ToString();
            //}
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //if (textBox1.Text.Length != 0)
            //{
            //    uint i = string_to_uint(5, textBox1.Text);
            //    textBox1.Text = i.ToString();
            //}
        }

        private void uL_TextChanged(object sender, EventArgs e)
        {
            //if (uL.Text.Length != 0)
            //{
            //    uint i = string_to_uint(1, uL.Text);
            //    uL.Text = i.ToString();
            //}
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //if (textBox2.Text.Length != 0)
            //{
            //    uint i;
            //    i = string_to_uint(0, textBox2.Text);
            //    textBox2.Text = i.ToString();
            //}
        }

        private void syf_Click(object sender, EventArgs e)
        {
            PCAS.syringe_plus(8);

        }
        private void syb_Click(object sender, EventArgs e)
        {
            PCAS.syringe_minus(8);
        }
        private void syff_Click(object sender, EventArgs e)
        {
            PCAS.syringe_plus(200);
        }
        private void sybb_Click(object sender, EventArgs e)
        {
            PCAS.syringe_minus(200);
        }
        private void set_Click(object sender, EventArgs e)
        {
            float target_temp = string_to_float(20, targettemp.Text, false);
            float target_moist = string_to_float(50, targetmoist.Text, false);
            if (target_temp > 30 || target_temp < 0)
            {
                MessageBox.Show("请输入正确的温度");
                return;
            }
            if (target_moist > 100 || target_moist < 0)
            {
                MessageBox.Show("请输入正确的湿度");
                return;
            }
            PCAS.set_target_temperature(target_temp);
            PCAS.set_target_moisture(target_moist);
            temptime.Stop();
        }

        private void exam_Click(object sender, EventArgs e)
        {
            uint i = string_to_uint(25, sampleid.Text, false);
            uint sample = string_to_uint(0, Sample.Text, false);
            float d = string_to_float(16f, sample_radius.Text, false);
            float a = string_to_float(0, angle.Text, false);
            updateGrid((int)sample);
            if (i > sample)
            {
                MessageBox.Show("请输入正确的样本编号");
                return;
            }
            if (a > 360 || a < -360)
            {
                MessageBox.Show("请输入正确的角偏移");
                return;
            }
            if (d < 0 || d > 30)
            {
                MessageBox.Show("请输入正确的半径");
                return;
            }
            PCAS.set_radius(d);
            PCAS.set_angle(a);
            PCAS.set_sample(sample);
            PCAS.move_to_sample(i);
        }

        private void addsample_Click(object sender, EventArgs e)
        {
            uint i = string_to_uint(0, holeid.Text, false);
            uint sample = string_to_uint(0, Sample.Text, false);
            float d = string_to_float(22.02f, radius.Text, false);
            float a = string_to_float(0, holeangle.Text, false);
            float u = string_to_float(1, uL.Text, false);
            if (i > sample)
            {
                MessageBox.Show("请输入正确的样本编号");
                return;
            }
            if (a > 360 || a < -360)
            {
                MessageBox.Show("请输入正确的角偏移");
                return;
            }
            if (d < 0 || d > 30)
            {
                MessageBox.Show("请输入正确的半径");
                return;
            }
            PCAS.set_hole_delta(delta_x_s, delta_y_s, delta_z_s);
            PCAS.set_hole_radius(d);
            PCAS.set_hole_angle(a);
            PCAS.set_hole_sample(sample);
            PCAS.set_hole_uL(u);
            PCAS.move_to_hole(i);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PCAS.pannel_in();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PCAS.pannel_out();
        }

        private uint sample = 0;
        private float d = 0;
        private float a = 0;
        private int time = 0;
        private bool exam_start = false;
        private void set_sampleid(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.sampleid.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(set_sampleid);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.sampleid.Text = text;
            }
        }
        private void set_exam(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.button3.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(set_exam);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.button3.Text = text;
                this.savedir.Visible = true;
            }
        }
        String reportfile_name = null;
        bool cycle = true;
        int delta_x = 0;
        int delta_y = 0;
        int delta_z = 0;
        int laser_test_time = 0;
        int delta_x_s = 0;
        int delta_y_s = 0;
        int delta_z_s = 0;
        float report_d = 0;
        float threshold_value = 0;
        private void sample_exam_thread()
        {

            string strReadLine = null;
            string regexStr = @"[-+]?\b(?:[0-9]*\.)*[0-9]+\b";
            MatchCollection mc;
            uint i = 1;
            PCAS.set_radius(d);
            PCAS.set_angle(a);
            PCAS.set_sample(sample);

            while (i <= sample)
            {
                if (cycle == true)
                    PCAS.move_to_sample(i);
                else
                {
                    if (i > x.Count - 1)
                        break;
                    PCAS.microscopexy(x[(int)i], y[(int)i]);
                }
                this.set_sampleid(i.ToString());
                Thread.Sleep(300);
                picture.Record_the_picture();
                Thread.Sleep(300);
                if (reportfile_name != null)
                {
                    StreamReader reportfile = null;
                    PCAS.micoscope_x(delta_x);
                    PCAS.micoscope_y(delta_y);
                    PCAS.micoscope_z(delta_z);
                    Thread.Sleep(laser_test_time);
                    reportfile = new StreamReader(reportfile_name);
                    while (!reportfile.EndOfStream)
                        strReadLine = reportfile.ReadLine(); //读取每行数据
                    if (strReadLine != null)
                    {
                        mc = Regex.Matches(strReadLine, regexStr);
                        if (mc.Count >= 8)
                        {
                            dataGridView1[3, (int)i - 1].Value = mc[6].Value;
                            report_d = string_to_float(0, mc[6].Value);
                            for (int k = 0; k < sample; k++)
                            {
                                dataGridView1[3, k].OwningRow.Selected = false;
                            }
                            dataGridView1[3, (int)i - 1].OwningRow.Selected = true;
                        }
                    }
                    reportfile.Close();
                    PCAS.micoscope_z(-delta_z);
                    PCAS.micoscope_x(-delta_x);
                    PCAS.micoscope_y(-delta_y);
                    if (exam_start == false)
                    {
                        return;
                    }
                }
                if (auto_add_sample == true)
                {
                    if (report_d > threshold_value)
                    {
                        if (i != 13 && i != 19 && i != 20 && i != 21 && i != 26)
                            PCAS.move_to_hole(i);
                    }
                }
                if (exam_start == false)
                {
                    return;
                }
                Thread.Sleep(time);
                i++;
            }
            exam_start = false;
            set_exam("检测");
        }

        //设置→4→检测
        private void button3_Click(object sender, EventArgs e)
        {
            Thread exam = new Thread(sample_exam_thread);
            cycle = comboBox3.SelectedIndex == 0;
            sample = string_to_uint(0, Sample.Text, false);
            d = string_to_float(14.05f, sample_radius.Text, false);
            a = string_to_float(0, angle.Text, false);
            threshold_value = string_to_float(0, threshold.Text, false);
            float t = string_to_float(0, laser_time.Text, false);
            laser_test_time = (int)(t * 1000 * 60);
            if (comboBox1.SelectedIndex == 0)
                time = (int)string_to_uint(5, textBox7.Text, false) * 1000;
            else
                time = (int)string_to_uint(5, textBox7.Text, false) * 1000 * 60;
            if (a > 360 || a < -360)
            {
                MessageBox.Show("请输入正确的角偏移");
                return;
            }
            if (d < 0 || d > 30)
            {
                MessageBox.Show("请输入正确的半径");
                return;
            }
            if (exam_start == false)
            {
                exam_start = true;
                button3.Text = "停止";
                savedir.Visible = false;
                exam.Start();
            }
            else
            {
                exam_start = false;
                savedir.Visible = true;
                button3.Text = "检测";
            }
        }

        Thread xp_t = null;
        Thread xpp_t = null;
        Thread yp_t = null;
        Thread ypp_t = null;
        Thread zp_t = null;
        Thread zpp_t = null;
        Thread xm_t = null;
        Thread xmm_t = null;
        Thread ym_t = null;
        Thread ymm_t = null;
        Thread zm_t = null;
        Thread zmm_t = null;

        bool xp_stop = false;
        bool xpp_stop = false;
        bool yp_stop = false;
        bool ypp_stop = false;
        bool zp_stop = false;
        bool zpp_stop = false;
        bool xm_stop = false;
        bool xmm_stop = false;
        bool ym_stop = false;
        bool ymm_stop = false;
        bool zm_stop = false;
        bool zmm_stop = false;

        private void xp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_x(1);
                i++;
                Thread.Sleep(100);
                if (xp_stop == true)
                    return;
            }
        }

        private void xpp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_x(100);
                i++;
                Thread.Sleep(100);
                if (xpp_stop == true)
                    return;
            }
        }

        private void yp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(1);
                i++;
                Thread.Sleep(100);
                if (yp_stop == true)
                    return;
            }
        }

        private void ypp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(100);
                i++;
                Thread.Sleep(100);
                if (ypp_stop == true)
                    return;
            }
        }

        private void zp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(1);
                i++;
                Thread.Sleep(100);
                if (zp_stop == true)
                    return;
            }
        }

        private void zpp_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(100);
                i++;
                Thread.Sleep(100);
                if (zpp_stop == true)
                    return;
            }
        }

        private void xm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_x(-1);
                i++;
                Thread.Sleep(100);
                if (xm_stop == true)
                    return;
            }
        }

        private void xmm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_x(-100);
                i++;
                Thread.Sleep(100);
                if (xmm_stop == true)
                    return;
            }
        }

        private void ym_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(-1);
                i++;
                Thread.Sleep(100);
                if (ym_stop == true)
                    return;
            }
        }

        private void ymm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_y(-100);
                i++;
                Thread.Sleep(100);
                if (ymm_stop == true)
                    return;
            }
        }

        private void zm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(-1);
                i++;
                Thread.Sleep(100);
                if (zm_stop == true)
                    return;
            }
        }

        private void zmm_thread()
        {
            int i = 0;
            while (true)
            {
                PCAS.micoscope_z(-100);
                i++;
                Thread.Sleep(100);
                if (zmm_stop == true)
                    return;
            }
        }

        private void xp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xp_t = new Thread(xp_thread);
                xp_stop = false;
                xp_t.Start();
            }
        }

        private void xpp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xpp_t = new Thread(xpp_thread);
                xpp_stop = false;
                xpp_t.Start();
            }
        }

        private void ym_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ym_t = new Thread(ym_thread);
                ym_stop = false;
                ym_t.Start();
            }
        }

        private void ymm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ymm_t = new Thread(ymm_thread);
                ymm_stop = false;
                ymm_t.Start();
            }
        }

        private void xmm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xmm_t = new Thread(xmm_thread);
                xmm_stop = false;
                xmm_t.Start();
            }
        }

        private void xm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xm_t = new Thread(xm_thread);
                xm_stop = false;
                xm_t.Start();
            }
        }

        private void yp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                yp_t = new Thread(yp_thread);
                yp_stop = false;
                yp_t.Start();
            }
        }

        private void ypp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ypp_t = new Thread(ypp_thread);
                ypp_stop = false;
                ypp_t.Start();
            }
        }

        private void zm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zm_t = new Thread(zm_thread);
                zm_stop = false;
                zm_t.Start();
            }
        }

        private void zmm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zmm_t = new Thread(zmm_thread);
                zmm_stop = false;
                zmm_t.Start();
            }
        }

        private void zp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zp_t = new Thread(zp_thread);
                zp_stop = false;
                zp_t.Start();
            }
        }

        private void zpp_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zpp_t = new Thread(zpp_thread);
                zpp_stop = false;
                zpp_t.Start();
            }
        }

        private void xp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xp_stop = true;
            }
        }

        private void xmm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xmm_stop = true;
            }
        }

        private void ymm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ymm_stop = true;
            }
        }

        private void ypp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ypp_stop = true;
            }
        }

        private void yp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                yp_stop = true;
            }
        }

        private void ym_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ym_stop = true;
            }
        }

        private void xpp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xpp_stop = true;
            }
        }

        private void xm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xm_stop = true;
            }
        }

        private void zpp_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zpp_stop = true;
            }
        }

        private void zmm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zmm_stop = true;
            }
        }

        private void zm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                zm_stop = true;
            }
        }

        public void set_show_picture()
        {
            button4.Text = "显示图像";
        }

        public void set_hide_picture()
        {
            button4.Text = "隐藏图像";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (picture.IsDisposed)
            {
                picture = new BasicForm();
                picture.parent_window = this;
                picture.Visible = false;
            }
            picture.Left = this.Left + this.Size.Width;
            picture.Top = this.Top;
            ;
            if (picture.Visible == false)
            {
                picture.Visible = true;
                set_hide_picture();
            }
            else
            {
                picture.Visible = false;
                set_show_picture();
            }
        }

        const int STEP_PER_UL = 8;
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            uint uL;
            uint sampleid;
            if (e.RowIndex < 0 || e.ColumnIndex < 1)
                return;
            if (this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value != null)
            {
                uL = string_to_uint(1, this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString(), false);
                sampleid = (uint)e.RowIndex + 1;
                PCAS.set_hole_uL(uL * STEP_PER_UL);
                PCAS.move_to_hole(sampleid);
            }
        }

        private void Detector_FormClosing(object sender, FormClosingEventArgs e)
        {
            picture.Close();
        }

        private void savedir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                picture.save_path = folderBrowserDialog1.SelectedPath;
            }
        }
        private bool auto_test = false;
        private void time_test_Click(object sender, EventArgs e)
        {
            if (auto_test == false)
            {
                uint time;
                if (comboBox2.SelectedIndex == 0)
                    time = string_to_uint(60, textBox1.Text, true) * 1000 * 60;
                else
                    time = string_to_uint(60, textBox1.Text, true) * 1000 * 60 * 60;
                uint sample = string_to_uint(0, Sample.Text, true);
                uint timeinterval = string_to_uint(5, textBox7.Text, true) * 1000;
                if (time < sample * timeinterval)
                {
                    MessageBox.Show("时间间隔太短");
                }
                autotesttime.Interval = (int)time;
                autotesttime.Start();
                auto_test = true;
                time_test.Text = "终止";
                if (exam_start == false)
                    button3.PerformClick();
            }
            else
            {
                auto_test = false;
                autotesttime.Stop();
                time_test.Text = "定时检查";
                if (exam_start == true)
                    button3.PerformClick();
            }
        }

        private void autotesttime_Tick(object sender, EventArgs e)
        {
            if (exam_start == false)
                button3.PerformClick();
        }

        bool cycle_start = false;
        bool cycle_pause = false;
        int r = 0;
        private void cycle_thread()
        {
            int angle = 0;
            PCAS.micoscope_x(-r);
            while (angle < 360)
            {
                if (cycle_start == false)
                    return;
                if (cycle_pause == true)
                {
                    Thread.Sleep(1);
                    continue;
                }
                PCAS.micoscope_x((int)(r - r * Math.Cos(Math.PI / 180 * angle)));
                PCAS.micoscope_y((int)(r * Math.Sin(Math.PI / 180 * angle)));
                angle++;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            PCAS.set_view_z();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            PCAS.set_hole_z();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Txt files (*.txt)|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)//Load setting file
            {
                reportfile_name = openFileDialog1.FileName;
            }
        }

        private void sensor_x_TextChanged(object sender, EventArgs e)
        {
            if (sensor_x.Text.Length != 0)
            {
                //float i = string_to_float(3, sensor_x.Text);
                //sensor_x.Text = i.ToString();
                update_delta();
            }
        }

        private void sensor_y_TextChanged(object sender, EventArgs e)
        {
            if (sensor_y.Text.Length != 0)
            {
                //float i = string_to_float(3, sensor_y.Text);
                //sensor_y.Text = i.ToString();
                update_delta();
            }
        }

        private void sensor_z_TextChanged(object sender, EventArgs e)
        {
            if (sensor_z.Text.Length != 0)
            {
                //float i = string_to_float(3, sensor_z.Text);
                //sensor_z.Text = i.ToString();
                update_delta();
            }
        }

        bool laser_test = true;
        private void button7_Click(object sender, EventArgs e)
        {
            update_delta();
            if (laser_test == true)
            {
                PCAS.micoscope_x(delta_x);
                PCAS.micoscope_y(delta_y);
                PCAS.micoscope_z(delta_z);
                laser_test = false;
                button7.Text = "复原";
            }
            else
            {
                PCAS.micoscope_z(-delta_z);
                PCAS.micoscope_x(-delta_x);
                PCAS.micoscope_y(-delta_y);
                laser_test = true;
                button7.Text = "校准";
            }
        }

        StreamReader tempfile = null;
        //设置→温度曲线
        private void button8_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "txt files (*.txt)|*.txt";
            if (openFileDialog3.ShowDialog() == DialogResult.OK)//Load setting file
            {
                tempfile = new StreamReader(openFileDialog3.FileName);
                temptime.Interval = 1000 * 10 * 60;
                temptime.Start();

                float temp;
                string strReadLine = tempfile.ReadLine(); //读取每行数据
                string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
                MatchCollection mc = Regex.Matches(strReadLine, regexStr);
                if (mc.Count < 1)
                    return;
                temp = float.Parse(mc[0].Value);
                PCAS.set_target_temperature(temp);
                targettemp.Text = temp.ToString();
            }
        }

        private void temptime_Tick(object sender, EventArgs e)
        {
            if (tempfile == null)
                return;
            if (!tempfile.EndOfStream)
            {
                float temp;
                string strReadLine = tempfile.ReadLine(); //读取每行数据
                string regexStr = @"[-+]?\b(?:[0-9]*\.)?[0-9]+\b";
                MatchCollection mc = Regex.Matches(strReadLine, regexStr);
                if (mc.Count < 1)
                    return;
                temp = float.Parse(mc[0].Value);
                PCAS.set_target_temperature(temp);
                targettemp.Text = temp.ToString();
            }
            else
            {
                temptime.Stop();
            }
        }

        bool syringe_test = true;
        private void button9_Click(object sender, EventArgs e)
        {
            update_delta_s();
            if (syringe_test == true)
            {
                PCAS.micoscope_x(delta_x_s);
                PCAS.micoscope_y(delta_y_s);
                PCAS.micoscope_z(delta_z_s);
                syringe_test = false;
                button9.Text = "复原";
            }
            else
            {
                PCAS.micoscope_z(-delta_z_s);
                PCAS.micoscope_x(-delta_x_s);
                PCAS.micoscope_y(-delta_y_s);
                syringe_test = true;
                button9.Text = "校准";
            }
        }

        private void syringe_x_TextChanged(object sender, EventArgs e)
        {
            if (syringe_x.Text.Length != 0)
            {
                update_delta_s();
            }
        }

        private void syringe_z_TextChanged(object sender, EventArgs e)
        {
            if (syringe_z.Text.Length != 0)
            {
                update_delta_s();
            }
        }

        private void syringe_y_TextChanged(object sender, EventArgs e)
        {
            if (syringe_y.Text.Length != 0)
            {
                update_delta_s();
            }
        }

        bool auto_add_sample = false;
        private void button10_Click(object sender, EventArgs e)
        {
            uint i = string_to_uint(0, holeid.Text, false);
            uint sample = string_to_uint(0, Sample.Text, false);
            float d = string_to_float(22.02f, radius.Text, false);
            float a = string_to_float(0, holeangle.Text, false);
            float u = string_to_float(1, uL.Text, false);
            if (i > sample)
            {
                MessageBox.Show("请输入正确的样本编号");
                return;
            }
            if (a > 360 || a < -360)
            {
                MessageBox.Show("请输入正确的角偏移");
                return;
            }
            if (d < 0 || d > 30)
            {
                MessageBox.Show("请输入正确的半径");
                return;
            }
            PCAS.set_hole_delta(delta_x_s, delta_y_s, delta_z_s);
            PCAS.set_hole_radius(d);
            PCAS.set_hole_angle(a);
            PCAS.set_hole_sample(sample);
            PCAS.set_hole_uL(u);
            if (auto_add_sample == false)
            {
                auto_add_sample = true;
                button10.Text = "自动";
            }
            else
            {
                auto_add_sample = false;
                button10.Text = "手动";
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PCAS.micoscope_x(1000);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            PCAS.microscopexy(100, 100);
        }



        /////////////////////////////////////////////////////saveTest/////////////////////////
        private string path1 = null;//img1's path
        private string path2 = null;//img2's path
        private string SourcePhotoPath = Application.StartupPath + @"/SourcePhoto/";// + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg";
        private void Detector_FormLoad(object sender, EventArgs e)
        {
            setListBack = new SetListBack(SetList);
            img1 = null;
            img2 = null;
            DirAllJpg();
            listBox1.SelectedIndex = 0;

        }
        public delegate void SetListBack(string strPath);

        public SetListBack setListBack;


        private void SetList(string strPath)
        {
            this.listBox1.Items.Insert(0, strPath);
            //this.listBox1.Items.Add(strPath);
        }

        static int Partition(FileInfo[] arr, int low, int high)
        {
            //进行一趟快速排序,返回中心轴记录位置
            // arr[0] = arr[low];
            FileInfo pivot = arr[low];//把中心轴置于arr[0]
            while (low < high)
            {
               while (low < high && arr[high].CreationTime <= pivot.CreationTime)
               //while (low < high && ((string.Compare(arr[high].Name, pivot.Name, true)) > 0))
                    --high;
                //将比中心轴记录小的移到低端
                Swap(ref arr[high], ref arr[low]);
                while (low < high && arr[low].CreationTime >= pivot.CreationTime)
                    ++low;
                Swap(ref arr[high], ref arr[low]);
                //将比中心轴记录大的移到高端
            }
            arr[low] = pivot; //中心轴移到正确位置
            return low;  //返回中心轴位置
        }
        static void Swap(ref FileInfo i, ref FileInfo j)
        {
            FileInfo t;
            t = i;
            i = j;
            j = t;
        }

        public static void QuickSort(FileInfo[] arr, int low, int high)
        {
            if (low <= high - 1)//当 arr[low,high]为空或只一个记录无需排序
            {
                int pivot = Partition(arr, low, high);
                QuickSort(arr, low, pivot - 1);
                QuickSort(arr, pivot + 1, high);
            }
        }

        private void DirAllJpg()
        {
            this.listBox1.Items.Clear();
            DirectoryInfo theFolder = new DirectoryInfo(Application.StartupPath + @"\photo\");//初始化指定路径上的类


            FileInfo[] fileInfo = theFolder.GetFiles();//返回当前目录的文件列表
            QuickSort(fileInfo, 0, fileInfo.Length - 1);

            foreach (FileInfo NextFile in fileInfo)  //遍历文件
                this.listBox1.Items.Add(NextFile.Name);
        }


        private void LoadImg(string path)
        {
            if (File.Exists(path))
            {
                pictureBox3.Image = Image.FromFile(path);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            path1 = Application.StartupPath + @"/photo/" + listBox1.Items[listBox1.SelectedIndex].ToString();
            img1 = (Bitmap)Image.FromFile(path1);
           // img1 = (Bitmap)Image.FromFile(path1);
            if (listBox1.Items.Count == 1)
            {

              //  string path2 = Application.StartupPath + @"/photo/" + listBox1.Items[listBox1.SelectedIndex + 1].ToString();
                img1 = (Bitmap)Image.FromFile(path1);
                //img2 = null;
                //Concatenate concatenate = new Concatenate(img1);
                pictureBox3.Image = img1;
            }
            if (listBox1.Items.Count > 1)
            {
                if (listBox1.SelectedIndex == (listBox1.Items.Count - 1))
                {
                    path2 = Application.StartupPath + @"/photo/" + listBox1.Items[listBox1.SelectedIndex - 1 ].ToString();
                    img2 = (Bitmap)Image.FromFile(path2);
                    
                }
                else //if (listBox1.SelectedIndex < listBox1.Items.Count)
                {
                    path2 = Application.StartupPath + @"/photo/" + listBox1.Items[listBox1.SelectedIndex + 1 ].ToString();
                    img2 = (Bitmap)Image.FromFile(path2);
                }

                Concatenate concatenate = new Concatenate(img1);
                pictureBox3.Image = concatenate.Apply(img2);
            }
        }


        private void button13_Click(object sender, EventArgs e)
        {
            Bitmap image1 = null;
            string filename = null;

            picture.Record_the_picture(out image1, out filename);
            DirAllJpg();
            listBox1.SelectedIndex = 0;

        }


        private void reflash_Click(object sender, EventArgs e)
        {
            DirAllJpg();
        }

        private bool auto_flag = true;
        private void auto_Click(object sender, EventArgs e)
        {
            if ((textBox3.Text == null) || (textBox4.Text == null))
            {
                MessageBox.Show("请输入正确的芯片尺寸", "错误");
            }
            else
            {
                double chip_length = double.Parse(textBox3.Text);
                double chip_width = double.Parse(textBox4.Text);
                string messege = "芯片长度为：" + textBox3.Text + "，宽度为：" + textBox4.Text + "？ 芯片边缘移至左上角？";
                string caption = "确认";
                DialogResult result;

                result = MessageBox.Show(messege, caption, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //确定移动多少步
                    int lenght_step = (int)chip_length * 1000 / 280;
                    int width_step = (int) chip_width * 1000 /380;
                    button13.PerformClick();

                    for (int i = 0; i < (width_step + 1); i++)
                    {
                        for (int j = 0; j < (lenght_step + 1); j++)
                        {
                            //if (auto_flag)
                            //{
                                PCAS.micoscope_y(56);
                                button13.PerformClick();
                                btnDoItAll.PerformClick();
                                //auto_flag = false;
                            //}
                            //else
                            //{
                            //    break;
                            //}
                        }
                        //if (auto_flag)
                        //{
                            PCAS.micoscope_x(76);
                        //}
                        //else
                        //{
                        //    break;
                        //}
                        for (int j = 0; j < lenght_step; j++)
                        {
                            //if (auto_flag)
                            //{
                                PCAS.micoscope_y(-56);
                                button13.PerformClick();
                                btnDoItAll.PerformClick();
                            //}
                            //else
                            //{
                            //    break;
                            //}
                        }
                    }
                }

            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            auto_flag = false;
        }

        ////////////////////////////////////////////////////////imgage joint////////////////////////////

        private Bitmap img1 = null;
        private Bitmap img2 = null;



        private IntPoint[] harrisPoints1;
        private IntPoint[] harrisPoints2;

        private IntPoint[] correlationPoints1;
        private IntPoint[] correlationPoints2;

        private MatrixH homography;

        private void btnHarris_Click(object sender, EventArgs e)
        {
            // Step 1: Detect feature points using Harris Corners Detector
            HarrisCornersDetector harris = new HarrisCornersDetector(0.05f, float.Parse(textBox5.Text));
            harrisPoints1 = harris.ProcessImage(img1).ToArray();
            harrisPoints2 = harris.ProcessImage(img2).ToArray();

            // Show the marked points in the original images
            Bitmap img1mark = new PointsMarker(harrisPoints1).Apply(img1);
            Bitmap img2mark = new PointsMarker(harrisPoints2).Apply(img2);

            // Concatenate the two images together in a single image (just to show on screen)
            Concatenate concatenate = new Concatenate(img1mark);
            pictureBox3.Image = concatenate.Apply(img2mark);
        }


        private void btnCorrelation_Click(object sender, EventArgs e)
        {
            // Step 2: Match feature points using a correlation measure
            CorrelationMatching matcher = new CorrelationMatching(9);
            IntPoint[][] matches = matcher.Match(img1, img2, harrisPoints1, harrisPoints2);

            // Get the two sets of points
            correlationPoints1 = matches[0];
            correlationPoints2 = matches[1];

            // Concatenate the two images in a single image (just to show on screen)
            Concatenate concat = new Concatenate(img1);
            Bitmap img3 = concat.Apply(img2);

            // Show the marked correlations in the concatenated image
            PairsMarker pairs = new PairsMarker(
                correlationPoints1, // Add image1's width to the X points to show the markings correctly
                correlationPoints2.Apply(p => new IntPoint(p.X + img1.Width, p.Y)));

            pictureBox3.Image = pairs.Apply(img3);
        }

        private void btnRansac_Click(object sender, EventArgs e)
        {
            // Step 3: Create the homography matrix using a robust estimator
            RansacHomographyEstimator ransac = new RansacHomographyEstimator(double.Parse(textBox6.Text), 0.95);
            homography = ransac.Estimate(correlationPoints1, correlationPoints2);

            // Plot RANSAC results against correlation results
            IntPoint[] inliers1 = correlationPoints1.Submatrix(ransac.Inliers);
            IntPoint[] inliers2 = correlationPoints2.Submatrix(ransac.Inliers);

            // Concatenate the two images in a single image (just to show on screen)
            Concatenate concat = new Concatenate(img1);
            Bitmap img3 = concat.Apply(img2);

            // Show the marked correlations in the concatenated image
            PairsMarker pairs = new PairsMarker(
                inliers1, // Add image1's width to the X points to show the markings correctly
                inliers2.Apply(p => new IntPoint(p.X + img1.Width, p.Y)));

            pictureBox3.Image = pairs.Apply(img3);
        }

        private void btnBlend_Click(object sender, EventArgs e)
        {
            // Step 4: Project and blend the second image using the homography
            Blend blend = new Blend(homography, img1);
            Bitmap image2 = blend.Apply(img2);
            pictureBox3.Image = image2;

            //path1 = Application.StartupPath + @"/photo/" + listBox1.Items[0].ToString();
            //path2 = Application.StartupPath + @"/photo/" + listBox1.Items[1].ToString();
            try
            {
                //目录不存在则创建
                if (!Directory.Exists(Application.StartupPath + @"/SourcePhoto/"))
                {
                    Directory.CreateDirectory(Application.StartupPath + @"/SourcePhoto/");
                }
                ////移动已匹配的图像到新文件夹
                img1.Save(SourcePhotoPath + listBox1.Items[0]);
                img2.Save(SourcePhotoPath + listBox1.Items[1]);
                img1.Dispose();
                img2.Dispose();
                File.Delete(Application.StartupPath + @"/photo/" + "\\" + listBox1.Items[0]);
                File.Delete(Application.StartupPath + @"/photo/" + "\\" + listBox1.Items[1].ToString());


            }
            catch (Exception)
            {
                MessageBox.Show("文件移动失败{0}", e.ToString());
            }
         //   image2.Save(SourcePhotoPath + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg", ImageFormat.Jpeg);
            //DirAllJpg();
            //listBox1.SelectedIndex = 0;
        }

        private void btnDoItAll_Click(object sender, EventArgs e)
        {
            //// Do it all
            //if (img1 != null && img2 != null)
            //{
            //    btnHarris.PerformClick();
            //    btnCorrelation.PerformClick();
            //    btnRansac.PerformClick();
            //    btnBlend.PerformClick();
            //}

            Image<Bgr, float> one = new Image<Bgr, float>(img1);
            Image<Bgr, float> two = new Image<Bgr, float>(img2);
            Image<Bgr, Byte> mImage = one.Convert<Bgr, Byte>();
            Image<Bgr, Byte> oImage = two.Convert<Bgr, Byte>();
            Image<Bgr, Byte> result = FindMatch(one, two);
            result = convert(result);
            //try
            //{
            //    pictureBox3.Image = result.Bitmap;
            //    img1.Dispose();
            //    img2.Dispose();
            //    File.Delete(path1);
            //    File.Delete(path2);
            //    DirAllJpg();
            //    result.Save(path1);
            //    //DirAllJpg();
            //}
            //catch (Exception exception)
            //{
            //    MessageBox.Show(exception.Message);

            //}

            ////stitcher
            // Stitcher stitcher = new Stitcher(false);
            //Image<Bgr, byte>[] source = new Image<Bgr, byte>[2];
            //source[0] = mImage;
            //source[1] = oImage;
            //result = stitcher.Stitch(source);
            pictureBox3.Image = result.Bitmap;

        }

/////////////////////////////////////////////////////////////////////////////////stitching///////////////


        public static Image<Bgr, Byte> FindMatch(Image<Bgr, float> fImage, Image<Bgr, float> lImage)
        {
            HomographyMatrix homography = null;
            SURFDetector surfCPU = new SURFDetector(500, false);


            int k = 2;
            double uniquenessThreshold = 0.8;
            Matrix<int> indices;

            Matrix<byte> mask;

            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            Image<Gray, Byte> fImageG = fImage.Convert<Gray, Byte>();
            Image<Gray, Byte> lImageG = lImage.Convert<Gray, Byte>();

            if (GpuInvoke.HasCuda)
            {
                GpuSURFDetector surfGPU = new GpuSURFDetector(surfCPU.SURFParams, 0.01f);
                using (GpuImage<Gray, Byte> gpuModelImage = new GpuImage<Gray, byte>(fImageG))
                //extract features from the object image
                using (GpuMat<float> gpuModelKeyPoints = surfGPU.DetectKeyPointsRaw(gpuModelImage, null))
                using (GpuMat<float> gpuModelDescriptors = surfGPU.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints))
                using (GpuBruteForceMatcher<float> matcher = new GpuBruteForceMatcher<float>(DistanceType.L2))
                {
                    modelKeyPoints = new VectorOfKeyPoint();
                    surfGPU.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);

                    // extract features from the observed image
                    using (GpuImage<Gray, Byte> gpuObservedImage = new GpuImage<Gray, byte>(lImageG))
                    using (GpuMat<float> gpuObservedKeyPoints = surfGPU.DetectKeyPointsRaw(gpuObservedImage, null))
                    using (GpuMat<float> gpuObservedDescriptors = surfGPU.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints))
                    using (GpuMat<int> gpuMatchIndices = new GpuMat<int>(gpuObservedDescriptors.Size.Height, k, 1, true))
                    using (GpuMat<float> gpuMatchDist = new GpuMat<float>(gpuObservedDescriptors.Size.Height, k, 1, true))
                    using (GpuMat<Byte> gpuMask = new GpuMat<byte>(gpuMatchIndices.Size.Height, 1, 1))
                    using (Emgu.CV.GPU.Stream stream = new Emgu.CV.GPU.Stream())
                    {
                        matcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuMatchIndices, gpuMatchDist, k, null, stream);
                        indices = new Matrix<int>(gpuMatchIndices.Size);
                        mask = new Matrix<byte>(gpuMask.Size);

                        //gpu implementation of voteForUniquess
                        using (GpuMat<float> col0 = gpuMatchDist.Col(0))
                        using (GpuMat<float> col1 = gpuMatchDist.Col(1))
                        {
                            GpuInvoke.Multiply(col1, new MCvScalar(uniquenessThreshold), col1, stream);
                            GpuInvoke.Compare(col0, col1, gpuMask, CMP_TYPE.CV_CMP_LE, stream);
                        }

                        observedKeyPoints = new VectorOfKeyPoint();
                        surfGPU.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

                        //wait for the stream to complete its tasks
                        //We can perform some other CPU intesive stuffs here while we are waiting for the stream to complete.
                        stream.WaitForCompletion();

                        gpuMask.Download(mask);
                        gpuMatchIndices.Download(indices);

                        if (GpuInvoke.CountNonZero(gpuMask) >= 4)
                        {
                            int nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                            if (nonZeroCount >= 4)
                                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                        }

                    }
                }
            }
            else
            {
                
                //extract features from the object image本来是个空的
                modelKeyPoints = new VectorOfKeyPoint();
                //modelKeyPoints = surfCPU.DetectKeyPointsRaw(fImageG, null);
                //检测图像的关键点，并且计算它的描述子（用一组向量将这个关键点描述出来）
                Matrix<float> modelDescriptors = surfCPU.DetectAndCompute(fImageG, null, modelKeyPoints);

                // extract features from the observed image本来是个空的
                observedKeyPoints = new VectorOfKeyPoint();
                //observedKeyPoints = surfCPU.DetectKeyPointsRaw(lImageG, null);
                Matrix<float> observedDescriptors = surfCPU.DetectAndCompute(lImageG, null, observedKeyPoints);
                //暴力匹配
                BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2Sqr);
                matcher.Add(modelDescriptors);

                indices = new Matrix<int>(observedDescriptors.Rows, k);
                using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
                {
                    matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
                    mask = new Matrix<byte>(dist.Rows, 1);
                    mask.SetValue(255);
                    Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
                }

                int nonZeroCount = CvInvoke.cvCountNonZero(mask);
                if (nonZeroCount >= 4)
                {
                    nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                    if (nonZeroCount >= 4)
                        homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                }
            }
            Image<Bgr, Byte> mImage = fImage.Convert<Bgr, Byte>();
            Image<Bgr, Byte> oImage = lImage.Convert<Bgr, Byte>();
            Image<Bgr, Byte> result = new Image<Bgr, byte>(mImage.Width + oImage.Width, mImage.Height);

            #region draw the projected on the image
            if (homography != null)
            {  //draw a rectangle along the projected model
                Rectangle rect = fImage.ROI;
                PointF[] pts = new PointF[] {
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};
                homography.ProjectPoints(pts);

               //result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(Color.Red), 5);

               HomographyMatrix origin = new HomographyMatrix();                //I perform a copy of the left image with a not real shift operation on the origin
               origin.SetIdentity();
               origin.Data[0, 2] = 0;
               origin.Data[1, 2] = 0;
               //create a blank image with specified width and height
               Image<Bgr, Byte> mosaic = new Image<Bgr, byte>(mImage.Width + oImage.Width + 2000, mImage.Height + oImage.Height + 2000);

               Image<Bgr, byte> warp_image = mosaic.Clone();

               mosaic = mImage.WarpPerspective(origin, mosaic.Width, mosaic.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, Emgu.CV.CvEnum.WARP.CV_WARP_DEFAULT, new Bgr(255, 255, 255));


               warp_image = oImage.WarpPerspective(homography, warp_image.Width, warp_image.Height, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC, Emgu.CV.CvEnum.WARP.CV_WARP_INVERSE_MAP, new Bgr(200, 0, 0));
               Image<Gray, byte> warp_image_mask = oImage.Convert<Gray, byte>();
               warp_image_mask.SetValue(new Gray(255));
               Image<Gray, byte> warp_mosaic_mask = mosaic.Convert<Gray, byte>();
               warp_mosaic_mask.SetZero();
               warp_mosaic_mask = warp_image_mask.WarpPerspective(homography, warp_mosaic_mask.Width, warp_mosaic_mask.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR, Emgu.CV.CvEnum.WARP.CV_WARP_INVERSE_MAP, new Gray(0));
               warp_image.Copy(mosaic, warp_mosaic_mask);

               return mosaic;
            }
            #endregion
            return null;
        }

        private Image<Bgr, Byte> convert(Image<Bgr, Byte> img)
        {
            Image<Gray, byte> imgGray = img.Convert<Gray, byte>();
            Image<Gray, byte> mask = imgGray.CopyBlank();

            Contour<Point> largestContour = null;
            double largestarea = 0;

            for (var contours = imgGray.FindContours(CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                RETR_TYPE.CV_RETR_EXTERNAL); contours != null; contours = contours.HNext)
            {
                if (contours.Area > largestarea)
                {
                    largestarea = contours.Area;
                    largestContour = contours;
                }
            }
            CvInvoke.cvSetImageROI(img, largestContour.BoundingRectangle);
            CvInvoke.cvSetImageROI(img, largestContour.BoundingRectangle);
            return img;
        }


    }
}
