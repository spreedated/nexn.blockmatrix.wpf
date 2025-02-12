using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace neXn.BlockMatrix
{
    public partial class BlockMatrixProgress
    {
        /// <summary>
        /// Color for "On" Blocks
        /// </summary>
        public Color ColorOn { get; set; } = Color.FromRgb(28, 166, 0);
        /// <summary>
        /// Color for "Off" Blocks
        /// </summary>
        public Color ColorOff { get; set; } = Color.FromRgb(200, 210, 220);
        /// <summary>
        /// Using randomcolors deactivates color properties
        /// </summary>
        public bool Randomcolors { get; set; }
        public FillStyles Fillstyle { get; set; } = 0;

        public enum FillStyles
        {
            Random = 0,
            Straight,
            Bars,
            BarsRightToLeft
        }

        private int _Value;
        public int Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                switch (this.Fillstyle)
                {
                    case FillStyles.Random:
                        this.FillRandom(value).Wait();
                        break;
                    case FillStyles.Straight:
                        this.FillStraight(value).Wait();
                        break;
                    case FillStyles.Bars:
                        this.FillBars(value).Wait();
                        break;
                    case FillStyles.BarsRightToLeft:
                        this.FillBars(value, true).Wait();
                        break;
                }
            }
        }

        private readonly Grid refGrid;
        private readonly List<Block> BlockList = [];
        private bool IsMatrixReady;

        public BlockMatrixProgress(Grid refGrid)
        {
            this.refGrid = refGrid;
        }

        public void CreateMatrix(int columCount = 19, int rowCount = 4, int BlockWidth = 36, int BlockHeight = 12)
        {
            int X = 10;
            int Y = 10;
            int count = 0;

            for (int i = 0; i < columCount * rowCount; i++)
            {
                Block acc = new(this.ColorOn, this.ColorOff)
                {
                    Width = BlockWidth,
                    Height = BlockHeight,
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(X, Y, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Name = "Block_" + count.ToString(),
                    ID = count,
                    X = X,
                    Y = Y
                };
                acc.Width = 36;
                count -= -1;
                
                BlockList.Add(acc);

                X -= -40;
                if (X >= (columCount * 40) + 10)
                {
                    X = 10;
                    Y -= -15;
                }

                if (count >= columCount * rowCount)
                {
                    break;
                }
            }

            BlockList.ForEach(x => this.refGrid.Children.Add(x));
            this.IsMatrixReady = true;
        }

        public void Clear()
        {
            BlockList.ForEach(x => x.Off());
        }

        public class Block : Border
        {
            private readonly SolidColorBrush OffBrush;
            private readonly SolidColorBrush OnBrush;
            public int ID { get; set; }
            public bool IsActive { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
            public Block(Color onColor, Color offColor)
            {
                base.Background = OffBrush;
                this.IsActive = false;
                this.OnBrush = new SolidColorBrush(onColor);
                this.OffBrush = new SolidColorBrush(offColor);
            }
            public void On()
            {
                base.Background = OnBrush;
                this.IsActive = true;
            }
            public void Off()
            {
                base.Background = OffBrush;
                this.IsActive = false;
            }
        }
    }

    #region "FillStyles"
    public partial class BlockMatrixProgress
    {
        private static Color RandomColor()
        {
            Random random0 = new(Guid.NewGuid().GetHashCode());
            Random random1 = new(Guid.NewGuid().GetHashCode());
            Random random2 = new(Guid.NewGuid().GetHashCode());
            return Color.FromRgb(Convert.ToByte(random0.Next(0, 255)), Convert.ToByte(random1.Next(0, 255)), Convert.ToByte(random2.Next(0, 255)));
        }

        public async Task FillStraight(int val)
        {
            if (BlockList.Count == 0)
            {
                await Task.Factory.StartNew(() => { while (!this.IsMatrixReady) { } });
            }
            if (this.Value == 0)
            {
                return;
            }
            int FillNumBlock = Convert.ToInt32(Math.Floor(((double)BlockList.Count / 100) * val));

            BlockList.ForEach(x =>
            {
                if (FillNumBlock >= 1)
                {
                    x.On();
                    if (this.Randomcolors)
                    {
                        x.Background = new SolidColorBrush(RandomColor());
                    }
                    FillNumBlock -= 1;
                }
                else
                {
                    x.Off();
                }
            });
        }

        public async Task FillRandom(int val)
        {
            if (BlockList.Count == 0)
            {
                await Task.Factory.StartNew(() => { while (!this.IsMatrixReady) { } });
            }
            if (this.Value == 0)
            {
                return;
            }
            int FillNumBlock = Convert.ToInt32(Math.Floor(((double)BlockList.Count / 100) * val));

            BlockList.ForEach(x => x.Off());

            while (FillNumBlock >= 1)
            {
                IEnumerable<Block> k = BlockList.Where((x) => { return !x.IsActive; });
                Random random = new(Guid.NewGuid().GetHashCode());
                int randNum = random.Next(0, k.Count());
                Block turnBlock = k.ToList()[randNum];
                turnBlock.On();

                if (this.Randomcolors && k != null)
                {
                    turnBlock.Background = new SolidColorBrush(RandomColor());
                }
                FillNumBlock -= 1;
            }
        }

        public async Task FillBars(int val, bool righttoleft = false)
        {
            if (BlockList.Count == 0)
            {
                await Task.Run(() =>
                {
                    while (!this.IsMatrixReady) { }
                });
            }
            if (this.Value == 0 || BlockList.Count == 0)
            {
                return;
            }

            int FillNumBlock = Convert.ToInt32(Math.Floor(((double)BlockList.Count / 100) * val));

            BlockList.ForEach(x => x.Off());

            for (int i = 0; i < FillNumBlock; i++)
            {
                IEnumerable<Block> inactiveBlock = BlockList.Where(x => { return !x.IsActive; });
                Block blocks;
                if (righttoleft)
                {
                    blocks = inactiveBlock.FirstOrDefault(x => { return x.X == inactiveBlock.Max(y => { return y.X; }); });
                }
                else
                {
                    blocks = inactiveBlock.FirstOrDefault(x => { return x.X == inactiveBlock.Min(y => { return y.X; }); });
                }
                if (blocks != null)
                {
                    blocks.On();
                    if (this.Randomcolors)
                    {
                        blocks.Background = new SolidColorBrush(RandomColor());
                    }
                }
            }
        }
    }
    #endregion

}