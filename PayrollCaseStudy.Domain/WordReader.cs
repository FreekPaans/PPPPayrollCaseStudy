using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PayrollCaseStudy.Domain {
    class WordReader {
        string _line;
        
        public WordReader(string line) {
            if(line == "") {
                line = null;
            }
            _line = line;
        }

        internal string Next() {
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

        internal static WordReader FromLine(string line) {
            return new WordReader(line);
        }

        internal string NextQuoted() {
            if(_line[0]!='"') {
                throw new Exception("Next is not quoted");
            }

            var nextQuote = _line.IndexOf('"', 1);
            if(nextQuote == -1) {
                throw new Exception("Closing quote not found");
            }

            var next = _line.Substring(1,nextQuote-1);
            _line = _line.Substring(nextQuote+2);
            return next;
        }

        internal bool HasNext() {
            return _line!=null;
        }

        internal decimal NextAsDecimal() {
            return ParseDecimal(Next());
        }

        private static decimal ParseDecimal(string value) {
            return decimal.Parse(value,CultureInfo.InvariantCulture);
        }

        internal int NextAsInt() {
            var word = Next();
            return int.Parse(word);
        }
    }
}
