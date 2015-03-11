using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.CommonTypes {
    public class WordReader {
        string _line;
        
        public WordReader(string line) {
            if(line == "") {
                line = null;
            }
            _line = line;
        }

        public string Next() {
            if(_line == null) {
                return null;
            }
            var nextSpace = _line.IndexOf(' ');
            if(nextSpace == -1) {
                var res = _line;
                _line = null;
                return res;
            }

            var next = _line.Substring(0,nextSpace);
            _line = _line.Substring(nextSpace+1);
            return next;
        }

        public static WordReader FromLine(string line) {
            return new WordReader(line);
        }

        public string NextQuoted() {
            if(_line[0]!='"') {
                throw new Exception("Next is not quoted");
            }

            var nextQuote = _line.IndexOf('"', 1);
            if(nextQuote == -1) {
                throw new Exception("Closing quote not found");
            }

            var next = _line.Substring(1,nextQuote-1);
            _line = _line.Substring(Math.Min(nextQuote+2,_line.Length));
            return next;
        }

        public bool HasNext() {
            return _line!=null;
        }

        public decimal NextAsDecimal() {
            return ParseDecimal(Next());
        }

        private static decimal ParseDecimal(string value) {
            return decimal.Parse(value,CultureInfo.InvariantCulture);
        }

        public int NextAsInt() {
            var word = Next();
            return int.Parse(word);
        }

        public Date NextAsDate() {
            return Date.Parse(Next());
        }

        public void Skip() {
            Next();
        }
    }
}
