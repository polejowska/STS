using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialTransmissionSimulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string convertToBinary(string textToConvert)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach(char L in textToConvert.ToString())
            {
                stringBuilder.Append(Convert.ToString(L, 2));
                stringBuilder.Append(11);
                stringBuilder.Append(" ");
                stringBuilder.Append(0);
            }

            stringBuilder.Insert(0, 0);
            string newT;
            newT = stringBuilder.ToString();

            int x = newT.Length;

            return newT.Remove(x-2);
        }

        public string convertToASCII(string binaryCode)
        {
            char c = ' ';
            string[] splitList = binaryCode.Split(c);

            foreach (string s in splitList)
            {
                s.Remove(9);
                s.Remove(8);
                s.Remove(0);
            }

            string newW = string.Join("", splitList);

            int wordLength = newW.Length;
            int elementsNumber = (wordLength - 1) / 7;
            int step = 6;

            List<Byte> bytesList = new List<Byte>();

            for(int i = 0; i < newW.Length; i+= step)
            {
                bytesList.Add(Convert.ToByte(newW.Substring(i, step), 2));
            }

            byte[] bytesNewArray = new byte[bytesList.Count];

            bytesList.CopyTo(bytesNewArray);

            return ASCIIEncoding.ASCII.GetString(bytesNewArray);
        }

        private static string ConvertToTxtString(String binaryCode)
        {
            const int step = 8;

            List<Byte> bytes = new List<Byte>();
            
            for (int i = 0; i < binaryCode.Length; i += step)
            {
                bytes.Add(Convert.ToByte(binaryCode.Substring(i, step), 2));
            }
                
            byte[] newByteArray = new byte[bytes.Count];
            bytes.CopyTo(newByteArray);
            return UTF8Encoding.UTF8.GetString(newByteArray);
        }

        public string save()
        {
            byte[] bArray = ASCIIEncoding.ASCII.GetBytes(senderTextBox.Text);
            string savedText = ASCIIEncoding.ASCII.GetString(bArray);
            return savedText;
        }


        private void sendToConverter_Click(object sender, EventArgs e)
        {
            converterBinaryCodeBox.Text = convertToBinary(senderTextBox.Text);
        }

        private void sendToReceiver(object sender, EventArgs e)
        {
            string sentText = senderTextBox.Text;
            char x = ' ';
            string[] splitText = sentText.Split(x);
            
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Data\Words.txt");
            string[] badWords = File.ReadAllLines(path);

            // Testing
            Console.WriteLine(badWords[3].ToString());


            for (int i = 0; i < splitText.Length; i++)
            {
                for (int j = 0; j < badWords.Length; j++)
                {
                    if (splitText[i].Contains(badWords[j]))
                    {
                        foreach (char c in splitText[i])
                        {
                            char w = '*';
                            splitText[i] = splitText[i].Replace(c, w);
                        }
                        receiverTextBox.Text = splitText[i];
                    }
                    else
                    {
                        senderTextBox.Text = receiverTextBox.Text;
                    }
                    string newWord = string.Join(" ", splitText);
                    senderTextBox.Text = newWord;
                    receiverTextBox.Text = newWord;
                }
            }
        }

        private void clearSender_Click(object sender, EventArgs e)
        {
            senderTextBox.Clear();
        }

        private void clearConverter_Click(object sender, EventArgs e)
        {
            converterBinaryCodeBox.Clear();
        }

        private void clearReceiver_Click(object sender, EventArgs e)
        {
            receiverTextBox.Clear();
        }

    }
}
