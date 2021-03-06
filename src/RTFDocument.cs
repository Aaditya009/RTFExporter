﻿using System;
using System.IO;
using System.Collections.Generic;

namespace RTFExporter {

    public class Margin {
        public float left;
        public float right;
        public float top;
        public float bottom;

        public Margin(float left, float right, float top, float bottom) {
            this.left = left;
            this.right = right;
            this.top = top;
            this.bottom = bottom;
        }
    }

    public enum Orientation {
        Landscape,
        Portrait
    }

    public enum Units {
        Inch,
        Millimeters,
        Centimeters
    }

    public class RTFDocument : IDisposable {
        public List<RTFParagraph> paragraphs = new List<RTFParagraph>();
        public List<Color> colors = new List<Color>();
        public List<string> fonts = new List<string>();
        public string author;
        public float width;
        public float height;
        public Orientation orientation;
        public Margin margin;
        public Units units;
        private FileStream fileStream;
        private StreamWriter streamWriter;
        public int version = 1;
        public List<string> keywords = new List<string>();

        public RTFDocument() {
            Init(8, 11, Orientation.Portrait, Units.Inch);
        }

        public RTFDocument(string path) {
            SetFile(path);
            Init(8, 11, Orientation.Portrait, Units.Inch);
        }

        public void SetFile(string path) {
            fileStream = new FileStream(path, FileMode.Create);
            streamWriter = new StreamWriter(fileStream);
        }

        public RTFDocument(float width = 8, float height = 11, Orientation orientation = Orientation.Portrait, Units units = Units.Inch) {
            Init(width, height, orientation, units);
        }

        public void Init(float width, float height, Orientation orientation, Units units) {
            this.width = width;
            this.height = height;
            this.orientation = orientation;
            this.units = units;

            switch (units) {
                case Units.Inch:
                    margin = new Margin(1, 1, 1, 1);
                    break;
                case Units.Millimeters:
                    margin = new Margin(25.4f, 25.4f, 25.4f, 25.4f);
                    break;
                case Units.Centimeters:
                    margin = new Margin(2.54f, 2.54f, 2.54f, 2.54f);
                    break;
            }
        }

        public void SetMargin(float left, float right, float top, float bottom) {
            margin.left = left;
            margin.right = right;
            margin.top = top;
            margin.bottom = bottom;
        }

        public RTFParagraph AppendParagraph() {
            RTFParagraph paragraph = new RTFParagraph(this);
            return paragraph;
        }

        public RTFParagraph AppendParagraph(RTFParagraphStyle style) {
            RTFParagraph paragraph = new RTFParagraph(this);
            paragraph.style = style;
            return paragraph;
        }

        public RTFParagraph AppendParagraph(Alignment alignment) {
            RTFParagraph paragraph = new RTFParagraph(this);
            paragraph.style = new RTFParagraphStyle(alignment);
            return paragraph;
        }

        public RTFParagraph AppendParagraph(Indent indent) {
            return AppendParagraph(Alignment.Left, indent);
        }

        public RTFParagraph AppendParagraph(Alignment alignment, Indent indent) {
            RTFParagraph paragraph = new RTFParagraph(this);
            paragraph.style = new RTFParagraphStyle(alignment, indent);
            return paragraph;
        }

        public RTFParagraph AppendParagraph(Alignment alignment, Indent indent, int spaceBefore, int spaceAfter) {
            RTFParagraph paragraph = new RTFParagraph(this);
            paragraph.style = new RTFParagraphStyle(alignment, indent, spaceBefore, spaceAfter);
            return paragraph;
        }

        public void Close() {
            streamWriter.Close();
            fileStream.Close();
        }

        public void Save() {
            streamWriter.Write(RTFParser.ToString(this));
        }

        public void Dispose() {
            Save();
            Close();
        }
    }

}
