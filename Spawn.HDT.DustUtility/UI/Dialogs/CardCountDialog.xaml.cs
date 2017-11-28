namespace Spawn.HDT.DustUtility.UI.Dialogs
{
    public partial class CardCountDialog
    {
        #region Ctor
        public CardCountDialog()
        {
            InitializeComponent();
        }

        public CardCountDialog(string name, int maxCount)
            : this()
        {
            NumericUpDownCtrl.Value = maxCount;
            NumericUpDownCtrl.Maximum = maxCount;

            HeaderLabel.Content = $"How many copies of \"{name}\"? ({maxCount} max.)";
        }
        #endregion
    }
}