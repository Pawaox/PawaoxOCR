# Description
This program is made to contain all of the OCR features i want to implement. It's a personal project i decided to upload here.

Right now, there is just one feature, "Screen OCR".

"Screen OCR" will continously take an image of a part of the screen, and perform OCR on it automatically. You dont need to upload images manually.
It can then parse the output, and additionally copy it to the clipboard.

# Implementation
It currently only supports the Tesseract OCR Engine, which can be found here: https://github.com/tesseract-ocr/tesseract

The C# .Net Nuget Package used is: https://www.nuget.org/packages/Tesseract/5.2.0

# Rquirements
Tesseract requires .traineddata files from https://github.com/tesseract-ocr/tessdata

These must currently be located in C:\Temp\tesseractData\. I plan to make this configurable if/when i return to this project.

# Usage
To begin parsing, click the red circle button at the top left.

The "Language" you want to read must be selected, it is the code form the traineddata file. For example, english would be "eng" (eng.traineddata). 
The list is automatically filled with data from the tesseractData folder

The "Capture Interval" is how often, in miliseconds, it should take a new image.

The Target Area can be defined with Top Position, Left Position, Width & Height, or "Show Area" can be opened to drag and resize (bottom right corner) a window that updates these values

As the program works by taking an Image, the "Target Area" box may interfere. You may get better results by hiding it after placing it where you want it.

![image](https://github.com/Pawaox/PawaoxOCR/assets/810291/e1aa7f88-b0a9-47c0-84ec-e69103860f41)

