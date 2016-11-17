﻿/* Copyright (c) 2016 Acrolinx GmbH */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Acrolinx.Sdk.Sidebar.Documents;
using Acrolinx.Sdk.Sidebar.Util.Configuration;

namespace Acrolinx.Demo.Sidebar
{
    public partial class Sample : Form
    {
        private int childFormNumber = 0;
        private string serverAddress = null;

        public Sample()
        {
            InitializeComponent();
            Acrolinx.Sdk.Sidebar.Util.Logging.Logger.LogToConsole();
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void simpleSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var simple = new SimpleSample(serverAddress);
            simple.MdiParent = this;
            simple.Show();
        }

        private void multiSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var multi = new MultiSample(serverAddress);
            multi.MdiParent = this;
            multi.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = openFileDialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                LoadFiles(openFileDialog.FileNames);
            }
        }

        private void LoadFiles(ICollection<string> files)
        {
            foreach (var fileName in files)
            {
                try
                {
                    var simple = new SimpleSample(serverAddress, getFormat(fileName), fileName, File.ReadAllText(fileName));
                    simple.MdiParent = this;
                    simple.Show();
                }
                catch (Exception err)
                {
                    MessageBox.Show(this, "Failed to load '" + fileName + "':" + Environment.NewLine + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private Format getFormat(string fileName)
        {
            if (fileName.ToLower().EndsWith(".htm") || fileName.ToLower().EndsWith(".html"))
            {
                return Format.HTML;
            }
            if (fileName.ToLower().EndsWith(".xml") || fileName.ToLower().EndsWith(".xhtml") || fileName.ToLower().EndsWith(".ditamap") || fileName.ToLower().EndsWith(".dita"))
            {
                return Format.XML;
            }
            if (fileName.ToLower().EndsWith(".md") || fileName.ToLower().EndsWith(".markdown"))
            {
                return Format.Markdown;
            }
            return Format.Text;
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var options = new Options(serverAddress);

            if (options.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            serverAddress = options.ServerAddress;
        }

         private void Sample_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }

            var dropped = ((string[])e.Data.GetData(DataFormats.FileDrop));
            var files = dropped.ToList();
            LoadFiles(files);
        }

        private void Sample_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }
    }
}
