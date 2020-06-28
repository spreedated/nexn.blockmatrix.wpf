// Copyright (c) 2020 Markus Wackermann
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;

namespace neXn
{
    namespace BlockMatrix
    {
        public class BlockMatrix
        {
            private readonly List<Block> BlockList = new List<Block>();
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
            public BlockMatrix(ref Grid refGrid)
            {
                this.refGrid = refGrid;
                this.AnimationSequence.Tick += AnimationSequence_Tick;
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
                    Block acc = new Block()
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
                    Debug.Print("Block created X: " + X.ToString() + " Y: " + Y.ToString());
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

                BlockList.All(x => { this.refGrid.Children.Add(x); return true; });
            }

            public class Block : Border
            {
                private readonly SolidColorBrush OffBrush = new SolidColorBrush(Color.FromRgb(200, 210, 220));
                private readonly SolidColorBrush OnBrush = new SolidColorBrush(Color.FromRgb(28, 166, 0));
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

            private readonly Timer AnimationSequence = new Timer();
            private void AnimationSequence_Tick(object sender, EventArgs e)
            {
                BlockList.All(x =>
                {
                    Random random = new Random(Guid.NewGuid().GetHashCode());
                    int rand = random.Next(1, 256);
                    if (rand <= RandomDegree)
                    {
                        x.Off();
                    }
                    else
                    {
                        x.On();
                    }
                    return true;
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
}