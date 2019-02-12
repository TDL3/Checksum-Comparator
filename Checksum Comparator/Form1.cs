using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Checksum_Comparator {
    public partial class Form1 : Form {
        private string[] checksum = new string[10];

        private static readonly int LABEL_TRUE = 0;
        private static readonly int LABEL_FALSE = 1;
        private static readonly int LABEL_EMPTY = 2;

        public Form1() {
            InitializeComponent();
        }

        private void Button_SelectFile_Click(object sender, EventArgs e) {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK) {
                textBox_FileDir.Text = dialog.FileName;
            }
        }

        private void Button_Compare_Click(object sender, EventArgs e) {
            CompareGenAndUserText(richTextBoxCRC32Gen, richTextBoxCRC32User, labelCRC32Flag);
            CompareGenAndUserText(richTextBoxCRC64Gen, richTextBoxCRC64User, labelCRC64Flag);
            CompareGenAndUserText(richTextBoxSHA256Gen, richTextBoxSHA256User, labelSHA256Flag);
            CompareGenAndUserText(richTextBoxSHA1Gen, richTextBoxSHA1User, labelSHA1Flag);
            CompareGenAndUserText(richTextBoxBLAKE2spGen, richTextBoxBLAKE2spUser, labelBLAKE2spFlag);
        }

        private void FileDir_Changed(object sender, EventArgs e) {
            //TODO: Make filrDir drag and dropable
            StringBuilder output = new StringBuilder();
            int lineCount = 0;
            Process SZ = new Process(); //SZ = 7-zip
            SZ.StartInfo.FileName = "7z.exe";
            SZ.StartInfo.Arguments = "h -scrccrc32 -scrccrc64 -scrcsha256 -scrcsha1 -scrcblake2sp " + "\"" + textBox_FileDir.Text + "\"";
            SZ.StartInfo.RedirectStandardOutput = true;
            SZ.StartInfo.UseShellExecute = false;
            SZ.StartInfo.CreateNoWindow = true;
            SZ.OutputDataReceived += new DataReceivedEventHandler((s, eve) => {
                if (!string.IsNullOrEmpty(eve.Data)) {
                    if (lineCount++ == 5) {
                        checksum = eve.Data.Split(' ');
                        SetChecksumInfoGen();
                    }
                }
            });
            SZ.Start();
            SZ.BeginOutputReadLine();
            SZ.WaitForExit();
        }
        private void SetChecksumInfoGen() {
            if (InvokeRequired) {
                richTextBoxCRC32Gen.BeginInvoke(new MethodInvoker(delegate { richTextBoxCRC32Gen.Text =  checksum[0]; }));
                richTextBoxCRC64Gen.BeginInvoke(new MethodInvoker(delegate { richTextBoxCRC64Gen.Text = checksum[1]; }));
                richTextBoxSHA256Gen.BeginInvoke(new MethodInvoker(delegate { richTextBoxSHA256Gen.Text = checksum[2]; }));
                richTextBoxSHA1Gen.BeginInvoke(new MethodInvoker(delegate { richTextBoxSHA1Gen.Text = checksum[3]; }));
                richTextBoxBLAKE2spGen.BeginInvoke(new MethodInvoker(delegate { richTextBoxBLAKE2spGen.Text = checksum[4]; }));
            }
        }

        private void CompareGenAndUserText(RichTextBox richTextBoxGen, RichTextBox richTextBoxUser, Label flagLabel) {
            string str1 = richTextBoxGen.Text;
            string str2 = richTextBoxUser.Text;
            if (str1 != "" && str2 != "") {
                for (int i = 0; i < str1.Length && i < str2.Length; i++) {
                    if (str1[i] != str2[i]) {
                        richTextBoxUser.Select(i, 1);
                        richTextBoxUser.SelectionColor = Color.Red;
                        SetFlagLabel(flagLabel, LABEL_FALSE);
                    } else {
                        richTextBoxUser.Select(i, 1);
                        richTextBoxUser.SelectionColor = Color.Green;
                        SetFlagLabel(flagLabel, LABEL_TRUE);
                    }
                }
            } else {
                SetFlagLabel(flagLabel, LABEL_EMPTY);
            }
        }

        private void SetFlagLabel(Label label, int flag) {
            if (flag == 0) {
                label.ForeColor = Color.Green;
                label.Text = "=";

            } else if (flag == 1) {
                label.ForeColor = Color.Red;
                label.Text = "≠";
            } else {
                labelBLAKE2spFlag.Text = "";
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e) {

        }
    }
}
