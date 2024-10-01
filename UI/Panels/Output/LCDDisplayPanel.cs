﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MobiFlight.Base;
using MobiFlight.UI.Panels.Config;

namespace MobiFlight.UI.Panels
{
    public partial class LCDDisplayPanel : UserControl
    {
        int Cols = 16;
        int Lines = 2;

        public LCDDisplayPanel()
        {
            InitializeComponent();
        }
        
        public void SetAddresses(List<ListItem> ports)
        {
            DisplayComboBox.DataSource = new List<ListItem>(ports);
            DisplayComboBox.DisplayMember = "Label";
            DisplayComboBox.ValueMember = "Value";
            if (ports.Count > 0)
                DisplayComboBox.SelectedIndex = 0;

            DisplayComboBox.Enabled = ports.Count > 0;
        }

        public void DisableOutputDefinition()
        {
            label2.Visible = false;
            panel2.Visible = false;
            label3.Visible = false;
            lcdDisplayTextBox.Text = string.Empty;
        }

        internal void syncFromConfig(OutputConfigItem config)
        {
            // preselect display stuff
            if (config.LcdDisplay.Address != null)
            {
                if (!ComboBoxHelper.SetSelectedItem(DisplayComboBox, config.LcdDisplay.Address))
                {
                    // TODO: provide error message
                    Log.Instance.log($"Exception on selecting item {config.LcdDisplay.Address} in LCD address ComboBox.", LogSeverity.Error);
                }
            }
            if (config.LcdDisplay.Lines.Count > 0)
                lcdDisplayTextBox.Lines = config.LcdDisplay.Lines.ToArray();

            EscapeCharTextBox.Text = config.LcdDisplay.EscapeChar?.ToString() ?? "";
        }

        internal OutputConfigItem syncToConfig(OutputConfigItem config)
        {
            // check if this is currently selected and properly initialized
            if (DisplayComboBox.SelectedValue == null) return config;

            config.LcdDisplay.Address = DisplayComboBox.SelectedValue.ToString().Split(',').ElementAt(0);

            config.LcdDisplay.Lines.Clear();
            foreach (String line in lcdDisplayTextBox.Lines)
            {
                config.LcdDisplay.Lines.Add(line);
            }

            config.LcdDisplay.EscapeChar = string.IsNullOrWhiteSpace(EscapeCharTextBox.Text) ? (char?)null : EscapeCharTextBox.Text[0];
            return config;
        }

        private void DisplayComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((sender as ComboBox).SelectedValue == null) return;

            Cols = int.Parse(((sender as ComboBox).SelectedValue.ToString()).Split(',').ElementAt(1));
            Lines = int.Parse(((sender as ComboBox).SelectedValue.ToString()).Split(',').ElementAt(2));
            lcdDisplayTextBox.Width = 4 + (Cols * 8);
            lcdDisplayTextBox.Height = Lines * 16;
        }

        private void lcdDisplayTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
