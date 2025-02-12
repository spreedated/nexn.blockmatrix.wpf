# BlockMatrix

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://buymeacoffee.com/spreed)

## Overview

Some simple and cool looking ProgressBar and WaitAnimation.

### Wait Animation

![](Screenshots/BlockMatrix_wait.gif)

### Random

![](Screenshots/BlockMatrixProgress_random.gif)

### Straight

![](Screenshots/BlockMatrixProgress_straight.gif)

### Bars

![](Screenshots/BlockMatrixProgress_bars.gif)

### Bars right to left

![](Screenshots/BlockMatrixProgress_barslefttoright.gif)


### Features
- Custom Blocksize
- Custom colors
- Different fillstyles
- Custom matrix size

### Usage (C#)
(WaitAnimation)

```
BlockMatrix matrix = new BlockMatrix(ref <YourGrid x:Name>);
matrix.CreateMatrix();
matrix.Start();
```

(ProgressBar)

```
BlockMatrixProgress matrix = new BlockMatrixProgress(ref <YourGrid x:Name>) { Fillstyle = BlockMatrixProgress.FillStyles.<SelectYourStyle> };
matrix.CreateMatrix();
matrix.Value = 0; // Set value
```

## Contributing
Contributions are welcome! Feel free to submit issues or pull requests.

## License
This project is licensed under the [MIT License](LICENSE.txt).

## Acknowledgments
Thanks for checking out Year In Progress! Stay inspired and keep pushing forward!

### Copyright
This version was written by **Dante Wackermann**

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png)](https://buymeacoffee.com/spreed)