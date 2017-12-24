﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using MyPad.Dialogs;

namespace MyPad
{
    public partial class MainForm : Form
    {
        private void toolStripButtonInsertImageURL_Click (object sender, EventArgs e)
        {
            enchanceImagelinkToolStripMenuItem_Click (sender, e);
        }

        private void enchanceImagelinkToolStripMenuItem_Click (object sender, EventArgs e)
        {
            TabPage tb = tabControl1.SelectedTab;
            if (tb as EditorTabPage == null) {
                return;
            }
            EditorTabPage etb = tb as EditorTabPage;
            etb.EnchanceImagelink ();
        }

        public string InputFileFromURL ()
        {
            TabPage tb = tabControl1.SelectedTab;
            if (tb as EditorTabPage == null) {
                return string.Empty;
            }
            EditorTabPage etb = tb as EditorTabPage;
            FileInfo fi = new FileInfo (etb.GetFileFullPathAndName ());

            var dialog = new InsertImageDialog ();
            dialog.FileName = fi.Name.Substring(0, fi.Name.Length - fi.Extension.Length) + "_pic1";
            if (dialog.ShowDialog () != DialogResult.OK) {
                return String.Empty;
            }
            if (fi.Directory.Exists == false) {
                Directory.CreateDirectory (fi.DirectoryName);
            }
            var newName = Path.Combine (fi.DirectoryName, dialog.FileName);
            Download (dialog.URL, newName);
            var resBuilder = new StringBuilder ();
            resBuilder.AppendFormat ($"<img src=\"{dialog.FileName}\" alt=\"{dialog.ALT}\"/>");
            return resBuilder.ToString ();
        }

        void Download (string uRL, string newName)
        {
            using (var client = new WebClient ()) {
                client.DownloadFile (uRL, newName);
            }
        }
    }

}