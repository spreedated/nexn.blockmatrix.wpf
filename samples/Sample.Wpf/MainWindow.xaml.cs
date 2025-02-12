using CommunityToolkit.Mvvm.ComponentModel;
using neXn.BlockMatrix;
using System.Windows;

namespace Sample.Wpf
{

    [ObservableObject]
    public partial class MainWindow : Window
    {
        [ObservableProperty]
        private string percentage = "0";

        public MainWindow()
        {
            this.InitializeComponent();
            this.DataContext = this;

            BlockMatrix blockMatrix = new(this.TheGrid);
            blockMatrix.CreateMatrix();
            blockMatrix.Start();

            BlockMatrixProgress blockMatrix1 = new(this.TheGrid1) { Fillstyle = BlockMatrixProgress.FillStyles.Random };
            blockMatrix1.CreateMatrix();

            BlockMatrixProgress blockMatrix2 = new(this.TheGrid2) { Fillstyle = BlockMatrixProgress.FillStyles.Straight };
            blockMatrix2.CreateMatrix();

            BlockMatrixProgress blockMatrix3 = new(this.TheGrid3) { Fillstyle = BlockMatrixProgress.FillStyles.Bars };
            blockMatrix3.CreateMatrix();

            BlockMatrixProgress blockMatrix4 = new(this.TheGrid4) { Fillstyle = BlockMatrixProgress.FillStyles.BarsRightToLeft };
            blockMatrix4.CreateMatrix();

            Task.Run(async () =>
            {
                int currentVal = 0;
                bool direction = false;

                while (true)
                {
                    await Task.Delay(80);

                    if (currentVal >= 101 || currentVal <= -1)
                    {
                        direction ^= true;
                        await Task.Delay(1500);
                    }

                    this.Dispatcher.Invoke(() =>
                    {
                        this.Percentage = currentVal.ToString();
                        blockMatrix1.Value = currentVal;
                        blockMatrix2.Value = currentVal;
                        blockMatrix3.Value = currentVal;
                        blockMatrix4.Value = currentVal;
                    });

                    if (!direction)
                    {
                        currentVal++;
                        continue;
                    }

                    currentVal--;
                }
            });
        }
    }
}