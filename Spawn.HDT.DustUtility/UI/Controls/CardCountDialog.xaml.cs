#region Using
using Spawn.HDT.DustUtility.Logging;
#endregion

namespace Spawn.HDT.DustUtility.UI.Controls
{
    public partial class CardCountDialog
    {
        #region Ctor
        public CardCountDialog()
        {
            InitializeComponent();

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, "Created new 'CardCountDialog' instance");
        }
        #endregion

        #region Initialize
        public void Initialize(string strCardName, int nCardCount)
        {
            NumericUpDownCtrl.Value = nCardCount;
            NumericUpDownCtrl.Maximum = nCardCount;

            HeaderLabel.Content = $"How many copies of \"{strCardName}\"? ({nCardCount} max.)";

            DustUtilityPlugin.Logger.Log(LogLevel.Debug, $"Set content: CardName={strCardName}, CardCount={nCardCount}");
        }
        #endregion
    }
}