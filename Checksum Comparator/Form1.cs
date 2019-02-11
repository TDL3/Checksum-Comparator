using System;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Checksum_Comparator {
    public partial class Form1 : Form {
        private string[] checksum = new string[10];

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
            SetChecksumGen();
            if (!(richTextBoxCRC32User.Text == "")
                && !(richTextBoxCRC32Gen.Text == "")
                && richTextBoxCRC32User.Text.Equals(richTextBoxCRC32Gen.Text)) {
                SetFlagLabel(labelCRC32Flag, true);
            } else if (richTextBoxCRC32User.Text != "") {
                SetFlagLabel(labelCRC32Flag, false);
            } else {
                labelCRC32Flag.Text = "";
            }

            if (!(richTextBoxCRC64User.Text == "")
                && !(richTextBoxCRC64Gen.Text == "")
                && richTextBoxCRC64User.Text.Equals(richTextBoxCRC64Gen.Text)) {
                SetFlagLabel(labelCRC64Flag, true);
            } else if (richTextBoxCRC64User.Text != "") {
                SetFlagLabel(labelCRC64Flag, false);
            } else {
                labelCRC64Flag.Text = "";
            }

            if (!(richTextBoxSHA256User.Text == "")
                && !(richTextBoxSHA256Gen.Text == "")
                && richTextBoxSHA256User.Text.Equals(richTextBoxSHA256Gen.Text)) {
                SetFlagLabel(labelSHA256Flag, true);
            } else if (richTextBoxSHA256User.Text != "") {
                SetFlagLabel(labelSHA256Flag, false);
            } else {
                labelSHA256Flag.Text = "";
            }

            if (!(richTextBoxSHA1User.Text == "")
                && !(richTextBoxSHA1Gen.Text == "")
                && richTextBoxSHA1User.Text.Equals(richTextBoxSHA1Gen.Text)) {
                SetFlagLabel(labelSHA1Flag, true);
            } else if (richTextBoxSHA1User.Text != "") {
                SetFlagLabel(labelSHA1Flag, false);
            } else {
                labelSHA1Flag.Text = "";
            }

            if (!(richTextBoxBLAKE2spUser.Text == "")
                && !(richTextBoxBLAKE2spGen.Text == "")
                && richTextBoxBLAKE2spUser.Text.Equals(richTextBoxBLAKE2spGen.Text)) {
                SetFlagLabel(labelBLAKE2spFlag, true);
            } else if (richTextBoxBLAKE2spUser.Text != "") {
                SetFlagLabel(labelBLAKE2spFlag, false);
            } else {
                labelBLAKE2spFlag.Text = "";
            }
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
            SZ.OutputDataReceived += new DataReceivedEventHandler((s, eve) => {
                if (!string.IsNullOrEmpty(eve.Data)) {
                    if (lineCount++ == 5) {
                        checksum = eve.Data.Split(' ');
                    }
                }
            });
            SZ.Start();
            SZ.BeginOutputReadLine();
            SZ.WaitForExit();
        }
        private void SetChecksumGen() {
            richTextBoxCRC32Gen.Text = checksum[0];
            richTextBoxCRC64Gen.Text = checksum[1];
            richTextBoxSHA256Gen.Text = checksum[2];
            richTextBoxSHA1Gen.Text = checksum[3];
            richTextBoxBLAKE2spGen.Text = checksum[4];
        }

        private void SetFlagLabel(Label label, bool equal) {
            if (equal) {
                label.ForeColor = Color.Green;
                label.Text = "=";

            } else {
                label.ForeColor = Color.Red;
                label.Text = "≠";
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void Panel1_Paint(object sender, PaintEventArgs e) {

        }
    }
}
