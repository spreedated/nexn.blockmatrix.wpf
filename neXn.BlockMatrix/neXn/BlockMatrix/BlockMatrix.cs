using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace neXn.BlockMatrix
{
    public class BlockMatrix
    {
        private readonly List<Block> BlockList = [];
        /// <summary>
        /// Returns the percantage of "On" Blocks as string ex. "19 %"
        /// </summary>
        public string UsagePercentage { get; set; }

        /// <summary>
        /// 0-255 / Byte <br/>
        /// How many blocks should be "On"
        /// </summary>
        public byte RandomDegree { get; set; } = 0xc0;

        private readonly Grid refGrid;

        /// <summary>
        /// Create a new BlockMatrix
        /// </summary>
        /// <param name="refGrid">Grid it should be drawn</param>
        public BlockMatrix(Grid refGrid)
        {
            this.refGrid = refGrid;
            this.AnimationSequence.Tick += this.AnimationSequence_Tick;
        }

        /// <summary>
        /// Draw a new Matrix
        /// </summary>
        /// <param name="columCount"></param>
        /// <param name="rowCount"></param>
        public void CreateMatrix(int columCount = 19, int rowCount = 4)
        {
            int X = 10; // +40
            int Y = 10; // +15
            int count = 0; //17 * 4 = 68

            for (int i = 0; i < columCount * rowCount; i++)
            {
                Block acc = new()
                {
                    Width = 36,
                    Height = 12,
                    CornerRadius = new CornerRadius(4),
                    Margin = new Thickness(X, Y, 0, 0),
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Name = "Block_" + count.ToString(),
                    ID = count
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
        }

        public class Block : Border
        {
            private readonly SolidColorBrush OffBrush = new(Color.FromRgb(200, 210, 220));
            private readonly SolidColorBrush OnBrush = new(Color.FromRgb(28, 166, 0));
            public int ID { get; set; }
            public bool IsActive { get; set; }
            public Block()
            {
                base.Background = OffBrush;
                this.IsActive = false;
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

        private readonly Timer AnimationSequence = new();
        private void AnimationSequence_Tick(object sender, EventArgs e)
        {
            BlockList.ForEach(x =>
            {
                Random random = new(Guid.NewGuid().GetHashCode());
                int rand = random.Next(1, 256);
                if (rand <= this.RandomDegree)
                {
                    x.Off();
                }
                else
                {
                    x.On();
                }
            });

            this.UsagePercentage = ((BlockList.Count(x => x.IsActive) * 100) / BlockList.Count).ToString() + " %";
        }
        public void Start()
        {
            this.AnimationSequence.Enabled = true;
            this.AnimationSequence.Interval = 100;
            this.AnimationSequence.Start();
        }
        public void Stop()
        {
            this.AnimationSequence.Enabled = false;
            this.AnimationSequence.Stop();
        }
    }
}
