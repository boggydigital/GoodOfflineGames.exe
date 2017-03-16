﻿using System;
using System.Collections.Generic;
using System.Linq;

using Interfaces.Console;
using Interfaces.LineBreaking;
using Interfaces.Presentation;

namespace Controllers.Presentation
{
    public class ConsolePresentationController : IPresentationController<string>
    {
        private ILineBreakingController lineBreakingController;
        private IConsoleController consoleController;

        private IList<int> previousScreenLinesLengths;

        private int previousWindowWidth;
        private int previousWindowHeight;

        public ConsolePresentationController(
            ILineBreakingController lineBreakingController,
            IConsoleController consoleController)
        {
            this.lineBreakingController = lineBreakingController;
            this.consoleController = consoleController;

            consoleController.Clear();

            previousScreenLinesLengths = new List<int>();
            consoleController.CursorVisible = false;

            previousWindowWidth = consoleController.WindowWidth;
            previousWindowHeight = consoleController.WindowHeight;
        }

        private int PresentLine(int line, string content)
        {
            var currentLength = content.Length;
            var previousLineLength = previousScreenLinesLengths.ElementAtOrDefault(line);
            var paddedContent = content;

            consoleController.SetCursorPosition(0, line);

            if (previousLineLength > currentLength)
                paddedContent = content.PadRight(content.Length + previousLineLength - currentLength);

            consoleController.Write(paddedContent);

            return currentLength;
        }

        private void PresentViewModel(string text, ref int currentScreenLine, IList<int> currentLinesLengths)
        {
            var lines = lineBreakingController.BreakLines(text, consoleController.WindowWidth);

            foreach (var line in lines)
            {
                var currentLineLength = PresentLine(
                        currentScreenLine++,
                        line);

                currentLinesLengths.Add(currentLineLength);
            }
        }

        public void Present(IEnumerable<string> views)
        {
            if (previousWindowWidth != consoleController.WindowWidth ||
                previousWindowHeight != consoleController.WindowHeight)
                consoleController.Clear();

            var wrappedViews = new List<string>();
            foreach (var view in views)
                wrappedViews.AddRange(view.Split(new string[] { "\n" }, StringSplitOptions.None));

            var currentLinesLengths = new List<int>();
            var currentScreenLine = 0;

            foreach (var view in wrappedViews)
                PresentViewModel(
                    view, 
                    ref currentScreenLine, 
                    currentLinesLengths);

            // erase previous lines that might be left on the screen
            var previousLinesCount = previousScreenLinesLengths.Count();
            if (previousLinesCount >= currentScreenLine)
            {
                for (var pp = currentScreenLine; pp < previousLinesCount; pp++)
                {
                    consoleController.SetCursorPosition(0, currentScreenLine++);
                    consoleController.Write(string.Empty.PadRight(previousScreenLinesLengths[pp]));
                }
            }

            previousScreenLinesLengths = currentLinesLengths;

            previousWindowWidth = consoleController.WindowWidth;
            previousWindowHeight = consoleController.WindowHeight;
        }
    }
}
