using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Security_SteganographyProject
{
    public partial class Form1 : Form
    {
        private StegImage stegImage;
        private RSA rsa, rsaStegPrvKey, rsaStegPubKey, rsaMLenPrvKey, rsaMLenPubKey, rsaAesPrvKey, rsaAesPubKey;
        private string prevStegImageLocation, saveEncContentLocation;
        private Point point;
        private OpenFileDialog ofd;
        private SaveFileDialog sfd;
        private FolderBrowserDialog fbd;
        private About about;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeAllKeys();

            stegImage = new StegImage("Steganography.png");
            prevStegImageLocation = "Steganography.png";
            pictureBox1.ImageLocation = stegImage.FileLocation;
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(pictureBox1, pictureBox1.ImageLocation);
            toolTip2.SetToolTip(textBox1, "Double-click to get loaded picture's path.");
            toolTip3.SetToolTip(textBox5, "Double-click to get loaded picture's path.");
            saveEncContentLocation = "";
            enableEncryptionDecryptionToolStripMenuItem.Checked = true;
            saveEncryptedMessageFileToolStripMenuItem.Checked = false;

            //Initializations For Embedding
            radioButton1.Checked = true;
            label2.Hide();
            textBox4.Hide();
            button2.Hide();
            button9.Hide();


            //Initializations For Extracting
            radioButton3.Checked = true;
            label6.Hide();
            textBox8.Hide();
            button6.Hide();
        }

        private void InitializeAllKeys()
        {
            rsaStegPubKey = new RSA(); // Steg Encryption Key
            rsaStegPrvKey = new RSA(); // Steg Decryption Key
            rsaAesPubKey = new RSA();
            rsaAesPrvKey = new RSA();
            rsaMLenPubKey = new RSA();
            rsaMLenPrvKey = new RSA();
            rsaStegPubKey.SetKey("Keys\\MSGPublicSet.pubrsa");
            rsaStegPrvKey.SetKey("Keys\\MSGPrivateSet.prvrsa");
            rsaAesPubKey.SetKey("Keys\\AESPublicSet.pubrsa");
            rsaAesPrvKey.SetKey("Keys\\AESPrivateSet.prvrsa");
            rsaMLenPubKey.SetKey("Keys\\LENPublicSet.pubrsa");
            rsaMLenPrvKey.SetKey("Keys\\LENPrivateSet.prvrsa");
        }

        // Embed Tab Functions

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                textBox3.Show();
                label2.Hide();
                textBox4.Hide();
                button2.Hide();
                button9.Hide();
                point = new Point();
                point.X = 23;
                point.Y = 370;
                button3.Location = point;
                point.Y = 399;
                button4.Location = point;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                textBox3.Hide();
                label2.Show();
                textBox4.Show();
                button2.Show();
                button9.Show();
                point = new Point();
                point.X = 23;
                point.Y = 265;
                button3.Location = point;
                point.Y = 294;
                button4.Location = point;
                point.Y = 323;
                button9.Location = point;
            }
        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            textBox1.Text = pictureBox1.ImageLocation;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png";
                ofd.ShowDialog(this);
                textBox1.Text = ofd.FileName;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "All Files|*.*";
                ofd.ShowDialog(this);
                textBox4.Text = ofd.FileName;
            }
            catch (FileNotFoundException FNFEx)
            {
                MessageBox.Show(this, FNFEx.Message + "\nPlease enter a valid file name.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveEncryptedMessageFileToolStripMenuItem.Checked == true && saveEncContentLocation == "")
            {
                MessageBox.Show(this, "Please provide a path to save encrypted content.", "Path Mising", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (textBox1.Text == "" || textBox2.Text == "")
                MessageBox.Show(this, "Incomplete Information.\nPlease check if all the necessary fields are filled up.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (radioButton1.Checked)
            {
                prevStegImageLocation = pictureBox1.ImageLocation;
                stegImage = new StegImage(textBox1.Text);

                if (textBox3.Text == "")
                    MessageBox.Show(this, "Incomplete Information.\nPlease check if all the necessary fields are filled up.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (stegImage.EmbedText(textBox3.Text, textBox2.Text, ref rsaStegPubKey, ref rsaAesPubKey, ref rsaMLenPubKey, enableEncryptionDecryptionToolStripMenuItem.Checked, saveEncryptedMessageFileToolStripMenuItem.Checked, saveEncContentLocation))
                {
                    MessageBox.Show(this, "Text Embedded Successfully!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    stegImage = new StegImage("tempImg.bmp");
                    pictureBox1.ImageLocation = "tempImg.bmp";
                    pictureBox1.Refresh();
                    toolTip1.SetToolTip(pictureBox1, pictureBox1.ImageLocation);
                }
                else
                    MessageBox.Show(this, "Text Embedding Failed!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (radioButton2.Checked)
            {
                prevStegImageLocation = pictureBox1.ImageLocation;
                stegImage = new StegImage(textBox1.Text);

                if (textBox4.Text == "")
                    MessageBox.Show(this, "Incomplete Information.\nPlease check if all the necessary fields are filled up.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else if (stegImage.EmbedFile(textBox4.Text, textBox2.Text, ref rsaStegPubKey, ref rsaAesPubKey, ref rsaMLenPubKey, enableEncryptionDecryptionToolStripMenuItem.Checked, saveEncryptedMessageFileToolStripMenuItem.Checked, saveEncContentLocation))
                {
                    MessageBox.Show(this, "File Embedded Successfully!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    stegImage = new StegImage("tempImg.bmp");
                    pictureBox1.ImageLocation = "tempImg.bmp";
                    pictureBox1.Refresh();
                    toolTip1.SetToolTip(pictureBox1, pictureBox1.ImageLocation);
                }
                else
                    MessageBox.Show(this, "File Embedding Failed!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string str = "Covering capacity of current image: " + stegImage.Capacity.ToString() + " bytes.";
            MessageBox.Show(str, "Covering Capacity", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                FileStream fs = new FileStream(textBox4.Text, FileMode.Open, FileAccess.Read, FileShare.None);
                MessageBox.Show("Size of file selected to embed: " + fs.Length + " Bytes", "File Size", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fs.Close();
            }
        }

        // Extract Tab Functions

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                textBox7.Show();
                label6.Hide();
                textBox8.Hide();
                button6.Hide();
                point = new Point();
                point.X = 23;
                point.Y = 301;
                button7.Location = point;
                button8.Hide();
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                textBox7.Hide();
                label6.Show();
                textBox8.Show();
                button6.Show();
                point = new Point();
                point.X = 23;
                point.Y = 196;
                button7.Location = point;
                button8.Show();
            }
        }

        private void textBox5_DoubleClick(object sender, EventArgs e)
        {
            textBox5.Text = pictureBox1.ImageLocation;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png";
                ofd.ShowDialog(this);
                textBox5.Text = ofd.FileName;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                sfd = new SaveFileDialog();
                sfd.Filter = "All Files|*.*";
                sfd.ShowDialog(this);
                if (sfd.FileName != "")
                    textBox8.Text = sfd.FileName;
            }
            finally
            {
                if (sfd != null)
                    sfd.Dispose();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (saveEncryptedMessageFileToolStripMenuItem.Checked == true && saveEncContentLocation == "")
            {
                MessageBox.Show(this, "Please provide a path to save encrypted content.", "Path Mising", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            FileInfo messageFile = null;
            if (textBox5.Text == "")
                MessageBox.Show(this, "Incomplete Information.\nPlease check if all the necessary fields are filled up.", "Incomplete Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else if (!File.Exists(textBox5.Text))
                MessageBox.Show(this, "File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (radioButton3.Checked)
            {
                prevStegImageLocation = pictureBox1.ImageLocation;
                stegImage = new StegImage(textBox5.Text);

                textBox7.Text = stegImage.ExtractText(ref rsaStegPrvKey, ref rsaAesPrvKey, ref rsaMLenPrvKey, enableEncryptionDecryptionToolStripMenuItem.Checked, saveEncryptedMessageFileToolStripMenuItem.Checked, saveEncContentLocation);
                if (textBox7.Text != null)
                {
                    MessageBox.Show(this, "Text Extracted Successfully!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pictureBox1.ImageLocation = stegImage.FileLocation;
                    toolTip1.SetToolTip(pictureBox1, pictureBox1.ImageLocation);
                }
                else
                    MessageBox.Show(this, "Text Extraction Failed!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (radioButton4.Checked)
            {
                prevStegImageLocation = pictureBox1.ImageLocation;
                stegImage = new StegImage(textBox5.Text);
                messageFile = stegImage.ExtractFile(ref rsaStegPrvKey, ref rsaAesPrvKey, ref rsaMLenPrvKey, enableEncryptionDecryptionToolStripMenuItem.Checked, saveEncryptedMessageFileToolStripMenuItem.Checked, saveEncContentLocation);

                if (messageFile != null)
                {
                    messageFile.CopyTo(textBox8.Text, true);
                    messageFile.Delete();
                    MessageBox.Show(this, "File Extracted Successfully!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    pictureBox1.ImageLocation = stegImage.FileLocation;
                    toolTip1.SetToolTip(pictureBox1, pictureBox1.ImageLocation);
                }
                else
                    MessageBox.Show(this, "File Extraction Failed!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox8.Text != "")
                {
                    Process.Start(textBox8.Text);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Menu Strip Items

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png";
                ofd.ShowDialog(this);
                if (ofd.FileName != "")
                {
                        stegImage = new StegImage(ofd.FileName);
                        pictureBox1.ImageLocation = ofd.FileName;
                        toolTip1.SetToolTip(pictureBox1, pictureBox1.ImageLocation);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(this, Ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void saveImageAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                sfd = new SaveFileDialog();
                sfd.DefaultExt = "png";
                sfd.Filter = "BMP Files (*.bmp)|*.bmp|PNG Files (*.png)|*.png";
                sfd.ShowDialog(this);
                if (sfd.FileName != "")
                {
                    if (sfd.FileName.ToLower().EndsWith("png"))
                        stegImage.Save(sfd.FileName, "png");
                    else if (sfd.FileName.ToLower().EndsWith("bmp"))
                        stegImage.Save(sfd.FileName, "bmp");
                }
            }
            catch (IOException IOEx)
            {
                MessageBox.Show(IOEx.Message);
            }
            finally
            {
                if (sfd != null)
                    sfd.Dispose();
            }
        }

        private void loadPreviousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.ResetText();
            textBox5.ResetText();

            stegImage = new StegImage(prevStegImageLocation);
            pictureBox1.ImageLocation = stegImage.FileLocation;
            toolTip1.SetToolTip(pictureBox1, pictureBox1.ImageLocation);
        }

        private void refreshCurrentImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void centerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Normal;
        }

        private void strechToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void zoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about = new About();
            about.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            Environment.Exit(0);
        }

        private void enableEncryptionDecryptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!enableEncryptionDecryptionToolStripMenuItem.Checked)
            {
                saveEncryptedMessageFileToolStripMenuItem.Checked = false;
                saveEncryptedMessageFileToolStripMenuItem.Enabled = false;
                aesKeyEncryptionToolStripMenuItem.Enabled = false;
            }
            else
            {
                saveEncryptedMessageFileToolStripMenuItem.Checked = false;
                saveEncryptedMessageFileToolStripMenuItem.Enabled = true;
                aesKeyEncryptionToolStripMenuItem.Enabled = true;
            }
        }

        private void saveEncryptedMessageFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveEncryptedMessageFileToolStripMenuItem.Checked)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "AESENC File (*.aesenc)|*.aesenc";
                if (saveEncContentLocation != "")
                    sfd.FileName = saveEncContentLocation;
                sfd.ShowDialog();
                if (sfd.FileName != "")
                    saveEncContentLocation = sfd.FileName;
                sfd.Dispose();
            }
        }

        private void generateKeyPairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                fbd = new FolderBrowserDialog();
                fbd.ShowDialog();
                if (fbd.SelectedPath != "")
                {
                    rsa = new RSA();
                    rsa.SaveNewKeys(fbd.SelectedPath);
                }
            }
            catch (IOException IOEx)
            {
                MessageBox.Show(IOEx.Message);
            }
            finally
            {
                if (fbd != null)
                    fbd.Dispose();
                if (rsa != null)
                    rsa.Dispose();
            }
        }

        private void setPublicKeyFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "PUBRSA File (*.pubrsa)|*.pubrsa";
                ofd.ShowDialog(this);
                if (ofd.FileName != "")
                {
                    //if (ofd.FileName.ToLower().EndsWith("pubrsa"))
                    rsaStegPubKey.SetKey(ofd.FileName);
                    /*else
                        MessageBox.Show(this, "Please select a public rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                }
            }
            catch (ArgumentException AEx)
            {
                MessageBox.Show(this, AEx.Message + "\nPlease select a public rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void setPrivateKeyFileToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "PRVRSA File (*.pubrsa)|*.prvrsa";
                ofd.ShowDialog(this);
                if (ofd.FileName != "")
                {
                    //if (ofd.FileName.ToLower().EndsWith("prvrsa"))
                    rsaStegPrvKey.SetKey(ofd.FileName);
                    /*else
                        MessageBox.Show(this, "Please select a private rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                }
            }
            catch (ArgumentException AEx)
            {
                MessageBox.Show(this, AEx.Message + "\nPlease select a private rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void setPublicKeyFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "PUBRSA File (*.pubrsa)|*.pubrsa";
                ofd.ShowDialog(this);
                if (ofd.FileName != "")
                {
                    if (ofd.FileName.ToLower().EndsWith("pubrsa"))
                        rsaAesPubKey.SetKey(ofd.FileName);
                    else
                        MessageBox.Show(this, "Please select a public rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException AEx)
            {
                MessageBox.Show(this, AEx.Message + "\nPlease select a public rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void setPrivateKeyFileToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "PRVRSA File (*.pubrsa)|*.prvrsa";
                ofd.ShowDialog(this);
                if (ofd.FileName != "")
                {
                    if (ofd.FileName.ToLower().EndsWith("prvrsa"))
                        rsaAesPrvKey.SetKey(ofd.FileName);
                    else
                        MessageBox.Show(this, "Please select a private rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException AEx)
            {
                MessageBox.Show(this, AEx.Message + "\nPlease select a private rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void setPublicEncryptionKeyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "PUBRSA File (*.pubrsa)|*.pubrsa";
                ofd.ShowDialog(this);
                if (ofd.FileName != "")
                {
                    if (ofd.FileName.ToLower().EndsWith("pubrsa"))
                        rsaMLenPubKey.SetKey(ofd.FileName);
                    else
                        MessageBox.Show(this, "Please select a public rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException AEx)
            {
                MessageBox.Show(this, AEx.Message + "\nPlease select a public rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        private void setPrivateDecryptionKeyFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ofd = new OpenFileDialog();
                ofd.FileName = "";
                ofd.Filter = "PRVRSA File (*.pubrsa)|*.prvrsa";
                ofd.ShowDialog(this);
                if (ofd.FileName != "")
                {
                    if (ofd.FileName.ToLower().EndsWith("prvrsa"))
                        rsaMLenPrvKey.SetKey(ofd.FileName);
                    else
                        MessageBox.Show(this, "Please select a private rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException AEx)
            {
                MessageBox.Show(this, AEx.Message + "\nPlease select a private rsa file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ofd != null)
                    ofd.Dispose();
            }
        }

        // Form Close

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (File.Exists("tempImg.bmp"))
                File.Delete("tempImg.bmp");
            if (stegImage != null)
                stegImage.Dispose();
            if (rsa != null)
                rsa.Dispose();
            if (rsaStegPrvKey != null)
                rsaStegPrvKey.Dispose();
            if (rsaStegPubKey != null)
                rsaStegPubKey.Dispose();
            if (rsaAesPrvKey != null)
                rsaAesPrvKey.Dispose();
            if (rsaAesPubKey != null)
                rsaAesPubKey.Dispose();
            pictureBox1.Dispose();
        }
    }
}