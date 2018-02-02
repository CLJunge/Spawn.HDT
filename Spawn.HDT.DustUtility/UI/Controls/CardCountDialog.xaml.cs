namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardCountDialog
    {
        #region Ctor
        public CardCountDialog()
        {
            InitializeComponent();
        }
        #endregion

        #region Initialize
        public void Initialize(string strCardName, int nCardCount)
        {
            NumericUpDownCtrl.Value = nCardCount;
            NumericUpDownCtrl.Maximum = nCardCount;

            HeaderLabel.Content = $"How many copies of \"{strCardName}\"? ({nCardCount} max.)";
        }
        #endregion
    }
}