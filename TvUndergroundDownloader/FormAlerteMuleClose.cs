using System;
using System.Windows.Forms;

namespace TvUndergroundDownloader
{
    public enum AlertChoiceEnum
    {
        Skip,
        Close,
        Disable
    }

    public partial class FormAlerteMuleClose : Form
    {
        private int Counter = 30;

        public AlertChoiceEnum AlertChoice { get; private set; }

        public FormAlerteMuleClose()
        {
            InitializeComponent();
        }

        private void buttonSkip_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            AlertChoice = AlertChoiceEnum.Skip;
            this.Close();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            AlertChoice = AlertChoiceEnum.Close;
            this.Close();
        }

        private void buttonDisable_Click(object sender, EventArgs e)
        {
            AlertChoice = AlertChoiceEnum.Disable;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = string.Format("Autoclose eMule between {0} sec", Counter);
            Counter--;
            if (Counter < 0)
            {
                AlertChoice = AlertChoiceEnum.Close;
                this.Close();
            }
        }
    }
}