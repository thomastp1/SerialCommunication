using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPortarduino.IsOpen) 
                {
                    // ik heb een verbinding -> de gebruiker wil deze verbreken
                    serialPortarduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Disconnected";
                }
                else
                {
                    //ik heb geen verbinding -> de gebruiker wil verbinding maken
                    serialPortarduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortarduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPortarduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPortarduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortarduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortarduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortarduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortarduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPortarduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortarduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortarduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortarduino.StopBits = StopBits.Two;


                    if (radioButtonHandshakeNone.Checked) serialPortarduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortarduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortarduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortarduino.Handshake = Handshake.XOnXOff;

                    serialPortarduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortarduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortarduino.Open();
                    string commando = "ping";
                    serialPortarduino.WriteLine(commando);
                    string antwoord = serialPortarduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "disconnect";
                        labelStatus.Text = "status: connected";
                    }
                    else
                    {
                        serialPortarduino.Close();
                        labelStatus.Text = "error: verkeerd antwoord";
                    }

                }
            
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }

        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
             if (serialPortarduino.IsOpen)
                {
                    string commando; //set d2 high of low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPortarduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortarduino.IsOpen)
                {
                    string commando; //set d3 high of low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPortarduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortarduino.IsOpen)
                {
                    string commando; //set d4 high of low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPortarduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortarduino.IsOpen)
                {
                    string commando = string.Format("set pwm9 {0}", trackBarPWM9.Value); // set pwm9 0...255
                    serialPortarduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM10_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortarduino.IsOpen)
                {
                    string commando = string.Format("set pwm10 {0}", trackBarPWM10.Value); // set pwm10 0...255
                    serialPortarduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM11_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortarduino.IsOpen)
                {
                    string commando = string.Format("set pwm11 {0}", trackBarPWM11.Value); // set pwm9 0...255
                    serialPortarduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerOefening3.Enabled = tabControl.SelectedIndex == 3;
            timeroefening4.Enabled = tabControl.SelectedIndex == 4;

        }

        private void timerOefening3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortarduino.IsOpen)
                {
                    serialPortarduino.ReadExisting();
                    string commando = "get d5";
                    serialPortarduino.WriteLine(commando);
                    string antwoord = serialPortarduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital5.Checked = (antwoord == "1");

                    commando = "get d6";
                    serialPortarduino.WriteLine(commando);
                    antwoord = serialPortarduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital6.Checked = (antwoord == "1");

                    commando = "get d7";
                    serialPortarduino.WriteLine(commando);
                    antwoord = serialPortarduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital7.Checked = (antwoord == "1");
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void timeroefening4_Tick(object sender, EventArgs e)
        {
            try
            {
             if (serialPortarduino.IsOpen)
                {
                    serialPortarduino.ReadExisting();
                    string commando = "get a0";
                    serialPortarduino.WriteLine(commando);
                    string antwoord = serialPortarduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    
                    int value = Int32.Parse(antwoord);
                    labelAnalog0.Text = value.ToString();
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortarduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
            
        }
    }
}
